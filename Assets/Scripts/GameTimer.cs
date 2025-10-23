using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text clockText; 
    [SerializeField] private float realDuration = 300f;
    [SerializeField] GameObject winScreen;
    [HideInInspector] public bool hasWon = false;

    private float timer;
    float multiplicator;
    bool multiplicatorBool;
    bool multiplicatorTempBool;
    [SerializeField] TMP_Text _multiplicatorText;

    [SerializeField] float intervaloMulti = 10;
    float temporizadorMulti = 0;

    [SerializeField] Button _multiplicadorButton;
    [SerializeField] GameObject _blockCounter;
    [SerializeField] TMP_Text _multiplicadorCounterText;

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
            if (!multiplicatorBool)
            {
                _multiplicatorText.color = Color.gray;
                timer += Time.deltaTime;
            }
            else if (multiplicatorBool && multiplicator > 0)
            {
                _multiplicatorText.color = Color.cyan;
                timer += Time.deltaTime * multiplicator;
                _multiplicatorText.text = " X" + multiplicator;
                TextPulse();
            }

            MultiplicatorTimer();
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

    void MultiplicatorTimer()
    {
        temporizadorMulti += Time.deltaTime;
        int segundos = Mathf.FloorToInt(temporizadorMulti);
        _multiplicadorCounterText.text = $"{segundos}";

        if (temporizadorMulti >= intervaloMulti && !multiplicatorTempBool)
        {
            multiplicatorTempBool = true;
            _blockCounter.SetActive(false);
            temporizadorMulti = 0f;
        }
        else if (temporizadorMulti > 0)
        {
            //temporizadorMulti = 0f;
        }
    }
    public void activeMultiplicator()
    {
        if (!multiplicatorTempBool) return;

        StartCoroutine(MultiplicatorTime());
    }

    IEnumerator MultiplicatorTime()
    {
        multiplicatorBool = true;
        temporizadorMulti = 0f;

        _multiplicadorCounterText.color = Color.cyan;

        _blockCounter.SetActive(true);

        yield return new WaitForSeconds(10);

        multiplicatorBool = false;

        _multiplicadorCounterText.color = Color.white;
        temporizadorMulti = 0f;
        multiplicatorTempBool = false;
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