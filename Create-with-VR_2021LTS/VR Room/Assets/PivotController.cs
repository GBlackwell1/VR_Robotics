using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotController : MonoBehaviour
{
    [SerializeField] GameObject hand;
    [SerializeField] GameObject ghostArm;
    [SerializeField] GameObject ghostPivot;

    // Defaults forselection activation
    private bool isSelected = false;
    // Start is called before the first frame update
    void Start()
    {
        
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
            } 
            else if (ghostPivot.name != "base")  // Add this clarifier for other objects in scene
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
    public void GhostArmDeactivation()
    {
        // This feature is disabled for the base since the base of both robots should
        // always be in the same position
        if (ghostPivot.transform.position == gameObject.transform.position 
            && ghostPivot.transform.rotation == gameObject.transform.rotation)  // Check for rotation since eccentric pivots will have same position
            ghostArm.SetActive(false);
    }
    // Only run the update function if something is selected
    public void SelectionHandler()
    {
        isSelected = !isSelected;
    }
}
