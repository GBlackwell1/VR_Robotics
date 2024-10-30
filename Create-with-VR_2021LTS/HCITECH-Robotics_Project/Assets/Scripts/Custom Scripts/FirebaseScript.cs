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
using Unity.XR.CoreUtils;
using UnityEditor.PackageManager;

public class FirebaseScript : MonoBehaviour
{
    // TODO: Have UI element where user's can type in participant ID, makes data logging easier
    private static string SERVER_NAME = "http://192.168.56.102:8000/get";
    [SerializeField] GameObject UIHandler;
    public static bool FLAG_READY = false;
    private static bool FLAG_TASK = false;
    private const string COLLECTION = "Fall24_Dev_Grouping";
    // Array of offset values
    IDictionary<string, float> offsets = new Dictionary<string, float> {
        {"joint1", 275f}, {"joint2", 150f}, {"joint3", 61f}, 
        {"joint4", 258f}, {"joint5", 12f}, {"joint6", 91f } 
    };
    private string[] STATUS_CODES = {"move pending", "move processing", "move preempted", "move success",
                             "move aborted", "move rejected", "move preempting", "move recalling", 
                             "move recalled", "move lost"};
    //private int taskMove = 0;
    //private int regMove = 0;
    private DateTime taskStart; private DateTime taskEnd;
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
                InitParticipant();
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
    /// <summary>
    /// Intializes the participant in firebase upon entering the program 
    /// </summary>
    private void InitParticipant()
    {
        if (FLAG_READY)
        {
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            DocumentReference docRef = db.Collection(COLLECTION).Document("uniqueID2");
            Dictionary<string, object> dataToSend = new Dictionary<string, object>
            {
                { "name", "Gabriel Blackwell" },
                { "testID", "uniqueID2" },
                { "platform_start", DateTime.Now },
            };
            docRef.SetAsync(dataToSend).ContinueWithOnMainThread(task =>
            {
                Debug.Log("DocRef test data has been sent to database!");
            });
        }
        else Debug.LogError("Firebase not initialized");
    }
    /// <summary>
    /// Logs to the database the starting and ending time of the task
    /// </summary>
    /// <param name="FieldName">Denotes whether starting or ending time for the task</param>
    public void TaskLogTime(string FieldName)
    {
        // Set the starting and ending times independent of the database
        if (FieldName.Equals("start_time"))
        {
            FLAG_TASK = true;
            taskStart = DateTime.Now;
        }
        else 
        { 
            FLAG_TASK = false;
            taskEnd = DateTime.Now; 
        }
        // If firebase intialized then log data to the database
        if (FLAG_READY)
        {
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            DocumentReference docRef = db.Collection(COLLECTION)
                                         .Document("uniqueID2").Collection("Task")
                                         .Document("Data");
            Dictionary<string, object> dataToSend = new Dictionary<string, object>
            {{ FieldName, DateTime.Now }};
            // If logging end time, log elapsed time too
            // TODO: What to do on the stop task button
            if (taskEnd != null && taskEnd > taskStart) 
                dataToSend.Add("elapsed_time", (taskEnd-taskStart).ToString());
            // Send document
            docRef.SetAsync(dataToSend).ContinueWithOnMainThread(task =>
            {
                Debug.Log("Updating "+FieldName+" for the task.");
            });
        }
        else Debug.LogError("Firebase not initialized");


    }
    /// <summary>
    /// Logs the move for each pivot to firebase
    /// </summary>
    /// <param name="type">The type of move submitted, relative or homing</param>
    /// <param name="move">Dictionary of moves seperated by pivot</param>
    private void LogMove(string type, IDictionary<string, float> move)
    {
        if (FLAG_READY)
        {
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            CollectionReference collRef;
            // Choose which collection to add to depending on whether the user is doing the task
            if (FLAG_TASK)
                collRef = db.Collection(COLLECTION).Document("uniqueID2")
                            .Collection("Task").Document("Data")
                            .Collection("Moves");
            else
                collRef = db.Collection(COLLECTION).Document("uniqueID2")
                            .Collection("Moves");
            Dictionary<string, object> docData = new Dictionary<string, object>{
                { "type", type },
                { "time_moved", DateTime.Now}
            };
            // Add all the data for each pivot into the document
            foreach (KeyValuePair<string, float> rotation in move)
                docData.Add(rotation.Key, rotation.Value);
            // Send the finalized document to the database
            collRef.AddAsync(docData).ContinueWithOnMainThread(task =>
            {
                Debug.Log("Updated movement collection in firebase");
            });
        }
        else Debug.LogError("Firebase not initialized");
    }
    /// <summary>
    /// Sends an initial connection string TRUE to the server to verify operation
    /// </summary>
    /// <returns>Web request</returns>
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
    /// <summary>
    /// Send a post request to set the phyiscal arm to the preset Home position
    /// </summary>
    public void CallHomeArm()
    {
        LogMove("homing", offsets);
        StartCoroutine(HomeArm());
    }   
    // TODO: Create move counter
    IEnumerator HomeArm()
    {
        Debug.Log("HOME CALLED");
        UIHandler.GetComponent<UITextHandler>().UpdateStatus(false, "Homing robot...");
        WWWForm form = new WWWForm();
        // Send some basic data
        // TODO: Test and make sure float conversions to string don't cause issues later in the stack
        foreach (KeyValuePair<string, float> move in offsets) 
            form.AddField(move.Key, move.Value.ToString());
        form.AddField("HOME", "true");
        // formulate request and send
        UnityWebRequest www = UnityWebRequest.Post(SERVER_NAME, form);

        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            UIHandler.GetComponent<UITextHandler>().UpdateStatus(true, www.error.Substring(0,10)+"...");
        }
        else
        {
            Debug.Log("Succesfully established connection to " + SERVER_NAME);
            // Next wait for success code
            Debug.Log("WAITING FOR RESPONSE...");
            UnityWebRequest statusRequest = UnityWebRequest.Get(SERVER_NAME);
            yield return statusRequest.SendWebRequest();
            if (statusRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(statusRequest.error);
                UIHandler.GetComponent<UITextHandler>().UpdateStatus(true,
                    statusRequest.error.Substring(0, 10) + "...");
            }
            else
            {
                // Format this text
                string response = statusRequest.downloadHandler.text;
                Debug.Log("Response: "+response);
                var split_data = response.Split(":");
                int status_code = Int32.Parse(split_data[1].Substring(0,1));
                UIHandler.GetComponent<UITextHandler>().UpdateStatus(true, STATUS_CODES[status_code]);
            }

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
        LogMove("relative", move);
        StartCoroutine(MovementCoroutine(move));
    }
    IEnumerator MovementCoroutine(IDictionary<string, float> move)
    {
        Debug.Log("MOVEMENT CALLED");
        UIHandler.GetComponent<UITextHandler>().UpdateStatus(false, "");
        WWWForm movementData = new WWWForm();
        // Add data to the WWWForm (by pivot)
        // Parse the pivot name into something usable 
        string data = "";
        foreach (KeyValuePair<string, float> rotation in move)
        {
            // move via offset and keep it within 360 degrees, notify server submit move happened
            double offset_adjustment = rotation.Value;
            movementData.AddField(rotation.Key, offset_adjustment.ToString());
            data += rotation.Key + ": " + offset_adjustment.ToString() + " ";
        }
        movementData.AddField("SUBMIT", "true");

        Debug.Log("MOVEMENT DATA: " + data);
        // Formulate the request and where to send it
        UnityWebRequest www = UnityWebRequest.Post(SERVER_NAME, movementData);
        yield return www.SendWebRequest();
        if(www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
            UIHandler.GetComponent<UITextHandler>().UpdateStatus(true, www.error.Substring(0, 10) + "...");
        } 
        else
        {
            Debug.Log("Succesfully posted movement data to " + SERVER_NAME);
            // Next step, wait for response from kinova
            Debug.Log("WAITING FOR RESPONSE...");
            UnityWebRequest statusRequest = UnityWebRequest.Get(SERVER_NAME);
            yield return statusRequest.SendWebRequest();
            if (statusRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(statusRequest.error);
                UIHandler.GetComponent<UITextHandler>().UpdateStatus(true, 
                    statusRequest.error.Substring(0, 10) + "...");
            }
            else
            {
                string response = statusRequest.downloadHandler.text;
                Debug.Log("Response: " + response);
                var split_data = response.Split(":");
                int status_code = Int32.Parse(split_data[1].Substring(0,1));
                UIHandler.GetComponent<UITextHandler>().UpdateStatus(true, STATUS_CODES[status_code]);
            }
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