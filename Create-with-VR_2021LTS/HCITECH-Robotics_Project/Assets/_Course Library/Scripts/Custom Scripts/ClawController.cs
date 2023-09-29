using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawController : MonoBehaviour
{
    [SerializeField] GameObject hand;
    [SerializeField] GameObject ghostArm;
    [SerializeField] GameObject finger1;
    [SerializeField] GameObject ghostFinger1;
    [SerializeField] GameObject resetFinger1;
    [SerializeField] GameObject finger2;
    [SerializeField] GameObject ghostFinger2;
    [SerializeField] GameObject resetFinger2;
    [SerializeField] GameObject finger3;
    [SerializeField] GameObject ghostFinger3;
    [SerializeField] GameObject resetFinger3;
    private GameObject robot;
    private float speed = 20f;
    private float maxAngleDelta = 59f;
    private float maxAngle = 0f;
    private float minAngle = 0f;

    // Defaults forselection activation
    private bool isSelected = false;
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
            if(ghostFinger1.transform.localEulerAngles.z < maxAngle)
            {
                ghostFinger1.transform.Rotate(0f, 0f, speed * Time.deltaTime);
                ghostFinger2.transform.Rotate(0f, 0f, speed * Time.deltaTime);
                ghostFinger3.transform.Rotate(0f, 0f, speed * Time.deltaTime);
            }
            
        } else
        {
            if(ghostFinger1.transform.localEulerAngles.z >= minAngle)
            {
                ghostFinger1.transform.Rotate(0f, 0f, speed* -Time.deltaTime);
                ghostFinger2.transform.Rotate(0f, 0f, speed * -Time.deltaTime);
                ghostFinger3.transform.Rotate(0f, 0f, speed * -Time.deltaTime);
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
            resetFinger1.SetActive(true);
            resetFinger2.SetActive(true);
            resetFinger3.SetActive(true);
            ghostFinger1.transform.localPosition = resetFinger1.transform.localPosition;
            ghostFinger1.transform.localRotation = resetFinger1.transform.localRotation;
            target1 = ghostFinger1;
            ghostFinger2.transform.localPosition = resetFinger2.transform.localPosition;
            ghostFinger2.transform.localRotation = resetFinger2.transform.localRotation;
            target2 = ghostFinger2;
            ghostFinger3.transform.localPosition = resetFinger3.transform.localPosition;
            ghostFinger3.transform.localRotation = resetFinger3.transform.localRotation;
            target3 = ghostFinger3;
            GhostArmDeactivation(true);
            resetFinger1.SetActive(false);
            resetFinger2.SetActive(false);
            resetFinger3.SetActive(false);
            // Once the robot is reset, no need to allow the user to reset again
            robot.GetComponent<RobotController>().resetReady = false;
        }
        else
        {
            target1 = ghostFinger1;
            target2 = ghostFinger2;
            target3 = ghostFinger3;
            // Once the robot is somewhere else, it can be reset
            robot.GetComponent<RobotController>().resetReady = true;
        }
        // Send the target data at the beginning of the movement
        /*firebase.SendMovementData(target.transform.localRotation.x,
                                  target.transform.localRotation.y,
                                  target.transform.localRotation.z,
                                  gameObject.name);
        */
        // While the target's rotation is not the same as the current pivot's
        // continue to rotate the pivot towards that direction
        while (target1.transform.localRotation != finger1.transform.localRotation)
        {
            float step = speed * Time.deltaTime;
            // Rotate the position of the pivot in relation of the ghost arm OR the invisible reset positions
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
        // Stop the current routine and tell the UI that the robot is ready to move!
        robot.GetComponent<RobotController>().moveReady = true;
        GhostArmDeactivation(false);
        data.Add("end_time", System.DateTime.UtcNow);
        firebase.SendTimeData(data);
        StopCoroutine(MovePivot(reset));
    }
}
