﻿using System;
using UnityEngine;

public class Timer : IUtilityUpdate
{
    public event Action TimeUp;

    float targetTime;
    float elapsedTime;
    int repeatCount;
    bool canRepeat;

    public bool IsActive { get; private set; }
    public float ProgressRatio { get; private set; }

    public Timer Set(float t, bool repeat = false, int count = -1)
    {
        targetTime = t;
        IsActive = true;
        canRepeat = repeat;
        repeatCount = count;
        elapsedTime = 0;
        UtilityManager.Subscribe(this);
        return this;
    }

    public void Start() => IsActive = true;
    public void Pause() => IsActive = false;
    public void Cancel() { IsActive = false; elapsedTime = 0; UtilityManager.UnSubscribe(this); }

    public void Update()
    {
        if (!IsActive) return;

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
        if (canRepeat)
        {
            if (repeatCount < 0) return true;
            
            if (repeatCount > 0)
            {
                repeatCount--;
                return true;
            }
        }

        UtilityManager.UnSubscribe(this);
        return false;
    }
}
