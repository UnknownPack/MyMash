using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    
    [Header("Abilites")]
    [SerializeField] private float flareDuration = 6f;
    [SerializeField] private float flareRechargeDuration = 15f;

    private bool canDeployFlare = true;
    public bool scrambled;
    private bool onBase = false;
    private GameObject onSoldier;
    private ParticleSystem flare;

    private Rigidbody2D rigidbody2D;
    private GameStateManager gameStateManager;
    private PlayerInput PlayerInput;
    private InputAction moveAction, flareAction, depositAction, restartAction;
    private Vector2 currentVector;
    // Start is called before the first frame update
    void Start()
    {
        PlayerInput = GetComponent<PlayerInput>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        gameStateManager = FindObjectOfType<GameStateManager>();
        flare = GetComponent<ParticleSystem>(); 
        moveAction = PlayerInput.actions.FindAction("Move");
        flareAction = PlayerInput.actions.FindAction("Flare");
        depositAction = PlayerInput.actions.FindAction("Deposit");
        restartAction = PlayerInput.actions.FindAction("Restart");
        flareAction.performed += OnFlareAction;
        depositAction.performed += OnDepositAction; 
        restartAction.performed += OnRestartAction;
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
    
    IEnumerator recharge()
    {
        canDeployFlare = false; 
        float flareDuration = flareRechargeDuration, elapsedTime = 0f;
        while (elapsedTime < flareDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        } 
        canDeployFlare = true;
    }

    
    private void OnFlareAction(InputAction.CallbackContext context) 
    {
        if(canDeployFlare)
        { 
            StartCoroutine(DeployFlare());
            StartCoroutine(recharge());
        }
    } 
    private void OnDepositAction(InputAction.CallbackContext context)
    {
        if(onBase)
        {
            gameStateManager.AddScore(currentPassengerCapactiy);
            currentPassengerCapactiy = 0;
            // Play sound effect here
        }
    }
    private void OnRestartAction(InputAction.CallbackContext context){ SceneManager.LoadScene(1); }

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
        flare.Play();
        yield return new WaitForSeconds(flareDuration); 
        scrambled = false;
    }
    
    void OnDestroy()
    {
        if(flareAction==null)
            Debug.LogError("flareAction is null");
        else
            flareAction.performed -= OnFlareAction;
        
        if(depositAction==null)
            Debug.LogError("depositAction is null");
        else
            depositAction.performed -= OnDepositAction;
        
        gameStateManager.PlayerFailed();
    }
 
}
