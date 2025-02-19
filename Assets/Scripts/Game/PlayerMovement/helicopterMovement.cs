using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class helicopterMovement : MonoBehaviour
{
    [Header("Movement")] 
    [SerializeField] private float tiltAngle = 60f;
    [SerializeField] private float shiftDuration = 15f;
    [SerializeField] private float roterVolume = 0.0f;
    [SerializeField] float thurst = 1f;
    
    private Rigidbody2D rigidbody2D;
    private PlayerInput PlayerInput;
    private InputAction moveAction;
    private Vector2 currentVector;
    // Start is called before the first frame update
    void Start()
    {
        PlayerInput = GetComponent<PlayerInput>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        moveAction = PlayerInput.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    { 
        currentVector = moveAction.ReadValue<Vector2>(); 
        roterVolume = currentVector.magnitude; 
        rigidbody2D.AddForce(transform.up * (thurst * currentVector.y));  
        TiltAngle(); 
    } 

    private void TiltAngle()
    {
        float targetAngleZ = 0f;
        if (currentVector.x > 0)
        {
            targetAngleZ = -shiftDuration; // Tilt right
        }
        else if (currentVector.x < 0)
        {
            targetAngleZ = shiftDuration; // Tilt left
        } 

        var clamp = Mathf.Clamp(targetAngleZ, -tiltAngle, tiltAngle);
        // Smoothly interpolate to the target angle
        Quaternion targetRotation = Quaternion.Euler(0, 0, clamp * 10f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * shiftDuration); 
    } 
 
}
