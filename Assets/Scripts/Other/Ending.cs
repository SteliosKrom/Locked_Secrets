using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    private float returnToMainMenuDelay = 7f;
    private float radius = 0.2f;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private GameObject endingOutro;

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState == GameState.OnPlaying)
        {
            if (IsGrounded())
            {
                EndGame();
            }
        }
    }

    public void EndGame()
    {
        GameManager.Instance.CurrentGameState = GameState.OnEnding;
        endingOutro.SetActive(true);
        StartCoroutine(ReturnToMainMenuDelay());
    }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(transform.position, radius, groundLayer);
    }

    public IEnumerator ReturnToMainMenuDelay()
    {
        yield return new WaitForSeconds(returnToMainMenuDelay);
        SceneManager.LoadScene("Main");
    }
}
