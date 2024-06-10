using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
/// <summary>
/// 制作方式：先从简谱上把每个歌曲的谱抄下来，1下面一个点就是1d，
///1上面一个点就是1u，然后和带时间戳的歌词文件合并，得到每个简谱的时间
/// </summary>
public class GeCiFormater : MonoBehaviour 
{

    public List<string> txt = new List<string>();

    public SongEntity aijiangshan = new SongEntity();

    public Dictionary<string, int> PianoKeyDic = new Dictionary<string, int>();

    void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {
        AddKeys();
        Debug.Log(PianoKeyDic.Keys.Count);
        Debug.Log(Application.persistentDataPath);

        ReadTxt();


    }

    /// <summary>
    /// 简谱到钢琴键映射
    /// d代表简谱数字下的一个点
    /// u代表简谱数字上的一个点
    /// </summary>
    void AddKeys()
    {
        PianoKeyDic["6dddd"] = 0;
            PianoKeyDic["6dddd7dddd"] = 1;////
        PianoKeyDic["7dddd"] = 2;

        PianoKeyDic["1ddd"] = 3;
            PianoKeyDic["1ddd2ddd"] = 4;////
        PianoKeyDic["2ddd"] = 5;
            PianoKeyDic["2ddd3ddd"] = 6;////
        PianoKeyDic["3ddd"] = 7;
        PianoKeyDic["4ddd"] = 8;
            PianoKeyDic["4ddd5ddd"] = 9;////
        PianoKeyDic["5ddd"] = 10;
            PianoKeyDic["5ddd6ddd"] = 11;////
        PianoKeyDic["6ddd"] = 12;
            PianoKeyDic["6ddd7ddd"] = 13;////
        PianoKeyDic["7ddd"] = 14;

        PianoKeyDic["1dd"] = 15;
            PianoKeyDic["1dd2dd"] = 16;////
        PianoKeyDic["2dd"] = 17;
            PianoKeyDic["2dd3dd"] = 18;////
        PianoKeyDic["3dd"] = 19;
        PianoKeyDic["4dd"] = 20;
            PianoKeyDic["4dd5dd"] = 21;////
        PianoKeyDic["5dd"] = 22;
            PianoKeyDic["5dd6dd"] = 23;////
        PianoKeyDic["6dd"] = 24;
            PianoKeyDic["6dd7dd"] = 25;////
        PianoKeyDic["7dd"] = 26;

        PianoKeyDic["1d"] = 27;
            PianoKeyDic["1d2d"] = 28;////
        PianoKeyDic["2d"] = 29;
            PianoKeyDic["2d3d"] = 30;////
        PianoKeyDic["3d"] = 31;
        PianoKeyDic["4d"] = 32;
            PianoKeyDic["4d5d"] = 33;////
        PianoKeyDic["5d"] = 34;
            PianoKeyDic["5d6d"] = 35;////
        PianoKeyDic["6d"] = 36;
            PianoKeyDic["6d7d"] = 37;////
        PianoKeyDic["7d"] = 38;

        PianoKeyDic["1"] = 39;
            PianoKeyDic["11"] = 40;/////
        PianoKeyDic["2"] = 41;
            PianoKeyDic["23"] = 42;/////
        PianoKeyDic["3"] = 43;
        PianoKeyDic["4"] = 44;
            PianoKeyDic["45"] = 45;/////
        PianoKeyDic["5"] = 46;
            PianoKeyDic["56"] = 47;//////
        PianoKeyDic["6"] = 48;
            PianoKeyDic["67"] = 49;/////
        PianoKeyDic["7"] = 50;

        PianoKeyDic["1u"] = 51;
            PianoKeyDic["1u2u"] = 52;////
        PianoKeyDic["2u"] = 53;
            PianoKeyDic["2u3u"] = 54;/////
        PianoKeyDic["3u"] = 55;
        PianoKeyDic["4u"] = 56;
            PianoKeyDic["4u5u"] = 57;/////
        PianoKeyDic["5u"] = 58;
            PianoKeyDic["5u6u"] = 59;/////
        PianoKeyDic["6u"] = 60;
            PianoKeyDic["6u7u"] = 61;/////
        PianoKeyDic["7u"] = 62;

        PianoKeyDic["1uu"] = 63;
            PianoKeyDic["1uu2uu"] = 64;/////
        PianoKeyDic["2uu"] = 65;
            PianoKeyDic["2uu3uu"] = 66;/////
        PianoKeyDic["3uu"] = 67;
        PianoKeyDic["4uu"] = 68;
            PianoKeyDic["4uu5uu"] = 69;/////
        PianoKeyDic["5uu"] = 70;
            PianoKeyDic["5uu6uu"] = 71;/////
        PianoKeyDic["6uu"] = 72;
            PianoKeyDic["6uu7uu"] = 73;//////
        PianoKeyDic["7uu"] = 74;

        PianoKeyDic["1uuu"] = 75;
            PianoKeyDic["1uuu2uuu"] = 76;/////
        PianoKeyDic["2uuu"] = 77;
            PianoKeyDic["2uuu3uuu"] = 78;/////
        PianoKeyDic["3uuu"] = 79;
        PianoKeyDic["4uuu"] = 80;
            PianoKeyDic["4uuu5uuu"] = 81;/////
        PianoKeyDic["5uuu"] = 82;
            PianoKeyDic["5uuu6uuu"] = 83;/////
        PianoKeyDic["6uuu"] = 84;
            PianoKeyDic["6uuu7uuu"] = 85;/////
        PianoKeyDic["7uuu"] = 86;

        PianoKeyDic["7uuuu"] = 87;
    }

    void ReadTxt()
    {
        var file = File.Open(Application.dataPath + "/Assets/LRC/sishigurenlai.txt", FileMode.Open);

        using (var stream = new StreamReader(file))
        {
            while (!stream.EndOfStream)
            {
                string oriData = stream.ReadLine();
                // '00:32.329', '00:34.465', '5下，6下，1，1，1，2，1', '211,185,465,201,227,409,438'
                oriData = oriData.Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries)[1];
                char c = '\'';
                oriData.Replace(" ", "");
                string[] datas = oriData.Split(new char[] { c});
                SongSentence sentence = new SongSentence();

                sentence.startTime = TimeStrToInt( datas[1]);
                sentence.endTime = TimeStrToInt(datas[3]);

                //Debug.Log(datas[1]); //startTime
                //Debug.Log(datas[3]); //endTime
                //Debug.Log(datas[5]); //sentenceChars
                //Debug.Log(datas[7]); //timeStamps


                string[] notas = datas[5].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);   //5下，6下，1，1，1，2，1 分割
                foreach (var item in notas)
                {
                    //Debug.Log(item);
                    //然后逐一检查空格，并从字典查找对应的钢琴键，添加到sentence.notationNums
                    string itemRemoveEmpty=item.Replace(" ", "");
                    if (!string.IsNullOrEmpty(itemRemoveEmpty))
                    {
                        //单个汉字音符数组
                        string[] singleChar = itemRemoveEmpty.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        List<int> singleCharPianoKeys = new List<int>();
                        foreach (var sChar in singleChar)
                        {
                            if (PianoKeyDic.ContainsKey(sChar))
                                singleCharPianoKeys.Add(PianoKeyDic[sChar]);
                            else
                                Debug.Log("NotContainsKey : " + sChar);
                        }
                        sentence.notationNums.Add(singleCharPianoKeys);
                    }
                }

                string[] timeStas = datas[7].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in timeStas)
                {
                    string itemRemoveEmpty= item.Replace(" ", "");
                    if (!string.IsNullOrEmpty(itemRemoveEmpty))
                        sentence.notationTimeStamp.Add(Int32.Parse( itemRemoveEmpty));
                }
                if (sentence.notationNums.Count != sentence.notationTimeStamp.Count)
                {
                    Debug.Log(sentence.startTime + "   " + sentence.endTime + "     "+
                    sentence.notationNums.Count+ "!="+ sentence.notationTimeStamp.Count);
                }
                //Debug.Log(datas[1]); //startTime
                //Debug.Log(datas[3]); //endTime
                //Debug.Log(datas[5]); //sentenceChars
                //Debug.Log(datas[7]); //timeStamps
                aijiangshan.Sentences.Add(sentence);
                txt.Add(oriData);
            }
        }
        //Debug.Log(txt[0]);

        file.Close();
    }
    public GameObject MusicSign;
    public Transform MusicSignRoot;
    void GenerateMusic(SongEntity _songEntity)
    {
        foreach (SongSentence item in _songEntity.Sentences)
        {
            int tmp = 0;
            for (int i = 0; i < item.notationNums.Count; i++)
            {
                for (int j = 0; j < item.notationNums[i].Count; j++)
                {
                    GameObject go = Instantiate(MusicSign) as GameObject;
                    go.transform.SetParent(MusicSignRoot);
                    go.SetActive(true);
                    go.GetComponent<MusicSign>().KeyId = item.notationNums[i][j];
                    if (i == 0)
                        tmp -= item.notationTimeStamp[0];
                    else
                        tmp += item.notationTimeStamp[i - 1];
                    go.transform.localPosition = new Vector3((item.startTime + tmp)*0.01f, 0, 0);
                }
            }
        }
    }
    bool startMove = false;
    // Update is called once per frame
    void Update () 
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            GenerateMusic(aijiangshan);
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            startMove = true;
        }
        if(startMove)
            MusicSignRoot.localPosition -= new Vector3(Time.deltaTime* Speed, 0, 0);

    }
    public float Speed = 10;
    public void ChangeNotationToPianoKey(string _str)
    {
        if (_str.Contains("下"))
        {
        }
    }

    /// <summary>
    /// 计算字符串中指定字符串出现次数
    /// </summary>
    /// <returns>The string count.</returns>
    /// <param name="str">String.</param>
    /// <param name="substring">Substring.</param>
    public int SubStringCount(string str,string substring)
    {
        if (str.Contains(substring))
        {
            string strReplaced = str.Replace(substring, "");
            return (str.Length - strReplaced.Length) / substring.Length;
        }
        return 0;
    }

    public int TimeStrToInt(string _timeStr)
    {
        string[] splitTimeStr = _timeStr.Split(new char[] { ':' });
        int msec = int.Parse(splitTimeStr[0])*60*1000;
        int ssec = (int)(float.Parse(splitTimeStr[1]) * 1000);
        return msec + ssec;
    }

}

/// <summary>
/// 一首歌曲实体
/// </summary>
[Serializable]
public class SongEntity
{
    /// <summary>
    /// 歌曲名称
    /// </summary>
    public string songName="";

    /// <summary>
    /// 演唱者
    /// </summary>
    public string singerName="";

    /// <summary>
    /// 全部歌词句子
    /// </summary>
    public List<SongSentence> Sentences = new List<SongSentence>();

}

/// <summary>
/// 歌词句子
/// </summary>
[Serializable]
public class SongSentence
{
    /// <summary>
    /// 该句歌词开始时间
    /// </summary>
    public int startTime;
    /// <summary>
    /// 该句歌词结束时间
    /// </summary>
    public int endTime;
    /// <summary>
    /// 该句歌词钢琴键ID
    /// 每个歌词可能有多个音符
    /// </summary>
    public List<List<int>> notationNums = new List<List<int>>();
    /// <summary>
    /// 该句歌词每个谱的时间戳
    /// </summary>
    public List<int> notationTimeStamp=new List<int>();

}
