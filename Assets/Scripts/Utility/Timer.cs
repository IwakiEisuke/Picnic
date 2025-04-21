using System;
using UnityEngine;

public class Timer
{
    public event Action TimeUp;

    float targetTime;
    float elapsedTime;
    int repeatCount;
    bool canRepeat;

    public bool IsActive { get; private set; }
    public float ProgressRatio { get; private set; }

    public void Set(float t, bool repeat = false, int count = 1)
    {
        targetTime = t;
        IsActive = true;
        canRepeat = repeat;
        repeatCount = count;
    }

    public void Start() => IsActive = true;
    public void Pause() => IsActive = false;
    public void Stop() { IsActive = false; elapsedTime = 0; }

    public void Update()
    {
        elapsedTime += Time.deltaTime;
        ProgressRatio = Mathf.Clamp01(elapsedTime / targetTime);

        if (elapsedTime >= targetTime)
        {
            TimeUp.Invoke();
            IsActive = Continue();
        }
    }

    private bool Continue()
    {
        if (canRepeat && repeatCount > 0) 
        {
            repeatCount--;
            return true;
        }

        return false;
    }
}
