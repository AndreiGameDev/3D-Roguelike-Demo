using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UI_Death : MonoBehaviour {
    UIDocument doc;
    VisualElement root;
    Button b_Retry;
    Button b_BackToMainMenu;
    Button b_Quit;
    Label l_Score;
    private void Start() {
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        UnityEngine.Cursor.visible = true;

        doc = GetComponent<UIDocument>();
        root = doc.rootVisualElement;

        b_Retry = root.Q<Button>("B_Retry");
        b_BackToMainMenu = root.Q<Button>("B_BackToMainMenu");
        b_Quit = root.Q<Button>("B_Quit");
        
        b_Retry.clicked += () => SceneManager.LoadScene("PlayerLevel");
        b_BackToMainMenu.clicked += () => SceneManager.LoadScene("MainMenuScene");
        b_Quit.clicked += () => Application.Quit();

        l_Score = root.Q<Label>("L_Score");
        l_Score.text = "Score: " + GameManager.Instance.Score.ToString();
    }
}
