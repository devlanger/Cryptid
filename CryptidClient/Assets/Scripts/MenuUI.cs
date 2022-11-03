using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private Button startNewButton;
    [SerializeField] private Button multiplayerButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Text versionText;

    private void Awake()
    {
        startNewButton.onClick.AddListener(StartNew);
        exitButton.onClick.AddListener(Exit);

        versionText.text = $"Ver {Application.version}";
    }

    private void Exit()
    {
        Application.Quit();
    }

    private void StartNew()
    {
        SceneManager.LoadScene(1);
    }
}
