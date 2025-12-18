using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddTriggerEvent : MonoBehaviour
{
    #region AUDIO
    [Header("AUDIO SOURCES")]
    [SerializeField] private AudioSource hoverAudioSource;

    [Header("AUDIO CLIPS")]
    [SerializeField] private AudioClip hoverAudioClip;
    #endregion

    // Event Triggers on TextMeshProUGUI
    public void PointerEnter(TextMeshProUGUI text)
    {
        text.color = Color.red;
        AudioManager.Instance.PlaySFX(hoverAudioSource, hoverAudioClip);
    }

    public void PointerExit(TextMeshProUGUI text)
    {
        text.color = Color.white;
    }

    public void PointerClick(TextMeshProUGUI text)
    {
        text.color = Color.white;
    }

    // Event Triggers on Images

    public void TogglePointerEnter(Image toggleImage)
    {
        toggleImage.color = Color.red;
        AudioManager.Instance.PlaySFX(hoverAudioSource, hoverAudioClip);
    }

    public void TogglePointerExit(Image toggleImage)
    {
        toggleImage.color = Color.white;
    }
}
