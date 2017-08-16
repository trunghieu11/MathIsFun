using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevenGameStudio.MathFrenzy {
    public class ButtonMute : ButtonHelper {

        public GameObject audioOnItem;
        public GameObject audioOffItem;

        void OnEnable() {
            if (!PlayerPrefsX.GetBool(Utils.MUTED_PREF)) {
                audioOnItem.SetActive(true);
                audioOffItem.SetActive(false);
            } else {
                audioOnItem.SetActive(false);
                audioOffItem.SetActive(true);
            }
        }

        /// <summary>
        /// Toggle button mute
        /// </summary>
        override public void OnClicked() {
            if (PlayerPrefsX.GetBool(Utils.MUTED_PREF)) {
                PlayerPrefsX.SetBool(Utils.MUTED_PREF, false);
            } else {
                PlayerPrefsX.SetBool(Utils.MUTED_PREF, true);
            }

            SetSoundState();
        }

        /// <summary>
        /// Set sound state
        /// </summary>
        public void SetSoundState() {
            if (!PlayerPrefsX.GetBool(Utils.MUTED_PREF)) {
                AudioListener.volume = 1;
                audioOnItem.SetActive(true);
                audioOffItem.SetActive(false);
            } else {
                AudioListener.volume = 0;
                audioOnItem.SetActive(false);
                audioOffItem.SetActive(true);
            }
        }
    }
}