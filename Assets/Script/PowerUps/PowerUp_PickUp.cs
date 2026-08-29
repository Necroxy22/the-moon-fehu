using UnityEngine;

public enum PowerUpType
{
    Hermes,
    Athena,
    Zeus,
    Pegasus
}

public class PowerUpPickup : MonoBehaviour
{
    public PowerUpType type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PowerUp_Manager manager = other.GetComponent<PowerUp_Manager>();
        if (manager == null) return;

        if (type == PowerUpType.Athena)
        {
            // Bubble Shield langsung pasang di badan player (world buff)
            manager.ActivateBubbleShield();
        }
        else
        {
            // Item manual: coba simpan ke inventory slot (J / K)
            bool stored = manager.TryStore(type);
            if (!stored) return; // Slot penuh, abaikan item
        }

        Destroy(gameObject);
    }
}
