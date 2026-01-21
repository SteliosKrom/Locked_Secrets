using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    private float returnToMainMenuDelay = 14f;
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameManager.Instance.CurrentGameState = GameState.None;
        GameManager.Instance.CurrentMenuState = MenuState.OnTitleMenu;
    }
}
