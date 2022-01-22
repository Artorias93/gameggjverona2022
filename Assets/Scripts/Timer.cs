using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float startTime;
    private float duration;
    private float newDuration;
    private bool started = false;

    public Timer(float duration)
    {
        this.duration = duration;
        this.newDuration = duration;
    }

    //Starts the timer if it has not been started
    public void Start()
    {
        if(!started)
        {
            started = true;
            startTime = Time.time;
            duration = newDuration;
        }
    }

    public void SetDuration(float newDuration)
    {
        this.newDuration = newDuration;
    }

    public bool Started()
    {
        return started;
    }

    public bool HasExpired()
    {
        return started && Time.time >= startTime + duration;
    }

    public void Reset()
    {
        started = false;
    }
}
