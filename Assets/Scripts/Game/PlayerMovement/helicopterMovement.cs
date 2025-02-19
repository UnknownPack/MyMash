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
    [SerializeField] float maxSpeed = 7f;

    [Header("Inventory")] 
    [SerializeField] private int maxPassengerCapactiy;
    [SerializeField] private int currentPassengerCapactiy = 0;
    [SerializeField] private int maxFlaresCapactiy;
    [SerializeField] private int currentFlaresCapactiy = 0; 
    
    [Header("Abilites")]
    [SerializeField] private float flareDuration = 6f;

    private bool canDeployFlare;
    private bool scrambled;
    private bool onBase = false;
    private GameObject onSoldier;

    private Rigidbody2D rigidbody2D;
    private GameStateManager gameStateManager;
    private PlayerInput PlayerInput;
    private InputAction moveAction, flareAction, depositAction;
    private Vector2 currentVector;
    // Start is called before the first frame update
    void Start()
    {
        PlayerInput = GetComponent<PlayerInput>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        gameStateManager = FindObjectOfType<GameStateManager>();
        
        moveAction = PlayerInput.actions.FindAction("Move");
        flareAction = PlayerInput.actions.FindAction("Flare");
        depositAction = PlayerInput.actions.FindAction("Deposit");
        flareAction.performed += OnFlareAction;
        depositAction.performed += OnDepositAction; 
    }

    // Update is called once per frame
    void Update()
    { 
        currentVector = moveAction.ReadValue<Vector2>(); 
        roterVolume = currentVector.magnitude; 
        rigidbody2D.AddForce(transform.up * (thurst * currentVector.y));  
        TiltAngle();      
    } 
    
    void FixedUpdate()
    { 
        if (rigidbody2D.velocity.magnitude > maxSpeed)
        {
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * maxSpeed;
        }
    }
 
    private void TiltAngle()
    {
        // Determine the target tilt angle based on input direction
        float targetAngleZ = 0f;
    
        if (currentVector.x > 0)
        {
            targetAngleZ = -tiltAngle; // Tilt right (negative Z)
        }
        else if (currentVector.x < 0)
        {
            targetAngleZ = tiltAngle; // Tilt left (positive Z)
        }

        // Convert to rotation
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngleZ);

        // Smoothly rotate towards target tilt
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * shiftDuration);
    }

    
    private void OnFlareAction(InputAction.CallbackContext context) { StartCoroutine(DeployFlare()); } 
    private void OnDepositAction(InputAction.CallbackContext context)
    {
        if(onBase)
        {
            gameStateManager.AddScore(currentPassengerCapactiy);
            currentPassengerCapactiy = 0;
            // Play sound effect here
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        onBase = other.CompareTag("Base");
        if (other.CompareTag("Soldier"))
        {
            if(currentPassengerCapactiy < maxPassengerCapactiy )
            {
                currentPassengerCapactiy++;
                Destroy(other.gameObject); 
            }
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tree"))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator DeployFlare()
    {
        scrambled = true;
        //emmit particles here
        yield return new WaitForSeconds(flareDuration);
        scrambled = false;
    }
    
    void OnDestroy()
    {
        flareAction.performed -= OnFlareAction;
        depositAction.performed -= OnDepositAction;
        gameStateManager.PlayerFailed();
    }
 
}
