using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
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
    [SerializeField] GameObject clawPivot;


    // Ghost arm joints
    [SerializeField] GameObject ghostBasePivot;
    [SerializeField] GameObject ghostSegmentOnePivot;
    [SerializeField] GameObject ghostSegmentTwoPivot;
    [SerializeField] GameObject ghostsSegmentThreePivot;
    [SerializeField] GameObject ghostSegmentFourPivot;
    [SerializeField] GameObject ghostSegmentFivePivot;
    [SerializeField] GameObject ghostClawPivot;
    private FirebaseScript ROSConnector;
    // Okay so this is going to be janky and might not work correctly but
    // Kinova's back end API should handle this so don't worry too much about it

    public bool moveReady = true;
    public bool submitReady = false;
    public bool resetReady = false;
    public bool nonClawMove = false;

    public void Start()
    {
        ROSConnector = GameObject.Find("FIREBASE").GetComponent<FirebaseScript>();
    }
    // Call one function instead of 5 different functions
    // upon the submit move button click
    public void MoveRobot()
    {
        IDictionary<string, float> move = new Dictionary<string, float>();
        // Engineering code moment
        move.Add("joint1",
            ghostBasePivot.transform.localEulerAngles.z);
        move.Add("joint2",
            ghostSegmentOnePivot.transform.localEulerAngles.x);
        move.Add("joint3",
            ghostSegmentTwoPivot.transform.localEulerAngles.x);
        move.Add("joint4",
            ghostsSegmentThreePivot.transform.localEulerAngles.z);
        move.Add("joint5",
            ghostSegmentFourPivot.transform.localEulerAngles.z);
        move.Add("joint6",
            ghostSegmentFivePivot.transform.localEulerAngles.z);
        ROSConnector.SendMovementData(move);
        moveReady = false;
        basePivot.GetComponent<PivotController>().TranslatePivot(false);
        segmentOnePivot.GetComponent<PivotController>().TranslatePivot(false);
        segmentTwoPivot.GetComponent<PivotController>().TranslatePivot(false);
        segmentThreePivot.GetComponent<PivotController>().TranslatePivot(false);
        segmentFourPivot.GetComponent<PivotController>().TranslatePivot(false);
        segmentFivePivot.GetComponent<PivotController>().TranslatePivot(false);
        clawPivot.GetComponent<ClawController>().TranslatePivot(false);
    }
    // Method for resetting the robot
    // The hardcoded true value means that it will reset to origin
    public void ResetRobot() {
        ROSConnector.CallHomeArm();
        moveReady = false;
        basePivot.GetComponent<PivotController>().TranslatePivot(true);
        segmentOnePivot.GetComponent<PivotController>().TranslatePivot(true);
        segmentTwoPivot.GetComponent<PivotController>().TranslatePivot(true);
        segmentThreePivot.GetComponent<PivotController>().TranslatePivot(true);
        segmentFourPivot.GetComponent<PivotController>().TranslatePivot(true);
        segmentFivePivot.GetComponent<PivotController>().TranslatePivot(true);
        clawPivot.GetComponent<ClawController>().TranslatePivot(true);
    }
}