using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

///<summary>
///<para>Copyright (C) 2010 北京暴风魔镜科技有限公司版权所有</para>
/// <para>文 件 名： StringHelper.cs</para>
/// <para>文件功能： 字符串访问帮助类</para>
/// <para>开发部门： 暴风魔镜</para>
/// <para>创 建 人： 刘享军</para>
/// <para>电子邮件： </para>
/// <para>创建日期：2015-10-20</para>
/// <para>修 改 人：</para>
/// <para>修改日期：</para>
/// <para>备    注：</para>
/// </summary>
public static class StringHelper
{
    /// <summary>
    /// 截取字符串
    /// </summary>
    /// <param name="str"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string Truncate(this string str, int length)
    {
        if (string.IsNullOrEmpty(str) || str.Length < length/2)
        {
            return str;
        }
        int count = 0;
        var sb = new StringBuilder();
        char[] ss = str.ToArray();
        for (int i = 0; i < ss.Length; i++)
        {
            count += (Encoding.UTF8.GetBytes(ss[i].ToString()).Length > 1 ? 2 : 1);
            sb.Append(ss[i]);
            if (count >= length) break;
        }
        return (sb.ToString().Length < str.Length) ? sb.Append("...").ToString() : str;
    }

    /// <summary>
    /// 获取字符串长度
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int GetLength(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return 0;
        }        
		int count = Encoding.UTF8.GetBytes(str).Length;
        return count;
    }

    /// <summary>
    /// 比较字符串（忽略大小写）
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="other">The other.</param>
    /// <returns></returns>
    public static bool IsEqualIgnoreCase(this string str, string other)
    {
        if (str == null || other == null)
        {
            return false;
        }

        return string.Compare(str, other, StringComparison.InvariantCultureIgnoreCase) == 0;
    }

    /// <summary>
    /// 判断字符串是否是在线视频地址
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
	public static bool  IsOnlineVideoUrl(this string str)
	{
		if (string.IsNullOrEmpty (str)) 
		{
			return false ;
		}
		if (str.StartsWith ("qstp://") || str.StartsWith ("http://")) 
		{
			return true ;
		}
		return false;
	}
}
