using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class helicopterMovement : MonoBehaviour
{
    [SerializeField] private float thurst = 1f;
    private float tiltAngle = 60f;
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
        Debug.Log(currentVector);
        ManageHelicopterTilt();
        ManageVerticalBehavior();
    }

    private void ManageHelicopterTilt()
    {
        float xValue = currentVector.x, yValue = currentVector.y, zRotation = transform.rotation.z;
        float tiltPercentage = xValue * zRotation;
        Quaternion target = Quaternion.Euler(transform.rotation.x, transform.rotation.y, tiltPercentage);
        transform.rotation = Quaternion.Slerp(transform.rotation, target,  Time.deltaTime * 2f);
        
        
    }

    private void ManageVerticalBehavior()
    { 
        rigidbody2D.AddForce(new Vector2(0, currentVector.y) * thurst, ForceMode2D.Force);
    }

    
}
