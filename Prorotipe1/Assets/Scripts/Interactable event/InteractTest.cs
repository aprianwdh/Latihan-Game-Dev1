using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InteractTest : MonoBehaviour
{
    private GameManager gameManager;
    private Player player;
    public int sceenIndex = 0;

    private void Start()
    {
        gameManager = GameManager.instance;
        player = FindAnyObjectByType<Player>();

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
                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    gameManager.player_last_position = player.transform.position;
                    Debug.Log("Player last position saved");
                }
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
