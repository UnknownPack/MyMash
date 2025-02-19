using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProceduralGeneratedMap : MonoBehaviour
{
    [SerializeField] private int mapLength = 34;
    [SerializeField] private int mapHeight = 18;
    [SerializeField] private int maxBaseNumber = 2;
    [SerializeField] private int xPosForSeperation = -12;
    
    private int currentNumOfBaseNumber = 0, currentNumOfSoldiers = 0, currentNumOfTrees = 0;
    
    [SerializeField] GameObject soldierPrefab;
    [SerializeField] GameObject BasePrefab;
    [SerializeField] GameObject treePrefab;
    private int maxSoldierCount = 0, maxTreesCount = 0;
    
    private GameStateManager gameStateManager;
 

    void Start()
    {
        gameStateManager = FindObjectOfType<GameStateManager>();
        maxSoldierCount = gameStateManager.soldierToWin;
        maxTreesCount = gameStateManager.treeCount;
        GenerateBase();
        GenerateSoldier();
        GenerateTrees();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateBase()
    {
        for (int y = 0; y < maxBaseNumber; y++)
        {
            float randomX = Random.Range(-34f, -12f);
            float randomY = Random.Range(-18f, 18f);
            Instantiate(BasePrefab, new Vector3(randomX, randomY, 0), Quaternion.identity);
        }  
    }

    void GenerateSoldier()
    {
        for (int y = 0; y < maxSoldierCount; y++)
        {
            float randomX = Random.Range(-10f, 34f);
            float randomY = Random.Range(-10f, 10f);
            Instantiate(soldierPrefab, new Vector3(randomX, randomY, 0), Quaternion.identity);
        }
    }

    void GenerateTrees()
    {
        for (int y = 0; y < maxTreesCount; y++)
        {
            float randomX = Random.Range(-10f, 34f);
            float randomY = Random.Range(-10f, 10f);
            Instantiate(treePrefab, new Vector3(randomX, randomY, 0), Quaternion.identity);
        }
    }
}
