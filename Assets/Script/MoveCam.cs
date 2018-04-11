using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveCam : MonoBehaviour
{
    public Transform startMarker;
    public Transform endMarker;
    public float speed = 1.0F;
    private float startTime;
    private float journeyLength;
    private Quaternion startRotation;
    private Quaternion endRotation;

    void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        startRotation = startMarker.rotation;
        endRotation = Quaternion.Euler(0,0,0);


    }

    // Update is called once per frame
    void Update()
    {
        
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
        startMarker.position = transform.position;
        
        transform.rotation = Quaternion.Lerp(startRotation, endRotation, distCovered*11);
        startMarker.rotation = transform.rotation;
        
        //Debug.Log(startMarker.rotation);

    }

}
