using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Configuración de Daño")]
    public int danoPorGolpe = 10;
    public float velocidadDeAtaque = 1.5f; // Pega cada 1.5 segundos

    private bool isAttacking;
    private PlayerStats playerStats;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            isAttacking = true;
            
            // Buscamos el PlayerStats en la raíz por si el collider choca con un hijo
            playerStats = other.GetComponentInParent<PlayerStats>(); 
            
            StartCoroutine(RutinaDeAtaque());
            Debug.Log("¡El enemigo te está atacando cuerpo a cuerpo!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            isAttacking = false;
            playerStats = null;
            Debug.Log("Saliste de la zona de peligro.");
        }
    }

    IEnumerator RutinaDeAtaque()
    {
        // Mientras sigas dentro de su rango de ataque y estés vivo
        while (isAttacking && playerStats != null)
        {
            playerStats.RecibirDano(danoPorGolpe);
            yield return new WaitForSeconds(velocidadDeAtaque);
        }
    }
}
