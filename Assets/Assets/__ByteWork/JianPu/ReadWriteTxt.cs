using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadWriteTxt : MonoBehaviour
{
    List<string> txt = new List<string>();
    // Use this for initialization
    void Start()
    {
        ReadTxt();

    }
    void ReadTxt()
    {
        var file = File.Open(Application.persistentDataPath + "/words.txt", FileMode.Open);

        using (var stream = new StreamReader(file))
        {
            while (!stream.EndOfStream)
            {
                string oriData = stream.ReadLine();
                oriData = oriData.Insert(0, "('");
                oriData = oriData.Insert(oriData.IndexOf(','), "'");
                oriData = oriData.Insert(oriData.IndexOf(',') + 1, "'");
                oriData = oriData.Insert(oriData.Length, "'),");
                txt.Add(oriData);
            }
        }
        Debug.Log(txt[0]);

        file.Close();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            Write();
        }
    }

    public void Write()
    {
        FileStream fs = new FileStream(Application.persistentDataPath + "/wordsql.txt", FileMode.Create);
        foreach (string item in txt)
        {
            //获得字节数组
            byte[] data = System.Text.Encoding.UTF8.GetBytes(item+"\n");
            //开始写入
            fs.Write(data, 0, data.Length);
        }
        //清空缓冲区、关闭流
        fs.Flush();
        fs.Close();

    }
}