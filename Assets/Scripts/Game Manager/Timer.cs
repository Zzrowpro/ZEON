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
           pointsPerSecond = math.clamp(pointsPerSecond, 1, 5);
        }
    }
    float rate = 1f/10f;

    void Update()
    {
        pointsPerSecond += rate * Time.deltaTime;
        elapsedtime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedtime/60);
        int seconds = Mathf.FloorToInt(elapsedtime%60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (scoreText)
        {
            scoreText.text = $"{(pointsPerSecond)}x : {MathF.Ceiling(DataManager.instance.points)}";   
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
        if(pointsPerSecond == 1f) scoreText.color = new Color(150,150,150); 
        else if(pointsPerSecond >= 2f) scoreText.color = new Color(30,100,255);
        else if(pointsPerSecond >= 3f) scoreText.color = new Color(50,205,50);
        else if(pointsPerSecond >= 4f) scoreText.color = new Color(255,140,0);
        else scoreText.color = new Color(255,30,30);
    }
}