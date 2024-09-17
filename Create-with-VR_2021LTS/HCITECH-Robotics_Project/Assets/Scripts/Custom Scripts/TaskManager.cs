using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] GameObject TaskUI;
    [SerializeField] GameObject Timer;
    [SerializeField] GameObject Task1;
    [SerializeField] GameObject TaskInteractable1;
    [SerializeField] GameObject Firebase;
    private int currTask = 0;
    
    
    // Enables the first task
    public void enableTask1(bool enabled)
    {
        TaskUI.SetActive(!enabled);
        Timer.SetActive(enabled);
        Task1.SetActive(enabled);
        TaskInteractable1.SetActive(enabled);
        currTask = enabled ? 1 : 0;
        Firebase.GetComponent<FirebaseScript>().TaskLogTime("start_time");
    }

    // Resets the currently enabled task
    public void resetCurrentTask()
    {
        switch (currTask)
        {
            case 1:
                Task1.GetComponentInChildren<TaskDriver1>().ResetTask();
                break;
        }
    }
}
