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

public class FirebaseScript : MonoBehaviour
{
    private static string SERVER_NAME = "http://localhost:8000/get";
    public static bool FLAG_READY = false;
    void Awake()
    {
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
    }
    private void Start()
    {
        StartCoroutine(ConnectionEstablish());
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

        if(www.result != UnityWebRequest.Result.Success)
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
    public void SendMovementData(float cartesian_x, float cartesian_y, float cartesian_z, string pivot_name)
    {
        StartCoroutine(MovementCoroutine(cartesian_x, cartesian_y, cartesian_z, pivot_name));
    }
    IEnumerator MovementCoroutine(float cartesian_x, float cartesian_y, float cartesian_z, string pivot_name)
    {
        WWWForm movementData = new WWWForm();
        // Add data to the WWWForm (by pivot)
        movementData.AddField(pivot_name, cartesian_x.ToString());
        movementData.AddField(pivot_name, cartesian_y.ToString());
        movementData.AddField(pivot_name, cartesian_z.ToString());
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
            Debug.Log("Succesfully posted time data to " + SERVER_NAME);
        }
    }

}