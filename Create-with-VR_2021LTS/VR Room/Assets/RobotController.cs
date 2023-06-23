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
    [SerializeField] GameObject jEPivot;

    private GameObject hand;
    // Start is called before the first frame update
    void Start()
    {
        // Currently only the right hand has the ability to interact with the robot
        hand = GameObject.Find("RightHand Controller");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
