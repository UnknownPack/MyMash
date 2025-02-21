using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public int minNumOfSoldiers = 3;
    public int maxNumOfSoldiers = 7;
    public int minNumOfTrees = 3;
    public int maxNumOfTrees = 7;
    public int soldierToWin  { get; private set; }
    public int treeCount  { get; private set; }
    public GameObject explosion;
    private GameObject player;
    private Vector3 playerPos;
    public int playerScore = 0; 
    private bool playerWon = false;
    private UiManager mainGameUi; 
    private Coroutine coroutine = null;
    private bool playerDead = false;
    private InputAction restartAction;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); 
        soldierToWin = Random.Range(minNumOfSoldiers, maxNumOfSoldiers);
        treeCount = Random.Range(minNumOfTrees, maxNumOfTrees);
    }

    private void Start()
    {
        player = FindObjectOfType<helicopterMovement>().gameObject;
        playerPos = player.transform.position;
        mainGameUi = FindObjectOfType<UiManager>(); 
        var playerInput = GetComponent<PlayerInput>();
        restartAction = playerInput.actions.FindAction("Restart");
        restartAction.Enable(); 
        restartAction.performed += OnRestartAction;
    }

    private void Update()
    {
        if (playerScore >= soldierToWin) {
            if (coroutine == null)
            {
                coroutine = StartCoroutine(endGameCoroutine("You Win"));
            }
        }

        if (player != null)
            playerPos = player.transform.position;
        
        if (player == null)
        {
            if (coroutine == null)
            {
                playerDead = true;
                coroutine = StartCoroutine(endGameCoroutine("Game Over"));
            }
        }
    }
    
    public void AddScore(int score){
        GetComponent<AudioSource>().Play(); 
        playerScore += score; 
    } 

    IEnumerator endGameCoroutine(string message)
    {
        if(playerDead)
            Instantiate(explosion, playerPos, Quaternion.identity);
        Time.timeScale = 0;
        mainGameUi.GetBase().style.backgroundColor = Color.white;
        float duration = 0.5f, elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledTime;
            mainGameUi.GetBase().style.backgroundColor = Color.Lerp(Color.white, Color.black, elapsedTime / duration);
            yield return null;
        }
        mainGameUi.GetBase().style.backgroundColor = Color.black;
        mainGameUi.DisplayEndGame();
        mainGameUi.SetEndGameMessage(message);
    }

    private void OnRestartAction(InputAction.CallbackContext context)
    {
        if(playerWon || playerDead)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(1);
        }
    }
     
}

