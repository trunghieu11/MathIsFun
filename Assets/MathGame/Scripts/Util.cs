
/***********************************************************************************************************
 * Produced by App Advisory - http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/




using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace ElevenGameStudio.MathFrenzy
{
	public static class Utils
	{
        public static string FIRST_PLAY_PREF = "_FirstPlay";
        public static string TOTAL_DIAMOND_PREF = "_TotalDiamond";
        public static string ARRAY_COLOR_SAVED_PREF = "_arrayColorSaved";
        public static string GAMEOVER_COUNT_PREF = "GAMEOVER_COUNT";
        public static string LAST_SCORE_PREF = "LAST_SCORE";
        public static string BEST_SCORE_PREF = "BEST_SCORE";
        public static string RESTART_FROM_GAMEOVER_PREF = "_RestartFromGameOver";
        public static string MUTED_PREF = "Muted";
        public static string LEADERBOARD_ID_PREF = "__LEADERBOARDID";
        public static string LAST_GET_DIAMOND_PREF = "_LastGetDiamond";
        public static int SCORES_EACH_LEVEL = 5;

        private static System.Random Random = new System.Random();

		public static float RandomValue()
		{
			return (float)Random.NextDouble();
		}

		public static float RandomRange(float min, float max)
		{
			return min + ((float)Random.NextDouble() * (max - min));
		}

		public static int RandomRange(int min, int max)
		{
			return Random.Next(min, max);
		}
	}
}