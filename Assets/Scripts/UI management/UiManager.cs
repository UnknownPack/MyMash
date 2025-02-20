using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UiManager : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _base, gameUI, gameOverUI;
    private Label soldiersRescued, soldiersInside, soldiersOutside, flareStatus, resultText;
    private ProgressBar resultProgressBar;
    private Button restartButton, menuButton;
    
    private helicopterMovement helicopterMovement;
    private GameStateManager gameStateManager;
     
    
    // Start is called before the first frame update
    void Start()
    {
        _uiDocument = GetComponent<UIDocument>();
        var root = _uiDocument.rootVisualElement;
        _base = root.Q<VisualElement>("base");
        
        gameUI = root.Q<VisualElement>("GameUI");
        soldiersRescued = root.Q<Label>("SR");
        soldiersInside = root.Q<Label>("SI");
        soldiersOutside = root.Q<Label>("SOF");
        flareStatus = root.Q<Label>("FlareStatus");
        resultProgressBar = root.Q<ProgressBar>("bar");
        gameUI.style.display = DisplayStyle.Flex;
            
        gameOverUI = root.Q<VisualElement>("GameOverUI");
        resultText = root.Q<Label>("ResultText");
        root.Q<Button>("restart").clicked += () => {  Time.timeScale = 1f; UnityEngine.SceneManagement.SceneManager.LoadScene(1); };
        root.Q<Button>("menu").clicked += () => {  Time.timeScale = 1f; UnityEngine.SceneManagement.SceneManager.LoadScene(0); };
        gameOverUI.style.display = DisplayStyle.None;
        
        helicopterMovement = FindObjectOfType<helicopterMovement>();
        gameStateManager = FindObjectOfType<GameStateManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (helicopterMovement != null )
        {
            soldiersInside.text = $"{helicopterMovement.GetCurrentCapacity()}/{helicopterMovement.GetMaxCapacity()} soldiers inside";
            resultProgressBar.style.display = helicopterMovement.canDeployFlare ? DisplayStyle.None : DisplayStyle.Flex;
            resultProgressBar.value = helicopterMovement.canDeployFlare ?  helicopterMovement.flareRechargeDuration :  helicopterMovement.currentRechargeTime;
            resultProgressBar.highValue = helicopterMovement.flareRechargeDuration; 
            flareStatus.text = helicopterMovement.canDeployFlare ? "Flares Ready" : "Recharging Flares...";
            flareStatus.style.color = helicopterMovement.canDeployFlare ? Color.green : Color.yellow;
        } 
        
        if (gameStateManager != null )
        { 
            soldiersRescued.text = $"{gameStateManager.playerScore} soldiers rescued";
            soldiersOutside.text = $"{gameStateManager.soldierToWin - gameStateManager.playerScore} soldiers remaining";
        } 
    }

    public void DisplayEndGame()
    {
        gameUI.style.display = DisplayStyle.None;
        gameOverUI.style.display = DisplayStyle.Flex;
    }

    public void SetEndGameMessage(string message)
    {
        resultText.text = message;
    }

    public VisualElement GetBase() { return gameOverUI; }

}
