using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutonomusMissle : MonoBehaviour
{
    private GameObject target {get; set;}
    private float initialLaunch, timeToManeuver, timeToLive, thrust, currentVelocity = 0;
    private Rigidbody rb;

    public void Initialize(GameObject newTarget, float launchTime, float maneuverTime, float liveTime, float thrustMagnitude)
    {
        target = newTarget;
        initialLaunch = launchTime;
        timeToManeuver = maneuverTime;
        timeToLive = liveTime;
        thrust = thrustMagnitude;
    }
    // Start is called before the first frame update
    void Start()
    { 
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        currentVelocity += Time.deltaTime * thrust;
        rb.AddForce(transform.up * currentVelocity);  
    }

    IEnumerator move()
    {
        float elapsedTime = 0, duration = initialLaunch;
        while (elapsedTime < duration)
        {
            elapsedTime+= Time.deltaTime;
        }
    }
}
