using GooglePlayGames.BasicApi;
using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using System;
using System.IO;

public class GooglePlayServices : MonoBehaviour
{
    private async void Start()
    {

        await UnityServices.InitializeAsync();
        Debug.Log(UnityServices.State);
        await SignInAnonymouslyAsync();
    }

    private void LoadProfile()
    {
#if UNITY_EDITOR
        string p = $"{System.IO.Directory.GetCurrentDirectory()}/user.txt";
        Debug.Log($"Load profile from {p} if exists.");
        if (File.Exists(p))
        {
            var profileName = File.ReadAllText(p);
            AuthenticationService.Instance.SwitchProfile(profileName);
            Debug.Log($"Switched auth profile: {profileName}");
        }
#endif
    }

    async Task SignInAnonymouslyAsync()
    {
        try
        {
            LoadProfile();

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            Debug.Log($"AccessToken: {AuthenticationService.Instance.AccessToken}");

            ConnectionManager.Instance.LoginWithAccessToken(AuthenticationService.Instance.PlayerId, AuthenticationService.Instance.AccessToken);
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    // Setup authentication event handlers if desired
    void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () => {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

        };

        AuthenticationService.Instance.SignInFailed += (err) => {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () => {
            Debug.Log("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired.");
        };
    }
}
