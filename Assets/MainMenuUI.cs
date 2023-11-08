using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    UIDocument mainMenuUI;
    VisualElement root;
    VisualElement ve_Difficulty;
    VisualElement ve_MainMenuButtons;
    Button b_PlaySettings;
    Button b_Quit;
    Button b_Easy;
    Button b_Hard;
    Button b_Play;
    Button b_BackToMM;
    GameManager gameManager;
    private void Awake() {
        gameManager = GameManager.Instance;
        mainMenuUI = GetComponent<UIDocument>();
        root = mainMenuUI.rootVisualElement;
        ve_Difficulty = root.Q("VE_Difficulty");
        ve_MainMenuButtons = root.Q("VE_MainMenuButtons");
        b_PlaySettings = root.Q<Button>("B_Settings");
        b_Play = root.Q<Button>("B_Play");
        b_Quit = root.Q<Button>("B_Quit");
        b_Easy = root.Q<Button>("B_Easy");
        b_Hard = root.Q<Button>("B_Hard");
        b_BackToMM = root.Q<Button>("B_BackMM");
    }
    private void Start() {
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        UnityEngine.Cursor.visible = true;
        b_PlaySettings.clicked += () => ShowDifficultyScreen();
        b_Quit.clicked += () => Application.Quit();
        b_Easy.clicked += () => EasyDifficulty();
        b_Hard.clicked += () => HardDifficulty();
        b_Play.clicked += () => SceneManager.LoadScene("PlayerLevel");
        b_BackToMM.clicked += () => ShowMainMenu();
    }

    //https://github.com/mdotstrange/MdotsCustomPlaymakerActions/blob/master/KnockbackAction.cs
    void ShowDifficultyScreen() {
        ve_MainMenuButtons.style.display = DisplayStyle.None;
        ve_Difficulty.style.display = DisplayStyle.Flex;
    }

    void ShowMainMenu() {
        ve_MainMenuButtons.style.display = DisplayStyle.Flex;
        ve_Difficulty.style.display = DisplayStyle.None;
    }

    void EasyDifficulty() {
        gameManager.difficultyAmplify = 2f;
    }
    void HardDifficulty() {
        gameManager.difficultyAmplify = 1f;
    }
}
