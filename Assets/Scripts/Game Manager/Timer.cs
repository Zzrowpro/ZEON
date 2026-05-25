using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour 
{
    [SerializeField] TextMeshProUGUI timerText;

    private float elapsedtime; 
    public float Elapsedtime
    {
        get{return elapsedtime;}
    }

    void Update()
    {
        elapsedtime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedtime/60);
        int seconds = Mathf.FloorToInt(elapsedtime%60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); 
    }
}