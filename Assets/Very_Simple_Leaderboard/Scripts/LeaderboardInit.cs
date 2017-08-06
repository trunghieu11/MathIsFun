﻿#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.
#pragma warning disable 0618 // obslolete
#pragma warning disable 0108 
#pragma warning disable 0649 //never used

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace ElevenGameStudio.social
{
	public class LeaderboardInit : MonoBehaviour 
	{
		public LEADERBOARDIDS leaderboardIds;
		public void SetLEADERBORDIDS(LEADERBOARDIDS t)
		{
			this.leaderboardIds = t;
		}

//		void Awake()
//		{
//			print("Awake - in initialized = " + LeaderboardManager.LEADERBOARD_INIT_IS_INITIALIZED);
//		}

		IEnumerator Start()
		{
			yield return 0;

			LeaderboardManager.LEADERBOARDID = this.leaderboardIds.LEADERBOARDID;
			PlayerPrefs.Save();

			yield return 0;

			if(!LeaderboardManager.LEADERBOARD_INIT_IS_INITIALIZED)
			{
				LeaderboardManager.Init();
			}
	
			LeaderboardManager.LEADERBOARD_INIT_IS_INITIALIZED = true;

			Destroy(gameObject);
		}
	}
}