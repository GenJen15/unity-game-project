using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;

public class SignInScript2 : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI feedbackText;

    [Header("Next Scene")]
    public string mainMenuSceneName = "MainMenuScene";
    
    public void OnBackButton()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
 
    public void OnLoginButton()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Please fill in all fields.";
            return;
        }

        if (!email.Contains("@") || !email.Contains("."))
        {
            feedbackText.text = "Please enter a valid email address.";
            return;
        }

        feedbackText.text = "Logging in...";

        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    void OnLoginSuccess(LoginResult result)
    {
        string email = emailInput.text;
        string displayName;

        if (result.InfoResultPayload?.PlayerProfile?.DisplayName != null)
            displayName = result.InfoResultPayload.PlayerProfile.DisplayName;
        else
            displayName = email.Split('@')[0];

        // Save session
        SessionManager.PlayerName = displayName;
        SessionManager.PlayerEmail = email;

        // Save locally for next session
        PlayerPrefs.SetString("LoggedInEmail", email);
        PlayerPrefs.SetString("LoggedInName", displayName);
        PlayerPrefs.Save();

        // Optional: update PlayFab display name
        if (result.InfoResultPayload?.PlayerProfile?.DisplayName == null)
        {
            PlayFabClientAPI.UpdateUserTitleDisplayName(
                new UpdateUserTitleDisplayNameRequest { DisplayName = displayName },
                r => Debug.Log("Display name set!"),
                e => Debug.LogWarning("Could not set display name: " + e.GenerateErrorReport())
            );
        }

        feedbackText.text = "Login successful!";
        emailInput.text = "";
        passwordInput.text = "";

        SceneManager.LoadScene(mainMenuSceneName);
    }

    void OnLoginFailure(PlayFabError error)
    {
        switch (error.Error)
        {
            case PlayFabErrorCode.InvalidEmailOrPassword:
                feedbackText.text = "Invalid email or password.";
                break;
            case PlayFabErrorCode.AccountNotFound:
                feedbackText.text = "Account not found. Please sign up first.";
                break;
            default:
                feedbackText.text = "Login failed: " + error.ErrorMessage;
                break;
        }

        Debug.LogError("Login failed: " + error.GenerateErrorReport());
    }

    void OnEnable()
    {
        emailInput.text = "";
        passwordInput.text = "";
        feedbackText.text = "";
    }
}