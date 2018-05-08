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
    public GameObject greetingMessageObj;
    

    public float speed = 1.0F;
    private float startTime;
    private float journeyLength;
    private float startDelayTime;

    private string tempHasLocation;

    private int startCnt;
    private int greetingCnt;
    private int startTimeCnt;



    private Quaternion startRotation;
    private Quaternion endRotation;




    private DatabaseReference db;


    //PlayerPrefs.GetString("PlayerName").ToString();
    //db.SetValueAsync("nolocation");

    void Start()
    {
        tempHasLocation = "nolocation";
        startCnt = 0;
        greetingCnt = 0;
        startTimeCnt = 0;
       
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
                
                

                transform.rotation = Quaternion.Lerp(startRotation, endRotation, distCovered*12.8f);
                startMarker.rotation = transform.rotation;
                if(startMarker.rotation.x != 0)
                {
                    transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney * 1.01f);
                    startMarker.position = transform.position;
                }

                if(startMarker.rotation.x==0)
                {
                    greatingMessage();

                    if(greetingMessageObj.transform.position.y == (float)-1.929 )
                    {
                        
                        if( startTimeCnt ==0)
                        {
                            startDelayTime = Time.time;
                            startTimeCnt = 1;
                        }
                        
                        if ((int)(Math.Abs(Time.time-startDelayTime))>3)
                        {
                            LoadMenu();
                        }
                        
                        
                    }
                        
                   
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
        //Application.LoadLevel("UniversityInside");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString("LocationID", tempHasLocation);
    }

    public void greatingMessage()
    {
  
        if(greetingCnt==0)
        {
            greetingMessageObj.SetActive(true);

        }
        
    }

    




}
