using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PianoKeyBoardMgr : MonoBehaviour 
{
    ///// <summary>
    ///// 五个钢琴键预制体
    ///// 顺序：完整键、右缺口键、两缺口键、左缺口键、黑键
    ///// </summary>
    //public List<GameObject> KeyBoardPrefabs = new List<GameObject>();

    //int[] keyIds = new int[88] {1, 4,3 ,   
    //                            1, 4,2,4, 3 ,1, 4,2,4, 2,4 ,3 ,
    //                            1, 4,2,4, 3 ,1, 4,2,4, 2,4 ,3 ,
    //                            1, 4,2,4, 3 ,1, 4,2,4, 2,4 ,3 ,
    //                            1, 4,2,4, 3 ,1, 4,2,4, 2,4 ,3 ,
    //                            1, 4,2,4, 3 ,1, 4,2,4, 2,4 ,3 ,
    //                            1, 4,2,4, 3 ,1, 4,2,4, 2,4 ,3 ,
    //                            1, 4,2,4, 3 ,1, 4,2,4, 2,4 ,3 ,
    //                            0  };

    //int[]whiteKeyIds=new int[52]{1, 3   ,
    //                            1 ,2, 3 ,1, 2, 2, 3 , 
    //                            1 ,2, 3 ,1, 2, 2, 3 , 
    //                            1 ,2, 3 ,1, 2, 2, 3 , 
    //                            1 ,2, 3 ,1, 2, 2, 3 , 
    //                            1 ,2, 3 ,1, 2, 2, 3 , 
    //                            1 ,2, 3 ,1, 2, 2, 3 , 
    //                            1 ,2, 3 ,1, 2, 2, 3 , 
    //                            0   };
    
    //int []blackKeyIds=new int[36]{4,
                                //4 ,4  ,  4 ,4, 4 , 
                                //4 ,4  ,  4 ,4, 4 , 
                                //4 ,4  ,  4 ,4, 4 , 
                                //4 ,4  ,  4 ,4, 4 , 
                                //4 ,4  ,  4 ,4, 4 , 
                                //4 ,4  ,  4 ,4, 4 , 
                                //4 ,4  ,  4 ,4, 4};
    //白键声音索引
    int[] whiteKeySoundID = new int[52]
    {
        0, 2,
        3, 5,7 ,8 ,10 ,12,14,
        15, 17 ,19 ,20 ,22 ,24,26,
        27, 29, 31 , 32 ,34, 36 ,38,
        39 ,41 ,43 ,44, 46 ,48, 50,
        51 ,53, 55 ,56 ,58, 60, 62,
        63 ,65 ,67 ,68 ,70 ,72 ,74,
        75 ,77, 79, 80, 82, 84, 86,
        87
    };
    //黑键声音索引
    int[] blackKeySoundID = new int[36]
    {
        1,
        4 ,6,9,11,13,
        16,18,21,23,25,
        28,30,33,35,37,
        40,42,45,47,49,
        52,54,57,59,61,
        64,66,69,71,73,
        76,78,81,83,85
    };

    /// <summary>
    /// A0-B0,最左侧三个键
    /// </summary>
    public GameObject FirstGroup;

    /// <summary>
    /// 钢琴最右侧白键盘
    /// </summary>
    public GameObject LastCompleteKey;

    /// <summary>
    /// 中间七组相同布局的键预制体
    /// </summary>
    public GameObject CommonGroupPrefab;

    //FirstGroup+585等于第一个通用布局位置，其后7个通用键盘之间相距1365
    public Button BtnFit;
    public Button BtnNomal;
    float pianosSize;
    void Awake()
    {
        BtnFit.onClick.AddListener(BtnFitClick);
        BtnNomal.onClick.AddListener(BtnNomalClick);


    }

    // Use this for initialization
    void Start () 
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void BtnFitClick()
    {
        float pianoSacle = 950 / GetPianoSize();
        transform.localScale = new Vector3(pianoSacle, pianoSacle, pianoSacle);
    }

    void BtnNomalClick()
    {
        transform.localScale = Vector3.one*0.5f;
    }

    Vector2 itemSize = new Vector2(194, 578);
    float space = 1;
    float GetPianoSize()
    {
        return (itemSize.x + space) * 52 - space;
    }

    //void InitSevenCommonKeyGroup()
    //{
    //    for (int i = 0; i < 7; i++)
    //    {
    //        GameObject go = Instantiate(CommonGroupPrefab) as GameObject;
    //        go.transform.SetParent(FirstGroup.transform.parent);
    //        go.transform.localScale = Vector3.one;
    //        go.SetActive(true);
    //        go.name = i.ToString();
    //        PianoGroup pianoGroup = go.GetComponent<PianoGroup>();
    //        for (int j = 0; j< pianoGroup.GroupKeys.Count; j++)
    //        {
    //            pianoGroup.GroupKeys[j].ID = 3+j + i * 12;
    //        }
    //        go.transform.localPosition = FirstGroup.transform.localPosition + new Vector3(585, 0, 0) + new Vector3(i * 1365, 0, 0);
    //    }
    //}


}
