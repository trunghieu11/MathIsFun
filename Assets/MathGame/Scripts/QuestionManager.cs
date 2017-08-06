using UnityEngine;
using UnityEditor;

namespace AppAdvisory.MathFrenzy {
    public class QuestionManager {
        private static Operator[] randomOperator = new Operator[] { Operator.plus, Operator.substract, Operator.multi, Operator.devide };

        public static Question GenerateQuestion(int level) {
            Operator choseOperator = randomOperator[UnityEngine.Random.Range(0, 4)];

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

                if (result > 999) {
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
                    break;
                }
            }

            return new Question(number1, number2, result, choseOperator);
        }

        private static int GetResult(int n1, int n2, Operator oper) {
            switch (oper) {
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
    }
}