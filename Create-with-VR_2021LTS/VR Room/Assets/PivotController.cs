using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotController : MonoBehaviour
{
    [SerializeField] GameObject hand;
    [SerializeField] GameObject ghostArm;
    [SerializeField] GameObject ghostPivot;

    // Defaults for ghost arm and selection activation
    private bool isActive = false;
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
            // If the pivot is base rotate around the base
            // if it's any other pivot, rotate around it's y
            if (ghostPivot.name == "base") 
            {
                ghostPivot.transform.Rotate(0f, 0f, hand.transform.rotation.x);
            } 
            else if (ghostPivot.name != "base")
            {
                ghostPivot.transform.Rotate(0f, hand.transform.rotation.x, 0f);
            }
        }
    }

    // Enable and disable the view of the GhostArm
    public void GhostArmActivation()
    {
        // tried flipping bool but it didnt work
        ghostArm.SetActive(true);
    }
    public void GhostArmDeactivation()
    {
        ghostArm.SetActive(false);
    }
    // Only run the update function if something is selected
    public void SelectionHandler()
    {
        isSelected = !isSelected;
    }
}
