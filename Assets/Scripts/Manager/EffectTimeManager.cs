using UnityEngine;

public class EffectTimeManager : Singleton<EffectTimeManager>
{
    [Header("Effect Settings")]
    public GameObject effectObject;   // Object effect có Animator
    public Animator effectAnimator;

    [Header("Animation Name")]
    public string animationName = "SkillEffect";

    private bool isPlaying = false;

    void Start()
    {
        if (effectObject != null)
            effectObject.SetActive(false);
    }

    // Gọi hàm này khi nhấn nút Skill
    public void OnUseSkill()
    {
        // Nếu đang chạy → bỏ qua
        if (isPlaying) return;

        PlayEffect();
    }

    void PlayEffect()
    {
        isPlaying = true;

        // Bật effect
        effectObject.SetActive(true);

        // Reset & chạy lại animation từ đầu
        effectAnimator.Play(animationName, 0, 0f);

        // Tự tắt khi animation kết thúc
        float animLength = GetAnimationLength(animationName);
        Invoke(nameof(StopEffect), animLength);
    }

    void StopEffect()
    {
        effectObject.SetActive(false);
        isPlaying = false;
    }

    float GetAnimationLength(string animName)
    {
        foreach (var clip in effectAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animName)
                return clip.length;
        }
        return 1f; // fallback
    }
}
