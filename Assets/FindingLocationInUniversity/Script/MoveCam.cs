using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;

public class MoveCam : MonoBehaviour
{
    public Transform startMarker;
    public Transform endMarker;
    public float speed = 1.0F;
    private float startTime;
    private float journeyLength;

    private string tempHasLocation;

    private int startCnt;


    private Quaternion startRotation;
    private Quaternion endRotation;
    private DatabaseReference db;


    //PlayerPrefs.GetString("PlayerName").ToString();
    //db.SetValueAsync("nolocation");

    void Start()
    {
        tempHasLocation = "nolocation";
        startCnt = 0;

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://robotic-speech.firebaseio.com/");
        db = FirebaseDatabase.DefaultInstance.GetReference("location");
        

        
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        startRotation = startMarker.rotation;
        endRotation = Quaternion.Euler(0,0,0);


    }

    void Update()
    {
        try
        {
            if(startCnt==0)
                    {
                        hasLocation();
                        if (!tempHasLocation.Equals("nolocation"))
                        {
                            startTime = Time.time;
                            startCnt = 1;
                        }
            
                    }
            else
            {
                float distCovered = (Time.time - startTime) * speed;
                float fracJourney = distCovered / journeyLength;
                transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
                startMarker.position = transform.position;
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, distCovered*11);
                startMarker.rotation = transform.rotation;
                if(startMarker.position==endMarker.position)
                {
                    LoadMenu();
                }
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Update Method Exception :" + ex);
        }
        
        

    }


    void hasLocation()
    {
   
        
        try
        {

            db.GetValueAsync().ContinueWith(task => {

                if (task.IsFaulted)
                {
                    // Handle the error...
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    tempHasLocation = snapshot.GetValue(true).ToString();
                    //Debug.Log(snapshot);
                    //Debug.Log(tempHasLocation);

                }
            });

           

        }
        catch(Exception ex)
        {
            Debug.Log("hasLocation Method Exception : "+ex);
            tempHasLocation = "None";
        }

        

    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("UniversityInside", LoadSceneMode.Single);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString("LocationID", tempHasLocation);
    }

    public void greatingMessage()
    {
        //TODO: animasyon eklenecek

    }




}
