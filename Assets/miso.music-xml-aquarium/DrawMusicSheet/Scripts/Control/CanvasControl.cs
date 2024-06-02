using System.Collections.Generic;
using UnityEngine;
using generator;
using symbol;
using util;
using xmlParser;

namespace control
{
    public class CanvasControl : MonoBehaviour
    {
        private string fileName = "Assets/miso.music-xml-aquarium/Materials/MusicXml/上春山完整版.xml";
        private CommonParams _commonParams = CommonParams.GetInstance();

        private GameObject _prefabSymbol;
        private GameObject _prefabText;
        private GameObject _prefabLine;
        private GameObject _prefabFileButton;
        // Use this for initialization
        private void Start()
        {

            // 初始化一些参数
            _prefabSymbol = (GameObject)Resources.Load("Prefabs/Prefab_Symbol");
            _prefabText = (GameObject)Resources.Load("Prefabs/Prefab_Text");
            _prefabLine = (GameObject)Resources.Load("Prefabs/Prefab_Line");
            _prefabFileButton = (GameObject)Resources.Load("Prefabs/Prefab_FileButton");
            // 设置到单例模式中对应的参数
            _commonParams.SetPrefabSymbol(_prefabSymbol);
            _commonParams.SetPrefabText(_prefabText);
            _commonParams.SetPrefabLine(_prefabLine);
            _commonParams.SetPrefabFileButton(_prefabFileButton);
            _commonParams.SetScoreName(fileName); // 设置要加载的xml文件名
            //        DrawScore("Assets/Materials/example.xml");
            string scoreName = _commonParams.GetScoreName();
	        DrawScore(scoreName);
//            DrawScore("Assets/Materials/MusicXml/印第安鼓.xml");
        }

        // Update is called once per frame
        private void Update()
        {

        }

        private void DrawScore(string filename)
        {
            // 解析MusicXml文件
            Debug.Log(filename);
            XmlFacade xmlFacade = new XmlFacade(filename);
            // 生成乐谱表
            ScoreGenerator scoreGenerator =
                new ScoreGenerator(xmlFacade.GetBeat().GetBeats(), xmlFacade.GetBeat().GetBeatType());
            List<List<Measure>> scoreList = scoreGenerator.Generate(xmlFacade.GetMeasureList(), Screen.width - 67);
            Debug.Log($"scoreList: {scoreList.Count}");
            foreach (var item in scoreList)
            {
                Debug.Log($"item.Count: {item.Count}");
            }
            // 准备绘制乐谱对象及其他参数
            GameObject parentObject = GameObject.Find("Canvas_Score");
            List<float> screenSize = new List<float>();
            screenSize.Add(Screen.width);
            screenSize.Add(Screen.height);
            List<string> scoreInfo = new List<string>();
            // 乐谱名称和作者信息
            scoreInfo.Add(xmlFacade.GetWorkTitle()); // 0
            scoreInfo.Add(xmlFacade.GetCreator()); // 1
            Debug.Log(parentObject == null);
            // 绘制乐谱视图
            ScoreView scoreView = new ScoreView(scoreList, parentObject, screenSize, scoreInfo);

            // 更改乐符颜色
//        Symbol symbol = scoreList[0][0].GetMeasureSymbolList()[0][1][2];
//        SymbolControl symbolControl = new SymbolControl(symbol);
//        symbolControl.SetColor(Color.red);
        }
    }
}