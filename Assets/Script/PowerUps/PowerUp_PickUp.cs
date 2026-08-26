using UnityEngine;

public enum PowerUpType
{
    Hermes,
    Athena,
    Zeus
}
public class PowerUpPickup : MonoBehaviour
{
    public PowerUpType type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PowerUp_Manager manager = other.GetComponent<PowerUp_Manager>();
        if (manager != null)
        {
            bool stored = manager.TryStore(type);
            if (!stored)
            {
                return;
            }
        }

        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.PlayItemSound();
        }

        Destroy(gameObject);
    }
}