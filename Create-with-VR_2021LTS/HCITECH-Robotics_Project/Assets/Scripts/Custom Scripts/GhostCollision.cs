using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GhostCollision : MonoBehaviour
{
    // This is how this id going to work:
    // I'm going to have references to all colliders in this script
    // Whenever a collision occurs, I'm going to print what two have had a collision, and this will be onTrigger
    // At that point (still undetermined), I'll add some type of modifier to the currently selected pivot
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Joint Collider")
        {
            Debug.Log("COLLISION HAPPENED");
            Debug.Log(gameObject.name + " collided with " + other.gameObject.name);
            GameObject.Find("ROBOTMANAGER").GetComponent<RobotManager>().LowerSpeedModifier();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Joint Collider")
        {
            Debug.Log("COLLIDER EXITED");
            GameObject.Find("ROBOTMANAGER").GetComponent<RobotManager>().ResetSpeedModifier();
        }
    }
}

/* This is for you Gabriel so you dont forget
    - Currently the way this is working, only one item has a rigidbody to detect the collisoins
    - You reference the pivot that moves the object that the collider that hit was
    - If you need to rotate the collider you just attach it to an empty game object and rotate that instead
    - Currently it only disables the current section you're moving has collided so errors WILL occur when a further child has collided, 
      it will not detect the collision properly
    - Currently collisiosn are checked on the ghost arm
 POTENTIAL FIXES/BUGS
    - instead of allowing the user to selct and move just freeze the object in position and not allow it to move, this would negate
      some of the issues that will occur when a child of object moving hits something and doesnt register
    - onTriggerExit doesnt work
    - the wholen rigidbody situation is weird, stuff was flying off, but we will eventually need to add more rigidbodies to check for all
      collisions
    - RigidbodyConstraints.FreezeAll will be helpful in the future
 WHAT I'M CURRENTLY DOING
    - I'm doing kinda a "sticky" method, where when a collision happens the speed at which the arm rotates is really slow so
      it encourages the user to rotate the opposite way
    - a better way is to check and see if reverse force is being applied on the pivot from the rotation and only allow the arm to move
      in the opposite direction if the force is great enough/in the right direction
*/