﻿using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NavMeshMove : MonoBehaviour {

    [SerializeField]

    private Transform _destination ;
    private GameObject _location;
    private NavMeshAgent _navMeshAgent;

    private GameObject image;
    private Transform startMarker;
    private Vector3 rot, rot2;
    public Text text;

    private DatabaseReference db;

    public float turnSpeed= 0.002F;
    private float tempSpeed;
    private int turnCnt;
    private int textIterator;
    private float textTimer;
    private float textTimerStart;
    private int timerCnt;
    private string greetingText;
    private int cntOnce;
    private float rotDestination;

    void Start()
    {
         try
        {
            startMarker = GameObject.Find("CamParent").transform;
            image = GameObject.Find("SpeechBubbleImg");
            image.SetActive(false);

            turnCnt = 0;
            timerCnt = 0;
            textIterator = 0;
            cntOnce = 1;
            tempSpeed = turnSpeed;

            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://robotic-speech.firebaseio.com/");
            db = FirebaseDatabase.DefaultInstance.GetReference("location");


            String tempLocation=PlayerPrefs.GetString("LocationID").ToString();
       

        
            _location = GameObject.Find(tempLocation);
            Debug.Log(" "+_location);
            _destination = _location.transform;
            _navMeshAgent = this.GetComponent<NavMeshAgent>();

            setRotation();
            setGreetingText(1);
            StartCoroutine(startInformation());


            if (_navMeshAgent == null)
            {
                Debug.LogError("Erorrrr");
            }

            }
        catch (Exception)
        {
            db.SetValueAsync("nolocation");
            db = FirebaseDatabase.DefaultInstance.GetReference("waitlocation");
            db.SetValueAsync("no");
            LoadMenu();
        }
    }


    void Update()
    {
       
        if (turnCnt==1)
        {
            
            changeRotation();
           
           
            if(Vector3.Distance(rot,rot2)<0.1)
            {
                turnSpeed = tempSpeed;
                cntOnce = 1;
                SetDestination();
                turnCnt = 2;
            }
            
        }
      
        else if(turnCnt==2 || turnCnt==3 )
        {
            
            Transform _startdestination = _navMeshAgent.transform;
            
            if(Mathf.Approximately(_navMeshAgent.transform.position.x , _destination.transform.position.x))
            {
                
                
                if(Mathf.Approximately(_navMeshAgent.transform.position.z,_destination.transform.position.z))
                {
                    
                    if (Mathf.Approximately(_navMeshAgent.transform.position.y, _destination.transform.position.y))
                    {
                        
                        
                        if (turnCnt==2)
                        {
                            
                            setRotation();
                            turnCnt = 3;
                        }
                        
                        changeRotation();

                        if(Vector3.Distance(rot, rot2) < 0.1)
                        {
                            

                            if(turnCnt==3)
                            {
                                setGreetingText(2);
                                turnCnt = 4;
                                
                                image.SetActive(true);
                                
                            }
                            
                            
                        }

                        
                        
                    }
                }
           
            
            
            }
        }

        else if(turnCnt==4)
        {
            endInformation();
        }
        else if(turnCnt==5)
        {
            db.SetValueAsync("nolocation");
            db = FirebaseDatabase.DefaultInstance.GetReference("waitlocation");
            db.SetValueAsync("no");
            LoadMenu();
        }
        


    }

    

    private void SetDestination()
    {
        if (_destination != null)
        {
            Vector3 targetVector = _destination.transform.position;            
            _navMeshAgent.SetDestination(targetVector);
            
        }
        

    }

  
    private void setRotation()
    {

        rot = startMarker.rotation.eulerAngles;
        rot2 = new Vector3(rot.x, rot.y + 180, rot.z);
        rotDestination = rot.y+(((rot2.y - rot.y) / 4) * 3);
        
    }
    
    public void changeRotation()
    {
       
        if (rot.y > rotDestination)
        {
            if (cntOnce == 1)
            {
               
                turnSpeed = turnSpeed * 3.5f;
                cntOnce = 0;
            }
            
                
        }
        rot = Vector3.Lerp(rot, rot2, turnSpeed);
        startMarker.rotation = Quaternion.Euler(rot);
        

    }




    public void setGreetingText(int situation)
    {
        if (situation == 1)
        {
            greetingText = "Merhaba, gitmek istediğiniz yer için yol gösterme sekansı başlatılıyor...";
        }
        else if (situation == 2)
        {
            greetingText = "Gitmek istediğiniz laboratuvar burası. Hoşçakalın...";
        }
    }


    IEnumerator startInformation()
    {
        image.SetActive(true);

        for (int i = 0; i < greetingText.Length; i++)
        {

            text.text += greetingText[i];
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.8f);
        text.text = "";
        turnCnt = 1;
        image.SetActive(false);


    }



    public void endInformation()
    {
        
        
        if(timerCnt==0)
        {
            textTimerStart = Time.time;
            timerCnt = 1;
        }
        if(textIterator==greetingText.Length)
        {
            if(timerCnt==1)
            {
                textTimer = Time.time;
                timerCnt = 2;
            }
            if((int)(Math.Abs(Time.time - textTimer))> 0.5f)
            {
              
                image.SetActive(false);
                turnCnt = 5;
            }
            
        }
        else
        {
            if (Math.Abs(Time.time - textTimerStart) > 0.1f)
            {
                text.text += greetingText[textIterator];
                textIterator += 1;
                textTimerStart = Time.time;
             
            }
           
        }
    
        
        


    }


    public void LoadMenu()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("UniversityPerspective", LoadSceneMode.Single);
        //Application.LoadLevel("UniversityPerspective");

    }

   

}
