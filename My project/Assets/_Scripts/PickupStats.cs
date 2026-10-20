using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupStats : MonoBehaviour
{
    [Tooltip("Marca la casilla si es vida. Desmárcala si es armadura")]
    public bool esCura = true; 
    public int cantidadQueDa = 25;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null)
            {
                // Solo lo recoges si no tienes la vida/armadura al máximo
                if (esCura && stats.health < stats.maxHealth)
                {
                    stats.Curar(cantidadQueDa);
                    Destroy(gameObject);
                }
                else if (!esCura && stats.armor < stats.maxArmor)
                {
                    stats.RepararArmadura(cantidadQueDa);
                    Destroy(gameObject);
                }
            }
        }
    }
}
