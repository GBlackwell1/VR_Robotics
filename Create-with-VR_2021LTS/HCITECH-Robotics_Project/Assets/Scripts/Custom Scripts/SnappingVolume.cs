using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class SnappingVolume : MonoBehaviour
{
    [SerializeField] private GameObject claw; // Holds the reference to the claw controller
    [SerializeField] private GameObject cube; // Holds the reference to the cube
    private ClawController clawController;
    private bool inArea = false;
    private bool isSnapped = false;

    private void Start()
    {
        clawController = claw.GetComponent<ClawController>();
    }

    private void Update()
    {
        // If it meets the condtions for snapping, snap it
        if (inArea && clawController.checkClosed())
        {
            isSnapped = true;
            Rigidbody cubeRigidbody = cube.GetComponent<Rigidbody>();
            if (cubeRigidbody != null)
            {
                cubeRigidbody.useGravity = false;
                cubeRigidbody.detectCollisions = false;
                cubeRigidbody.isKinematic = false;
            }
            cube.transform.position = transform.position;
            cube.transform.rotation = transform.rotation;
        } else if (isSnapped)
        {
            // Otherwise, if it is snapped, release it
            ReleaseCube();
        }
    }

    // When the cube enters the area, tell update that it is fair game
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(cube))
        {
            inArea = true;
            
        }
    }

    //If the cube exits the trigger and is still snapped, bring it back. Otherwise, release it
    void OnTriggerExit(Collider other)
    {
        if(isSnapped && !clawController.checkClosed())
        {
            inArea = false;
            ReleaseCube();
        } else if(isSnapped)
        {
            cube.transform.position = transform.position;
            cube.transform.rotation = transform.rotation;
        }
        
    }

    // Call this function to release the cube
    public void ReleaseCube()
    {
        Rigidbody cubeRigidbody = cube.GetComponent<Rigidbody>();
        if (cubeRigidbody != null)
        {
            cubeRigidbody.useGravity = true;
        }
        isSnapped = false;
        StartCoroutine(enableCollision());

    }

    // Waits for a short period for the cube to leave the claw, then enables collisions
    IEnumerator enableCollision()
    {
        yield return new WaitForSeconds(0.05f);
        cube.GetComponent<Rigidbody>().detectCollisions = true;
        
    }
}
