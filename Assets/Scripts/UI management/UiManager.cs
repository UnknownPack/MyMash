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
        root.Q<Button>("restart").clicked += () => { UnityEngine.SceneManagement.SceneManager.LoadScene(1); };
        root.Q<Button>("menu").clicked += () => { UnityEngine.SceneManagement.SceneManager.LoadScene(0); };
        gameOverUI.style.display = DisplayStyle.None;
        
        helicopterMovement = FindObjectOfType<helicopterMovement>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
