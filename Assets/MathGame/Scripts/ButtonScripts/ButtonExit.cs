using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ElevenGameStudio.MathFrenzy {
    public class ButtonExit : ButtonHelper {
        override public void OnClicked() {
            CloseGame();
        }

        /// <summary>
        /// Exit game when user click button exit
        /// </summary>
        public void CloseGame() {
            Debug.Log("Close games!!");
            Application.Quit();
        }
    }
}