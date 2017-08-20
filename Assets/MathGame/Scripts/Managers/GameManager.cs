
/***********************************************************************************************************
 * Produced by App Advisory - http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/




using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using MenuBarouch;
#if APPADVISORY_LEADERBOARD
using ElevenGameStudio.social;
#endif
#if APPADVISORY_ADS
using AppAdvisory.Ads;
#endif

namespace ElevenGameStudio.MathFrenzy {
    public class GameManager : MonoBehaviour {
        public int numberOfPlayToShowInterstitial = 5;

        static System.Random _random = new System.Random();

        public int timeTotalGame;
        public int timeMalus;
        public int timeBonus;

        public Color NORMAL_COLOR;
        public Color GOOD_COLOR;
        public Color FAIL_COLOR;
        public Image BACKGROUND_BACK;

        public ParticleEmitter particleSuccessPrefab;

        public Text point;

        public GameObject QUESTIONS_GO;

        public GameObject BUTTONS_GO;

        public GameObject POINTS;
        public Text pointsText;

        public Text questionNumber1;
        public Text questionOperator; //+=0 -=1 *=2 /=3
        public Text questionNumber2;
        public Text questionResult;

        public Text[] reponses;

        public int level; //responsible to change the speed of the fill out => so the difficulty

        public int score;

        public int correctAnswer; //count the number of good answer, ie. the score

        public Slider slider; //the slider in the top of the game screen

        public Text levelText; //the text to see the level during the game

        private Vector2 pivotPoint;

        public AudioClip goodAnswerSound;
        public AudioClip wrongAnswerSound;
    
        private Question lastQuestion = new Question(-1, -1, 1, Operator.MULTI);

        public delegate void _GameOver();
        public static event _GameOver OnGameOver;

        //play fx when answer is wrong
        public void PlaySoundFalse() {
            GetComponent<AudioSource>().PlayOneShot(wrongAnswerSound);
        }

        //play fx when answer is good
        public void PlaySoundGood() {
            GetComponent<AudioSource>().PlayOneShot(goodAnswerSound);
        }

        //play the music
        public void PlayMusic() {
            GetComponent<AudioSource>().Play();
        }

        //stop the music
        public void StopMusic() {
            GetComponent<AudioSource>().Stop();
        }

        void OnEnable() {
            Application.targetFrameRate = 60;
            StartGame();
        }

        void OnDisable() {
            //StopMusic();
        }

        //method to start the game
        private void StartGame() {
            //PlayMusic();

            //reset the score
            score = Math.Max(1, ScoreManager.GetLastScore() - 5);

            //reset the level
            level = score / Utils.SCORES_EACH_LEVEL + 1;

            point.text = score.ToString();

            levelText.text = "Level " + level.ToString();

            //create the first question
            CreateQuestion();

            //start the game
            StartCoroutine(TimerStart());
        }

        private void CreateQuestion() {
            Question question = QuestionManager.GenerateQuestion(level);
            while (question.Equals(lastQuestion)) {
                question = QuestionManager.GenerateQuestion(level);
            }
            lastQuestion = question;
            SetText(question.Number1, question.Number2, question.Oper, question.Answer);
        }

        //decrease continiously the timer (= the slider), and if = 0 ==> gameover
        IEnumerator TimerStart() {
            slider.maxValue = timeTotalGame;
            slider.value = timeTotalGame;

            while (true) {

                // Make game faster here
                //float timer = 0.01f + ((float)Mathf.Sqrt(level)) / 100f;
                //timer = Math.Min(0.1f, timer * 3);

                float timer = 0.08f - (float)Math.Sqrt(level) / 200f;
                //float timer = 0.008f;

                slider.value -= timer;

                //Debug.Log("Timer: " + timer);

                //if the slider == 0 ===> game over
                if (slider.value <= 0) {
                    break;
                }


                yield return new WaitForSeconds(0.01f);
            }

            GameOver();
        }


        private void GameOver() {
            ScoreManager.SaveScore(score, level);

            FindObjectOfType<MenuManager>().GoToMenu();

            ReportScoreToLeaderboard(score);

            ShowAds();

            if (OnGameOver != null) {
                OnGameOver();
            }
        }
        
        //set the question text
        private void SetText(int n1, int n2, Operator oper, int result) {
            questionNumber1.text = n1.ToString();
            questionNumber2.text = n2.ToString();
            questionOperator.text = GetOperator(oper);
            questionResult.text = result.ToString();

            QuestionType questionType = QuestionManager.RandomQuestionType();

            switch (questionType) {
                case QuestionType.MISS_NUM_1:
                    questionNumber1.text = "?";
                    correctAnswer = n1;
                    break;
                case QuestionType.MISS_NUM_2:
                    questionNumber2.text = "?";
                    correctAnswer = n2;
                    break;
                case QuestionType.MISS_OPERATOR:
                    questionOperator.text = "?";
                    correctAnswer = (int)oper;
                    break;
                case QuestionType.MISS_RESULT:
                    questionResult.text = "?";
                    correctAnswer = result;
                    break;
            }

            questionNumber1.transform.parent.Find("Selected").gameObject.SetActive(questionType == QuestionType.MISS_NUM_1);
            questionNumber2.transform.parent.Find("Selected").gameObject.SetActive(questionType == QuestionType.MISS_NUM_2);
            questionOperator.transform.parent.Find("Selected").gameObject.SetActive(questionType == QuestionType.MISS_OPERATOR);
            questionResult.transform.parent.Find("Selected").gameObject.SetActive(questionType == QuestionType.MISS_RESULT);

            // generate 4 answer
            if (questionType != QuestionType.MISS_OPERATOR) {
                string[] answers = new string[4];
                answers = QuestionManager.GenerateFakeAnswer(correctAnswer).ToArray();

                for (int i = 0; i < 4; i++) {
                    reponses[i].fontSize = 100;
                    reponses[i].text = answers[i];
                }
            } else {
                reponses[0].text = "+";
                reponses[0].fontSize = 130;
                reponses[1].text = "-";
                reponses[1].fontSize = 150;
                reponses[2].text = "x";
                reponses[3].text = "÷";
            }
        }

        public void OnClicked(Text text) {
            int myAnswer;

            bool isMaybeOperator = text.text.Length <= 1;

            if (text.text.Contains("+") && isMaybeOperator) {
                myAnswer = 0;
            } else if (text.text.Contains("-") && isMaybeOperator) {
                myAnswer = 1;
            } else if (text.text.Contains("x") && isMaybeOperator) {
                myAnswer = 2;
            } else if (text.text.Contains("÷") && isMaybeOperator) {
                myAnswer = 3;
            } else {
                myAnswer = int.Parse(text.text);
            }

            if (correctAnswer == myAnswer) {
                score++;

                if (score % Utils.SCORES_EACH_LEVEL == 0) {
                    level++;
                    levelText.text = "Level " + level.ToString();
                }

                pointsText.text = score.ToString();

                StartCoroutine(CorrectAnswerAnim());

                slider.value += timeTotalGame;

                AnimColorBACKGROUND_BACK(true);

                BUTTONS_GO.GetComponent<Animator>().Play("AnimButtonGame");

                PlaySoundGood();
            } else {
                slider.value -= timeTotalGame / 5;

                PlaySoundFalse();

                AnimColorBACKGROUND_BACK(false);
            }
        }

        private string GetOperator(Operator oper) {
            switch (oper) {
                case Operator.PLUS:
                    return "+";
                case Operator.SUBSTRACT:
                    return "-";
                case Operator.MULTI:
                    return "x";
                case Operator.DEVIDE:
                    return "÷";
                default:
                    return "";
            }
        }

        IEnumerator CorrectAnswerAnim() {
            float time = 0.2f;

            QUESTIONS_GO.GetComponent<Animator>().Play("AnimQuestionChange");

            var goParticle = Instantiate(particleSuccessPrefab.gameObject) as GameObject;
            var tParticle = goParticle.transform;
            tParticle.position = new Vector3(point.transform.position.x, point.transform.position.y, point.transform.position.z + 2);
            tParticle.rotation = Quaternion.identity;
            goParticle.SetActive(true);
            goParticle.GetComponent<ParticleEmitter>().Emit(50);
            yield return new WaitForSeconds(time + 0.01f);

            CreateQuestion();
        }

        public void AnimColorBACKGROUND_BACK(bool isGoodAnswer) {
            StartCoroutine(AnimColorBACKGROUND_BACK_Corout(isGoodAnswer));
        }

        IEnumerator AnimColorBACKGROUND_BACK_Corout(bool isGoodAnswer) {
            Color c = FAIL_COLOR;

            var time = 0.3f;
            var originalTime = time;

            if (isGoodAnswer)
                c = GOOD_COLOR;

            while (time > 0.0f) {
                time -= Time.deltaTime;
                BACKGROUND_BACK.color = Color.Lerp(NORMAL_COLOR, c, time / originalTime);
                yield return 0;
            }

        }
        
        void ReportScoreToLeaderboard(int p) {
            if (LeaderboardManager.IsInitialized()) {
			    LeaderboardManager.ReportScore(p);
            }
        }

        /// <summary>
        /// If using Very Simple Ads by App Advisory, show an interstitial if number of play > numberOfPlayToShowInterstitial: http://u3d.as/oWD
        /// </summary>
        public void ShowAds() {
            int count = PlayerPrefs.GetInt("GAMEOVER_COUNT", 0);
            count++;
            PlayerPrefs.SetInt("GAMEOVER_COUNT", count);
            PlayerPrefs.Save();

#if APPADVISORY_ADS
			if(count > numberOfPlayToShowInterstitial)
			{
#if UNITY_EDITOR
			print("count = " + count + " > numberOfPlayToShowINterstitial = " + numberOfPlayToShowInterstitial);
#endif
			if(AdsManager.instance.IsReadyInterstitial())
			{
#if UNITY_EDITOR
				print("AdsManager.instance.IsReadyInterstitial() == true ----> SO ====> set count = 0 AND show interstial");
#endif
				PlayerPrefs.SetInt("GAMEOVER_COUNT",0);
				AdsManager.instance.ShowInterstitial();
			}
			else
			{
#if UNITY_EDITOR
				print("AdsManager.instance.IsReadyInterstitial() == false");
#endif
			}

		}
		else
		{
			PlayerPrefs.SetInt("GAMEOVER_COUNT", count);
		}
		PlayerPrefs.Save();
#else
            if (count >= numberOfPlayToShowInterstitial) {
                PlayerPrefs.SetInt("GAMEOVER_COUNT", 0);
            } else {
                PlayerPrefs.SetInt("GAMEOVER_COUNT", count);
            }
            PlayerPrefs.Save();
#endif
        }
    }

}