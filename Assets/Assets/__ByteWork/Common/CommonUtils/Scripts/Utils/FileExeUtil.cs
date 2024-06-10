using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System;

public class FileExeUtil {
	
	
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
				bytes = texture.EncodeToJPG();
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

	//压缩图片尺寸
	public static Texture2D ScaleTexture(Texture2D source)
	{
		return ScaleTexture(source , source.width , source.height);
	}

	/// <summary>
	/// 压缩图片尺寸，大小.
	/// </summary>
	/// <returns>The texture.</returns>
	/// <param name="source">Source.</param>
	/// <param name="targetWidth">Target width.</param>
	/// <param name="targetHeight">Target height.</param>
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

    /// <summary>
    /// 将字符串写入utf8编码的文件中
    /// </summary>
    /// <param name="_textToWrite"></param>
    /// <param name="_directory"></param>
    /// <param name="_fileName"></param>
    public static void WriteStringToUtf8File(string _textToWrite, string _fullFilePath)
    {
        if (File.Exists(_fullFilePath))
            File.Delete(_fullFilePath);
        FileStream fs = new FileStream(_fullFilePath, FileMode.Create);
        byte[] data = System.Text.Encoding.UTF8.GetBytes(_textToWrite);
        fs.Write(data, 0, data.Length);
        fs.Flush();
        fs.Close();
    }

}
