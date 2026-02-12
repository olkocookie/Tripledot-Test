using UnityEngine;
using UnityEngine.UI;

public class BackgroundClickDetector : MonoBehaviour
{
    [SerializeField] private BottomBarView bottomBarView;
    private Button button;

    private void Start()
    {
        button = gameObject.AddComponent<Button>();
        button.onClick.AddListener(OnBackgroundClicked);
    }

    private void OnBackgroundClicked()
    {
        bottomBarView.DeactivateAll();
    }
}