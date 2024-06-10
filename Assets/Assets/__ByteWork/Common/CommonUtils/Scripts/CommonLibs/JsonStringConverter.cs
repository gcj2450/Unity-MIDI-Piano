using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEngine;

public class JsonStringConverter : MonoBehaviour
{

    private static JsonStringConverter _instance;
    /// <summary>
    /// Singleton,方便各模块访问
    /// </summary>
    public static JsonStringConverter Instance
    {
        get
        {
            if (_instance == null)
            {
                var app = FindObjectOfType(typeof(JsonStringConverter)) as JsonStringConverter;
                if (app == null)
                {
                    var appObject = new GameObject("JsonStringConverter");
                    app = appObject.AddComponent<JsonStringConverter>();
                }
                _instance = app;
            }

            return _instance;
        }
    }
        /// <summary>Json字符串转类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
    public T JsonStringToClass<T>(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            //return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
            return JsonFx.Json.JsonReader.Deserialize<T>(value);
        }
        else
        {
            return default(T);
        }
    }

    //public  void ClassListToJson<T>(List<T> classList)
    //{
    //    JsonWriter.Serialize(classList);
    //}

    /// <summary>JSON字符串转类List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
	public List<T> JsonStringToClassList<T>(string json) where T : class
    {
        List<T> tList = new List<T>();

        if (json != null && json.Length > 1)
        {
            T[] list = JsonReader.Deserialize<T[]>(json);
            foreach (T t in list)
            {
                tList.Add(t);
            }
        }
        return tList;
    }
    /// <summary>  Json字符串转类数组
    /// </summary>
    public T[] JsonToClasseArray<T>(string json) where T : class
    {
        //Debug.Log(json);  
        T[] list = JsonReader.Deserialize<T[]>(json);
        return list;
    }
    /// <summary>Json文件转类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path">文件地址</param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public  T JsonFileToClass<T>(string path)
    {
        var streamReader = new StreamReader(path);
        string data = streamReader.ReadToEnd();
        streamReader.Close();

        if (!string.IsNullOrEmpty(data))
        {
            string deStr = EncryptDecrypt.AESDecrypt(data);
            return JsonFx.Json.JsonReader.Deserialize<T>(deStr);
        }
        else
        {
            return default(T);
        }
    }
    /// <summary>Json文件转类List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public List<T> JsonFileToClasseList<T>(string path)
    {
        var streamReader = new StreamReader(path);
        string data = streamReader.ReadToEnd();
        streamReader.Close();
        List<T> tList = new List<T>();
        if (!string.IsNullOrEmpty(data))
        {
            T[] list = JsonReader.Deserialize<T[]>(data);
            foreach (T t in list)
            {
                tList.Add(t);
            }
        }
        return tList;
    }
    /// <summary>Json文件转类数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public  T[] JsonFileToClasseArray<T>(string path)
    {
        var streamReader = new StreamReader(path);
        string data = streamReader.ReadToEnd();
        streamReader.Close();
        //List<T> tList = new List<T>();
        if (!string.IsNullOrEmpty(data))
        {
            T[] list = JsonReader.Deserialize<T[]>(data);
            return list;
        }
        else
        {
            return default(T[]);
        }
    }
    /// <summary>类转json字符串，并且保存到文件中
    /// </summary>
    /// <param name="path"></param>
    /// <param name="fileName"></param>
    /// <param name="_obj"></param>
    public  void ClassToJsonAndSave(string path, string fileName, object _obj)
    {
        string objToStr = ClassToJson(_obj);
        string enStr = EncryptDecrypt.AESEncrypt(objToStr);
        SaveString(path, fileName, enStr);
    }
    /// <summary>保存字符串到本地的txt文本中
    /// </summary>
    /// <param name="path"></param>
    /// <param name="fileName"></param>
    /// <param name="_obj"></param>
    public  void SaveString(string path, string fileName, string _obj)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        if (File.Exists(path + fileName))
            File.Delete(path + fileName);
        var streamWriter = new StreamWriter(path + fileName);
        streamWriter.Write(_obj);
        streamWriter.Close();
    }

    //public  void ClassToJsonAndSave(string fileName, object _obj)
    //{
    //    string objToStr = ClassToJson(_obj);
    //    SaveString(fileName,  objToStr);
    //}

    //public  void SaveString(string _filePath,string _obj)
    //{
    //    FileStream fileStream = null;
    //    try
    //    {
    //        if (File.Exists(_filePath))
    //        {
    //            File.Delete(_filePath);
    //        }
    //        fileStream = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None);
    //        var formatter = new BinaryFormatter();
    //        formatter.Serialize(fileStream, _obj);
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.Log(string.Format("save cache exception:{0}-----------ex:{1}", _filePath, ex.Message));
    //    }
    //    finally
    //    {
    //        if (fileStream != null)
    //        {
    //            fileStream.Close();
    //        }
    //    }
    //}

    /// <summary>类转json字符串
    /// </summary>
    /// <param name="ob"></param>
    /// <returns></returns>
    public  string ClassToJson(object ob)
    {
        return JsonFx.Json.JsonWriter.Serialize(ob);
    }

    //====参阅最好用的 unity3d Json数据传输，插件JsonFx ！！=========
    //http://blog.csdn.net/wangping1288888/article/details/9336247


    /// <summary>根据一个JSON，得到一个类
    /// </summary>  
     public T JsonToClass<T>(string json) where T : class
    {
        T t = JsonReader.Deserialize<T>(json);
        return t;
    }


}