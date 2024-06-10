using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System;
using System.IO;

public class FileCacheManager : MonoBehaviour
{

	public static string ImageCachePath = Application.persistentDataPath + "/UniImageCache/";
	private static FileCacheManager Instance;

	public static Texture2D GetCache (string url, int width, int hight)
	{
		string img_name = MD5Encrypt (url) + ".jpg"; //根据URL获取文件的名字

		if ( File.Exists (ImageCachePath + img_name) )
		{
			FileStream fs = File.OpenRead (ImageCachePath + img_name); //OpenRead
			int filelength = 0;
			filelength = (int)fs.Length; //获得文件长度 
			Byte[] image = new Byte[filelength]; //建立一个字节数组 
			fs.Read (image, 0, filelength); //按字节流读取 
			Texture2D text = new Texture2D (width, hight);
			bool flag = text.LoadImage (image);
            if (flag)
            {
                //读取文件
                return text;
            }
            else
            {
                Debug.Log("Image load error! ");
                return null;
            }
		} else
		{
			return null;
		}
	}

	public static void LoadAndSave (string url, Action<Texture2D> callBack)
	{
		string img_name = MD5Encrypt (url) + @".jpg";
		string path = ImageCachePath + img_name;
		GameObject obj = GameObject.Find ("LoadFileObject");
		//MonoBehaviour behaviour;
		if ( obj == null )
		{
			obj = new GameObject ("LoadFileObject");
			Instance = obj.AddComponent<FileCacheManager> ();
		}
		Instance = obj.GetComponent<FileCacheManager> ();
		Instance.StartCoroutine (Instance.DownLoadToLocal (url, path, callBack));
	}

	protected IEnumerator DownLoadToLocal (string url, string savePath, Action<Texture2D> callBack = null)
	{
		WWW www = new WWW (url);
		yield return www;
		if ( www.error == null )
		{
			Texture2D texture = ScaleTexture(www.texture , (int)(www.texture.width / 2.5) , (int)(www.texture.height /2.5));
			if ( callBack != null )
			{
				callBack (texture);
			}
			if ( !Directory.Exists (FileCacheManager.ImageCachePath) )
			{
				Directory.CreateDirectory (FileCacheManager.ImageCachePath);
			}
			byte[] bytes = EncodeTexture(texture , savePath);
//			DebugHelper.Log(savePath);
			if(bytes == null)
			{
				bytes = www.bytes;
			}
			FileStream fs = File.Create (savePath);
//			fs.Write (www.bytes, 0, www.bytes.Length);
			fs.Write(bytes , 0 , bytes.Length);
			fs.Close ();
		} else
		{
			Debug.LogWarning (www.error);
		}
		
	}

	public static byte[] EncodeTexture(Texture2D texture , string path)
	{
		Byte[] bytes = null;
		string Extension = Path.GetExtension (path).ToUpper();
		switch (Extension)
		{
			case ".PNG":
				bytes = texture.EncodeToPNG();
				break;
			case ".JPG":
				bytes = texture.EncodeToJPG();
				break;
			default:
				Debug.LogWarning("不支持图片格式：" + Extension);
				break;
		}
		return bytes;
	}

	///   <summary>  
	///   给一个字符串进行MD5加密  
	///   </summary>  
	///   <param   name="strText">待加密字符串</param>  
	///   <returns>加密后的字符串</returns>  
	public static string MD5Encrypt (string strText)
	{
		
		MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider ();  
		byte[] InBytes = Encoding.GetEncoding ("utf-8").GetBytes (strText);  
		byte[] OutBytes = md5.ComputeHash (InBytes);  
		string OutString = "";  
		for (int i = 0; i < OutBytes.Length; i++)
		{
			OutString += OutBytes [i].ToString ("x2");  
		}
		return OutString;
	}

	public static Texture2D ScaleTexture(Texture2D source)
	{
		return ScaleTexture(source , source.width , source.height);
	}
	
	public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
	{
		var result = new Texture2D(targetWidth, targetHeight, source.format, false);
		//float incX = (1.0f / targetWidth);
		//float incY = (1.0f / targetHeight);
		for (int i = 0; i < result.height; ++i)
		{
			for (int j = 0; j < result.width; ++j)
			{
				Color newColor = source.GetPixelBilinear(j / (float)result.width, i / (float)result.height);
				result.SetPixel(j, i, newColor);
			}
		}
		result.Apply();
		return result;
	}

}
