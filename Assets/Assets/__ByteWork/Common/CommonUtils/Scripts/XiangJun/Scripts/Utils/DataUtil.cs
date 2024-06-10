using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Data util.string,float,int 等常用数据工具类
/// </summary>
public static class DataUtil {


    /// <summary>
    /// 替换字符串中的子字符串。
    /// </summary>
    /// <param name="input">原字符串</param>
    /// <param name="oldValue">旧子字符串</param>
    /// <param name="newValue">新子字符串</param>
    /// <param name="count">替换数量</param>
    /// <param name="startAt">从第几个字符开始</param>
    /// <returns>替换后的字符串</returns>
    public static String ReplaceFirst(this string input, string oldValue, string newValue, int startAt = 0)
    {
        int pos = input.IndexOf(oldValue, startAt);
        if (pos < 0)
        {
            return input;
        }
        return string.Concat(input.Substring(0, pos), newValue, input.Substring(pos + oldValue.Length));
    }

	/// <summary>
	/// Strings to int.
	/// </summary>
	/// <returns>The to int.</returns>
	/// <param name="value">Value.</param>
    public static int StringToInt(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            if (value.IndexOf(".") != -1)//如果是浮点类型字符串去掉过多的小数位防止强转后数据溢出
            {
                float tempValue = StringToFloat(value);
                return (int)FloatFormat(tempValue);
            }
            else
            {
                return int.Parse(value);
            }
        }
        else
        {
            return 0;
        }
    }

	/// <summary>
	/// Strings to float.
	/// </summary>
	/// <returns>The to float.</returns>
	/// <param name="value">Value.</param>
    public static float StringToFloat(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            return float.Parse(value);
        }
        else
        {
            return 0;
        }
    }

	/// <summary>
	/// Floats to int.
	/// </summary>
	/// <returns>The to int.</returns>
	/// <param name="value">Value.</param>
	public static int FloatToInt(float value)
	{
		return (int)FloatFormat (value);
	}

	/// <summary>
	/// 浮点数精度处理.
	/// </summary>
	/// <returns>The format.</returns>
	/// <param name="num">Number.</param>
	/// <param name="format">Format.默认为7位</param>
	public static float FloatFormat(float num , string format = "f7")
	{
		return float.Parse(num.ToString(format));
	}

	public static string  Get_uft8(string unicodeString)
	{
		System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding();
		Byte[] encodedBytes = utf8.GetBytes(unicodeString);
		String decodedString = utf8.GetString(encodedBytes);
		return decodedString;
	}
}
