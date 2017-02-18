
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
using AppAdvisory.social;
#endif
#if APPADVISORY_ADS
using AppAdvisory.Ads;
#endif

namespace AppAdvisory.MathFrenzy {
    public class GameManager : MonoBehaviour {

        private enum Operator {
            plus = 0,
            substract = 1,
            multi = 2,
            devide = 3
        }

        private Operator[] randomOperator = new Operator[] { Operator.plus, Operator.substract, Operator.multi, Operator.devide};

        public int numberOfPlayToShowInterstitial = 5;

        public string VerySimpleAdsURL = "http://u3d.as/oWD";

        static System.Random _random = new System.Random();

        public AudioClip musicBackground;
        public AudioClip goodAnswerSound;
        public AudioClip falsedAnswerSound;

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

        public int _score;

        public int correctAnswer; //count the number of good answer, ie. the score

        public Slider slider; //the slider in the top of the game screen

        public Text levelText; //the text to see the level during the game

        private Vector2 pivotPoint;

        int _result = 0;
        int _number1 = 0;
        int _number2 = 0;
        Operator _choseOperateur = 0;

        public delegate void _GameOver();
        public static event _GameOver OnGameOver;


        //play fx when answer is wrong
        void PlaySoundFalse() {
            GetComponent<AudioSource>().PlayOneShot(falsedAnswerSound);
        }

        //play fx when answer is good
        void PlaySoundGood() {
            GetComponent<AudioSource>().PlayOneShot(goodAnswerSound);
        }

        //play the music
        void PlayMusic() {
            GetComponent<AudioSource>().Play();
        }

        //stop the music
        void StopMusic() {
            GetComponent<AudioSource>().Stop();
        }

        void OnEnable() {
            Application.targetFrameRate = 60;
            StartGame();
        }

        void OnDisable() {
            StopMusic();
        }

        //method to start the game
        private void StartGame() {
            PlayMusic();

            //reset the score
            _score = 0;

            //reset the level
            level = 1;

            point.text = _score.ToString();

            levelText.text = "Level " + level.ToString();

            //create the first question
            ChooseOperator();

            //start the game
            StartCoroutine(TimerStart());
        }

        //decrease continiously the timer (= the slider), and if = 0 ==> gameover
        IEnumerator TimerStart() {
            slider.maxValue = timeTotalGame;
            slider.value = timeTotalGame;

            while (true) {

                // Make game faster here
                float timer = 0.01f + ((float)Mathf.Sqrt(level)) / 100f;

                timer = Math.Min(0.1f, timer * 3);

                slider.value -= timer;

                //Debug.Log("Timer: " + timer);

                //if the slider == 0 ===> game over
                if (slider.value == 0) {
                    break;
                }


                yield return new WaitForSeconds(0.01f);
            }

            GameOver();
        }


        private void GameOver() {
            ScoreManager.SaveScore(_score, level);

            FindObjectOfType<MenuBarouch.MenuManager>().GoToMenu();

            ReportScoreToLeaderboard(_score);

            ShowAds();

            if (OnGameOver != null) {
                OnGameOver();
            }
        }

        //choose operateur for the question : + = 0    - = 1     * = 2      / = 3
        void ChooseOperator() {
            Operator choseOperator = randomOperator[UnityEngine.Random.Range(0, 4)];

            CreateQuestion(choseOperator);
        }

        void CreateQuestion(Operator choseOperator) {

            int result = 0;
            int number1 = 0;
            int number2 = 0;

            int essai = 0;

            while (true) {
                essai++;

                // check this math is avaiable
                bool isOK = true;

                // choose number
                if (choseOperator == Operator.devide) {
                    int mult = UnityEngine.Random.Range(2 + level, 2 + 2 * level);
                    number2 = UnityEngine.Random.Range(2 + level / 2, 3 + level);

                    number1 = mult * number2;
                } else if (choseOperator == Operator.substract) {
                    number2 = UnityEngine.Random.Range(1 + level / 2, 5 + level);
                    number1 = UnityEngine.Random.Range(number2 + level / 2, number2 + 5 + 2 * level);
                } else {
                    number1 = UnityEngine.Random.Range(1 + level / 2, 5 + 2 * level);
                    number2 = UnityEngine.Random.Range(1 + level / 2, 5 + 2 * level);
                }

                // get game result
                result = GetResult(number1, number2, choseOperator);

                // check if this game have only one result
                if (choseOperator == Operator.substract || choseOperator == Operator.devide) {
                    int resultDIV = 0;
                    int resultMINUS = 0;

                    resultDIV = number1 / number2;
                    resultMINUS = number1 - number2;

                    if (resultDIV == resultMINUS) {
                        isOK = false;
                    }

                }
                
                if (choseOperator == Operator.plus || choseOperator == Operator.multi) {
                    int resultMULT = 0;
                    int resultPLUS = 0;

                    resultMULT = number1 * number2;
                    resultPLUS = number1 + number2;

                    if (resultMULT == resultPLUS) {
                        isOK = false;
                    }
                }

                // check this level not same previous level
                if (_result == result && _number1 == number1 && _number2 == number2 && _choseOperateur == choseOperator) {
                    isOK = false;
                }

                if (result <= 0) {
                    isOK = false;
                }

                // check operator devide is correct
                if (choseOperator == Operator.devide) {
                    if (number1 % number2 != 0) {
                        isOK = false;
                    }

                    if (number1 / number2 == 0) {
                        isOK = false;
                    }

                    if (number1 / number2 == 1) {
                        isOK = false;
                    }
                } else {
                    if (choseOperator == Operator.devide) {
                        if (number1 == 0 || number1 == 1 || number2 == 0 || number2 == 1 || result == 0 || result == 1) {
                            isOK = false;
                        }
                    }
                }

                if (result <= 0 || number1 <= 0 || number2 <= 0) {
                    isOK = false;
                }
                
                if (result > 99) {
                    isOK = false;
                }


                //Check again!!!
                if (isOK) {
                    if (choseOperator == Operator.plus) {
                        int resultTest = number1 + number2;
                        if (resultTest != result) {
                            isOK = false;
                        }
                    }
                    if (choseOperator == Operator.substract) {
                        int resultTest = number1 - number2;
                        if (resultTest != result) {
                            isOK = false;
                        }
                    }
                    if (choseOperator == Operator.multi) {
                        int resultTest = number1 * number2;
                        if (resultTest != result) {
                            isOK = false;
                        }
                    }
                    if (choseOperator == Operator.devide) {
                        int resultTest = number1 / number2;
                        if (resultTest != result) {
                            isOK = false;
                        }
                    }

                }


                if (isOK) {
                    _result = result;
                    _number1 = number1;
                    _number2 = number2;
                    _choseOperateur = choseOperator;

                    break;
                }
            }

            SetText(number1, number2, choseOperator, result);
        }

        //set the question text
        private void SetText(int n1, int n2, Operator oper, int result) {
            int TYPE_QUESTION = UnityEngine.Random.Range(0, 4);

            if (TYPE_QUESTION == 0) {
                questionNumber1.text = "?";

                questionNumber2.text = n2.ToString();

                questionOperator.text = GetOperator(oper);

                questionResult.text = result.ToString();

                correctAnswer = n1;
            }

            if (TYPE_QUESTION == 1) {
                questionNumber1.text = n1.ToString();

                questionNumber2.text = n2.ToString();

                questionOperator.text = "?";

                questionResult.text = result.ToString();
                
                correctAnswer = (int)oper;
            }

            if (TYPE_QUESTION == 2) {
                questionNumber1.text = n1.ToString();

                questionNumber2.text = "?";

                questionOperator.text = GetOperator(oper);

                questionResult.text = result.ToString();

                correctAnswer = n2;
            }

            if (TYPE_QUESTION == 3) {
                questionNumber1.text = n1.ToString();

                questionNumber2.text = n2.ToString();

                questionOperator.text = GetOperator(oper);

                questionResult.text = "?";

                correctAnswer = result;
            }

            questionNumber1.transform.parent.FindChild("Selected").gameObject.SetActive(TYPE_QUESTION == 0);
            questionOperator.transform.parent.FindChild("Selected").gameObject.SetActive(TYPE_QUESTION == 1);
            questionNumber2.transform.parent.FindChild("Selected").gameObject.SetActive(TYPE_QUESTION == 2);
            questionResult.transform.parent.FindChild("Selected").gameObject.SetActive(TYPE_QUESTION == 3);

            // generate 4 answer
            if (TYPE_QUESTION != 1) {
                int[] answers = new int[4];

                List<int> listAnswers = new List<int>();

                listAnswers.Add(correctAnswer);

                while (true) {
                    int ans = 0;

                    int addRange = 0;

                    while (true) {
                        bool isOk = true;

                        ans = UnityEngine.Random.Range(1, correctAnswer * 2 + 3);

                        if (ans <= 0) {
                            isOk = false;
                        }

                        if (isOk) {
                            break;
                        }

                        addRange++;
                    }

                    if (!listAnswers.Contains(ans)) {
                        listAnswers.Add(ans);
                    }

                    if (listAnswers.Count == 4) {
                        break;
                    }
                }

                listAnswers.Sort();

                answers = listAnswers.ToArray();

                Array.Sort(answers);

                for (int i = 0; i < 4; i++) {
                    reponses[i].fontSize = 90;
                    reponses[i].text = answers[i].ToString();
                }
            } else {
                reponses[0].text = "+";
                reponses[0].fontSize = 130;
                reponses[1].text = "-";
                reponses[1].fontSize = 130;
                reponses[2].text = "x";
                reponses[3].text = "÷";
            }
        }


        public void OnClicked(Text text) {
            int myAnswer;

            bool isMaybeOperator = text.text.Length <= 1;

            if (text.text.Contains("+") && isMaybeOperator)
                myAnswer = 0;
            else if (text.text.Contains("-") && isMaybeOperator)
                myAnswer = 1;
            else if (text.text.Contains("x") && isMaybeOperator)
                myAnswer = 2;
            else if (text.text.Contains("÷") && isMaybeOperator)
                myAnswer = 3;
            else
                myAnswer = int.Parse(text.text);

            if (correctAnswer == myAnswer) {
                _score++;

                if (_score % 5 == 0) {
                    level++;
                    levelText.text = "Level " + level.ToString();
                }

                pointsText.text = _score.ToString();

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


        private int GetResult(int n1, int n2, Operator oper) {
            switch(oper) {
                case Operator.plus:
                    return n1 + n2;
                case Operator.substract:
                    return n1 - n2;
                case Operator.multi:
                    return n1 * n2;
                case Operator.devide:
                    return n1 / n2;
                default:
                    return 0;
            }
        }

        private string GetOperator(Operator oper) {
            switch (oper) {
                case Operator.plus:
                    return "+";
                case Operator.substract:
                    return "-";
                case Operator.multi:
                    return "x";
                case Operator.devide:
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

            ChooseOperator();
        }

        public static string[] RandomizeStrings(string[] arr) {
            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
            // Add all strings from array
            // Add new random int each time
            foreach (string s in arr) {
                list.Add(new KeyValuePair<int, string>(_random.Next(), s));
            }
            // Sort the list by the random number
            var sorted = from item in list
                         orderby item.Key
                         select item;
            // Allocate new string array
            string[] result = new string[arr.Length];
            // Copy values to array
            int index = 0;
            foreach (KeyValuePair<int, string> pair in sorted) {
                result[index] = pair.Value;
                index++;
            }
            // Return copied array
            return result;
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

        // <summary>
        /// If using Very Simple Leaderboard by App Advisory, report the score : http://u3d.as/qxf
        /// </summary>
        void ReportScoreToLeaderboard(int p) {
#if APPADVISORY_LEADERBOARD
			LeaderboardManager.ReportScore(p);
#else
            //print("Get very simple leaderboard to use it : http://u3d.as/qxf");
#endif
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
                Debug.LogWarning("To show ads, please have a look to Very Simple Ad on the Asset Store, or go to this link: " + VerySimpleAdsURL);
                Debug.LogWarning("Very Simple Ad is already implemented in this asset");
                Debug.LogWarning("Just import the package and you are ready to use it and monetize your game!");
                Debug.LogWarning("Very Simple Ad : " + VerySimpleAdsURL);
                PlayerPrefs.SetInt("GAMEOVER_COUNT", 0);
            } else {
                PlayerPrefs.SetInt("GAMEOVER_COUNT", count);
            }
            PlayerPrefs.Save();
#endif
        }
    }

}