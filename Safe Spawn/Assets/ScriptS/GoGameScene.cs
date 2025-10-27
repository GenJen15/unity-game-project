using UnityEngine;
using UnityEngine.SceneManagement;

public class GoGameScene : MonoBehaviour
{
    [Header("Next Scene")]
    public string nextSceneName = "QuizScene";

    public void OnEasyButton()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
