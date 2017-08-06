
/***********************************************************************************************************
 * Produced by App Advisory - http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/




using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MenuBarouch;

namespace ElevenGameStudio.MathFrenzy
{
	public class ButtonShare : ButtonHelper 
	{
        public string shareURL;

		override public void OnClicked()
		{
            if (Application.isEditor) {
                //#if UNITY_EDITOR
                //				if(UnityEditor.EditorUtility.DisplayDialog("********** Attention! **********","Very Simple Share doesn't work in the editor.","OK"))
                //				{
                //					Invoke("OnClickedButtonCloseShareWindow",1f);
                //				}
                //#endif
                Debug.LogWarning("********** Share doesn't work in the editor. **********");
                return;
            }

#if UNITY_ANDROID
            // Create Refernece of AndroidJavaClass class for intent
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");

            // Create Refernece of AndroidJavaObject class intent
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

            // Set action for intent
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            intentObject.Call<AndroidJavaObject>("setType", "text/plain");

            //Set Subject of action
            //intentObject.Call("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "Text Sharing ");

            //Set title of action or intent
            //intentObject.Call("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), "Text Sharing ");

            // Set actual data which you want to share
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareURL);
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            // Invoke android activity for passing intent to share data
            AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share");
            currentActivity.Call("startActivity", jChooser);
#endif
        }
    }
}