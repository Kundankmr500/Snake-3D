using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public int BestScoreCount;
    public TextMeshProUGUI BestScoreText;

    public void Start()
    {
        if (BestScoreText != null)
        {
            BestScoreCount = PlayerPrefs.GetInt("BestScore");
            BestScoreText.text = Constants.BESTSCORE + BestScoreCount;
        }
    }

    public void GoToNextScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}
