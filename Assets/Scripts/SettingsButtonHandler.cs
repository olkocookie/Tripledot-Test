using UnityEngine;
using UnityEngine.UI;

public class SettingsButtonHandler : MonoBehaviour
{
    [SerializeField] private Button settingsButton;
    [SerializeField] private GameObject settingsPopup; // Changed to GameObject

    private void Start()
    {
        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OpenSettings);
        }
    }

    private void OpenSettings()
    {
        if (settingsPopup != null)
        {
            settingsPopup.SetActive(true); // Just activate it!
        }
    }

    private void OnDestroy()
    {
        if (settingsButton != null)
        {
            settingsButton.onClick.RemoveListener(OpenSettings);
        }
    }
}