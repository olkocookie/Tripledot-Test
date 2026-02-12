using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class LevelCompletedScreenController : MonoBehaviour
{
    [Header("Screen References")]
    [SerializeField] private CanvasGroup screenCanvasGroup;
    [SerializeField] private GameObject homeScreen; // Reference to hide/show home screen
    
    [Header("Title Animation")]
    [SerializeField] private RectTransform titleLevelImage;
    [SerializeField] private RectTransform titleCompletedImage;
    [SerializeField] private float titleDelay = 0.2f;
    [SerializeField] private float titleDuration = 0.6f;
    [SerializeField] private Vector2 titleTargetPosition = new Vector2(0, 400);
    
    [Header("Star Animation")]
    [SerializeField] private RectTransform starBig;
    [SerializeField] private float starDelay = 0.4f;
    [SerializeField] private float starDuration = 0.8f;
    
    [Header("Star Score Animation")]
    [SerializeField] private RectTransform starScoreText;
    [SerializeField] private float scoreDelay = 0.8f;
    [SerializeField] private float scoreDuration = 0.5f;
    
    [Header("Rewards Animation")]
    [SerializeField] private RectTransform rewardsContainer;
    [SerializeField] private RectTransform[] rewardElements; // Coin, Crown, etc.
    [SerializeField] private float rewardsDelay = 1.0f;
    [SerializeField] private float rewardStagger = 0.1f;
    
    [Header("Buttons Animation")]
    [SerializeField] private RectTransform buttonsContainer; // The container with Layout Group
    [SerializeField] private float buttonsDelay = 1.3f;
    [SerializeField] private float buttonsDuration = 0.5f;
    [SerializeField] private Vector2 buttonsTargetPosition = new Vector2(0, -600); // Adjust in Inspector
    
    [Header("Particles")]
    [SerializeField] private ParticleSystem particlesBurst;
    [SerializeField] private ParticleSystem particlesLoopedTop;
    [SerializeField] private ParticleSystem particlesLoopedMiddle;
    
    [Header("Buttons")]
    [SerializeField] private Button homeButtonComponent;
    [SerializeField] private Button continueButtonComponent;
    
    [Header("Animation Settings")]
    [SerializeField] private float transitionDuration = 0.4f;

    private void Awake()
    {
        // Setup button listeners
        if (homeButtonComponent != null)
        {
            homeButtonComponent.onClick.AddListener(OnHomeButtonClicked);
        }
        
        if (continueButtonComponent != null)
        {
            continueButtonComponent.onClick.AddListener(OnContinueButtonClicked);
        }
        
        // Start hidden
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        // Play show animation when activated
        AnimateIn();
    }

    private void AnimateIn()
    {
        // Hide home screen smoothly
        if (homeScreen != null)
        {
            CanvasGroup homeCanvasGroup = homeScreen.GetComponent<CanvasGroup>();
            if (homeCanvasGroup != null)
            {
                homeCanvasGroup.DOFade(0f, transitionDuration).SetEase(Ease.OutQuad);
            }
        }
        
        // Fade in screen
        screenCanvasGroup.alpha = 0f;
        screenCanvasGroup.DOFade(1f, transitionDuration).SetEase(Ease.OutQuad);
        
        // Reset all elements to initial state
        ResetElements();
        
        // Start animation sequence
        AnimateTitle();
        AnimateStar();
        AnimateStarScore();
        AnimateRewards();
        AnimateButtons();
        ActivateParticles();
    }

    private void ResetElements()
    {
        // Title - start above screen
        if (titleLevelImage != null)
        {
            titleLevelImage.anchoredPosition = new Vector2(titleLevelImage.anchoredPosition.x, Screen.height);
            titleLevelImage.localScale = Vector3.one;
        }
        
        if (titleCompletedImage != null)
        {
            titleCompletedImage.anchoredPosition = new Vector2(titleCompletedImage.anchoredPosition.x, Screen.height);
            titleCompletedImage.localScale = Vector3.one;
        }
        
        // Star - start small and rotated
        if (starBig != null)
        {
            starBig.localScale = Vector3.zero;
            starBig.rotation = Quaternion.Euler(0, 0, -180f);
        }
        
        // Score - start invisible
        if (starScoreText != null)
        {
            starScoreText.localScale = Vector3.zero;
        }
        
        // Rewards - start small
        foreach (var reward in rewardElements)
        {
            if (reward != null)
            {
                reward.localScale = Vector3.zero;
            }
        }
        
        // Buttons container - start below screen
        if (buttonsContainer != null)
        {
            buttonsContainer.anchoredPosition = new Vector2(buttonsContainer.anchoredPosition.x, -Screen.height);
        }
    }

    private void AnimateTitle()
    {
        // "Level" drops in first
        if (titleLevelImage != null)
        {
            titleLevelImage.DOAnchorPos(titleTargetPosition, titleDuration)
                .SetDelay(titleDelay)
                .SetEase(Ease.OutBounce);
        }
    
        // "Completed!" drops in slightly after
        if (titleCompletedImage != null)
        {
            titleCompletedImage.DOAnchorPos(titleTargetPosition, titleDuration)
                .SetDelay(titleDelay + 0.15f)
                .SetEase(Ease.OutBounce);
        }
    }

    private void AnimateStar()
    {
        if (starBig == null) return;
        
        // Scale up with elastic bounce
        starBig.DOScale(1f, starDuration)
            .SetDelay(starDelay)
            .SetEase(Ease.OutElastic);
        
        // Rotate to upright position
        starBig.DORotate(Vector3.zero, starDuration)
            .SetDelay(starDelay)
            .SetEase(Ease.OutBack);
    }

    private void AnimateStarScore()
    {
        if (starScoreText == null) return;
        
        // Pop in with punch
        starScoreText.DOScale(1f, scoreDuration)
            .SetDelay(scoreDelay)
            .SetEase(Ease.OutBack);
        
        // Optional: Punch scale for extra juice
        starScoreText.DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 0.5f)
            .SetDelay(scoreDelay + scoreDuration);
    }

    private void AnimateRewards()
    {
        for (int i = 0; i < rewardElements.Length; i++)
        {
            if (rewardElements[i] == null) continue;
            
            float delay = rewardsDelay + (i * rewardStagger);
            
            rewardElements[i].DOScale(1f, 0.4f)
                .SetDelay(delay)
                .SetEase(Ease.OutBack);
        }
    }

    private void AnimateButtons()
    {
        if (buttonsContainer != null)
        {
            buttonsContainer.DOAnchorPos(buttonsTargetPosition, buttonsDuration)
                .SetDelay(buttonsDelay)
                .SetEase(Ease.OutBack);
        }
    }

    private void ActivateParticles()
    {
        // Burst particles (one-shot)
        if (particlesBurst != null)
        {
            DOVirtual.DelayedCall(starDelay + 0.2f, () =>
            {
                particlesBurst.Play();
            });
        }
        
        // Looped particles (continuous)
        if (particlesLoopedTop != null)
        {
            DOVirtual.DelayedCall(0.5f, () =>
            {
                particlesLoopedTop.Play();
            });
        }
        
        if (particlesLoopedMiddle != null)
        {
            DOVirtual.DelayedCall(0.6f, () =>
            {
                particlesLoopedMiddle.Play();
            });
        }
    }

    private void OnHomeButtonClicked()
    {
        AnimateOut(() =>
        {
            // Show home screen
            if (homeScreen != null)
            {
                CanvasGroup homeCanvasGroup = homeScreen.GetComponent<CanvasGroup>();
                if (homeCanvasGroup != null)
                {
                    homeCanvasGroup.DOFade(1f, transitionDuration).SetEase(Ease.OutQuad);
                }
            }
            
            gameObject.SetActive(false);
        });
    }

    private void OnContinueButtonClicked()
    {
        AnimateOut(() =>
        {
            // Handle continue logic here
            Debug.Log("Continue to next level");
            gameObject.SetActive(false);
        });
    }

    private void AnimateOut(Action onComplete = null)
    {
        // Stop looped particles
        if (particlesLoopedTop != null) particlesLoopedTop.Stop();
        if (particlesLoopedMiddle != null) particlesLoopedMiddle.Stop();
        
        // Fade out screen
        screenCanvasGroup.DOFade(0f, transitionDuration)
            .SetEase(Ease.InQuad)
            .OnComplete(() => onComplete?.Invoke());
        
        // Optional: Scale down star as it fades
        if (starBig != null)
        {
            starBig.DOScale(0f, transitionDuration).SetEase(Ease.InBack);
        }
    }

    private void OnDestroy()
    {
        if (homeButtonComponent != null)
        {
            homeButtonComponent.onClick.RemoveListener(OnHomeButtonClicked);
        }
        
        if (continueButtonComponent != null)
        {
            continueButtonComponent.onClick.RemoveListener(OnContinueButtonClicked);
        }
    }
}