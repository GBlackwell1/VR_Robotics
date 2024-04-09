using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebCamScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<string> cams = new List<string>();
        foreach (WebCamDevice device in WebCamTexture.devices)
        {
            cams.Add(device.name);
        }
        WebCamTexture webcam = new WebCamTexture(cams[0]);
        GetComponent<Renderer>().material.mainTexture = webcam;
        webcam.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
