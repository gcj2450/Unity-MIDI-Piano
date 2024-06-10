using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// 编码转换工具类
/// </summary>
public class CodingHelper
{
    public static string Md5(string input)
    {
        MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);
        var sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString();
    }

    public static string Sha1(string s)
    {
        byte[] cleanBytes = Encoding.Default.GetBytes(s);
        byte[] hashedBytes = SHA1.Create().ComputeHash(cleanBytes);
        return BitConverter.ToString(hashedBytes).Replace("-", "");
    }

    public static string EncodeBase64(Encoding encode, string source)
    {
        string s;
        byte[] bytes = encode.GetBytes(source);
        try
        {
            s = Convert.ToBase64String(bytes);
        }
        catch
        {
            s = source;
        }
        return s;
    }

    /// <summary>
    /// Base64加密，采用utf8编码方式加密
    /// </summary>
    /// <param name="source">待加密的明文</param>
    /// <returns>加密后的字符串</returns>
    public static string EncodeBase64(string source)
    {
        return EncodeBase64(Encoding.UTF8, source);
    }

    /// <summary>
    /// Base64解密
    /// </summary>
    /// <param name="encode">解密采用的编码方式，注意和加密时采用的方式一致</param>
    /// <param name="result">待解密的密文</param>
    /// <returns>解密后的字符串</returns>
    public static string DecodeBase64(Encoding encode, string result)
    {
        string decode = "";
        byte[] bytes = Convert.FromBase64String(result);
        try
        {
            decode = encode.GetString(bytes);
        }
        catch
        {
            decode = result;
        }
        return decode;
    }

    /// <summary>
    /// Base64解密，采用utf8编码方式解密
    /// </summary>
    /// <param name="result">待解密的密文</param>
    /// <returns>解密后的字符串</returns>
    public static string DecodeBase64(string result)
    {
        return DecodeBase64(Encoding.UTF8, result);
    }

    /// <summary>
    /// 转成Url编码
    /// 没找到c#中转换的方法，直接替换了=和&
    /// </summary>
    /// <returns>The encode.</returns>
    /// <param name="str">String.</param>
    public static string UrlEncode(string str)
    {
        str = Uri.EscapeDataString(str);
        Regex reg = new Regex(@"%[a-f0-9]{2}");
        string upper = reg.Replace(str, m => m.Value.ToUpperInvariant());
        return upper;
    }

    public static string Hmasha1(string key, string content)
    {
        Encoding encoding = Encoding.UTF8;
        var keyByte = encoding.GetBytes(key);
        string result = null;
        using (var hmacsha1 = new HMACSHA1(keyByte))
        {
            hmacsha1.ComputeHash(encoding.GetBytes(content));
            result = ByteToString(hmacsha1.Hash).ToLower();
        }
        return result;
    }

    private static string ByteToString(byte[] buff)
    {
        string sbinary = "";
        for (int i = 0; i < buff.Length; i++)
            sbinary += buff[i].ToString("X2"); /* hex format */
        return sbinary;
    }    

}
