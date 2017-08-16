﻿
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
using ElevenGameStudio.MathFrenzy;

namespace MenuBarouch {

    public class MenuManager : MonoBehaviour {
        public GameObject MENU;

        public GameObject GAME;

        public GameObject SETTING;

        public Image BACKGROUND_BACK;

        public Color NORMAL_COLOR;

        void Awake() {
            MENU.SetActive(true);
            GAME.SetActive(false);
            SETTING.SetActive(false);
            SetSoundState();
        }

        /// <summary>
        /// Set sound state
        /// </summary>
        public void SetSoundState() {
            if (!PlayerPrefsX.GetBool(Utils.MUTED_PREF)) {
                AudioListener.volume = 1;
            } else {
                AudioListener.volume = 0;
            }
        }

        //open the game
        public void GoToGame() {
            float time = 0.2f;
            GoOut(MENU, time, 0);
            GoIn(GAME, time, time);
        }

        //open the menu
        public void GoToMenu() {
            float time = 0.2f;
            GoOut(GAME, time, 0);
            GoIn(MENU, time, time);
        }

        //open the setting menu
        public void OpenSettings() {
            float time = 0.2f;
            GoOut(MENU, time, 0);
            GoIn(SETTING, time, time);
        }

        //close the setting menu
        public void CloseSettings() {
            float time = 0.2f;
            GoOut(SETTING, time, 0);
            GoIn(MENU, time, time);
        }

        void Update() {
            BACKGROUND_BACK.color = Color.Lerp(BACKGROUND_BACK.color, NORMAL_COLOR, Time.time);

#if UNITY_ANDROID
            if (Input.GetKey(KeyCode.Escape)) {
                if (MENU.activeSelf) {
                    FindObjectOfType<ButtonExit>().OnClicked();
                }
            }
#endif
        }

        //animation scale from 1 to 0
        public void GoOut(GameObject obj, float time, float delay) {
            obj.transform.localScale = Vector3.one;
            StartCoroutine(GoInOrOutCorout(obj, 0, time, delay, () => {
                obj.transform.localScale = Vector3.zero;
                obj.SetActive(false);
            }));

        }

        //animation scale from 0 to 1
        public void GoIn(GameObject obj, float time, float delay) {
            obj.transform.localScale = Vector3.zero;
            StartCoroutine(GoInOrOutCorout(obj, 1, time, delay, () => {
                obj.transform.localScale = Vector3.one;
                obj.SetActive(true);
            }));

        }

        //do the animation scale
        IEnumerator GoInOrOutCorout(GameObject obj, float scale, float time, float delay, Action callback) {
            obj.SetActive(true);

            yield return new WaitForSeconds(delay);

            var originalScale = obj.transform.localScale;
            var targetScale = Vector3.one * scale;
            var originalTime = time;

            while (time > 0.0f) {
                time -= Time.deltaTime;
                obj.transform.localScale = Vector3.Lerp(targetScale, originalScale, time / originalTime);
                yield return 0;
            }

            if (callback != null)
                callback();
        }
    }
}