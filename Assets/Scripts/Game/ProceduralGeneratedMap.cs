using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProceduralGeneratedMap : MonoBehaviour
{
    [SerializeField] private int mapLength = 34;
    [SerializeField] private int mapHeight = 18;
    [SerializeField] private int maxBaseNumber = 2;
    [SerializeField] private int xPosForSeperation = -12;
    [SerializeField] private float distanceBetween = 0.5f;
    [SerializeField] private float distanceTolerance = 3.75f;
    
    private int currentNumOfBaseNumber = 0, currentNumOfSoldiers = 0, currentNumOfTrees = 0;
    
    [SerializeField] GameObject soldierPrefab;
    [SerializeField] GameObject BasePrefab;
    [SerializeField] GameObject treePrefab;
    private int maxSoldierCount = 0, maxTreesCount = 0;
    List<Vector2> positionList = new List<Vector2>();
    
    private GameStateManager gameStateManager;
 

    void Start()
    {
        gameStateManager = FindObjectOfType<GameStateManager>();
        maxSoldierCount = gameStateManager.soldierToWin;
        maxTreesCount = gameStateManager.treeCount;
        positionList = GenerateList().OrderBy(_ => Random.value).ToList();
         
        GenerateBase();
        GenerateSoldier();
        GenerateTrees(); 
    } 

    List<Vector2> GenerateList()
    {
        List<Vector2> list = new List<Vector2>();
        for (float x = xPosForSeperation+4; x <= mapLength; x+=distanceBetween)
        {
            for (float i = xPosForSeperation+4; i <= mapHeight; i+=distanceBetween)
            {
                list.Add(new Vector2(x, i));
            }
        }
        return list;
    }
    
    // gives a vector2 from list and removes nearby positons from list
    Vector2 giveVector2Position()
    { 
        int index = Random.Range(0, positionList.Count);
        Vector2 selectedPosition = positionList[index]; 
        positionList.RemoveAll(pos => Vector2.Distance(pos, selectedPosition) <= distanceTolerance);
        return selectedPosition;
    }


    void GenerateBase()
    { 
        int randomX = Random.Range(-28, -12);
        int randomY = Random.Range(-18, 6);
        Instantiate(BasePrefab, new Vector2(randomX, randomY), Quaternion.identity); 
    }

    void GenerateSoldier()
    {
        for (int y = 0; y < maxSoldierCount; y++)
        {
            if(positionList.Count <= 0)
            {
                Debug.LogError("no more valid positions");
                break;
            }
            Vector2 position = giveVector2Position();
            Instantiate(soldierPrefab, position, Quaternion.identity);
        }
    }

    void GenerateTrees()
    {
        for (int y = 0; y < maxTreesCount; y++)
        {
            if(positionList.Count <= 0)
            {
                Debug.LogError("no more valid positions");
                break;
            }
            Vector2 position = giveVector2Position();
            Instantiate(treePrefab, position, Quaternion.identity);
        }
    }
}
