using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PivotController : MonoBehaviour
{
    [SerializeField] GameObject hand;
    [SerializeField] GameObject ghostArm;
    [SerializeField] GameObject ghostPivot;
    private GameObject robot;
    private float speed = 20f;

    // Defaults forselection activation
    private bool isSelected = false;

    // Default robot starting positions
    [SerializeField] GameObject ResetPostion;
    // Start is called before the first frame update
    void Start()
    {
        robot = GameObject.Find("Robot Arm");
    }
    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            Debug.Log(ghostPivot.name);
            // If the pivot is base rotate around the z-axis
            // if it's any other pivot, rotate around it's y-axis
            if (ghostPivot.name == "Pivot - Upper Assembly" )
            {
                ghostPivot.transform.Rotate(0f, hand.transform.rotation.x, 0f);
            }  // Add below clarifier for other objects in scene
            else
            {
                ghostPivot.transform.Rotate(0f, 0f, hand.transform.rotation.x);
            }
        }
    }

    // Enable and disable the view of the GhostArm
    public void GhostArmActivation()
    {
        ghostArm.SetActive(true);
    }
    // If the position of the ghost arm is not the same as the visible arm
    // do not deactivate it, leave it active until the positions are the same
    public void GhostArmDeactivation(bool isReset)
    {
        // This feature is disabled for the base since the base of both robots 
        // should always be in the same position
        if (ghostPivot.transform.position == gameObject.transform.position
            && ghostPivot.transform.rotation == gameObject.transform.rotation
           && !isReset)
        {
            // Check for rotation since eccentric pivots will have same position
            ghostArm.SetActive(false);
            robot.GetComponent<RobotController>().moveReady = true;
        }
        else if (isReset) { ghostArm.SetActive(false); }

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
        GameObject target;
        // So check if the button wants us to reset, if so then assign target
        // To reset position else assign it to the regular ghost arm pivot
        if (reset)
        {
            ResetPostion.SetActive(true);
            ghostPivot.transform.position = ResetPostion.transform.position;
            ghostPivot.transform.rotation = ResetPostion.transform.rotation;
            target = ghostPivot;
            GhostArmDeactivation(true);
            ResetPostion.SetActive(false);
        } else
        {
            target = ghostPivot;
        }
        
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
            
        } 
        // Stop the current routine and tell the UI that the robot is ready to move!
        robot.GetComponent<RobotController>().moveReady = true;
        StopCoroutine(MovePivot(reset));
    }
}
