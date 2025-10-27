using UnityEngine;
using UnityEngine.SceneManagement;

public class GoProfileScene : MonoBehaviour
{
    [Header("Next Scene")]
    public string nextSceneName = "ProfileScene";

    public void OnProfileButton()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
