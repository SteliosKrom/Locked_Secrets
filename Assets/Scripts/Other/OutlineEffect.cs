using UnityEngine;

public class OutlineEffect : MonoBehaviour
{
    public static OutlineEffect Instance;

    [SerializeField] private Outline[] outlineEffects;

    public Outline[] OutlineEffects => outlineEffects;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DisableFirstPuzzleObjectsOutlineEffect();
    }

    public void EnableFirstPuzzleItemsOutlineEffect()
    {
        foreach (Outline outline in OutlineEffects)
        {
            outline.enabled = true;
        }
    }

    public void DisableFirstPuzzleObjectsOutlineEffect()
    {
        foreach (Outline outline in outlineEffects)
        {
            outline.enabled = false;
        }
    }
}
