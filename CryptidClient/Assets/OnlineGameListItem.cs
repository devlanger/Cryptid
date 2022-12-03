using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnlineGameListItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI idText;

    private string state;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Play);
    }

    private void Play()
    {
        GameController.InitialState = JsonConvert.DeserializeObject<GameState>(state);
        SceneManager.LoadScene(1);
    }

    public void Fill(OnlineGamesUI.ListGameDto item)
    {
        idText.text = item.id;
        state = item.currentState;
    }
}
