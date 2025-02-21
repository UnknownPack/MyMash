using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UiManager : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _base, gameUI, gameOverUI;
    private Label soldiersRescued, soldiersInside, soldiersOutside, flareStatus, resultText;
    private ProgressBar resultProgressBar, effectBar;
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
        effectBar = root.Q<ProgressBar>("effectBar");
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
            //resultProgressBar.style.display = helicopterMovement.canDeployFlare ? DisplayStyle.None : DisplayStyle.Flex;
            resultProgressBar.title = helicopterMovement.canDeployFlare ?"Flares Ready" : "Recharging Flares...";
            resultProgressBar.style.color = helicopterMovement.canDeployFlare ? new Color(0, 153, 51, 255) : Color.yellow;
            resultProgressBar.value = helicopterMovement.canDeployFlare ?  helicopterMovement.flareRechargeDuration :  helicopterMovement.currentRechargeTime;
            resultProgressBar.highValue = helicopterMovement.flareRechargeDuration; 
            
            effectBar.style.display = helicopterMovement.scrambled ? DisplayStyle.Flex : DisplayStyle.None;
            effectBar.title = helicopterMovement.scrambled ? "scrambling..." : " ";
            effectBar.value = helicopterMovement.currentEffectTime;
            effectBar.highValue = helicopterMovement.flareDuration; 
             
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
