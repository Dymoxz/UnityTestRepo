using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    public CauldronEnemy cauldron; // Drag the Cauldron here in Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cauldron.StartRunning();
            Destroy(gameObject); // Remove trigger so it only happens once
        }
    }
}