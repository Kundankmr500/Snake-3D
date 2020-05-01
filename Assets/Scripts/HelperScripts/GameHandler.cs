using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance;
    public GameObject BombObj;
    public GameObject GameOverPanel;
    public float MinX;
    public float MaxX;
    public float MinY;
    public float MaxY;
    public float ZPos;
    [HideInInspector]
    public bool CanPlayGame;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI StreakText;
    public List<Fruit> Fruits = new List<Fruit>();
    
    
    private int scoreCount;
    private int streakCount;
    private List<string> streakList = new List<string>();
    
    
    
    // Start is called before the first frame update
    void Awake()
    {
        CreateInstance();
    }

    private void Start()
    {
        Invoke("StartSpawning",2f);
    }

    void CreateInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void StartSpawning()
    {
        CanPlayGame = true;
        streakCount = 1;
        StartCoroutine(SpawnPickUps());
    }

    void CancelSpawning()
    {
        CancelInvoke("StartSpawning");
        StopAllCoroutines();
    }

    IEnumerator SpawnPickUps()
    {
        yield return new WaitForSeconds(Random.Range(1f, 1.5f));

        if (Random.Range(0, 10) >= 2)
        {
            Instantiate(Fruits[Random.Range(0, Fruits.Count)].FruitObj, 
                new Vector3(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY), ZPos),
                        Quaternion.identity);
        }
        else
        {
            Instantiate(BombObj, 
                new Vector3(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY), ZPos),
                Quaternion.identity);
        }
        Invoke("StartSpawning",0);
    }
    
    public void CheckStreakValue(string fruitTag)
    {
        if (streakList.Count != 0 && streakList[streakList.Count - 1] == fruitTag)
        {
            streakList.Add(fruitTag);
        }
        else
        {
            streakList.Clear();
            streakList.Add(fruitTag);
        }
    }

    public void CalculateScore(int fruitPoint)
    {
        streakCount = streakList.Count;
        scoreCount += fruitPoint * streakCount;
        ScoreText.text = Constants.SCORE + scoreCount;
        StreakText.text = Constants.STREAK + streakCount;
        if (scoreCount > PlayerPrefs.GetInt("BestScore"))
            SetBestScore(scoreCount);
    }

    public void SetBestScore(int bestScore)
    {
        PlayerPrefs.SetInt("BestScore", bestScore);
    }
    

    public void GameOver()
    {
        CanPlayGame = false;
        CancelSpawning();
        GameOverPanel.SetActive(true);
        AudioManager.Instance.PlayDeadSound();
        StartCoroutine(GoToMenuScene());
    }

    IEnumerator GoToMenuScene()
    {
        yield return new WaitForSeconds(5f);
        GetComponent<MenuScript>().GoToNextScene(0);
    }
    
}
