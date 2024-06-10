using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//OLRequestManager.RequestData requestData = new OLRequestManager.RequestData();
//requestData.m_url = detailUrl;
//            requestData.m_callBack = detailJsonObjectCallback;
//            requestData.m_cache = false;
//            OLRequestManager.Instance.AddRequest(requestData);

/// <summary>
/// 数据加载
/// </summary>
public class OLRequestManager : MonoBehaviour
{

    public class RequestData
    {
        public string m_url;
        public Action<string, string> m_callBack;
        public bool m_cache = false;
        //public WWW m_www;
        public byte m_retryTime = 3;
    }


    public static OLRequestManager Instance;
    List<RequestData> m_requestList;
    public const int MaxRequestThread = 2;
    int m_curRequestNum;
    string CachePath="";

    string CacheFile = "Request.dat";

    Dictionary<string, string> CacheMap;

    void Awake()
    {
        CachePath = Application.persistentDataPath + "/OLCache/";
        m_requestList = new List<RequestData>();
        Instance = this;
        m_curRequestNum = 0;
        LoadCache();
    }
    void Start()
    {

    }
    void OnDestroy()
    {
        SaveCache();
    }

    void NotifyRequest()
    {
        if (m_requestList.Count > 0 && m_curRequestNum < MaxRequestThread)
        {
            m_curRequestNum++;
            RequestData curData = m_requestList[0];
            StartCoroutine(DoRequest(curData));
            m_requestList.RemoveAt(0);
        }
    }
    IEnumerator DoRequest(RequestData request)
    {
        WWW www = new WWW(request.m_url);
        yield return www;

        m_curRequestNum--;
        if (www.error == null)
        {
            request.m_callBack(request.m_url, www.text);
            if (request.m_cache)
                CachRequest(request.m_url, www.text);
            NotifyRequest();
        }
        else
        {
            if (request.m_retryTime > 0)
            {
                Debug.Log("> request retry " + request.m_retryTime);
                request.m_retryTime--;
                AddRequest(request);
            }
            else
            {
                Debug.Log("> request error: " + www.error);
                request.m_callBack(request.m_url, null);
                NotifyRequest();
            }
        }
    }

    public void AddRequest(RequestData request)
    {
        if (request.m_cache)
        {
            string cacheResult = CheckCache(request.m_url);
            if (cacheResult != null && cacheResult.Length > 1)
            {
                request.m_callBack(request.m_url, cacheResult);
            }
        }

        m_requestList.Add(request);
        NotifyRequest();
    }

    #region cache
    void CachRequest(string url, string text)
    {
        if (text == null || text.Length < 1)
            return;

        if (CacheMap.ContainsKey(url))
            CacheMap[url] = text;
        else
            CacheMap.Add(url, text);
    }
    string CheckCache(string url)
    {
        if (CacheMap.ContainsKey(url))
            return CacheMap[url];

        return null;
    }

    void LoadCache()
    {
        if (File.Exists(CachePath + CacheFile))
        {
            try
            {
                FileStream readstream = new FileStream(CachePath + CacheFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryFormatter formatter = new BinaryFormatter();
                CacheMap = (Dictionary<string, string>)formatter.Deserialize(readstream);
                readstream.Close();
            }
            catch (Exception e)
            {
                Debug.Log("request cache Deserialize Exception! "+e.Message);
                CacheMap = new Dictionary<string, string>();
            }
        }
        else
        {
            CacheMap = new Dictionary<string, string>();
        }

    }
    void SaveCache()
    {
        if (!Directory.Exists(CachePath))
        {
            Debug.Log("gcj: " + CachePath);
            Directory.CreateDirectory(CachePath);
        }
        else
            Debug.Log("gcj: " + CachePath);

        FileStream stream = new FileStream(CachePath + CacheFile, FileMode.Create, FileAccess.Write, FileShare.None);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, CacheMap);
        stream.Close();
    }
    #endregion

}