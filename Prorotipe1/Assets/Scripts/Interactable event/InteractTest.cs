using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InteractTest : MonoBehaviour
{
    private GameManager gameManager;
    public int sceenIndex = 0;

    private void Start()
    {
        gameManager = GameManager.instance;

        if (gameManager == null)
        {
            Debug.LogError("Game Manager not found!");
        }
    }

    private void Update()
    {
        if (gameManager.interactable && gameManager!=null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && gameManager!=null)
        {
            gameManager.interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && gameManager != null)
        {
            gameManager.interactable = false;
        }
    }

    public void Interact()
    {
        Debug.Log("Move to sceen " + sceenIndex);
        GameManager.instance.NextScene(sceenIndex);
    }
}
