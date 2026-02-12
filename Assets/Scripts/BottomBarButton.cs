using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BottomBarButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button button;
    [SerializeField] private RectTransform iconContainer;
    [SerializeField] private RectTransform platform; // Button_BG
    [SerializeField] private CanvasGroup platformCanvasGroup;
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text buttonNameText;
    [SerializeField] private CanvasGroup textCanvasGroup;
    
    [Header("State")]
    [SerializeField] private bool isLocked = false;
    [SerializeField] private Sprite lockedIcon;
    [SerializeField] private Sprite normalIcon; // Store the unlocked icon
    
    [Header("Animation Values")]
    [SerializeField] private float normalY = 0f;
    [SerializeField] private float elevatedY = 80f;
    [SerializeField] private float normalScale = 1f;
    [SerializeField] private float activeScale = 1.2f;
    [SerializeField] private float iconFadeDelay = 0.05f;
    [SerializeField] private float textFadeDelay = 0.15f;
    [SerializeField] private float platformSlideOffset = 50f; // How far to slide from
    
    [SerializeField] private float platformInactiveWidth = 80f;  // Platform width when inactive
    [SerializeField] private float platformInactiveHeight = 80f; //Platform height when inactive
    [SerializeField] private float platformActiveWidth = 300f;   // Platform width when active (wider!)
    [SerializeField] private float platformActiveHeight = 140f; // Platform height when active (higher!)
    
    [Header("Unlock Settings")]
    [SerializeField] private string unlockedButtonName = "New";
    [SerializeField] private float unlockedWidth = 250f;
    
    public event Action<int> OnButtonClicked;
    public bool IsLocked => isLocked;
    
    private int buttonIndex;
    private bool isActive = false;

    public void Initialize(int index)
    {
        buttonIndex = index;
        
        // Store normal icon if not already set
        if (normalIcon == null && !isLocked)
        {
            normalIcon = iconImage.sprite;
        }
        
        // Set initial state
        iconContainer.anchoredPosition = new Vector2(iconContainer.anchoredPosition.x, normalY);
        iconContainer.localScale = Vector3.one * normalScale;
        
        // Set platform initial size (width AND height) and hide it
        if (platform != null && platformCanvasGroup != null)
        {
            platform.sizeDelta = new Vector2(platformInactiveWidth, platformInactiveHeight);
            platform.anchoredPosition = new Vector2(platform.anchoredPosition.x, normalY);
            platformCanvasGroup.alpha = 0f;
        }

        // Hide text initially
        if (textCanvasGroup != null)
        {
            textCanvasGroup.alpha = 0f;
        }

        // Update visual if locked
        if (isLocked)
        {
            SetLockedVisual();
        }

        // Setup button click
        if (button != null)
        {
            button.onClick.AddListener(HandleClick);
        }
    }

    private void HandleClick()
    {
        OnButtonClicked?.Invoke(buttonIndex);
    }

    public void Activate(float duration, Ease ease, Action onComplete = null)
    {
        if (isActive) return;
        isActive = true;

        // Kill any ongoing tweens
        iconContainer.DOKill();
        platform?.DOKill();
        platformCanvasGroup?.DOKill();
        textCanvasGroup?.DOKill();

        // Animate platform expansion (WIDTH and HEIGHT) + Slide In
        if (platform != null && platformCanvasGroup != null)
        {
            // Start from the left (negative X offset)
            platform.anchoredPosition = new Vector2(-platformSlideOffset, 0);
        
            // Slide to center while expanding
            platform.DOAnchorPosX(0, duration).SetEase(ease);
        
            platform.DOSizeDelta(new Vector2(platformActiveWidth, platformActiveHeight), duration)
                .SetEase(ease);
        
            platformCanvasGroup.DOFade(1f, duration * 0.3f).SetEase(Ease.OutQuad);
        }

        // Animate icon with slight delay
        iconContainer.DOAnchorPosY(elevatedY, duration)
            .SetEase(ease)
            .SetDelay(iconFadeDelay);
    
        iconContainer.DOScale(activeScale, duration)
            .SetEase(Ease.OutBack)
            .SetDelay(iconFadeDelay);

        // Animate text
        if (textCanvasGroup != null)
        {
            textCanvasGroup.DOFade(1f, duration * 0.4f)
                .SetDelay(textFadeDelay)
                .SetEase(Ease.OutQuad);
        }

        // Optional: Icon animation
        AnimateIconActivation(duration);

        // Invoke callback after animation
        DOVirtual.DelayedCall(duration, () => onComplete?.Invoke());
    }

    public void Deactivate(float duration, Ease ease, Action onComplete = null)
    {
        if (!isActive) return;
        isActive = false;

        // Kill any ongoing tweens
        iconContainer.DOKill();
        platform?.DOKill();
        platformCanvasGroup?.DOKill();
        textCanvasGroup?.DOKill();

        // Animate platform shrinking (WIDTH and HEIGHT) + Slide Out
        if (platform != null && platformCanvasGroup != null)
        {
            // Slide to the right while shrinking
            platform.DOAnchorPosX(platformSlideOffset, duration).SetEase(ease);
        
            platform.DOSizeDelta(new Vector2(platformInactiveWidth, platformInactiveHeight), duration)
                .SetEase(ease);
        
            platformCanvasGroup.DOFade(0f, duration * 0.5f).SetEase(Ease.InQuad);
        }

        // Animate icon back with bounce
        iconContainer.DOAnchorPosY(normalY, duration)
            .SetEase(Ease.OutBounce); // Adds subtle bounce on landing

        iconContainer.DOScale(normalScale, duration)
            .SetEase(ease);

        // Hide text
        if (textCanvasGroup != null)
        {
            textCanvasGroup.DOFade(0f, duration * 0.3f).SetEase(Ease.InQuad);
        }

        DOVirtual.DelayedCall(duration, () => onComplete?.Invoke());
    }

    public void SetActiveImmediate()
    {
        isActive = true;
        
        iconContainer.anchoredPosition = new Vector2(iconContainer.anchoredPosition.x, elevatedY);
        iconContainer.localScale = Vector3.one * activeScale;
        
        if (platform != null && platformCanvasGroup != null)
        {
            // Set platform size immediately (width AND height)
            platform.sizeDelta = new Vector2(platformActiveWidth, platformActiveHeight);
            platformCanvasGroup.alpha = 1f;
        }

        if (textCanvasGroup != null)
        {
            textCanvasGroup.alpha = 1f;
        }
    }

    public void PlayLockedAnimation()
    {
        // Wiggle/shake animation for locked buttons
        iconContainer.DOShakeRotation(0.5f, 15f, 10, 90f);
    }

    public void UnlockButton(float duration = 0.5f)
    {
        if (!isLocked) return;
        
        isLocked = false;
        
        // Animate width expansion
        LayoutElement layoutElement = GetComponent<LayoutElement>();
        if (layoutElement != null)
        {
            DOTween.To(
                () => layoutElement.preferredWidth,
                x => layoutElement.preferredWidth = x,
                unlockedWidth,
                duration
            ).SetEase(Ease.OutBack);
        }
        
        // Restore visuals
        if (normalIcon != null)
        {
            iconImage.sprite = normalIcon;
        }
        
        if (buttonNameText != null)
        {
            buttonNameText.text = unlockedButtonName;
        }
        
        // Play unlock animation
        PlayUnlockAnimation();
    }

    private void AnimateIconActivation(float duration)
    {
        // Subtle scale pulse/bounce
        iconImage.transform.DOPunchScale(Vector3.one * 0.1f, duration * 0.5f, 3, 0.5f);
    }

    private void PlayUnlockAnimation()
    {
        // Pop effect when unlocking
        iconImage.transform.DOPunchScale(Vector3.one * 0.3f, 0.5f, 5, 0.5f);
    }

    private void SetLockedVisual()
    {
        if (lockedIcon != null)
        {
            iconImage.sprite = lockedIcon;
        }
        
        // Hide text for locked buttons
        if (buttonNameText != null)
        {
            buttonNameText.text = "";
        }
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(HandleClick);
        }
    }
}