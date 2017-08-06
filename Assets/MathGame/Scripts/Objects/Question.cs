using UnityEngine;
using UnityEditor;

namespace AppAdvisory.MathFrenzy {
    public class Question {
        public int Number1 { get; set; }
        public int Number2 { get; set; }
        public int Answer { get; set; }
        public Operator Oper { get; set; }

        public Question(int number1, int number2, int answer, Operator oper) {
            Number1 = number1;
            Number2 = number2;
            Answer = answer;
            Oper = oper;
        }

        // override object.Equals
        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }

            return Equals(obj as Question);
        }

        private bool Equals(Question obj) {
            return obj.Number1 == this.Number1 && obj.Number2 == this.Number2 && obj.Answer == this.Answer && obj.Oper == this.Oper;
        }

        // override object.GetHashCode
        public override int GetHashCode() {
            int hash = 11;
            hash = hash * 13 + Number1.GetHashCode();
            hash = hash * 13 + Number2.GetHashCode();
            hash = hash * 13 + Answer.GetHashCode();
            hash = hash * 13 + Oper.GetHashCode();
            return hash;
        }
    }
}