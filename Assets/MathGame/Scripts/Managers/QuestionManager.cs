using System;
using System.Collections.Generic;
using UnityEngine;

namespace ElevenGameStudio.MathFrenzy {
    public class QuestionManager {
        private static Operator[] randomOperator = new Operator[] { Operator.PLUS, Operator.SUBSTRACT, Operator.MULTI, Operator.DEVIDE };

        public static Question GenerateQuestion(int level) {
            Operator choseOperator = randomOperator[UnityEngine.Random.Range(0, 4)];
            //Operator choseOperator = Operator.DEVIDE;

            int result = 0;
            int number1 = 0;
            int number2 = 0;

            int retry = 0;

            while (retry < 10) {
                retry++;

                // check this math is avaiable
                bool isOK = true;

                // choose number
                if (choseOperator == Operator.DEVIDE) {
                    int mult = UnityEngine.Random.Range(1 + level, 2 * level);
                    number2 = UnityEngine.Random.Range(1 + level, 3 + 2 * level);

                    number1 = mult * number2;
                } else if (choseOperator == Operator.SUBSTRACT) {
                    number2 = UnityEngine.Random.Range(1 + level, 5 + 3 * level);
                    number1 = UnityEngine.Random.Range(number2 + level, number2 + 5 + 3 * level);
                } else if (choseOperator == Operator.MULTI) {
                    number1 = UnityEngine.Random.Range(1 + level, 2 * level);
                    number2 = UnityEngine.Random.Range(1 + level, 3 + 2 * level);
                } else {
                    number1 = UnityEngine.Random.Range(1 + level, 5 + 3 * level);
                    number2 = UnityEngine.Random.Range(1 + level, 5 + 3 * level);
                }

                // get game result
                result = GetResult(number1, number2, choseOperator);

                // check if this game have only one result
                if (choseOperator == Operator.SUBSTRACT || choseOperator == Operator.DEVIDE) {
                    int resultDIV = 0;
                    int resultMINUS = 0;

                    resultDIV = number1 / number2;
                    resultMINUS = number1 - number2;

                    if (resultDIV == resultMINUS) {
                        isOK = false;
                    }
                }

                if (choseOperator == Operator.PLUS || choseOperator == Operator.MULTI) {
                    int resultMULT = 0;
                    int resultPLUS = 0;

                    resultMULT = number1 * number2;
                    resultPLUS = number1 + number2;

                    if (resultMULT == resultPLUS) {
                        isOK = false;
                    }
                }

                if (result <= 0) {
                    isOK = false;
                }

                // check operator devide is correct
                if (choseOperator == Operator.DEVIDE) {
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
                    if (choseOperator == Operator.DEVIDE) {
                        if (number1 == 0 || number1 == 1 || number2 == 0 || number2 == 1 || result == 0 || result == 1) {
                            isOK = false;
                        }
                    }
                }

                if (result <= 0 || number1 <= 0 || number2 <= 0) {
                    isOK = false;
                }

                if (result > 999) {
                    isOK = false;
                }


                //Check again!!!
                if (isOK) {
                    if (choseOperator == Operator.PLUS) {
                        int resultTest = number1 + number2;
                        if (resultTest != result) {
                            isOK = false;
                        }
                    }
                    if (choseOperator == Operator.SUBSTRACT) {
                        int resultTest = number1 - number2;
                        if (resultTest != result) {
                            isOK = false;
                        }
                    }
                    if (choseOperator == Operator.MULTI) {
                        int resultTest = number1 * number2;
                        if (resultTest != result) {
                            isOK = false;
                        }
                    }
                    if (choseOperator == Operator.DEVIDE) {
                        int resultTest = number1 / number2;
                        if (resultTest != result) {
                            isOK = false;
                        }
                    }

                }


                if (isOK) {
                    break;
                }
            }

            if (retry >= 10) {
                return new Question(1, 1, 2, Operator.PLUS);
            }

            Debug.Log(number1 + " " + number2 + " " + result + " " + choseOperator.ToString());
            return new Question(number1, number2, result, choseOperator);
        }

        private static int GetResult(int n1, int n2, Operator oper) {
            switch (oper) {
                case Operator.PLUS:
                    return n1 + n2;
                case Operator.SUBSTRACT:
                    return n1 - n2;
                case Operator.MULTI:
                    return n1 * n2;
                case Operator.DEVIDE:
                    return n1 / n2;
                default:
                    return 0;
            }
        }

        public static QuestionType RandomQuestionType() {
            int type = UnityEngine.Random.Range(0, 4);
            switch (type) {
                case 0:
                    return QuestionType.MISS_NUM_1;
                case 1:
                    return QuestionType.MISS_NUM_2;
                case 2:
                    return QuestionType.MISS_OPERATOR;
                case 3:
                    return QuestionType.MISS_RESULT;
            }
            return QuestionType.MISS_NUM_1;
        }

        public static string[] GenerateFakeAnswer(int correctAnswer) {
            string value = correctAnswer.ToString();
            List<string> result = new List<string> {
                value
            };
            while (result.Count < 4) {
                string x = FakeAnswer(value);
                if (!result.Contains(x)) {
                    result.Add(x);
                }
            }
            for (int i = 0; i < 10; i++) {
                int idx1 = UnityEngine.Random.Range(0, 4);
                int idx2 = UnityEngine.Random.Range(0, 4);
                string temp = result[idx1];
                result[idx1] = result[idx2];
                result[idx2] = temp;
            }
            return result.ToArray();
        }

        private static string FakeAnswer(string value) {
            int size = value.Length;
            int retry = 0;
            while (retry < 10) {
                retry++;
                char[] s = value.ToCharArray();
                int idx = UnityEngine.Random.Range(0, size);
                int diff = UnityEngine.Random.Range(1, Math.Max(2, 4 / size));
                int type = UnityEngine.Random.Range(0, 2);
                if (type == 0) {
                    s[idx] = (char)(s[idx] - diff);
                } else {
                    s[idx] = (char)(s[idx] + diff);
                }

                if ((s[idx] > '0' && s[idx] <= '9') || (s[idx] == '0' && idx > 0)) {
                    return new string(s);
                }
            }

            Debug.Log("Something wrong while create FakeAnswer");
            return "1";
        }
    }
}