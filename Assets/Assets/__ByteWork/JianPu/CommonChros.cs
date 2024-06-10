using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonChros : MonoBehaviour 
{
    public List<List<int>> CommonChrosList = new List<List<int>>();
    float chrosDist = 3.5f;

    void AddChrosData()
    {
        List<int> firstChros = new List<int>();
        firstChros.Add(39);//1
        firstChros.Add(43);//3
        firstChros.Add(46);//5
        CommonChrosList.Add(firstChros);

        List<int> secChros = new List<int>();
        secChros.Add(36);//6d
        secChros.Add(39);//1
        secChros.Add(43);//3
        CommonChrosList.Add(secChros);

        List<int> thirdChros = new List<int>();
        thirdChros.Add(32);//4d
        thirdChros.Add(36);//6d
        thirdChros.Add(39);//1
        CommonChrosList.Add(thirdChros);

        List<int> forthChros = new List<int>();
        forthChros.Add(34);//5d
        forthChros.Add(38);//7d
        forthChros.Add(41);//2
        CommonChrosList.Add(forthChros);
    }

    void Awake()
    {
        AddChrosData();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
