using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    // <joint> <joint name/references>
    [SerializeField] GameObject jBase;
    [SerializeField] GameObject jBox;
    [SerializeField] GameObject jEPivotTwo;
    [SerializeField] GameObject jHandControl;
    [SerializeField] GameObject jEPivot;
    // Okay so this is going to be janky and might not work correctly but
    // Kinova's back end API should handle this so don't worry too much about it
    public bool moveReady = true;
    
    // Call one function instead of 5 different functions
    // upon the submit move button click
    public void MoveRobot()
    {
        moveReady = false;
        jBase.GetComponent<PivotController>().TranslatePivot(false);
        jBox.GetComponent<PivotController>().TranslatePivot(false);
        jEPivotTwo.GetComponent<PivotController>().TranslatePivot(false);
        jHandControl.GetComponent<PivotController>().TranslatePivot(false);
        jEPivot.GetComponent<PivotController>().TranslatePivot(false);
    }
    // Method for resetting the robot
    // The hardcoded true value means that it will reset to origin
    public void ResetRobot() {
        moveReady = false;
        jBase.GetComponent<PivotController>().TranslatePivot(true);
        jBox.GetComponent<PivotController>().TranslatePivot(true);
        jEPivotTwo.GetComponent<PivotController>().TranslatePivot(true);
        jHandControl.GetComponent<PivotController>().TranslatePivot(true);
        jEPivot.GetComponent<PivotController>().TranslatePivot(true);
    }
}