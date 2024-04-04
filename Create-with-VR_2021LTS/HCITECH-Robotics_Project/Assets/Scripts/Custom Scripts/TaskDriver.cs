using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskDriver : MonoBehaviour
{

    public TextMeshProUGUI TimeLable;
    private bool isTiming;
    private float time;

    // Handles the timer and label
    void FixedUpdate()
    {
        if(isTiming)
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
    }

    // Detects when a task item has entered the box
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Task Item")
        {
            isTiming = false;
        }
    }
}
