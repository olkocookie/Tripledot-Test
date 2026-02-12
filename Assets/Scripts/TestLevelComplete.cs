using UnityEngine;
using UnityEngine.UI;

public class TestLevelComplete : MonoBehaviour
{
    [SerializeField] private Button testButton;
    [SerializeField] private GameObject levelCompletedScreen;

    private void Start()
    {
        if (testButton != null)
        {
            testButton.onClick.AddListener(() => levelCompletedScreen.SetActive(true));
        }
    }
}