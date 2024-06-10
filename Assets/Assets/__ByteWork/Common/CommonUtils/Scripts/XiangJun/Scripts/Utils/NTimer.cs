using System;
using UnityEngine;
/// <summary>
/// Nafio 计时
/// </summary>
public class NTimer {
	float durSec;
	float startTimeSec;
	bool isRunning = false;
	
	public NTimer(){
		isRunning = true;
	}

	public NTimer(float durSec){
		Start(durSec);
	}
	
	public void Start(float durSec){
		this.durSec=durSec;
		startTimeSec=Time.realtimeSinceStartup;
		isRunning=true;
	}

	public bool IsRunning()
	{
		return isRunning;
	}

	public bool IsOK(){
        if (!isRunning){
            return false;
        }

		if(Time.realtimeSinceStartup-startTimeSec>=durSec){
			return true;
		}
		return false;
	}
	
    public float SurplusTime(){
		return startTimeSec + durSec - Time.realtimeSinceStartup;
    }

	public float DuringTime(){
		return  durSec ;
	}

	public float SurplusTimePercentage(){
		float t = SurplusTime();
		if(t <= 0)
			return 0;
		return t / durSec;
	}

	public void Reset(){
		Start(durSec);
	}
	
    public void Stop(){
        isRunning = false;
    }

    public void SetTimeOK(){
		isRunning = true;
        startTimeSec = 0;
    }
}//end of class




