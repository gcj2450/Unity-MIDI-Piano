using System.Threading;
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class LoadSequence : MonoBehaviour
{

    public enum LoadState
    {
        run,
        stop
    }

    public int LoadNum { get { return loadList.Count; } }
    public int index;

    private LoadState state = LoadState.stop;

    private Action<string, Texture2D> CompleteCallback;
    private Action<string, Texture2D> ProgressCallback;

    private List<string> loadList = new List<string>();

    private WWW loadWWW;

    public void Init(Action<string, Texture2D> CompleteCallback, int index)
    {
        this.CompleteCallback = CompleteCallback;
        this.index = index;
    }

    public void AddLoad(string url)
    {
        loadList.Add(url);
        if (state == LoadState.stop)
        {
            StartLoad();
        }
    }

    public void StartLoad()
    {
        state = LoadState.run;
        string url = loadList[0];
        string extension = Path.GetExtension(url).ToUpper();
        string imgName = FileExeUtil.MD5Encrypt(url) + extension;
        string path = LoaderManager.ImageCachePath + imgName;
        StartCoroutine(DownLoad(url, path));
    }

    protected IEnumerator DownLoad(string url, string path)
    {
        loadWWW = new WWW(url);
        yield return loadWWW;
        if (loadWWW.error == null)
        {
            try
            {
                //Texture2D texture = FileExeUtil.ScaleTexture(loadWWW.texture, (int)(loadWWW.texture.width / 2.5), (int)(loadWWW.texture.height / 2.5));
                //不做缩放直接存储
                Texture2D texture = loadWWW.texture;
                if (CompleteCallback != null)
                {
                    CompleteCallback(url, texture);
                }
                if (!Directory.Exists(FileCacheManager.ImageCachePath))
                {
                    Directory.CreateDirectory(FileCacheManager.ImageCachePath);
                }
                byte[] bytes = FileExeUtil.EncodeTexture(texture, path);
                if (bytes == null)
                {
                    bytes = loadWWW.bytes;
                }
                SaveAsync(bytes, path);
            }
            catch (Exception e)
            {
                Debug.Log("!!!!!!!!!!!DownLoadToLocal:" + e.ToString());
                if (CompleteCallback != null)
                {
                    CompleteCallback(url, loadWWW.texture);
                }
            }
        }
        else
        {
            Debug.Log("DownLoad Image url : " + url + "  error : " + loadWWW.error);
            if (CompleteCallback != null)
            {
                CompleteCallback(url, null);
            }
        }
//		loadWWW.Dispose ();
		Resources.UnloadUnusedAssets ();
		loadWWW = null;
        loadList.Remove(url);
        if (loadList.Count <= 0)
		{
            state = LoadState.stop;
        }
    }

    private void SaveAsync(byte[] data, string path)
    {
        Loom.RunAsync(() =>
        {
            using (FileStream fs = File.Create(path))
            {
                fs.Write(data, 0, data.Length);
                fs.Close();
            }
        });
    }

	void Update () {
		if(loadWWW != null)
		{
			if(state == LoadState.run)
			{
				//@超时判断
				//				Debug.Log("[s." + index + "] Update progress : " + loadWWW.progress);
			}
			else
			{
				
			}
		}else if(state == LoadState.run)
		{
			StartLoad();
		}
	}
}
