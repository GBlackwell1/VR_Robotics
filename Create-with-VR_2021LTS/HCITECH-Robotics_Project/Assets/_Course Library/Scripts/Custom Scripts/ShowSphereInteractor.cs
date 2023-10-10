using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSphereInteractor : MonoBehaviour
{
    // So we can't disable the game object here because it will disable the colliders
    // Need to enable and disable the mesh renderer component
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hand") {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            // Commented this out @Mia because they have no reference right now because of the scene merge conflicts
            // Uncomment them when you get the objects back in the scene
            /*gameObject.transform.GetChild(0).gameObject.SetActive(true);*/
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Hand")
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
           /* gameObject.transform.GetChild(0).gameObject.SetActive(false);*/
        }
    }
}

