using UnityEngine;

public class TopBarResponsive : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private RectTransform coinsDisplay;
    [SerializeField] private RectTransform heartsDisplay;
    [SerializeField] private RectTransform starsDisplay;
    [SerializeField] private RectTransform settingsButton;
    
    [Header("Layout Settings")]
    [SerializeField] private float baseLeftMargin = 20f;
    [SerializeField] private float baseSpacing = 10f;
    [SerializeField] private float baseRightMargin = 20f;
    
    [Header("Responsive Scaling")]
    [Tooltip("Aspect ratio below which icons scale down (0.46 = 9:21 and narrower)")]
    [SerializeField] private float narrowScreenThreshold = 0.46f;

    [Tooltip("Scale multiplier for narrow screens (0.85 = 85% size)")]
    [SerializeField] private float narrowScreenScale = 0.85f;

#if UNITY_EDITOR
    private int lastScreenWidth;
    private int lastScreenHeight;
#endif

    private void Start()
    {
        AdjustLayout();
        
#if UNITY_EDITOR
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
#endif
    }

#if UNITY_EDITOR
    private void Update()
    {
        // Detect resolution changes in editor
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
            AdjustLayout();
        }
    }
#endif

    private void AdjustLayout()
    {
        float screenAspect = (float)Screen.width / Screen.height;
        
        // Determine scale based on screen aspect ratio
        float scale = screenAspect < narrowScreenThreshold ? narrowScreenScale : 1f;
        
        // Apply scale
        coinsDisplay.localScale = Vector3.one * scale;
        heartsDisplay.localScale = Vector3.one * scale;
        starsDisplay.localScale = Vector3.one * scale;
        settingsButton.localScale = Vector3.one * scale;
        
        // Reposition elements based on their scaled widths
        float currentX = baseLeftMargin;
        
        // Coins
        coinsDisplay.anchoredPosition = new Vector2(currentX, coinsDisplay.anchoredPosition.y);
        currentX += (coinsDisplay.rect.width * scale) + baseSpacing;
        
        // Hearts
        heartsDisplay.anchoredPosition = new Vector2(currentX, heartsDisplay.anchoredPosition.y);
        currentX += (heartsDisplay.rect.width * scale) + baseSpacing;
        
        // Stars
        starsDisplay.anchoredPosition = new Vector2(currentX, starsDisplay.anchoredPosition.y);
        
        // Settings button (right-aligned)
        settingsButton.anchoredPosition = new Vector2(-baseRightMargin, settingsButton.anchoredPosition.y);
    }
}