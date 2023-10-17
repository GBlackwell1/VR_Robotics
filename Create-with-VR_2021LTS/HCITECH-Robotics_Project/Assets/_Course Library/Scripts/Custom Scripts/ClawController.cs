using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ClawController : MonoBehaviour
{
    [SerializeField] GameObject hand;
    [SerializeField] GameObject ghostArm;
    [SerializeField] GameObject finger1;
    [SerializeField] GameObject ghostFinger1;
    [SerializeField] GameObject finger2;
    [SerializeField] GameObject ghostFinger2;
    [SerializeField] GameObject finger3;
    [SerializeField] GameObject ghostFinger3;
    private GameObject robot;
    private float speed = 20f;
    private float maxAngleDelta = 59f;
    private float maxAngle = 0f;
    private float minAngle = 0f;

    // Defaults forselection activation
    private bool isSelected = false;
    private bool isClosed = false;
    private bool isMoving = false;
    private String start = "open";
    private FirebaseScript firebase;

    // Start is called before the first frame update
    void Start()
    {
        maxAngle = finger1.transform.localEulerAngles.z + maxAngleDelta;
        minAngle = finger1.transform.localEulerAngles.z;
        robot = GameObject.Find("Robot Arm - New");
        firebase = GameObject.Find("FIREBASE").GetComponent<FirebaseScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            // If the claw is open and has been selected, close it
            if(ghostFinger1.transform.localEulerAngles.z < maxAngle)
            {
                isMoving = true;
                ghostFinger1.transform.Rotate(0f, 0f, speed * Time.deltaTime);
                ghostFinger2.transform.Rotate(0f, 0f, speed * Time.deltaTime);
                ghostFinger3.transform.Rotate(0f, 0f, speed * Time.deltaTime);
                robot.GetComponent<RobotController>().moveReady = false;
            } else // If the claw is closed and has been selected, do nothing
            {
                if(isMoving)
                {
                    // Before the claw enables movement, it checks to make sure it is in a new position,
                    // or that some other part of the robot has been moved
                    if(start == "closed" && !robot.GetComponent<RobotController>().nonClawMove)
                    {
                        robot.GetComponent<RobotController>().submitReady = false;
                        GhostArmDeactivation(false);
                    }
                    robot.GetComponent<RobotController>().moveReady = true;
                    isMoving = false;
                }
                isClosed = true;
            }
            
        } else
        {
            // If the claw is closed and has not been selected, open it
            if (ghostFinger1.transform.localEulerAngles.z >= minAngle)
            {
                isMoving= true;
                ghostFinger1.transform.Rotate(0f, 0f, speed* -Time.deltaTime);
                ghostFinger2.transform.Rotate(0f, 0f, speed * -Time.deltaTime);
                ghostFinger3.transform.Rotate(0f, 0f, speed * -Time.deltaTime);
                robot.GetComponent<RobotController>().moveReady = false;
            }
            else // If the claw is open and has not been selected, do nothing
            {
                if (isMoving)
                {
                    // Before the claw enables movement, it checks to make sure it is in a new position,
                    // or that some other part of the robot has been moved
                    if (start == "open" && !robot.GetComponent<RobotController>().nonClawMove)
                    {
                        robot.GetComponent<RobotController>().submitReady = false;
                        GhostArmDeactivation(false);
                    }
                    robot.GetComponent<RobotController>().moveReady = true;
                    isMoving = false;
                }
                isClosed = false;
            }
        }
    }

    // Enable and disable the view of the GhostArm
    public void GhostArmActivation()
    {
        ghostArm.SetActive(true);
        // The user can submit a move once they've moved the ghost robot
        robot.GetComponent<RobotController>().submitReady = true;
    }

    // If the position of the ghost arm is not the same as the visible arm
    // do not deactivate it, leave it active until the positions are the same
    public void GhostArmDeactivation(bool isReset)
    {
        // This feature is disabled for the base since the base of both robots 
        // should always be in the same position
        if (!isReset)
        {
            // Check for rotation since eccentric pivots will have same position
            ghostArm.SetActive(false);
            robot.GetComponent<RobotController>().moveReady = true;
            // User does not get to submit a move to the robot when the ghost arm is deactivated
            robot.GetComponent<RobotController>().submitReady = false;
        }
        else { ghostArm.SetActive(false); }

    }
    // Only run the update function if something is selected
    public void SelectionHandler()
    {
        isSelected = !isSelected;
    }
    // Method that's called when needed to move the robot arms' pivots
    // boolean here determines whether or not the submit move button
    // or the reset button has been hit
    public void TranslatePivot(bool reset)
    {
        // This check to see if pivot points are equal
        if (ghostFinger1.transform.rotation != finger1.transform.rotation || reset)
        {
            StartCoroutine(MovePivot(reset));
        }
        else
        {
            robot.GetComponent<RobotController>().moveReady = true;
        }
    }

    IEnumerator MovePivot(bool reset)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("start_time", System.DateTime.UtcNow);
        GameObject target1;
        GameObject target2;
        GameObject target3;

        // So check if the button wants us to reset, if so then assign target
        // To reset position else assign it to the regular ghost arm pivot
        if (reset)
        {
            if(isClosed)
            {
                GhostArmDeactivation(true);
                robot.GetComponent<RobotController>().moveReady = false;
                while (finger1.transform.localEulerAngles.z >= minAngle)
                {
                    finger1.transform.Rotate(0f, 0f, speed * -Time.deltaTime);
                    finger2.transform.Rotate(0f, 0f, speed * -Time.deltaTime);
                    finger3.transform.Rotate(0f, 0f, speed * -Time.deltaTime);
                    robot.GetComponent<RobotController>().moveReady = false;
                    yield return null;
                }
                isClosed = false;
                GhostArmActivation();
                SelectionHandler();
                ghostFinger1.transform.localPosition = finger1.transform.localPosition;
                ghostFinger1.transform.localRotation = finger1.transform.localRotation;
                ghostFinger2.transform.localPosition = finger2.transform.localPosition;
                ghostFinger2.transform.localRotation = finger2.transform.localRotation;
                ghostFinger3.transform.localPosition = finger3.transform.localPosition;
                ghostFinger3.transform.localRotation = finger3.transform.localRotation;
            }
            robot.GetComponent<RobotController>().resetReady = false;
            start = "open";
        }
        else
        {
            target1 = ghostFinger1;
            target2 = ghostFinger2;
            target3 = ghostFinger3;
            // Once the robot is somewhere else, it can be reset
            robot.GetComponent<RobotController>().resetReady = true;
            // While the target's rotation is not the same as the current pivot's
            // continue to rotate the pivot towards that direction
            while (target1.transform.localRotation != finger1.transform.localRotation)
            {
                float step = speed * Time.deltaTime;
                finger1.transform.localRotation =
                    Quaternion.RotateTowards(finger1.transform.localRotation, target1.transform.localRotation, step);
                finger2.transform.localRotation =
                    Quaternion.RotateTowards(finger2.transform.localRotation, target2.transform.localRotation, step);
                finger3.transform.localRotation =
                    Quaternion.RotateTowards(finger3.transform.localRotation, target3.transform.localRotation, step);
                robot.GetComponent<RobotController>().moveReady = false;
                yield return null;
                if (!ghostArm.activeInHierarchy && !reset)
                {
                    GhostArmActivation();
                }

            }
            if(isClosed)
            {
                start = "closed";
            } else
            {
                start = "open";
            }
            
        }
        // Stop the current routine and tell the UI that the robot is ready to move!
        robot.GetComponent<RobotController>().moveReady = true;
        robot.GetComponent<RobotController>().nonClawMove = false;
        GhostArmDeactivation(false);
        data.Add("end_time", System.DateTime.UtcNow);
        firebase.SendTimeData(data);
        StopCoroutine(MovePivot(reset));
    }
}
