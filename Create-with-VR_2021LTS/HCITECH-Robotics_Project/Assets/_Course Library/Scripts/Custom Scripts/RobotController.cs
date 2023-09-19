using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    // <joint> <joint name/references>
    [SerializeField] GameObject basePivot;
    [SerializeField] GameObject segmentOnePivot;
    [SerializeField] GameObject segmentTwoPivot;
    [SerializeField] GameObject segmentThreePivot;
    [SerializeField] GameObject segmentFourPivot;
    [SerializeField] GameObject segmentFivePivot;
    // Okay so this is going to be janky and might not work correctly but
    // Kinova's back end API should handle this so don't worry too much about it
    public bool moveReady = true;

    // Call one function instead of 5 different functions
    // upon the submit move button click
    public void MoveRobot()
    {
        moveReady = false;
        basePivot.GetComponent<PivotController>().TranslatePivot(false);
        segmentOnePivot.GetComponent<PivotController>().TranslatePivot(false);
        segmentTwoPivot.GetComponent<PivotController>().TranslatePivot(false);
        segmentThreePivot.GetComponent<PivotController>().TranslatePivot(false);
        segmentFourPivot.GetComponent<PivotController>().TranslatePivot(false);
        segmentFivePivot.GetComponent<PivotController>().TranslatePivot(false);
    }
    // Method for resetting the robot
    // The hardcoded true value means that it will reset to origin
    public void ResetRobot() {
        moveReady = false;
        basePivot.GetComponent<PivotController>().TranslatePivot(true);
        segmentOnePivot.GetComponent<PivotController>().TranslatePivot(true);
        segmentTwoPivot.GetComponent<PivotController>().TranslatePivot(true);
        segmentThreePivot.GetComponent<PivotController>().TranslatePivot(true);
        segmentFourPivot.GetComponent<PivotController>().TranslatePivot(true);
        segmentFivePivot.GetComponent<PivotController>().TranslatePivot(true);
    }
}