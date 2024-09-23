using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript1 : MonoBehaviour
{
    [SerializeField] RobotController robotController;
    AudioSource completedMove;
    AudioSource moveInProgress;
    private bool playRobotMovement = false;
    // Start is called before the first frame update
    void Start()
    {
        // Get audio sources from all children under manager
        completedMove = gameObject.GetComponentsInChildren<AudioSource>()[1];
        moveInProgress = gameObject.GetComponentsInChildren<AudioSource>()[2];
    }

    // Update is called once per frame
    void Update()
    {
        // Play movement sound if the robot is moving
        if (playRobotMovement && !robotController.moveReady)
        {
            playRobotMovement = false;
            moveInProgress.Play();
        }
        else if (robotController.moveReady)
        {
            if (!playRobotMovement)
                completedMove.Play();
            playRobotMovement = true;
            moveInProgress.Stop();
        }
    }
}
