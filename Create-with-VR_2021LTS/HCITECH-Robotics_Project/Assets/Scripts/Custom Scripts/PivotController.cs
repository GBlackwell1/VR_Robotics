using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PivotController : MonoBehaviour
{
    private Dictionary<string, Vector3> reset_positions = new Dictionary<string, Vector3>();
    private Dictionary<string, Vector3> reset_locations = new Dictionary<string, Vector3>();
    [SerializeField] GameObject hand;
    [SerializeField] GameObject ghostArm;
    [SerializeField] GameObject ghostPivot;
    private GameObject robot;
    private float speed = 20f;
    private float rotationModifier;
    // Defaults forselection activation
    private bool isSelected = false;
    private FirebaseScript ROSConnector;
    private RobotManager robotManager;

    // Default robot starting positions
    // [SerializeField] GameObject ResetPostion;
    // Start is called before the first frame update
    void Start()
    {
        robot = GameObject.Find("Robot Arm - New");
        ROSConnector =  GameObject.Find("FIREBASE").GetComponent<FirebaseScript>();
        robotManager = GameObject.Find("ROBOTMANAGER").GetComponent<RobotManager>();
        GhostArmDeactivation(false);
    }
    // Update is called once per frame
    void Update()
    {
        rotationModifier = GameObject.Find("ROBOTMANAGER").GetComponent<RobotManager>().GetSpeedModifier();
        if (isSelected)
        {
            // If the pivot is base rotate around 
            // if it's any other pivot, rotate around it's y-axis
            if (ghostPivot.name == "Segment 1 - Pivot" || ghostPivot.name == "Segment 2 - Pivot")
            {
                ghostPivot.transform.Rotate(hand.transform.rotation.x*rotationModifier, 0f, 0f);
            }  // Add below clarifier for other objects in scene
            else
            {
                ghostPivot.transform.Rotate(0f, 0f, hand.transform.rotation.x*rotationModifier);
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
        //If a part is selected, the arm knows that a move has occured
        robot.GetComponent<RobotController>().nonClawMove = true;
    }
    // Method that's called when needed to move the robot arms' pivots
    // boolean here determines whether or not the submit move button
    // or the reset button has been hit
    public void TranslatePivot(bool reset)
    {
        // This check to see if pivot points are equal
        if (ghostPivot.transform.rotation != gameObject.transform.rotation || reset)
        {
            StartCoroutine(MovePivot(reset));
        } else
        {
            robot.GetComponent<RobotController>().moveReady = true;
        }
    }

    IEnumerator MovePivot(bool reset)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("start_time", System.DateTime.UtcNow);
        GameObject target;
        // So check if the button wants us to reset, if so then assign target
        // To reset position else assign it to the regular ghost arm pivot
        if (reset)
        {
            // ResetPostion.SetActive(true);
            ghostPivot.transform.localPosition = robotManager.reset_positions[gameObject.name];
            ghostPivot.transform.localRotation = robotManager.reset_rotations[gameObject.name];
            target = ghostPivot;
            GhostArmDeactivation(true);
            ROSConnector.CallHomeArm();
            // ResetPostion.SetActive(false);
            // Once the robot is reset, no need to allow the user to reset again
            robot.GetComponent<RobotController>().resetReady = false;
        }
        else
        {
            target = ghostPivot;
            // Once the robot is somewhere else, it can be reset
            robot.GetComponent<RobotController>().resetReady = true;
        }
        // Send the target data at the beginning of the movement
        ROSConnector.SendMovementData(gameObject.name,
            (gameObject.name == "Segment 1 - Pivot" || gameObject.name == "Segment 2 - Pivot") 
            ? ghostPivot.transform.localRotation.x : ghostPivot.transform.localRotation.z);

        // While the target's rotation is not the same as the current pivot's
        // continue to rotate the pivot towards that direction
        while (target.transform.localRotation != gameObject.transform.localRotation)
        {
            float step = speed * Time.deltaTime;
            // Rotate the position of the pivot in relation of the ghost arm OR the invisible reset positions
            gameObject.transform.localRotation =
                Quaternion.RotateTowards(gameObject.transform.localRotation, target.transform.localRotation, step);
            robot.GetComponent<RobotController>().moveReady = false;
            yield return null;
            if(!ghostArm.activeInHierarchy && !reset)
            {
                GhostArmActivation();
            }
            
        } 
        // Stop the current routine and tell the UI that the robot is ready to move!
        robot.GetComponent<RobotController>().moveReady = true;
        GhostArmDeactivation(false);
        ROSConnector.SendTimeData(System.DateTime.UtcNow);
        StopCoroutine(MovePivot(reset));
    }
}
