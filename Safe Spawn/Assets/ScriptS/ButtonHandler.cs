using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Assign which answer this button represents")]
    public AnswerType answer; // uses the same enum as GameManager

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // calm bounce animation
        transform.localScale = originalScale * 0.9f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = originalScale;

        // call SubmitAnswer on the singleton GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SubmitAnswer(answer);
        }
        else
        {
            Debug.LogError("GameManager instance not found in scene.");
        }
    }
}
