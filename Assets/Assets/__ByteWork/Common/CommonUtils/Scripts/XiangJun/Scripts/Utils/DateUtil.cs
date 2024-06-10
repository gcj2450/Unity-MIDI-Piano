using UnityEngine;
using System;
using System.Collections;

public class DateUtil {

	/************************************************************************/
	/* 格式化时间：hh：mm：ss                                               */
	/************************************************************************/
	
	public static string FormatHourAndMinAndSec(int totalSec)
	{
		TimeSpan ts = TimeSpan.FromSeconds(totalSec);
		return FormatUnit(ts.Hours.ToString()) + ":" + FormatUnit(ts.Minutes.ToString()) + ":" + FormatUnit(ts.Seconds.ToString());
	}

	private static string FormatUnit(string str)
	{
		if (str.Length % 2 != 0) {
			return "0" + str;
		} else {
			return str;
		}
	}


    public static long getTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalMilliseconds);
    }

	/************************************************************************/
	/* 获得UTC当前秒数                                                      */
	/************************************************************************/
	/// <summary>
	/// 获得UTC当前秒数
	/// </summary>
	/// <value>The now sec.</value>
	public static int NowSec
	{
		get
		{
			DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
			DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
			TimeSpan toNow = dtNow.Subtract(dtStart);
			string timeStamp = toNow.Ticks.ToString();
			int now = int.Parse(timeStamp.Substring(0, timeStamp.Length - 7));
			return now;
		}
	}

	/// <summary>
	/// 时间戳转为C#格式时间
	/// </summary>
	/// <returns>The to date time.</returns>
	/// <param name="timeStamp">Time stamp.</param>
	private static DateTime StampToDateTime(string timeStamp)
	{
		DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
		long lTime = long.Parse(timeStamp + "0000000");
		TimeSpan toNow = new TimeSpan(lTime); 
		return dateTimeStart.Add(toNow);
	}

	/// <summary>
	/// 根据时间戳的秒数格式化时间,三种表现形式：今天，昨天，具体时间(Year-Month-Day）
	/// </summary>
	/// <returns>The date.</returns>
	/// <param name="tSec">以秒为单位的时间戳.</param>
	public static string FormatDate(int tSec)
	{
		//TimeSpan ts = TimeSpan.FromSeconds((double)tSec);
		DateTime dt = StampToDateTime(tSec.ToString());
		DateTime nowDt = DateTime.Today;
		//如果大于今天0点，显示今天
		if (dt.Year == nowDt.Year && dt.Month == nowDt.Month) 
		{
			if(dt.Day == nowDt.Day)
			{
				return "今天";
			}else if(dt.Day == nowDt.Day-1)
			{
				return "昨天";
			}
		}
		return dt.Year + "-" + dt.Month + "-" + dt.Day;
	}
}
