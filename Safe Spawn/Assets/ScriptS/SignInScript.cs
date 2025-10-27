using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;


public class SignInScript : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField retypePasswordInput;
    public TextMeshProUGUI feedbackText;

     [Header("Next Scene")]
    public string mainMenuSceneName = "MainMenuScene";

    public void OnBackButton()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void OnSignUpButton()
    {
        Debug.Log("Sign-Up button clicked");

        string email = emailInput.text.Trim();
        string password = passwordInput.text;
        string retypePassword = retypePasswordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(retypePassword))
        {
            feedbackText.text = "Please fill in all fields.";
            Debug.Log("Feedback: " + feedbackText.text);
            return;
        }

        if (!email.Contains("@") || !email.Contains("."))
        {
            feedbackText.text = "Please enter a valid email address.";
            return;
        }

        if (password != retypePassword)
        {
            feedbackText.text = "Passwords do not match!";
            return;
        }

        if (password.Length < 6)
        {
            feedbackText.text = "Password must be at least 6 characters long.";
            return;
        }

        var request = new RegisterPlayFabUserRequest
        {
            Email = email,
            Password = password,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        feedbackText.text = "Account created successfully! Logging you in...";

        // Store credentials before clearing

        string email = emailInput.text;
        string password = passwordInput.text;

        // Clear fields for visual feedback
        emailInput.text = "";
        passwordInput.text = "";
        retypePasswordInput.text = "";

        var loginRequest = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password
        };
        PlayFabClientAPI.LoginWithEmailAddress(loginRequest, OnLoginSuccess, OnLoginFailure);
    }

    void OnRegisterFailure(PlayFabError error)
    {
        switch (error.Error)
        {
            case PlayFabErrorCode.EmailAddressNotAvailable:
                feedbackText.text = "An account with this email already exists.";
                break;

            case PlayFabErrorCode.InvalidEmailAddress:
                feedbackText.text = "Invalid email address.";
                break;

            case PlayFabErrorCode.InvalidPassword:
                feedbackText.text = "Invalid password format.";
                break;

            default:
                feedbackText.text = "Sign-up failed: " + error.ErrorMessage;
                break;
        }

        Debug.LogError("Error signing up: " + error.GenerateErrorReport());
    }

    void OnLoginSuccess(LoginResult result)
    {
        feedbackText.text = "Welcome!";
        Debug.Log("User logged in successfully.");

         // ✅ Set display name (using email prefix)
        string email = emailInput.text.Trim();
        string displayName = email.Split('@')[0];

        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = displayName
        },
        updateResult =>
        {
            Debug.Log("Display name set to: " + displayName);
            PlayerPrefs.SetString("LoggedInEmail", email);
            PlayerPrefs.SetString("LoggedInName", displayName);
            PlayerPrefs.Save();

            SceneManager.LoadScene(mainMenuSceneName);
        },
        error =>
        {
            Debug.LogWarning("Could not set display name: " + error.GenerateErrorReport());
            PlayerPrefs.SetString("LoggedInEmail", email);
            PlayerPrefs.SetString("LoggedInName", email);
            PlayerPrefs.Save();

            SceneManager.LoadScene(mainMenuSceneName);
        });
    }

    void OnLoginFailure(PlayFabError error)
    {
        feedbackText.text = "Login failed: " + error.ErrorMessage;
        Debug.LogError(error.GenerateErrorReport());
    }

    void OnEnable()
    {
        emailInput.text = "";
        passwordInput.text = "";
        retypePasswordInput.text = "";
        feedbackText.text = "";
    }
}
