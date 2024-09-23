using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript1 : MonoBehaviour
{
    [SerializeField] RobotController robotController;
    GhostVisibility ghostArm;
    AudioSource completedMove;
    AudioSource moveInProgress;
    private bool playRobotMovement = false;
    // Start is called before the first frame update
    void Start()
    {
        // Get audio sources from all children under manager
        completedMove = gameObject.GetComponentsInChildren<AudioSource>()[1];
        moveInProgress = gameObject.GetComponentsInChildren<AudioSource>()[2];
        ghostArm = GameObject.Find("Ghost Arm - New").GetComponent<GhostVisibility>();
        moveInProgress.Play(); moveInProgress.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        // Play movement sound if the robot is moving
        if (playRobotMovement && !robotController.moveReady)
        {
            playRobotMovement = false;
            moveInProgress.UnPause();
        }
        else if (robotController.moveReady)
        {
            if (!playRobotMovement && ghostArm.GhostMatch())
                completedMove.Play();
            playRobotMovement = true;
            moveInProgress.Pause();
        }
    }
}
