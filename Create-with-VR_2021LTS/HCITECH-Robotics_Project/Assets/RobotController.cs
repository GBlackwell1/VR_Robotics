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
    // [SerializeField] GameObject jEPivot;
    // Okay so this is going to be janky and might not work correctly but
    // Kinova's back end API should handle this so don't worry too much about it
    public bool moveReady = true;
    
    // Call one function instead of 5 different functions
    // upon the submit move button click
    public void MoveRobot()
    {
        moveReady = false;
        jBase.GetComponent<PivotController>().TranslatePivot();
        jBox.GetComponent<PivotController>().TranslatePivot();
        jEPivotTwo.GetComponent<PivotController>().TranslatePivot();
        jHandControl.GetComponent<PivotController>().TranslatePivot();
        // jEPivot.GetComponent<PivotController>().TranslatePivot();
    }
    // Method for resetting the robot
    // The hardcoded true value means that it will reset to origin
    public void ResetRobot() {
        moveReady = false;
        jBase.GetComponent<PivotController>().TranslatePivot(true);
        jBox.GetComponent<PivotController>().TranslatePivot(true);
        jEPivotTwo.GetComponent<PivotController>().TranslatePivot(true);
        jHandControl.GetComponent<PivotController>().TranslatePivot(true);
    }
}