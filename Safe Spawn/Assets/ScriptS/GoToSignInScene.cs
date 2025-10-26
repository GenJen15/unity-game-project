using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToSignInScene : MonoBehaviour
{
    [Header("Next Scene")]
    public string nextSceneName = "SignInScene";

    public void OnSignInButton()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
