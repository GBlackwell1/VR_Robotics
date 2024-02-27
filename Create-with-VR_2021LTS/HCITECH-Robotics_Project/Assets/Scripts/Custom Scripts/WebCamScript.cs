using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebCamScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var cam = "";
        foreach (WebCamDevice device in WebCamTexture.devices)
        {
            cam = device.name;
        }
        WebCamTexture webcam = new WebCamTexture(cam);
        GetComponent<Renderer>().material.mainTexture = webcam;
        webcam.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
