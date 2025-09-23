using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text clockText; 
    [SerializeField] private float realDuration = 300f;
    [SerializeField] GameObject winScreen;
    private bool hasWon = false;

    private float timer;

    void Start()
    {
        winScreen.SetActive(false);
        timer = 0f;
    }

    void Update()
    {
        if (!hasWon)
        {
            timer += Time.deltaTime;
            UpdateClock();

            if (timer >= realDuration) WinGame();
        }
    }

    void UpdateClock()
    {
        float progress = timer / realDuration;

        float gameHour = Mathf.Lerp(1f, 6f, progress);

        int hour = Mathf.FloorToInt(gameHour);

        int minute = Mathf.FloorToInt((gameHour - hour) * 60f);

        clockText.text = string.Format("{0:00}:{1:00} AM", hour, minute);
    }


    void WinGame()
    {
        GameManager.Instance.EndGame2();
        winScreen.SetActive(true);  
        //Time.timeScale = 0f;
        hasWon = true;
    }
}