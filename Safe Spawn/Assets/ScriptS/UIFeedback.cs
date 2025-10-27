using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFeedback : MonoBehaviour
{
    public Image correctIcon;
    public Image wrongIcon;

    public void ShowCorrect()
    {
        if (correctIcon != null)
        {
            correctIcon.gameObject.SetActive(true);
            wrongIcon.gameObject.SetActive(false);
            StartCoroutine(HideAfterDelay());
        }
    }

    public void ShowWrong()
    {
        if (wrongIcon != null)
        {
            wrongIcon.gameObject.SetActive(true);
            correctIcon.gameObject.SetActive(false);
            StartCoroutine(HideAfterDelay());
        }
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(0.9f);
        if (correctIcon != null) correctIcon.gameObject.SetActive(false);
        if (wrongIcon != null) wrongIcon.gameObject.SetActive(false);
    }
}
