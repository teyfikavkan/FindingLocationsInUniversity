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

    DatabaseReference db;

    // Use this for initialization
    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://robotic-speech.firebaseio.com/");
        db = FirebaseDatabase.DefaultInstance.GetReference("location");


        string location=PlayerPrefs.GetString("LocationID").ToString();
        _location = GameObject.Find(location);
        _destination = _location.transform;
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        if (_navMeshAgent == null)
        {
            Debug.LogError("Erorrrr");
        }

        else
        {
            startInformation();
            SetDestination();
        }
    }



    // Update is called once per frame
    void Update()
    {
        Debug.Log("Target :"+_destination.transform.position);
        Debug.Log("Start :" + _navMeshAgent.transform.position);
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


    private void SetDestination()
    {
        if (_destination != null)
        {
            Vector3 targetVector = _destination.transform.position;
            //Debug.Log(targetVector);
            
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
}
