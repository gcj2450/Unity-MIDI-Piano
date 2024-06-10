using System.Threading;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
public class LoaderManager : MonoBehaviour {
	
	public static string ImageCachePath = Application.persistentDataPath + "/UniImageCache/";
	public const int SequenceNum = 4;

	private List<LoadSequence> sequenceList = new List<LoadSequence> ();

	private Dictionary<string , Action<Texture2D,string>> loadMap;

	private static LoaderManager instance;

	public static LoaderManager Instance
	{
		get
		{
			if ( instance == null )
			{
				GameObject globalObject = ObjectUtil.GetSceneObjectByName("GlobalObject");
				instance = globalObject.AddComponent<LoaderManager>();
			}
			return instance;
		}
	}

	void Awake()
	{
		instance = this;
		LoadSequence sequece;
		for(int i = 1; i <= SequenceNum ; i++)
		{
			sequece = new GameObject ("Sequece" + i).AddComponent<LoadSequence>();
			sequece.transform.parent = transform;
			sequece.Init(SequeceComplete , i);
			sequenceList.Add (sequece);
		}
		loadMap = new Dictionary<string ,Action<Texture2D,string>> ();
	}

	/// <summary>
	/// 开始加载资源
	/// </summary>
	/// <param name="url">URL.</param>
	/// <param name="callback">Callback.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public void StartLoad(string url , Action<Texture2D,string > callback ,int width, int hight)
	{
		url = url.Replace(" ", "%20");
        string extension = Path.GetExtension(url).ToUpper();
        string imgName = FileExeUtil.MD5Encrypt(url) + extension; //根据URL获取文件的名字
        if (File.Exists(ImageCachePath + imgName))
        {
            LoadLocalFile(url, callback, width, hight);
        }
        else
        {
            LoadNetFile(url, callback);
        }
	}

	private void LoadLocalFile(string url , Action<Texture2D,string > callback, int width, int hight)
	{
	    var imageCachePath = ImageCachePath;
#if UNITY_EDITOR
        string extension = Path.GetExtension(url).ToUpper();
        string imgName = FileExeUtil.MD5Encrypt(url) + extension; //根据URL获取文件的名字
        FileStream fs = File.OpenRead(imageCachePath + imgName); //OpenRead
        int filelength = (int)fs.Length; //获得文件长度 
        var image = new Byte[filelength]; //建立一个字节数组 
        fs.Read(image, 0, filelength); //按字节流读取 

        var text = new Texture2D(width, hight);
        text.LoadImage(image);
        callback(text, imgName);
#else
        Loom.RunAsync(() =>
	    {
            string extension = Path.GetExtension(url).ToUpper();
            string imgName = FileExeUtil.MD5Encrypt(url) + extension; //根据URL获取文件的名字
            FileStream fs = File.OpenRead(imageCachePath + imgName); //OpenRead
            int filelength = (int)fs.Length; //获得文件长度 
            var image = new Byte[filelength]; //建立一个字节数组 
            fs.Read(image, 0, filelength); //按字节流读取 
            Loom.QueueOnMainThread(() =>
            {
                var text = new Texture2D(width, hight);
                text.LoadImage(image);
                callback(text,imgName);
            });
	    });
#endif

    }

    private void LoadNetFile(string url , Action<Texture2D,string> callback)
	{
		if(loadMap.ContainsKey(url))
		{
			loadMap[url] = callback;
			return;
		}
		//选择序列
		LoadSequence sequence = GetSequence ();
		if ( sequence != null )
		{
//			Debug.LogWarning("chose index:" + sequence.index);
			sequence.AddLoad (url);
			loadMap.Add( url , callback);
		} else
		{
			Debug.LogWarning("下载序列获取失败。");
		}
	}


	/// <summary>
	/// 加载前，清除掉之前所有的下载序列
	/// </summary>
	public void LoadByExclusive()
	{
		Debug.Log ("未实现");
	}

	private LoadSequence GetSequence()
	{
		LoadSequence sequence = null;
		foreach (LoadSequence tempSequence in sequenceList)
		{
			if(sequence == null || tempSequence.LoadNum < sequence.LoadNum)
			{
				sequence = tempSequence;
			}
		}
		return sequence;
	}

//	private bool IsFilter(string url)
//	{
//		if ( loadMap.ContainsKey (url) )
//		{
//		}
//	}

	private void SequeceComplete(string url , Texture2D texture)
	{
		if ( loadMap.ContainsKey (url) )
		{
			if ( loadMap [url] != null )
			{
                url = url.Replace(" ", "%20");
                string extension = Path.GetExtension(url).ToUpper();
                string imgName = FileExeUtil.MD5Encrypt(url) + extension; //根据URL获取文件的名字

                loadMap [url] (texture, imgName);
			}
			loadMap.Remove (url);
		}
	}
}
