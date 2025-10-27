using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Sprites (auto loaded if empty)")]
    public List<Sprite> allEmotionSprites = new List<Sprite>();

    [Header("UI References")]
    public Image emotionDisplay;
    public TMP_Text scoreText;
    public TMP_Text roundText;     // NEW — optional, to show current round
    public TMP_Text resultText;    // NEW — shows final score message
    public UIFeedback uiFeedback;

    

    [Header("Gameplay Settings")]
    public float feedbackDuration = 0.9f;
    public int totalRounds = 5;    // Total number of rounds

    private int score = 0;
    private int currentRound = 0;  // Track current round
    private List<Sprite> remaining = new List<Sprite>();

    [Header("Next Scene")]
    public string nextSceneName = "ResultsScene";

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    void Start()
    {
        if (allEmotionSprites.Count == 0)
        {
            Sprite[] loaded = Resources.LoadAll<Sprite>("PersonalSprites");
            allEmotionSprites.AddRange(loaded);
        }

        if (emotionDisplay != null)
            emotionDisplay.color = new Color(1, 1, 1, 0);

        ResetGame();
        ShowNextRound();
    }

    void ResetGame()
    {
        score = 0;
        currentRound = 0;
        UpdateScoreUI();

        remaining.Clear();
        remaining.AddRange(allEmotionSprites);
        Shuffle(remaining);

        if (emotionDisplay != null)
        {
            emotionDisplay.sprite = null;
            emotionDisplay.color = new Color(1, 1, 1, 0);
        }

        if (resultText != null)
            resultText.text = "";

        UpdateRoundUI();
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(i, list.Count);
            T tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    void UpdateRoundUI()
    {
        if (roundText != null)
            roundText.text = $"Round: {currentRound}/{totalRounds}";
    }

    public void ShowNextRound()
    {
        if (currentRound >= totalRounds)
        {
            EndGame();
            return;
        }

        currentRound++;
        UpdateRoundUI();

        if (remaining.Count == 0)
        {
            ResetGame();
            return;
        }

        Sprite s = remaining[0];
        remaining.RemoveAt(0);

        if (emotionDisplay != null)
        {
            emotionDisplay.sprite = s;
            // emotionDisplay.SetNativeSize();
            emotionDisplay.color = Color.white;
        }
    }

    public void SubmitAnswer(AnswerType chosen)
    {
        if (emotionDisplay.sprite == null) return;

        string name = emotionDisplay.sprite.name.ToLower();
        AnswerType correct = DetectEmotionFromName(name);

        if (chosen == correct)
        {
            score++;
            UpdateScoreUI();
            if (uiFeedback != null) uiFeedback.ShowCorrect();
        }
        else
        {
            if (uiFeedback != null) uiFeedback.ShowWrong();
        }

        StartCoroutine(NextAfterDelay(feedbackDuration));
    }

    IEnumerator NextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowNextRound();
    }

    void EndGame()
    {
        if (emotionDisplay != null)
        {
            emotionDisplay.sprite = null;
            emotionDisplay.color = new Color(1, 1, 1, 0);
        }

        if (resultText != null)
            resultText.text = $"🎉 Quiz Complete!\nFinal Score: {score}/{totalRounds}";

        Debug.Log($"Game Over! Final Score: {score}/{totalRounds}");

        StartCoroutine(LoadResultsSceneAfterDelay(1.5f));
    }

     IEnumerator LoadResultsSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextSceneName);
    }

    AnswerType DetectEmotionFromName(string filename)
    {
        if (filename.Contains("happy") || filename.Contains("joy") || filename.Contains("smile")) return AnswerType.Happy;
        if (filename.Contains("sad") || filename.Contains("cry") || filename.Contains("tear")) return AnswerType.Sad;
        if (filename.Contains("angry") || filename.Contains("mad") || filename.Contains("anger")) return AnswerType.Angry;
        if (filename.Contains("scared") || filename.Contains("fear")) return AnswerType.Sad;
        return AnswerType.Happy;
    }

    public int GetScore() => score;
    public int GetTotalRounds() => totalRounds;
}

public enum AnswerType { Happy, Sad, Angry }