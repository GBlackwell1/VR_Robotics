using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Firebase;
using Firebase.Database;
using JetBrains.Annotations; 
using Firebase.Storage;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Text.RegularExpressions;
using System;

public class FirebaseScript : MonoBehaviour
{
    private static string SERVER_NAME = "http://192.168.56.102:8000/get";
    public static bool FLAG_READY = false;
    // Array of offset values
    IDictionary<string, float> offsets = new Dictionary<string, float>();


    void Awake()
    {
        offsets.Add("joint1", 275f); offsets.Add("joint2", 150f);
        offsets.Add("joint3", 61f); offsets.Add("joint4", 78f);
        offsets.Add("joint5", 12f); offsets.Add("joint6", -89f);
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
                Debug.Log("Firebase was available and connection has been established");
                // Set a flag here to indicate whether Firebase is ready to use by your app.
                FLAG_READY = true;
                /*DevelopmentFunction();*/
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
                FLAG_READY = false;
            }
        });
        StartCoroutine(ConnectionEstablish());
        StartCoroutine(HomeArm());
    }
    private void Start()
    {
        
    }
    /*
     * description: Currently being used to send some initial test data, check
     *  the output of the node server, should reply with "Connection established with UNITY"
     *  assuming that it has ran correctly
     */
    IEnumerator ConnectionEstablish()
    {
        WWWForm form = new WWWForm();
        form.AddField("Connection", "TRUE");
        UnityWebRequest www = UnityWebRequest.Post(SERVER_NAME, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Succesfully established connection to " + SERVER_NAME);
        }
    }
    public void CallHomeArm()
    {
        StartCoroutine(HomeArm());
    }
    IEnumerator HomeArm()
    {
        Debug.Log("HOME CALLED");
        WWWForm form = new WWWForm();
        // Send some basic data
        form.AddField("joint1", 275); form.AddField("joint2", 150);
        form.AddField("joint3", 61); form.AddField("joint4", 258);
        form.AddField("joint5", 12); form.AddField("joint6", 91);
        form.AddField("HOME", "true");
        // formulate request and send
        UnityWebRequest www = UnityWebRequest.Post(SERVER_NAME, form);
/*        UnityWebRequest stopMovement = UnityWebRequest.Post(SERVER_NAME, haltMovement);*/
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Succesfully established connection to " + SERVER_NAME);
        }
    }
    // Sends movement data in the form of a WWWForm to node server

    // TODO: A couple things to keep in mind, we shouldn't enable the user to submit again until it
    // has returned success, considering adding the UI handler for the movement enabled to the master
    // controller, and handling the enabling and disabling in here

    // TODO: What happens if there is an error? Something we need to think about in the future once
    // we succesfully establish connection and are sending data back and forth to the robot

    // TODO: Do we need to keep a log of this data? Where do we store it, needs to be stored on the server
    // right, because what if we have to go back and query this information if there is a missed move?
    // We need to hold all the rotations and positions of each node until the robot has returned a succesful
    // completion code for the currently requested move.
    public void SendMovementData(IDictionary<string, float> move)
    {
        // This queues data in ROS, should move each pivot one by one
        StartCoroutine(MovementCoroutine(move));
    }
    IEnumerator MovementCoroutine(IDictionary<string, float> move)
    {
        Debug.Log("MOVEMENT CALLED");
        WWWForm movementData = new WWWForm();
        // Add data to the WWWForm (by pivot)
        // Parse the pivot name into something usable 
        foreach (KeyValuePair<string, float> rotation in move)
        {
            // move via offset and keep it within 360 degrees, notify server submit move happened
            double offset_adjustment = (rotation.Value + offsets[rotation.Key]) % 360;
            movementData.AddField(rotation.Key, offset_adjustment.ToString());
        }
        movementData.AddField("SUBMIT", "true");
        // Formulate the request and where to send it
        UnityWebRequest www = UnityWebRequest.Post(SERVER_NAME, movementData);
        yield return www.SendWebRequest();
        if(www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        } 
        else
        {
            Debug.Log("Succesfully posted movement data to " + SERVER_NAME);
        }
    }
    // Don't currently know how we're using this but it is nice to have
    public void SendTimeData(System.DateTime time)
    {
        StartCoroutine(TimeCoroutine(time));
    }
    IEnumerator TimeCoroutine(System.DateTime time)
    {
        WWWForm timeData = new WWWForm();
        // Add data to the WWWForm
        timeData.AddField("TimeExecuted", time.ToString());
        // Fomrulate the request and where to send it
        UnityWebRequest www = UnityWebRequest.Post(SERVER_NAME, timeData);
        yield return www.SendWebRequest();

        if(www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            /*Debug.Log("Succesfully posted time data to " + SERVER_NAME);*/
        }
    }

}