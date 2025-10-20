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
    float multiplicator;
    [SerializeField] TMP_Text _multiplicatorText;

    void Start()
    {
        winScreen.SetActive(false);
        timer = 0f;
    }

    void Update()
    {
        if (!hasWon)
        {
            multiplicator = Mathf.Clamp(GameManager.Instance.multiplicatorTime, 0, 4);
            if (multiplicator <= 1)
            {
                _multiplicatorText.color = Color.gray;
                timer += Time.deltaTime;
            }
            else
            {
                _multiplicatorText.color = Color.cyan;
                timer += Time.deltaTime * multiplicator;
                _multiplicatorText.text = " X" + multiplicator;
                TextPulse();
            }
            //tiempoFinal = realDuration * Mathf.Pow(2, GameManager.Instance.multiplicatorTime);
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

    private IEnumerator TextPulse()
    {
        float minSize = 25;
        float maxSize = 35;
        while (true)
        {
            float t = Mathf.PingPong(Time.time, 1f);
            _multiplicatorText.fontSize = Mathf.Lerp(minSize, maxSize, t);
            yield return null;
        }
    }
}