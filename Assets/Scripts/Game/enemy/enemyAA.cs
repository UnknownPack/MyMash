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
    
    [Header("prefabs")]
    [SerializeField] private GameObject missle;
    
    private float trackingTime = 0;
    private bool fired = false, track = true;
    private GameObject tracking;
    private LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tracking)
        {
            trackingTime += Time.deltaTime; 
            lineRenderer.SetPosition(0, transform.GetChild(0).transform.position);
            lineRenderer.SetPosition(1, tracking.transform.position);
            float progress = trackingTime / timeToDetect;
            progress = Mathf.Clamp(progress, 0f, timeToDetect);
            ManageLineWidth(detectionLineWidth * progress);
            ManageLineColor(Color.Lerp(Color.yellow, Color.red, progress));
        }

        if (trackingTime >= timeToDetect)
        {
            Instantiate(missle, transform.GetChild(1).transform.position, transform.GetChild(1).transform.rotation);
            StartCoroutine(Reload());
        }
        
         
    }

    IEnumerator Reload()
    {
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
            Debug.Log(other.name);
            tracking = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        tracking = null;
        trackingTime = 0; 
        lineRenderer.SetPosition(1, Vector2.zero);
        ManageLineWidth(0);
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
