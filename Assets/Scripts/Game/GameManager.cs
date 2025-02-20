using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public int minNumOfSoldiers = 3;
    public int maxNumOfSoldiers = 7;
    public int minNumOfTrees = 3;
    public int maxNumOfTrees = 7;
    public int soldierToWin  { get; private set; }
    public int treeCount  { get; private set; }
    private int playerScore = 0; 
    private bool playerWon = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); 
        soldierToWin = Random.Range(minNumOfSoldiers, maxNumOfSoldiers);
        treeCount = Random.Range(minNumOfTrees, maxNumOfTrees);
    }

    private void Update()
    {
        if (playerScore >= soldierToWin)  
            Debug.LogError("win");
        
        
         
    }
    
    public void AddScore(int score){playerScore += score;} 
    public void PlayerFailed() { StartCoroutine(endGameCoroutine());}

    IEnumerator endGameCoroutine()
    {
        yield return null;
        SceneManager.LoadScene(1);
    }
     
}

