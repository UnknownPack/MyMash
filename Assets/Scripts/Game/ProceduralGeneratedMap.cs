using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneratedMap : MonoBehaviour
{
    [SerializeField] private int mapLength = 9;
    [SerializeField] private int mapHeight = 5;
    [SerializeField] private int baseNumber = 2;
    
    [SerializeField] GameObject soldierPrefab;
    [SerializeField] GameObject BasePrefab;
    private int soldiderNumber;
    
    private GameStateManager gameStateManager;
 

    void Start()
    {
        gameStateManager = FindObjectOfType<GameStateManager>();
        soldiderNumber = gameStateManager.soldierToWin;
        Instantiate(soldierPrefab, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateBase()
    {
        
    }

    void GenerateSoldier()
    {
        
    }

    void GenerateTrees()
    {
        
    }
}
