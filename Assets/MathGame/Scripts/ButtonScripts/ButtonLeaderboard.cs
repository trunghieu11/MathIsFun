﻿
/***********************************************************************************************************
 * Produced by App Advisory - http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/




using UnityEngine;
using System.Collections;
#if APPADVISORY_LEADERBOARD
using ElevenGameStudio.social;
#endif

namespace ElevenGameStudio.MathFrenzy
{
	public class ButtonLeaderboard : ButtonHelper 
	{
		override public void OnClicked()
		{
			OnClickedOpenLeaderboard();
		}

		/// <summary>
		/// If player clics on the leaderbord button, we call this method. Works only on mobile (iOS & Android) if using Very Simple Leaderboard by App Advisory : http://u3d.as/qxf
		/// </summary>
		public void OnClickedOpenLeaderboard()
		{
            LeaderboardManager.Init();
            Debug.Log("Clicked Show Leaderboard");
			LeaderboardManager.ShowLeaderboardUI();
		}
	}
}