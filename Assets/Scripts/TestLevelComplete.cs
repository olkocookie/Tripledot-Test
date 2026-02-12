using UnityEngine;
using UnityEngine.UI;

public class TestLevelComplete : MonoBehaviour
{
    [SerializeField] private Button testButton;
    [SerializeField] private GameObject levelCompletedScreen; // Changed to GameObject

    private void Start()
    {
        if (testButton != null)
        {
            testButton.onClick.AddListener(() => levelCompletedScreen.SetActive(true));
        }
    }
}