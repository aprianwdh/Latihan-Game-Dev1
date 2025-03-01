using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InteractTest : MonoBehaviour
{

    private void Update()
    {
        if (FindAnyObjectByType<GameManager>().interactable)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindAnyObjectByType<GameManager>().interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindAnyObjectByType<GameManager>().interactable = false;
        }
    }

    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
