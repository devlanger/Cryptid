using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static LoginUI;

public class LoginUI : ViewUI
{
    [Header("Login")]
    [SerializeField] private TMP_InputField loginEmailField;
    [SerializeField] private TMP_InputField loginPasswordField;

    [Header("Registration")]
    [SerializeField] private TMP_InputField registerEmailField;
    [SerializeField] private TMP_InputField registerPasswordField;
    [SerializeField] private TMP_InputField registerNicknameField;
    [SerializeField] private TMP_InputField registerUsernameField;

    [SerializeField] private GameObject loginScreen;
    [SerializeField] private GameObject registerScreen;
    [SerializeField] private ViewUI mainScreen;

    [SerializeField] private GameObject loadingIndicator;
    [SerializeField] private TextMeshProUGUI loadingIndicatorText;

    private Coroutine c;

    public string url;

    private void Awake()
    {
        GoToLogin();
    }

    public void GoToRegister()
    {
        registerScreen.gameObject.SetActive(true);
        loginScreen.gameObject.SetActive(false);
    }

    public void GoToLogin()
    {
        registerScreen.gameObject.SetActive(false);
        loginScreen.gameObject.SetActive(true);
        Activate();
        mainScreen.Deactivate();
    }

    public void GoToMainScreen()
    {
        registerScreen.gameObject.SetActive(false);
        loginScreen.gameObject.SetActive(false);
        Deactivate();
        mainScreen.Activate();
    }

    public void Register()
    {
        if (c != null)
        {
            return;
        }

        c = StartCoroutine(Register(new RegisterDto
        {
            Username = registerUsernameField.text,
            Email = registerEmailField.text,
            Nickname = registerNicknameField.text,
            Password = registerPasswordField.text,
        }));
    }

    public void Login()
    {
        if(c != null)
        {
            return;
        }

        c = StartCoroutine(Login(new LoginDto(loginEmailField.text, loginPasswordField.text)));
    }

    public class LoginDto
    {
        public LoginDto(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterDto
    {
        public string Username { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    private IEnumerator Login(LoginDto loginDto)
    {
        loadingIndicator.gameObject.SetActive(true);
        loadingIndicatorText.SetText("Loading...");

        string postData = JsonConvert.SerializeObject(loginDto);
        byte[] bytes = Encoding.UTF8.GetBytes(postData);

        using (UnityWebRequest www = new UnityWebRequest($"{url}/api/account/login", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bytes);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.responseCode == 200)
            {
                Debug.Log(www.downloadHandler.text);
                loadingIndicator.gameObject.SetActive(false);

                var json = JObject.Parse(www.downloadHandler.text);
                MenuUI.Nickname = (string)json["nickname"];

                GoToMainScreen();
            }
            else
            {
                Debug.Log($"{www.error}: {www.downloadHandler.text}");
                loadingIndicatorText.SetText(www.downloadHandler.text);
            }
        }

        c = null;
    }

    private IEnumerator Register(RegisterDto registerDto)
    {
        loadingIndicator.gameObject.SetActive(true);
        loadingIndicatorText.SetText("Loading...");

        string postData = JsonConvert.SerializeObject(registerDto);
        byte[] bytes = Encoding.UTF8.GetBytes(postData);

        using (UnityWebRequest www = new UnityWebRequest($"{url}/api/account/register", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bytes);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.responseCode == 200)
            {
                Debug.Log(www.downloadHandler.text);
                loadingIndicator.gameObject.SetActive(false);
                loginEmailField.text = registerDto.Email;
                loginPasswordField.text = "";

                registerUsernameField.text = "";
                registerEmailField.text = "";
                registerNicknameField.text = "";
                registerPasswordField.text = "";

                GoToLogin();
            }
            else
            {
                Debug.Log($"{www.error}: {www.downloadHandler.text}");
                loadingIndicatorText.SetText(www.downloadHandler.text);
            }
        }

        c = null;
    }
}
