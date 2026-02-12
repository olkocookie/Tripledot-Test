using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingsPopup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup popupCanvasGroup;
    [SerializeField] private RectTransform popupContent;
    [SerializeField] private GameObject blurOverlay;
    [SerializeField] private GameObject darkenOverlay;
    [SerializeField] private Image blurOverlayImage;
    [SerializeField] private Image darkenOverlayImage;
    [SerializeField] private Button closeButton;
    
    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.4f;
    [SerializeField] private float darkenAlpha = 0.85f;
    [SerializeField] private Ease showEase = Ease.OutBack;
    [SerializeField] private Ease hideEase = Ease.InBack;

    private void OnEnable()
    {
        // When activated, play show animation
        AnimateIn();
        
        // Setup close button (only once)
        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(Hide); // Remove first to avoid duplicates
            closeButton.onClick.AddListener(Hide);
        }
    }

    private void AnimateIn()
    {
        // Activate overlays
        if (blurOverlay != null) blurOverlay.SetActive(true);
        if (darkenOverlay != null) darkenOverlay.SetActive(true);
        
        // Kill any ongoing tweens
        popupCanvasGroup?.DOKill();
        popupContent?.DOKill();
        blurOverlayImage?.DOKill();
        darkenOverlayImage?.DOKill();
        
        // Fade in blur overlay
        if (blurOverlayImage != null)
        {
            Color blurColor = blurOverlayImage.color;
            blurColor.a = 0f;
            blurOverlayImage.color = blurColor;
            blurOverlayImage.DOFade(1f, animationDuration).SetEase(Ease.OutQuad);
        }
        
        // Fade in darken overlay
        if (darkenOverlayImage != null)
        {
            Color darkenColor = darkenOverlayImage.color;
            darkenColor.a = 0f;
            darkenOverlayImage.color = darkenColor;
            darkenOverlayImage.DOFade(darkenAlpha, animationDuration).SetEase(Ease.OutQuad);
        }
        
        // Fade in popup
        if (popupCanvasGroup != null)
        {
            popupCanvasGroup.alpha = 0f;
            popupCanvasGroup.DOFade(1f, animationDuration).SetEase(Ease.OutQuad);
        }
        
        // Scale in popup content
        if (popupContent != null)
        {
            popupContent.localScale = Vector3.one * 0.8f;
            popupContent.DOScale(1f, animationDuration).SetEase(showEase);
        }
    }

    public void Hide()
    {
        // Kill any ongoing tweens
        popupCanvasGroup?.DOKill();
        popupContent?.DOKill();
        blurOverlayImage?.DOKill();
        darkenOverlayImage?.DOKill();
        
        // Fade out blur
        if (blurOverlayImage != null)
        {
            blurOverlayImage.DOFade(0f, animationDuration).SetEase(Ease.InQuad);
        }
        
        // Fade out darken
        if (darkenOverlayImage != null)
        {
            darkenOverlayImage.DOFade(0f, animationDuration).SetEase(Ease.InQuad);
        }
        
        // Fade out popup
        if (popupCanvasGroup != null)
        {
            popupCanvasGroup.DOFade(0f, animationDuration).SetEase(Ease.InQuad);
        }
        
        // Scale out popup content
        if (popupContent != null)
        {
            popupContent.DOScale(0.8f, animationDuration)
                .SetEase(hideEase)
                .OnComplete(() => 
                {
                    gameObject.SetActive(false);
                    if (blurOverlay != null) blurOverlay.SetActive(false);
                    if (darkenOverlay != null) darkenOverlay.SetActive(false);
                });
        }
    }

    private void OnDestroy()
    {
        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(Hide);
        }
    }
}