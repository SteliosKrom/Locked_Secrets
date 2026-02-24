using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    private float splashScreenDelay = 6f;

    [SerializeField] private GameObject seizureWarning;
    [SerializeField] private GameObject headsetPanel;

    private void Start()
    {
        StartCoroutine(SplashScreenDelay());
    }

    public IEnumerator SplashScreenDelay()
    {
        seizureWarning.SetActive(true);
        yield return new WaitForSeconds(splashScreenDelay);
        headsetPanel.SetActive(true);
        yield return new WaitForSeconds(splashScreenDelay);
        SceneManager.LoadScene("Main");
    }
}   
