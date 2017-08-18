
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

namespace ElevenGameStudio.MathFrenzy {
    public class MenuLogic : MonoBehaviour {
        public Transform Title;

        public Text score;
        public Text bestScore;

        public GameObject newBestScoreLabel;

        public void OnEnable() {
            foreach (Transform t in Title) {
                t.localScale = Vector3.one;
            }

            score.text = ScoreManager.GetLastScore().ToString();
            bestScore.text = ScoreManager.GetBestScore().ToString();

            bool isNewBest = ScoreManager.GetLastScoreIsBest();

            if (isNewBest) {
                newBestScoreLabel.SetActive(true);
            } else {
                newBestScoreLabel.SetActive(false);
            }
        }

        public void OnDisable() {
            foreach (Transform t in Title) {
                t.localScale = Vector3.one;
            }
        }
    }
}