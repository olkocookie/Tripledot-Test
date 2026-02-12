using UnityEngine;
using UnityEngine.UI;

public class AdaptiveBackground : MonoBehaviour
{
    [SerializeField] private float tabletYOffset = -111f;
    
    private AspectRatioFitter _aspectFitter;
    private RectTransform _rectTransform;
    private bool _isTabletMode = false;

#if UNITY_EDITOR
    private int _lastScreenWidth;
    private int _lastScreenHeight;
#endif

    private void Start()
    {
        _aspectFitter = GetComponent<AspectRatioFitter>();
        _rectTransform = GetComponent<RectTransform>();
        
        DetermineDeviceMode();
        
#if UNITY_EDITOR
        _lastScreenWidth = Screen.width;
        _lastScreenHeight = Screen.height;
#endif
    }

#if UNITY_EDITOR
    private void Update()
    {
        // Editor only - detect resolution changes for testing
        if (Screen.width != _lastScreenWidth || Screen.height != _lastScreenHeight)
        {
            _lastScreenWidth = Screen.width;
            _lastScreenHeight = Screen.height;
            DetermineDeviceMode();
        }
    }
#endif

    private void LateUpdate()
    {
        if (!_isTabletMode) return;

        // Always enforce position on tablets
        Vector2 currentPos = _rectTransform.anchoredPosition;
        if (Mathf.Abs(currentPos.y - tabletYOffset) > 0.1f)
        {
            _rectTransform.anchoredPosition = new Vector2(currentPos.x, tabletYOffset);
        }
    }

    private void DetermineDeviceMode()
    {
        float screenAspect = (float)Screen.width / Screen.height;

        if (screenAspect > 0.65f)
        {
            // Tablet mode - apply Y offset
            _isTabletMode = true;
            
            if (_aspectFitter != null)
            {
                _aspectFitter.enabled = true;
                _aspectFitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
            }
        }
        else
        {
            // Phone mode - keep centered
            _isTabletMode = false;
            
            if (_aspectFitter != null)
            {
                _aspectFitter.enabled = true;
                _aspectFitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
            }

            if (_rectTransform != null)
            {
                _rectTransform.anchoredPosition = Vector2.zero;
            }
        }
    }
}