using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 3;

    private int currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;   
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        LeanTween.color(gameObject, Color.red, 0.2f)
        .setEase(LeanTweenType.easeInOutQuint)
        .setOnComplete(() =>
        {
            LeanTween.color(gameObject, Color.white, 0.2f)
            .setEase(LeanTweenType.easeInOutQuint)
            .setOnComplete(() =>
            {
                if (currentHealth <= 0)
                {
                    Die();
                }
            });
        });

    }

    void Die()
    {
        Destroy(gameObject);
    }
}
