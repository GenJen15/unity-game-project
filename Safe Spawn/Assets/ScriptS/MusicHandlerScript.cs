using UnityEngine;

public class MusicHandlerScript : MonoBehaviour
{
    private AudioSource musicSource;
    private bool isMusicOn = true;
    public TMPro.TextMeshProUGUI buttonText;
    
    void Awake()
    {
        // ✅ Modern way: use FindObjectsByType instead of FindObjectsOfType
        var existing = FindObjectsByType<MusicHandlerScript>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        if (existing.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        musicSource = GetComponent<AudioSource>();

        if (musicSource == null)
        {
            Debug.LogWarning("No AudioSource found on MusicHandlerScript GameObject!");
        }
    }

    public void ToggleMusic()
    {
        if (musicSource == null) return;

            isMusicOn = !isMusicOn;
            musicSource.mute = !isMusicOn;

        if (buttonText != null)
            buttonText.text = isMusicOn ? "Music: ON" : "Music: OFF";
    }
}
