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


    private Vector3 rot;
    private Vector3 rot2;
    private float rotDestination;
    private float rotSpeed;
    private int cntOnce;

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

        rot = startMarker.rotation.eulerAngles;
        rot2 = new Vector3(0, 0, 0);
        rotSpeed = 0.009f;
        cntOnce = 1;

        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
   

        rotDestination = rot.x + (((rot2.x - rot.x) / 4) * 3);
        
    }

    void Update()
    {
        try
        {
            if (startCnt == 0)
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
                if (rot.x < rotDestination)
                {
                    if (cntOnce == 1)
                    {

                        rotSpeed = rotSpeed * 3.5f;
                        cntOnce = 0;
                        //fracJourney = fracJourney * 3f;
                    }
                    


                }

                
                if (startMarker.position.x > (-7.16))
                {
                    transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney * 1.01f);
                    startMarker.position = transform.position;

                }

                if (startMarker.rotation.x > 0.003f)
                {
                    rot = Vector3.Lerp(rot, rot2, rotSpeed);
                    startMarker.rotation = Quaternion.Euler(rot);
                }

                
               
                
                if (startMarker.position.x <= (-7.16))
                {
                    greatingMessage();

                    if (greetingMessageObj.transform.position.y == (float)-2.265)
                    {

                        if (startTimeCnt == 0)
                        {
                            startDelayTime = Time.time;
                            startTimeCnt = 1;
                        }

                        if ((int)(Math.Abs(Time.time - startDelayTime)) > 3)
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
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString("LocationID", tempHasLocation);
        SceneManager.LoadScene("UniversityInside", LoadSceneMode.Single);
        //Application.LoadLevel("UniversityInside");
        
    }

    public void greatingMessage()
    {
  
        if(greetingCnt==0)
        {
            greetingMessageObj.SetActive(true);

        }
        
    }

    




}
