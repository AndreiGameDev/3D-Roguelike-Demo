using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UI_FinishedLevelScript : MonoBehaviour
{
    UIDocument doc;
    VisualElement root;
    Button b_PlayAgain;
    Button b_BackToMainMenu;
    Button b_Quit;
    Label l_Score;
    GameManager manager;
    private void Start() {
        manager = GameManager.Instance;
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        UnityEngine.Cursor.visible = true;
        doc = GetComponent<UIDocument>();
        root = doc.rootVisualElement;
        b_PlayAgain = root.Q<Button>("B_PlayAgain");
        b_BackToMainMenu = root.Q<Button>("B_BackToMainMenu");
        b_Quit = root.Q<Button>("B_Quit");
        l_Score = root.Q<Label>("L_Score");
        b_PlayAgain.clicked += () => SceneManager.LoadScene("PlayerLevel");
        b_BackToMainMenu.clicked += () => SceneManager.LoadScene("MainMenuScene");
        b_Quit.clicked += () => Application.Quit();
        l_Score.text = "Score: " + manager.Score.ToString();
    }
}
