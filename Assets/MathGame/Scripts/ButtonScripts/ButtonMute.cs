using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppAdvisory.MathFrenzy {
    public class ButtonMute : ButtonHelper {

        public GameObject audioOnItem;
        public GameObject audioOffItem;

        void OnEnable() {
            if (!PlayerPrefsX.GetBool(Util.MUTED_PREF)) {
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
            if (PlayerPrefsX.GetBool(Util.MUTED_PREF)) {
                PlayerPrefsX.SetBool(Util.MUTED_PREF, false);
            } else {
                PlayerPrefsX.SetBool(Util.MUTED_PREF, true);
            }

            SetSoundState();
        }

        /// <summary>
        /// Set sound state
        /// </summary>
        public void SetSoundState() {
            if (!PlayerPrefsX.GetBool(Util.MUTED_PREF)) {
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