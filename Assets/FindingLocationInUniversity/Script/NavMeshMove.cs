using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
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

    //private GameObject camObj;
    private Transform startMarker;
    private Quaternion startRotation;
    private Quaternion endRotation;
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
        

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://robotic-speech.firebaseio.com/");
        db = FirebaseDatabase.DefaultInstance.GetReference("location");


        string location=PlayerPrefs.GetString("LocationID").ToString();
        _location = GameObject.Find("location1");
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
                    
                        endInformation();
                        db.SetValueAsync("nolocation");
                        LoadMenu();
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

        }

    }
}
