
/***********************************************************************************************************
 * Produced by App Advisory - http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/




using UnityEngine;
using System.Collections;

namespace ElevenGameStudio.MathFrenzy {
    public class ScoreManager {
        private static string LAST_SCORE = "LAST_SCORE";
        private static string LAST_LEVEL = "LAST_LEVEL";
        private static string NEW_BEST = "NEW_BEST";
        private static string BEST_SCORE = "BEST_SCORE";
        private static string UPDATED_LAST_SCORE_TO_LEADER_BOARD = "UPDATED_LAST_SCORE_TO_LEADER_BOARD";
        private static string UPDATED_BEST_SCORE_TO_LEADER_BOARD = "UPDATED_BEST_SCORE_TO_LEADER_BOARD";

        public static void SaveScore(int lastScore, int level) {
            PlayerPrefs.SetInt(LAST_SCORE, lastScore);
            PlayerPrefs.SetInt(LAST_LEVEL, level);
            PlayerPrefsX.SetBool(UPDATED_LAST_SCORE_TO_LEADER_BOARD, false);

            int best = GetBestScore();

            PlayerPrefsX.SetBool(NEW_BEST, lastScore > best);
            
            if (lastScore > best) {
                PlayerPrefs.SetInt(BEST_SCORE, lastScore);
                PlayerPrefsX.SetBool(UPDATED_LAST_SCORE_TO_LEADER_BOARD, false);
            }
            
            PlayerPrefs.Save();
        }

        public static int GetLastScore() {
            return PlayerPrefs.GetInt(LAST_SCORE);
        }


        public static int GetLastLevel() {
            return PlayerPrefs.GetInt(LAST_LEVEL);
        }

        public static bool GetLastScoreIsBest() {
            int temp = PlayerPrefs.GetInt(NEW_BEST);
            if (temp == 1) {
                return true;
            }
            return false;
        }

        public static int GetBestScore() {
            return PlayerPrefs.GetInt(BEST_SCORE);
        }

        public static bool IsUpdatedLastScoreToLeaderBoard() {
            return PlayerPrefsX.GetBool(UPDATED_LAST_SCORE_TO_LEADER_BOARD);
        }

        public static void UpdateLastScoreToLeaderBoard() {
            PlayerPrefsX.SetBool(UPDATED_LAST_SCORE_TO_LEADER_BOARD, true);
        }

        public static bool IsUpdatedBestScoreToLeaderBoard() {
            return PlayerPrefsX.GetBool(UPDATED_BEST_SCORE_TO_LEADER_BOARD);
        }

        public static void UpdateBestScoreToLeaderBoard() {
            PlayerPrefsX.SetBool(UPDATED_BEST_SCORE_TO_LEADER_BOARD, true);
        }
    }
}