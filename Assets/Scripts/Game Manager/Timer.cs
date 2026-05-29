using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class Timer : MonoBehaviour 
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI scoreText;

    private float elapsedtime; 
    public float Elapsedtime
    {
        get{return elapsedtime;}
    }

    private float pointsPerSecond = 1f;
    public float PointsPerSecond
    {
        get{return pointsPerSecond;}
        set
        {
           pointsPerSecond = math.clamp(value, 1, 5);
        }
    }
    float rate = 1f/200f;

    void Update()
    {
        PointsPerSecond += rate * Time.deltaTime;
        elapsedtime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedtime/60);
        int seconds = Mathf.FloorToInt(elapsedtime%60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (scoreText)
        {
            scoreText.text = $"{Mathf.FloorToInt(pointsPerSecond)}x : {MathF.Ceiling(DataManager.instance.points)}";
        }
        changeColorBasedOnPoints();
        scalePoints(); 
    }


    private void scalePoints()
    {
       DataManager.instance.points += pointsPerSecond * Time.deltaTime;
    }

    private void changeColorBasedOnPoints()
{
    int level = Mathf.FloorToInt(pointsPerSecond);
    switch (level)
    {
        case 1:  scoreText.color = new Color(150/255f, 150/255f, 150/255f); break; // grey
        case 2:  scoreText.color = new Color( 30/255f, 100/255f, 255/255f); break; // blue
        case 3:  scoreText.color = new Color( 50/255f, 205/255f,  50/255f); break; // green
        case 4:  scoreText.color = new Color(255/255f, 140/255f,   0/255f); break; // orange
        default: scoreText.color = new Color(255/255f,  30/255f,  30/255f); break; // red (5+)
    }
}
}