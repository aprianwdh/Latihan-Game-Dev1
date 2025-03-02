using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InteractTest : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();

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
        Debug.Log("Interacted with " + gameObject.name);
        GameManager.instance.NextSceen();
    }
}
