using UnityEngine;

public class Spike : MonoBehaviour
{
    private Player player;

    public int damage = 1;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.TakeDamage(damage);
            // Animasi: Gerakkan ke atas, lalu hilangkan
            LeanTween.moveY(gameObject, transform.position.y + 5f, 0.5f).setEase(LeanTweenType.easeOutQuad);
            LeanTween.scale(gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInQuad).setOnComplete(() => Destroy(gameObject));
        }
    }
}
