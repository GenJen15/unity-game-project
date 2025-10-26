using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        if (PlayerPrefs.HasKey("LoggedInEmail"))
        {
            string email = PlayerPrefs.GetString("LoggedInEmail");
            Login(email);
        }
        else
        {
            Logout();
        }
    }

    public void Login(string username)
    {
        isLoggedIn = true;
        playerName = username;
        UpdateUI();
    }

    public void Logout()
    {
        isLoggedIn = false;
        playerName = "";
        PlayerPrefs.DeleteKey("LoggedInEmail");
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
            welcomeText.gameObject.SetActive(false);
            logoutButton.gameObject.SetActive(false);
        }
    }
}