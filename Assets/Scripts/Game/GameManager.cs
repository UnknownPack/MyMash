using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private int playerScore = 0; 
    private bool playerWon = false;

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
    }

    private void Update()
    {
        if (playerScore >= soldierToWin)  
            Debug.LogError("win");

        if (player != null)
            playerPos = player.transform.position;
         
    }
    
    public void AddScore(int score){playerScore += score;} 
    public void PlayerFailed() { StartCoroutine(endGameCoroutine());}

    IEnumerator endGameCoroutine()
    {
        Instantiate(explosion, playerPos, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
    }
     
}

