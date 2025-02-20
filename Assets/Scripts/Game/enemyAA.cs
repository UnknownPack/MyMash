using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAA : MonoBehaviour
{
    [SerializeField] private float timeToDetect = 3f;
    [SerializeField] private float timeToFire = 1.5f;
    [SerializeField] private float detectionLineWidth = 0.25f;
    private float trackingTime = 0;
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
            
        }
        
        
        Debug.Log("updating");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
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
