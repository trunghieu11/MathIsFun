using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ElevenGameStudio.social;

public class OpenLeaderboard : MonoBehaviour 
{
	void Awake()
	{
		GetComponent<Button>().onClick.AddListener(OnClicked);
	}

	void OnClicked()
	{
		LeaderboardManager.ShowLeaderboardUI();
	}
}
