using UnityEngine;

public class UnlockDoorAnimationEvent : MonoBehaviour
{
    public void TriggerUnlock()
    {
        AudioManager.Instance.UnlockDoor.source.transform.position = AudioManager.Instance.TriggerInteractable3DAudio.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.UnlockDoor);
    }
}
