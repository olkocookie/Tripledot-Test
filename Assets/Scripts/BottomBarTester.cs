using UnityEngine;
using UnityEngine.UI;

public class BottomBarTester : MonoBehaviour
{
    [SerializeField] private BottomBarView bottomBarView;
    [SerializeField] private Button showButton;
    [SerializeField] private Button hideButton;

    private void Start()
    {
        if (showButton != null)
        {
            showButton.onClick.AddListener(() => bottomBarView.Show());
        }

        if (hideButton != null)
        {
            hideButton.onClick.AddListener(() => bottomBarView.Hide());
        }
    }
}