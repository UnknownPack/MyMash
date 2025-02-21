using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAA : MonoBehaviour
{
    [Header("detection")]
    [SerializeField] private float timeToDetect = 3f;
    [SerializeField] private float reloadTime = 5f;
    [SerializeField] private float detectionLineWidth = 0.25f;

    [Header("missile stats")] [SerializeField]
    private float initialLaunch;
    [SerializeField] private float timeToManeuver;
    [SerializeField] private float timeToLive;
    [SerializeField] private float thrust;
    [SerializeField] private float turnRate;
    [SerializeField] private float maxSpeed;
    
    [Header("prefabs")]
    [SerializeField] private GameObject missle;
    
    private float trackingTime = 0;
    private bool fired = false, track = true;
    private GameObject tracking;
    private LineRenderer lineRenderer;
    private AudioSource audioSource;
    private AudioSource anentaAudioSource;
    [Header("sounds")]
    [SerializeField] List<AudioClip> audioClip;
    
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();
        anentaAudioSource = transform.GetChild(0).gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tracking && !tracking.GetComponent<helicopterMovement>().scrambled )
        {
            if(anentaAudioSource.isPlaying == false)
                anentaAudioSource.Play();
            trackingTime += Time.deltaTime;  
            lineRenderer.SetPosition(0, transform.GetChild(0).transform.position);
            lineRenderer.SetPosition(1, tracking.transform.position);
            float progress = trackingTime / timeToDetect;
            progress = Mathf.Clamp(progress, 0f, timeToDetect);
            ManageLineWidth(detectionLineWidth * progress);
            ManageLineColor(Color.Lerp(Color.yellow, Color.red, progress));
        }
        else
        {
            ManageLineWidth(0);
            anentaAudioSource.Stop();
        }

        if (trackingTime >= timeToDetect && !fired)
        {  
            GameObject missile = Instantiate(missle, transform.GetChild(1).transform.position, transform.GetChild(1).transform.rotation);
            audioSource.clip = audioClip[1]; 
            audioSource.Play();
            missile.GetComponent<Rigidbody2D>().AddForce(transform.up * 5f, ForceMode2D.Impulse);
            missile.GetComponent<AutonomusMissle>().Initialize(tracking, initialLaunch, timeToManeuver, timeToLive, thrust, turnRate, maxSpeed);
            StartCoroutine(Reload());
            anentaAudioSource.Stop();
        }
        
         
    }

    IEnumerator Reload()
    {
        trackingTime = 0f;
        fired = true;
        track = false;
        yield return new WaitForSeconds(reloadTime);
        fired = false;
        track = true;
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && track)
        {  
            tracking = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            tracking = null;
            trackingTime = 0;
            lineRenderer.SetPosition(1, Vector2.zero);
            ManageLineWidth(0);
        }
    }

    void ManageLineWidth(float width)
    {
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }

    void ManageLineColor(Color color)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }
}
