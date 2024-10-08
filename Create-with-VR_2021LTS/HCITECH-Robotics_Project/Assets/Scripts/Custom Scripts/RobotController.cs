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


    // LOG HERE with some way to detect whether we are working with the task or the 
    public void MoveRobot()
    {
        IDictionary<string, float> move = new Dictionary<string, float>();
        // Engineering code moment
        move.Add("joint1",
            basePivot.GetComponent<PivotController>().GetAngle());
        move.Add("joint2",
            segmentOnePivot.GetComponent<PivotController>().GetAngle(false));
        // Just needed to be negative for pivots 3 and up
        move.Add("joint3",
            -segmentTwoPivot.GetComponent<PivotController>().GetAngle(false));
        move.Add("joint4",
            -segmentThreePivot.GetComponent<PivotController>().GetAngle());
        move.Add("joint5",
            -segmentFourPivot.GetComponent<PivotController>().GetAngle());
        move.Add("joint6",
            -segmentFivePivot.GetComponent<PivotController>().GetAngle());
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