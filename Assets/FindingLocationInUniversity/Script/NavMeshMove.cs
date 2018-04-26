using Firebase;
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

    Transform _destination ;
    GameObject _location;
    NavMeshAgent _navMeshAgent;
    string location;
    private Transform startMarker;
    private Quaternion startRotation;
    private Quaternion endRotation;
    private Quaternion endRotation2;
    public float turnSpeed= 0.002F;
    private int turnCnt;

    private GameObject image;
    public Text text;
    private string greetingText;

    private DatabaseReference db;

    // Use this for initialization
    void Start()
    {
        turnCnt = 0;

        image = GameObject.Find("SpeechBubbleImg");
        image.SetActive(false);
       
        startMarker = GameObject.Find("CamParent").transform;
        startRotation = startMarker.rotation;
        endRotation = Quaternion.Euler(0, -90, 0);

        //başlangıçta baloncuğun içindeki yazıyı ayarlama ve yazıyı bastırma.
        setGreetingText(1);
        StartCoroutine(startInformation());

        //database bağlantısı için
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://robotic-speech.firebaseio.com/");
        db = FirebaseDatabase.DefaultInstance.GetReference("location");


         location=PlayerPrefs.GetString("LocationID").ToString();
        //location = "location6";
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
        
        if(turnCnt==1)
        {

            changeRotation(1);
            //if (startRotation == endRotation)
            if(Math.Abs(Math.Abs(endRotation.y) - Math.Abs(startRotation.y)) < 0.01)
            {
                //Debug.Log("Start Rotation :" + startMarker.rotation);
                //Debug.Log("End Rotation :" + endRotation);
                SetDestination();
                turnCnt = 2;
            }
            
        }
      
        else if(turnCnt==2 || turnCnt==3)
        {
            
            Transform _startdestination = _navMeshAgent.transform;
            if (_navMeshAgent.transform.position.x ==_destination.transform.position.x)
            {
           
                if (_navMeshAgent.transform.position.z == _destination.transform.position.z)
                {
               
                    if (_navMeshAgent.transform.position.y == _destination.transform.position.y)
                    {
                        if(turnCnt==2)
                        {
                            
                            startRotation = startMarker.rotation;
                            //endRotation2 = Quaternion.Euler(0, -180, 0);
                            setRotation(location);
                            turnCnt = 3;
                        }

                        Debug.Log("Start Rotation :" + startMarker.rotation);
                        Debug.Log("End Rotation :" + endRotation2);
                        changeRotation(2);
                        
                        //if ((startMarker.rotation.y + endRotation2.y)==0)
                        if (Math.Abs(Math.Abs(endRotation2.y)-Math.Abs(startRotation.y))<0.01)
                        {
                            //Debug.Log("Start Rotation :" + startMarker.rotation);
                            //Debug.Log("End Rotation :" + endRotation2);
                            endInformation();
                            db.SetValueAsync("nolocation");
                            LoadMenu();
                        }

                        
                        
                    }
                }
           
            
            
            }
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

  
    private void setRotation(String location)
    {


        if (location.Equals("location1"))
        {
            endRotation2 = Quaternion.Euler(0, -180, 0);
        }
        else if (location.Equals("location3"))
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




    public void setGreetingText(int situation)
    {
        if (situation == 1)
        {
            greetingText = "Merhaba";
        }
        else if (situation == 2)
        {
            greetingText = "Merhaba,ben deneme text bu bir denemedir iki üç bir iki dört";
        }
    }


    IEnumerator startInformation()
    {
        image.SetActive(true);

        for (int i = 0; i < greetingText.Length; i++)
        {

            text.text += greetingText[i];
            yield return new WaitForSeconds(0.4f);
        }

        turnCnt = 1;
        image.SetActive(false);


    }


    public void endInformation()
    {
        //TODO son için bilgi verecek
    }


    public void LoadMenu()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("UniversityPerspective", LoadSceneMode.Single);
        //Application.LoadLevel("UniversityPerspective");

    }

}
