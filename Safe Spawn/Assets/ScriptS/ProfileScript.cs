using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class ProfileMenu : MonoBehaviour
{
    [Header("UI References")]
    public Button signInButton;
    public Button signUpButton;
    public TextMeshProUGUI welcomeText;
    public Button logoutButton;

    private bool isLoggedIn = false;
    private string playerName = "";

    void Start()
    {
        // Check if there is a session first
        if (!string.IsNullOrEmpty(SessionManager.PlayerEmail))
        {
            Login(SessionManager.PlayerName);
        }
        // Fallback to PlayerPrefs if session is empty
        else if (PlayerPrefs.HasKey("LoggedInEmail"))
        {
            string email = PlayerPrefs.GetString("LoggedInEmail");
            string name = PlayerPrefs.GetString("LoggedInName", email);

            // You can check PlayFab session if needed
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                Login(name);
            }
            else
            {
                SilentLogin(email, name);
            }
        }
        else
        {
            Logout();
        }

        UpdateUI();
    }

    public void Login(string username)
    {
        isLoggedIn = true;
        playerName = username;
        UpdateUI();
    }

    void SilentLogin(string email, string name)
    {
        playerName = name;
        isLoggedIn = true;
        UpdateUI();
    }

    public void Logout()
    {
        isLoggedIn = false;
        playerName = "";
        SessionManager.PlayerName = "";
        SessionManager.PlayerEmail = "";

        PlayerPrefs.DeleteKey("LoggedInEmail");
        PlayerPrefs.DeleteKey("LoggedInName");
        PlayerPrefs.Save();

        PlayFabClientAPI.ForgetAllCredentials();
        UpdateUI();
    }

    void UpdateUI()
    {
        if (isLoggedIn)
        {
            signInButton.gameObject.SetActive(false);
            signUpButton.gameObject.SetActive(false);
            logoutButton.gameObject.SetActive(true);
            welcomeText.gameObject.SetActive(true);
            welcomeText.text = $"Welcome, {playerName}!";
        }
        else
        {
            signInButton.gameObject.SetActive(true);
            signUpButton.gameObject.SetActive(true);
            logoutButton.gameObject.SetActive(false);
            welcomeText.gameObject.SetActive(false);
        }
    }
}