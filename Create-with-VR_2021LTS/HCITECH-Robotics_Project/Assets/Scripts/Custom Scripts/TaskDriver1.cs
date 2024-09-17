using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskDriver1 : MonoBehaviour
{
    [SerializeField] GameObject timer;
    [SerializeField] GameObject cube;
    [SerializeField] SnappingVolume snappingVolume;
    [SerializeField] GameObject Firebase;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        initialPosition = cube.transform.position;
        initialRotation = cube.transform.rotation;

    }

    // Moves task items back to original location
    public void ResetTask()
    {
        snappingVolume.ReleaseCube();
        cube.transform.rotation = initialRotation;
        cube.transform.position = initialPosition;
    }

    // Detects when a task item has entered the box
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Task Item")
        {
            timer.GetComponent<Timer>().StopTimer();
            Firebase.GetComponent<FirebaseScript>().TaskLogTime("end_time");
        }
    }
}
