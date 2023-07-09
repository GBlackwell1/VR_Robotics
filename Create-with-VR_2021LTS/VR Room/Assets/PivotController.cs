using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotController : MonoBehaviour
{
    [SerializeField] GameObject hand;
    [SerializeField] GameObject ghostArm;
    [SerializeField] GameObject ghostPivot;
    private GameObject robot;
    private float speed = 100f;

    // Defaults forselection activation
    private bool isSelected = false;

    // Default robot starting positions
    [SerializeField] GameObject ResetPostion;
    // Start is called before the first frame update
    void Start()
    {
        robot = GameObject.Find("Robot_Arm_01");
    }
    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            // If the pivot is base rotate around the z-axis
            // if it's any other pivot, rotate around it's y-axis
            if (ghostPivot.name == "base") 
            {
                ghostPivot.transform.Rotate(0f, 0f, hand.transform.rotation.x);
            }  // Add below clarifier for other objects in scene
            else if (ghostPivot.name != "base")  
            {
                ghostPivot.transform.Rotate(0f, hand.transform.rotation.x, 0f);
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
        } else if (isReset) { ghostArm.SetActive(false); }
            
    }
    // Only run the update function if something is selected
    public void SelectionHandler()
    {
        isSelected = !isSelected;
    }
    // Method that's called when needed to move the robot arms' pivots
    // boolean here determines whether or not the submit move button
    // or the reset button has been hit
    public void TranslatePivot(bool reset = false)
    {
        // This check to see if pivot points are equal, if they aren't 
        // start a coroutine, user can currently click submit several times
        // and this negates excess runtime calculations
        if(ghostPivot.transform.rotation != gameObject.transform.rotation)
        {
            StartCoroutine(MovePivot(reset));
        }
    }

    IEnumerator MovePivot(bool reset)
    {
        GameObject target;
        // So check if the button wants us to reset, if so then assign target
        // To reset position else assign it to the regular ghost arm pivot
        if (reset)
        {
            ghostPivot.transform.position = ResetPostion.transform.position;
            GhostArmDeactivation(true);
        }
        target = ghostPivot;
        // While the target's rotation is not the same as the current pivot's
        // continue to rotate the pivot towards that direction
        while (target.transform.rotation != gameObject.transform.rotation)
        {
            // Rotate the position of the pivot in relation of the ghost arm
            if (gameObject.name == "base" || gameObject.name == "hand_controller")
            {
                gameObject.transform.Rotate
                    (0f, 0f,
                    (target.transform.rotation.z - gameObject.transform.rotation.z) * Time.deltaTime * speed);
            }
            else
            {
                gameObject.transform.Rotate
                    (0f,
                    (target.transform.rotation.y - gameObject.transform.rotation.y) * Time.deltaTime * speed,
                    0f);
            }
            yield return null;
        }
        Debug.Log("CoRoutine Should stop here");
        // Little cheeky but move the robot the rest of the way
        // Basically unity stops calculations when floats get reasonably close so just
        // Snap the robot into place
        gameObject.transform.rotation = target.transform.rotation;
        // Stop the current routine and tell the UI that the robot is ready to move!
        StopCoroutine(MovePivot(reset));
        robot.GetComponent<RobotController>().moveReady = true;
    }
}
