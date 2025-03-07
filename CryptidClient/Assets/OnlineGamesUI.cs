using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static LoginUI;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;

public class OnlineGamesUI : ViewUI
{
    [SerializeField] private ContentList list;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button backButton;
    [SerializeField] private MenuRouterUI router;
    [SerializeField] private GameObject loadingIndicator;

    public class ListGameDto
    {
        public string id;
        public string currentState;
        public bool isMyTurn;
    }

    private void Awake()
    {
        backButton.onClick.AddListener(Back);
        newGameButton.onClick.AddListener(NewGame);
    }

    private void Back()
    {
        router.GoToView("main");
    }

    private void NewGame()
    {
        router.GoToView("multiplayer");
    }

    public override void Activate()
    {
        base.Activate();
        list.ClearList();
        StartCoroutine(GetGamesForUser());
    }

    private IEnumerator GetGamesForUser()
    {
        loadingIndicator.SetActive(true);
        string url = $"{NetworkConfiguration.API_URL}/api/games/user/{UserData.id}";
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            www.SetRequestHeader("Authorization", $"Bearer {UserData.token}");

            yield return www.SendWebRequest();

            if (www.responseCode == 200)
            {
                Debug.Log(www.downloadHandler.text);

                List<ListGameDto> data = JsonConvert.DeserializeObject<List<ListGameDto>>(www.downloadHandler.text);
                foreach (var item in data)
                {
                    JObject obj = JObject.Parse(item.currentState);
                    item.isMyTurn = obj.GetValue("CurrentPlayerId").ToString() == LoginUI.UserData.id;
                }

                foreach (var item in data.OrderByDescending(i => i.isMyTurn))
                {
                    var inst = list.AddToList<OnlineGameListItem>();
                    inst.Fill(item);
                }

                loadingIndicator.SetActive(false);
            }
            else
            {
                loadingIndicator.SetActive(false);
                Debug.LogError($"{www.error}: {www.downloadHandler.text}");
            }
        }
    }
}
