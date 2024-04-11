using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI TimeLable;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject resetButton;
    private bool isTiming;
    private float time;

    void OnEnable()
    {
        resetButton.SetActive(false);
    }

    // Handles the timer and label
    void FixedUpdate()
    {
        if (isTiming)
        {
            time += Time.deltaTime;
            string timeString = time.ToString();
            int index = time.ToString().IndexOf(".");
            TimeLable.text = "Time: " + timeString.Substring(0, index + 2);
        }
    }

    // Starts keeping track of time
    public void StartTimer()
    {
        time = 0f;
        isTiming = true;
        resetButton.SetActive(true);
        startButton.SetActive(false);
    }
    
    // Stops the timer
    public void StopTimer()
    {
        isTiming = false;
    }

    // Resets the timer
    public void ResetTimer()
    {
        resetButton.SetActive(false);
        startButton.SetActive(true);
        time = 0f;
        isTiming = false;
        TimeLable.text = "Time: 0.0";
    }

}
