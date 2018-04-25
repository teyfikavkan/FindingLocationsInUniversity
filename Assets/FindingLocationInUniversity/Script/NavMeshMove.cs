using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class NavMeshMove : MonoBehaviour {

    [SerializeField]

    Transform _destination ;
    GameObject _location;
    NavMeshAgent _navMeshAgent;
    string location;
    //private GameObject camObj;
    private Transform startMarker;
    private Quaternion startRotation;
    private Quaternion endRotation;
    private Quaternion endRotation2;
    public float turnSpeed= 0.002F;
    private int turnCnt;



    private DatabaseReference db;

    // Use this for initialization
    void Start()
    {
        turnCnt = 0;
        //camObj = GameObject.Find("CamParent");
        startMarker = GameObject.Find("CamParent").transform;
        startRotation = startMarker.rotation;
        endRotation = Quaternion.Euler(0, -90, 0);

        Debug.Log("Start Rotation :" + startRotation);
        Debug.Log("End Rotation :" + endRotation);


        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://robotic-speech.firebaseio.com/");
        db = FirebaseDatabase.DefaultInstance.GetReference("location");


         location=PlayerPrefs.GetString("LocationID").ToString();
        _location = GameObject.Find(location);
        _destination = _location.transform;
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        if (_navMeshAgent == null)
        {
            Debug.LogError("Erorrrr");
        }

        
    }


    void Update()
    {
        
        if(turnCnt==0)
        {

            changeRotation(1);
            if (startMarker.rotation==endRotation)
            {
                //Debug.Log("içerde");
                SetDestination();
                turnCnt = 1;
            }
            
        }
      
        else
        {
            Debug.Log("else");
            Transform _startdestination = _navMeshAgent.transform;
            if (_navMeshAgent.transform.position.x ==_destination.transform.position.x)
            {
           
                if (_navMeshAgent.transform.position.z == _destination.transform.position.z)
                {
               
                    if (_navMeshAgent.transform.position.y == _destination.transform.position.y)
                    {
                        if(turnCnt==1)
                        {
                            
                            startRotation = startMarker.rotation;
                            //endRotation2 = Quaternion.Euler(0, -180, 0);
                            setRotation(location);

                            //endRotation2.y = -startRotation.y;
                            Debug.Log("Start Rotation :" + startRotation);
                            Debug.Log("End Rotation :" + endRotation2);
                            
                            turnCnt = 2;
                        }
                        
                        changeRotation(2);
                        
                        if (startMarker.rotation == endRotation2)
                        {
                            endInformation();
                            db.SetValueAsync("nolocation");
                            LoadMenu();
                        }

                        
                        
                    }
                }
           
            
            
            }
        }
        


    }

    private void setRotation(String location)
    {
        
        
        if (location.Equals("location1"))
        {
            endRotation2 = Quaternion.Euler(0, -180, 0);
        }
        else if(location.Equals("location3"))
        {
            endRotation2 = Quaternion.Euler(0, -180, 0);
        }
        else if (location.Equals("location5"))
        {
            endRotation2 = Quaternion.Euler(0, -180, 0);
        }
        else if (location.Equals("location7"))
        {
            endRotation2 = Quaternion.Euler(0, -180, 0);
        }


        else if (location.Equals("location2"))
        {
            endRotation2 = Quaternion.Euler(0, 0, 0);
        }
        else if (location.Equals("location4"))
        {
            endRotation2 = Quaternion.Euler(0, 0, 0);
        }
        else if (location.Equals("location6"))
        {
            endRotation2 = Quaternion.Euler(0, 0, 0);
        }
        else if (location.Equals("location8"))
        {
            endRotation2 = Quaternion.Euler(0, 0, 0);
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

    public void LoadMenu()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("UniversityPerspective", LoadSceneMode.Single);
        //Application.LoadLevel("UniversityPerspective");
        
    }

    public void startInformation()
    {
        //TODO başlangıç için bilgi verecek
    }

    public void endInformation()
    {
        //TODO son için bilgi verecek
    }

    public void changeRotation(int situation)
    {
        if(situation==1)
        {
           
            startMarker.rotation = Quaternion.Lerp(startRotation, endRotation, turnSpeed);
            startRotation = startMarker.rotation;

        }
        else if(situation==2)
        {
            startMarker.rotation = Quaternion.Lerp(startRotation, endRotation2, turnSpeed);
            startRotation = startMarker.rotation;
        }

    }
}
