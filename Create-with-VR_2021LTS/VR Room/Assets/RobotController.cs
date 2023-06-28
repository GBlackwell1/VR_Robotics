using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    // <joint> <joint name/references>
    [SerializeField] GameObject jBase;
    [SerializeField] GameObject jBox;
    [SerializeField] GameObject jEPivotTwo;
    [SerializeField] GameObject jHandControl;
    // [SerializeField] GameObject jEPivot;
    public bool moveReady = true;
    
    // Call one function instead of 5 different functions
    // upon the submit move button click
    public void MoveRobot()
    {
        jBase.GetComponent<PivotController>().TranslatePivot();
        //jBox.GetComponent<PivotController>().TranslatePivot();
        //jEPivotTwo.GetComponent<PivotController>().TranslatePivot();
        //jHandControl.GetComponent<PivotController>().TranslatePivot();
        // jEPivot.GetComponent<PivotController>().TranslatePivot();
    }
}