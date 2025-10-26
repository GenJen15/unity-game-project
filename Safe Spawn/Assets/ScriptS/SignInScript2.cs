using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
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

        // Validation
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

        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }

        };

        feedbackText.text = "Logging in...";
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    void OnLoginSuccess(LoginResult result)
    {
        feedbackText.text = "Login successful!";
        Debug.Log("Logged in successfully as: " + emailInput.text);

        ResetLoginForm();

        PlayerPrefs.SetString("LoggedInEmail", emailInput.text);
        PlayerPrefs.Save();

        if(result.InfoResultPayload.PlayerProfile != null)
        {
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
        }
            
        if(name != null)
        {
            // to be continued
        }
        Invoke(nameof(GoToMainMenu), 1f);
    }

    // Failure
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

    public void ResetLoginForm()
    {
        emailInput.text = "";
        passwordInput.text = "";
        feedbackText.text = "";
    }

    void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    void OnEnable()
    {
        ResetLoginForm();
    }
}
