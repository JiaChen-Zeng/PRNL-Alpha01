using System;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager INSTANCE { get; private set; }

    [SerializeField] private TextMeshProUGUI timerText;

    public float CurrentTime { get; private set; }
    private bool paused = true;

    TimeManager()
    {
        if (INSTANCE) throw new InvalidOperationException("TimeManagerのインスタンス一つのみ");

        INSTANCE = this;
    }

    private void Update()
    {
        if (!paused) UpdateTime();
    }

    public void StartTimer()
    {
        paused = false;
    }

    public void StopTimer()
    {
        paused = true;
    }

    public void ResetTimer()
    {
        CurrentTime = 0;
    }

    private void UpdateTime()
    {
        CurrentTime += Time.deltaTime;

        timerText.text = $"{(int)CurrentTime / 60:D2}:{(int)CurrentTime % 60:D2}";
    }
}
