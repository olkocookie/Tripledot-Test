using UnityEngine;
using UnityEngine.UI;

public class TestLevelComplete : MonoBehaviour
{
    [SerializeField] private Button testButton;
    [SerializeField] private LevelCompletedScreenController levelCompletedScreen;

    private void Start()
    {
        if (testButton != null)
        {
            testButton.onClick.AddListener(() => levelCompletedScreen.Show());
        }
    }
}