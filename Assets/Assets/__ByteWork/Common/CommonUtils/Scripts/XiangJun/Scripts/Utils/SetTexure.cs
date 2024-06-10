using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetTexure : MonoBehaviour {

    private Texture   _defaultTexture;
    public Action<bool> LoadedSuccess;
    /// <summary>
    /// 显示图片的NGUI对象
    /// </summary>
    public RawImage  Texture;

    void Awake()
    {
        _defaultTexture = Texture.texture;
    }

    /// <summary>
    /// 设置图片地址并开始加载
    /// </summary>
    /// <param name="url"></param>
    public void SetPicture(string path)
    {
        Texture.texture  = _defaultTexture;
        if (string.IsNullOrEmpty(path)) return;
        StartCoroutine(DownLoad(path));
    }
	protected IEnumerator DownLoad(string path)
	{
		var loadWWW = new WWW(path);
		yield return loadWWW;
		if (loadWWW.error == null) {
			try {
				//Texture2D texture = FileExeUtil.ScaleTexture (loadWWW.texture, (int)(loadWWW.texture.width / 2.5), (int)(loadWWW.texture.height / 2.5));
                LoadedCallBack(loadWWW.texture);

			} catch (Exception e) {
				Debug.Log ("!!!!!!!!!!!DownLoadToLocal:" + e.ToString ());
			}
		}
	}

    private void LoadedCallBack(Texture  loadTex)
	{
        if (loadTex)
        {
            Texture.texture = loadTex;
            Texture.texture.filterMode = FilterMode.Bilinear;
            if (LoadedSuccess != null)
            {
                LoadedSuccess(true);
            }
        }
        else
        {
            if (LoadedSuccess != null)
            {
                LoadedSuccess(false);
            }
        }
	}
}
