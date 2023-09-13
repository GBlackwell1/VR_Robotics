using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using JetBrains.Annotations;
using Firebase.Storage;
using Firebase.Firestore;
using Firebase.Extensions;

public class FirebaseScript : MonoBehaviour
{
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
    public class TestData 
    {
        public string data1;
        public int data2;
        public bool data3;

        public TestData()
        {
            this.data1 = "testing beepity booop";
            this.data2 = 1;
            this.data3 = true;
        }
    }
    void DevelopmentFunction()

    {
        if (FLAG_READY)
        {
            // Get the root of the database
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            // Create some test object and asyncronously send it to firebase
            // This works super similarly to java
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                {"item_1", 1 },
                {"item_2", "something" },
                {"item_3", true },

            };
            db.Collection("test_data").Document("data_dict").SetAsync(data).ContinueWithOnMainThread(task =>
            {
                Debug.Log("Added data to the test_data collection, specifically dictionary");
            });
            db.Collection("test_data").Document("data_obj").SetAsync(new TestData()).ContinueWithOnMainThread(task =>
            {
                Debug.Log("Added data to the test_date collection, specifically object");
            });

        }
        else Debug.LogError("In FirebaseScript.DevelopmentFunction, " +
            "firebase was not initialized before calling this function");
    }
    
    
    // Future methods
    public void SendMovementData(float cartesian_x, float cartesian_y, float cartesian_z, string pivot_name)
    {
        if (FLAG_READY) 
        {
            // Get the database
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            DocumentReference docRef = db.Collection("developers")
                                         .Document("gabriel")
                                         .Collection("movements")
                                         .Document(pivot_name);
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "cartesian_x", cartesian_x },
                { "cartesian_y", cartesian_y },
                { "cartesian_z", cartesian_z },
            };
            docRef.SetAsync(data).ContinueWithOnMainThread(task => Debug.Log("Added data belonging to pivot: " 
                + pivot_name + " to firestore"));
            // TODO: How are we going to handle the next move?
            // Is there a way that we can send the pivot location
        }
        else Debug.LogError("In FirebaseScript.SendMovementData, " +
            "firebase was not initialized before calling this function");
    }
    void SendTimeData()
    {
        if (!FLAG_READY) { }
        else Debug.LogError("In FirebaseScript.TimeData, " +
            "firebase was not initialized before calling this function");
    }

}
