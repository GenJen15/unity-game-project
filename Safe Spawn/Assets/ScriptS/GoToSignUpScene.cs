using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToSignUpScene : MonoBehaviour
{
    [Header("Next Scene")]
    public string nextSceneName = "LoginAndRegisterScene";

    public void OnSignUpButton()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
