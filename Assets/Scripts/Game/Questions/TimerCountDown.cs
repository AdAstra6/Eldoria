using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class TimerCountDown : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float totalTime;// Time in seconds
    public float criticalTime;

    [SerializeField] private GameObject timerObject;
    [SerializeField] private Image image;

    private float currentTime;
    private bool isRunning = false;

    public Action TimerEnd;

    public void Awake()
    {
        image = GetComponent<Image>();
    }


    void Start()
    {
        currentTime = totalTime;
        isRunning = true;
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;
        if (currentTime <= criticalTime) image.color = Color.red;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isRunning = false;
            image.color = Color.white;
            TimerEnd.Invoke();
            // You can call a method here, like: OnTimerEnd();
        }

        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    // Optional: Reset method
    public void Restart()
    {
        image.color = Color.white;
        currentTime = totalTime;
        isRunning = true;
    }

    public bool IsTimeFinished()
    {
        return !isRunning;
    }
    public void SetTotalTime(float time)
    {
        totalTime = time;
    }
}
