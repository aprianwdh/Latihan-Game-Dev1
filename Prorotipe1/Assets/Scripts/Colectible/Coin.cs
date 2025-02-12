using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager gameManager = FindAnyObjectByType<GameManager>();
            gameManager.current_coin_count++;

            // Animasi: Gerakkan ke atas, lalu hilangkan
            LeanTween.moveY(gameObject, transform.position.y + 5f, 0.5f).setEase(LeanTweenType.easeOutQuad);
            LeanTween.scale(gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInQuad).setOnComplete(() => Destroy(gameObject));
        }
    }
}
