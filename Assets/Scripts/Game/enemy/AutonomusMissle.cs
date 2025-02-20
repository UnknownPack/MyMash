using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutonomusMissle : MonoBehaviour
{
    private GameObject target {get; set;}
    private float initialLaunch, timeToManeuver, timeToLive, thrust, turnRate, currentVelocity = 0, lifeClock = 0;
    private bool track = false;
    private Coroutine trackCoroutine;
    private Rigidbody2D rb;

    public void Initialize(GameObject newTarget, float launchTime, float maneuverTime, float liveTime, float thrustMagnitude, float turnRate)
    {
        target = newTarget;
        initialLaunch = launchTime;
        timeToManeuver = maneuverTime;
        timeToLive = liveTime;
        thrust = thrustMagnitude;
        this.turnRate = turnRate;
    }
    // Start is called before the first frame update
    void Start()
    { 
        rb = GetComponent<Rigidbody2D>();
        trackCoroutine = StartCoroutine(move());
    }

    // Update is called once per frame
    void Update()
    {
        currentVelocity += Time.deltaTime * thrust;
        lifeClock += Time.deltaTime;
        rb.AddForce(transform.up * currentVelocity); 
        
        if(lifeClock >= timeToLive)
        {
            StopCoroutine(trackCoroutine);
            trackCoroutine = null;
            Destroy(gameObject);
        }
        if(track)
            trackTarget();
    }

    IEnumerator move()
    { 
        yield return new WaitForSeconds(initialLaunch);
        track = true;
        yield return new WaitForSeconds(timeToManeuver);
        track = false;
    }

    void trackTarget()
    {
        if (target == null) return;
        Vector3 direction = target.transform.position - transform.position;
        // Calculate angle in degrees, using atan2
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0.0f, 0.0f, angle); 
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnRate);
    }
}
