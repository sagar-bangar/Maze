using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("UI Settings")]
    public TMP_Text timerText;


    private float currentTime;
    private bool isTimerRunning;

    void Start()
    {
        StartTimer();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            currentTime += Time.deltaTime;
            UpdateUIText();
        }
    }

    void UpdateUIText()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + FormatTime(currentTime);
        }
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void ResetTimer()
    {
        currentTime = 0f;
        UpdateUIText(); 
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
