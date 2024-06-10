using System;
using UnityEngine;

///<summary>
///<para>Copyright (C) 2010 北京暴风魔镜科技有限公司版权所有</para>
/// <para>文 件 名： PlayerPrefsProxy.cs</para>
/// <para>文件功能： PlayerPrefs访问代理</para>
/// <para>开发部门： 暴风魔镜</para>
/// <para>创 建 人： 刘享军</para>
/// <para>电子邮件： </para>
/// <para>创建日期：2015-10-20</para>
/// <para>修 改 人：</para>
/// <para>修改日期：</para>
/// <para>备    注：</para>
/// </summary>
public static class PlayerPrefsProxy
{
    private static bool _isChanged;
    private static float _lastSaveTime;
    private const float SaveInterval = 5;

    /// <summary>
    /// 清除所有数据
    /// </summary>
    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        SetChanged();
    }

    /// <summary>
    /// 删除key
    /// </summary>
    /// <param name="key"></param>
    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
        SetChanged();
    }

    /// <summary>
    /// 获取float类型数据
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static float GetFloat(string key)
    {
        return GetFloat(key, 0.0f);
    }

    /// <summary>
    /// 获取float类型数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static float GetFloat(string key, float defaultValue)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    /// <summary>
    /// 设置float类型数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        SetChanged();
    }

    /// <summary>
    /// 获取int类型数据
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static int GetInt(string key)
    {
        return GetInt(key, 0);
    }

    /// <summary>
    /// 获取int类型数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static int GetInt(string key, int defaultValue)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    /// <summary>
    /// 设置int类型数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        SetChanged();
    }

    /// <summary>
    /// 获取string类型数据
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetString(string key)
    {
        return PlayerPrefs.GetString(key, null);
    }

    /// <summary>
    /// 获取string类型数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static string GetString(string key, string defaultValue)
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }

    /// <summary>
    /// 设置string类型数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        SetChanged();
    }

    /// <summary>
    /// 判断key是否存在
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    /// <summary>
    /// 根据key设置任意类型数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void Set<T>(string key, T value)
    {
        var type = typeof(T);
        object v = value;

        if (type == typeof(float))
        {
            SetFloat(key, (float)v);
            return;
        }

        if (type == typeof(int))
        {
            SetInt(key, (int)v);
            return;
        }

        SetString(key, value.ToString());
        SetChanged();
    }

    /// <summary>
    /// 获取任意类型数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T Get<T>(string key)
    {
        if (!HasKey(key))
        {
            return default(T);
        }

        var type = typeof(T);
        object value;

        if (type == typeof(float))
        {
            value = GetFloat(key);
            return (T)value;
        }

        if (type == typeof(int))
        {
            value = GetInt(key);
            return (T)value;
        }

        if (type == typeof(bool))
        {
            value = bool.Parse(GetString(key));
            return (T)value;
        }

        if (type.IsEnum)
        {
            var name = GetString(key);
            return (T)Enum.Parse(type, name, true);
        }

        value = GetString(key);
        return (T) value;
    }

    /// <summary>
    /// 立即保存所有修改
    /// </summary>
    public static void Save()
    {
        if (!_isChanged)
        {
            return;
        }

        try
        {
            _lastSaveTime = Time.time;
            PlayerPrefs.Save();
            _isChanged = false;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }

    /// <summary>
    /// 周期性检查并保存用户数据
    /// </summary>
    public static void CheckAndSave()
    {
        if (CanSave())
        {
            Save();
        }
    }

    private static void SetChanged()
    {
        _isChanged = true;
    }

    private static bool CanSave()
    {
        return _isChanged && Time.time - _lastSaveTime > SaveInterval;
    }
}
