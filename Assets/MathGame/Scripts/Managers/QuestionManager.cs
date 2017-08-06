namespace ElevenGameStudio.MathFrenzy {
    public class QuestionManager {
        private static Operator[] randomOperator = new Operator[] { Operator.PLUS, Operator.SUBSTRACT, Operator.MULTI, Operator.DEVIDE };

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
                if (choseOperator == Operator.DEVIDE) {
                    int mult = UnityEngine.Random.Range(2 + level, 2 + 2 * level);
                    number2 = UnityEngine.Random.Range(2 + level / 2, 3 + level);

                    number1 = mult * number2;
                } else if (choseOperator == Operator.SUBSTRACT) {
                    number2 = UnityEngine.Random.Range(1 + level / 2, 5 + level);
                    number1 = UnityEngine.Random.Range(number2 + level / 2, number2 + 5 + 2 * level);
                } else {
                    number1 = UnityEngine.Random.Range(1 + level / 2, 5 + 2 * level);
                    number2 = UnityEngine.Random.Range(1 + level / 2, 5 + 2 * level);
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
    }
}