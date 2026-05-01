using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimerScript : MonoBehaviour
{
    public Text timerText;
    private float timeRemaining = 600f;
    public Color normalColor = Color.white;   
    public Color warningColor = Color.red;    

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 30f)
            {
                timerText.color = warningColor;
            }
            else
            {
                timerText.color = normalColor;
            }

            UpdateTimerText();
        }
        else
        {
            timeRemaining = 0;
            timerText.color = normalColor; 
            GameManager.instance.EndGame();
        }
    }

    private void UpdateTimerText()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeRemaining);
        timerText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }
}
