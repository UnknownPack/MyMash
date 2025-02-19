using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public int minNumOfSoldiers = 3;
    public int maxNumOfSoldiers = 7;
    public int soldierToWin  { get; private set; }
    private int playerScore = 0; 
    private bool playerWon = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        soldierToWin = Random.Range(minNumOfSoldiers, maxNumOfSoldiers);
    }

    private void Update()
    {
        //if (playerScore >= soldierToWin)  
        
        
         
    }
    
    public void AddScore(int score){playerScore += score;} 
    public void PlayerFailed() { StartCoroutine(endGameCoroutine());}

    IEnumerator endGameCoroutine()
    {
        yield return null;
        SceneManager.LoadScene(0);
    }
     
}

