using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainScreenManager : MonoBehaviour
{
    private UIDocument uiDocument;
    private Button startButton;
    // Start is called before the first frame update
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        startButton = uiDocument.rootVisualElement.Q<Button>("startButton");
        startButton.RegisterCallback<ClickEvent>(StartGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    } 

    private void StartGame(ClickEvent evt)
    {
        StartCoroutine(loadScreen());
    }

    IEnumerator loadScreen()
    {
        yield return null;
        SceneManager.LoadScene(1);
    }
}
