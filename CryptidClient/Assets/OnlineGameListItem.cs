using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    [SerializeField] GameObject overlay;

    private string state;

    public bool myTurn = false;

    private void Awake()
    {
        overlay.SetActive(false);
        GetComponent<Button>().onClick.AddListener(Play);
    }

    private void Play()
    {
        ConnectionController.Instance.LoadGameState(idText.text, state);
    }

    public void Fill(OnlineGamesUI.ListGameDto item)
    {
        idText.text = item.id;
        state = item.currentState;

        if(string.IsNullOrEmpty(state))
        {
            return;
        }

        JObject obj = JObject.Parse(state);
        myTurn = obj.GetValue("CurrentPlayerId").ToString() == LoginUI.UserData.id;
        overlay.SetActive(myTurn);
    }
}
