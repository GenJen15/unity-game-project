using UnityEngine;
using TMPro;

public class ResultsDisplay : MonoBehaviour
{
    public TMP_Text finalScoreText;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            int score = GameManager.Instance.GetScore();
            int total = GameManager.Instance.GetTotalRounds();
            finalScoreText.text = $"You Scored {score}/{total}! Great Job!";
        }
        else
        {
            finalScoreText.text = "No score data found!";
        }
    }
}