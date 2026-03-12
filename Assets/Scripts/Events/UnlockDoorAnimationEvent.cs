using UnityEngine;

public class UnlockDoorAnimationEvent : MonoBehaviour
{
    public void TriggerUnlock()
    {
        AudioManager.Instance.UnlockedDoor.source.transform.position = AudioManager.Instance.TriggerInteractable3DAudio.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.UnlockedDoor.source, AudioManager.Instance.UnlockedDoor.clip);
    }
}
