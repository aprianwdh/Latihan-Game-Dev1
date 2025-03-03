using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public int healthUp = 1;

    private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameManager.current_health_player += healthUp;
            // Animasi: Gerakkan ke atas, lalu hilangkan
            LeanTween.scale(gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInQuad).setOnComplete(() => Destroy(gameObject));
        }
    }
}
