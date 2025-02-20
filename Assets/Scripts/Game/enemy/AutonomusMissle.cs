using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutonomusMissle : MonoBehaviour
{
    public GameObject explosionPrefab;
    private GameObject target {get; set;}
    private float initialLaunch, timeToManeuver, timeToLive, thrust, turnRate, maxSpeed, currentVelocity = 0, lifeClock = 0;
    private bool track = false;
    private Coroutine trackCoroutine;
    private Rigidbody2D rb;
    private helicopterMovement movement;

    public void Initialize(GameObject newTarget, float launchTime, float maneuverTime, float liveTime, float thrustMagnitude, float turnRate, float maxSpeed)
    {
        target = newTarget;
        initialLaunch = launchTime;
        timeToManeuver = maneuverTime;
        timeToLive = liveTime;
        thrust = thrustMagnitude;
        this.turnRate = turnRate;
        this.maxSpeed = maxSpeed;
    }
    // Start is called before the first frame update
    void Start()
    { 
        rb = GetComponent<Rigidbody2D>();
        trackCoroutine = StartCoroutine(move());
        movement = FindObjectOfType<helicopterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        currentVelocity += Time.deltaTime * thrust;
        currentVelocity = Mathf.Clamp(currentVelocity, 0, maxSpeed);
        lifeClock += Time.deltaTime;
        rb.AddForce(transform.up * currentVelocity); 
        
        if(lifeClock >= timeToLive)
        {
            StopCoroutine(trackCoroutine);
            trackCoroutine = null;
            Destroy(gameObject);
        }
        
        if(track && movement!=null && !movement.scrambled)
            trackTarget(target.transform.position);   
    }

    IEnumerator move()
    { 
        yield return new WaitForSeconds(initialLaunch);
        track = true;
        yield return new WaitForSeconds(timeToManeuver);
        track = false;
    }

    void trackTarget(Vector3 targetPos)
    {
        if (target == null) return;
        Vector3 direction = targetPos - transform.position ;
        // Calculate angle in degrees, using atan2
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg- 90f;
        Quaternion targetRotation = Quaternion.Euler(0.0f, 0.0f, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnRate);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject); 
        } 
        Destroy(gameObject);  
    }

    private void OnDestroy()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}
