using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BottomBarView : MonoBehaviour
{
    [field: Header("Events")]
    public event Action<int> OnContentActivated;
    public event Action OnClosed;
    
    [Header("Configuration")]
    [SerializeField] private List<BottomBarButton> buttons = new List<BottomBarButton>();
    [SerializeField] private int defaultActiveButtonIndex = 2; // Center "Home" button
    
    [Header("Animation Settings")]
    [SerializeField] private float transitionDuration = 0.6f;
    [SerializeField] private Ease activationEase = Ease.OutBack;
    [SerializeField] private Ease deactivationEase = Ease.OutSine;
    
    [Header("Show/Hide Animation")]
    [SerializeField] private float showDuration = 0.7f;
    [SerializeField] private float hideDuration = 0.5f;
    [SerializeField] private Ease showEase = Ease.OutBack;
    [SerializeField] private Ease hideEase = Ease.InBack;
    [SerializeField] private float hideOffset = 100f; // Extra distance to hide below screen
    
    private int currentActiveIndex = -1;
    private bool isTransitioning = false;

    private void Start()
    {
        // Start at correct position (bottom of screen)
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, -rectTransform.rect.height); // Start hidden below
    
        InitializeButtons();
        ActivateButton(defaultActiveButtonIndex, immediate: true);
    
        // Show after 0.5 seconds
        DOVirtual.DelayedCall(0.5f, () => Show());
        
        // TEST: Unlock left button after 3 seconds
        //DOVirtual.DelayedCall(3f, () => UnlockButton(0));
    
        // TEST: Unlock right button after 5 seconds
        //DOVirtual.DelayedCall(5f, () => UnlockButton(4));
    }

    private void InitializeButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i; // Capture for closure
            buttons[i].Initialize(index);
            buttons[i].OnButtonClicked += HandleButtonClicked;
        }
    }

    private void HandleButtonClicked(int buttonIndex)
    {
        // Ignore if already active or transitioning
        if (buttonIndex == currentActiveIndex || isTransitioning)
            return;

        // Check if button is locked
        if (buttons[buttonIndex].IsLocked)
        {
            buttons[buttonIndex].PlayLockedAnimation();
            return;
        }

        // Switch to new button
        SwitchActiveButton(buttonIndex);
    }

    private void SwitchActiveButton(int newIndex)
    {
        isTransitioning = true;

        // Deactivate current button (if any)
        if (currentActiveIndex >= 0)
        {
            buttons[currentActiveIndex].Deactivate(transitionDuration, deactivationEase);
        }

        // Activate new button
        buttons[newIndex].Activate(transitionDuration, activationEase, () =>
        {
            isTransitioning = false;
        });

        currentActiveIndex = newIndex;

        // Fire event
        OnContentActivated?.Invoke(newIndex);
    }

    private void ActivateButton(int index, bool immediate = false)
    {
        if (immediate)
        {
            buttons[index].SetActiveImmediate();
            currentActiveIndex = index;
            OnContentActivated?.Invoke(index);
        }
        else
        {
            SwitchActiveButton(index);
        }
    }

    /// <summary>
    /// Deactivate all buttons. Optional feature.
    /// </summary>
    public void DeactivateAll()
    {
        if (currentActiveIndex >= 0)
        {
            buttons[currentActiveIndex].Deactivate(transitionDuration, deactivationEase);
            currentActiveIndex = -1;
            OnClosed?.Invoke();
        }
    }

    /// <summary>
    /// Manually set active button from external code
    /// </summary>
    public void SetActiveButton(int index)
    {
        if (index < 0 || index >= buttons.Count)
        {
            Debug.LogError($"Invalid button index: {index}");
            return;
        }

        SwitchActiveButton(index);
    }

    /// <summary>
    /// Show or hide a button (useful for progressive unlocking)
    /// </summary>
    public void SetButtonVisibility(int buttonIndex, bool visible)
    {
        if (buttonIndex >= 0 && buttonIndex < buttons.Count)
        {
            buttons[buttonIndex].gameObject.SetActive(visible);
        }
    }

    /// <summary>
    /// Unlock a previously locked button with animation
    /// </summary>
    public void UnlockButton(int buttonIndex)
    {
        if (buttonIndex >= 0 && buttonIndex < buttons.Count)
        {
            buttons[buttonIndex].UnlockButton();
        }
    }

    /// <summary>
    /// Slide bottom bar up from offscreen
    /// </summary>
    public void Show(Action onComplete = null)
    {
        // For bottom-anchored UI, we animate the anchoredPosition, not localPosition
        RectTransform rectTransform = GetComponent<RectTransform>();
    
        rectTransform.DOAnchorPosY(0, showDuration)
            .From(new Vector2(0, -rectTransform.rect.height - hideOffset)) // Start below screen
            .SetEase(showEase)
            .OnComplete(() => onComplete?.Invoke());
    }

    /// <summary>
    /// Slide bottom bar down offscreen
    /// </summary>
    public void Hide(Action onComplete = null)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
    
        rectTransform.DOAnchorPosY(-rectTransform.rect.height - hideOffset, hideDuration)
            .SetEase(hideEase)
            .OnComplete(() => onComplete?.Invoke());
    }

    private void OnDestroy()
    {
        // Cleanup event subscriptions
        foreach (var button in buttons)
        {
            if (button != null)
            {
                button.OnButtonClicked -= HandleButtonClicked;
            }
        }
    }
}