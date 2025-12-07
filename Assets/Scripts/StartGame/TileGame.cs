using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TileGame : MonoBehaviour
{
    private Image sunImage;
    private Tween sunTween;

    [Header("Opacity Settings")]
    [Range(0f, 1f)] public float minAlpha = 0.3f;
    [Range(0f, 1f)] public float maxAlpha = 0.8f;
    public float duration = 1.5f;

    private void Awake()
    {
        sunImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        StartSunEffect();
    }

    private void OnDisable()
    {
        sunTween?.Kill();
    }

    private void StartSunEffect()
    {
        if (sunImage == null) return;

        Color c = sunImage.color;
        c.a = minAlpha;
        sunImage.color = c;

        sunTween = sunImage
            .DOFade(maxAlpha, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
