
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Header("Estadísticas")]
    public int maxHealth = 100;
    public int health;
    public int maxArmor = 100;
    public int armor = 0; // Inicia sin armadura

    [Header("Conexión al HUD")]
    public TextMeshProUGUI textoVida;
    public TextMeshProUGUI textoArmadura;

    void Start()
    {
        health = maxHealth;
        ActualizarHUD();
    }

    public void RecibirDano(int cantidad)
    {
        if (armor > 0)
        {
            int danoSobrante = cantidad - armor;
            armor -= cantidad;
            
            if (armor < 0) armor = 0;

            if (danoSobrante > 0)
            {
                health -= danoSobrante;
            }
        }
        else
        {
            health -= cantidad;
        }

        // EL SEGURO: Evita que la vida baje de 0
        if (health < 0) 
        {
            health = 0;
        }

        ActualizarHUD();

        if (health == 0)
        {
            Debug.Log("¡Te mataron:c");
            // Aquí puedes llamar a una pantalla de Game Over después
        }
    }

    public void Curar(int cantidad)
    {
        health += cantidad;
        if (health > maxHealth) health = maxHealth;
        ActualizarHUD();
    }

    public void RepararArmadura(int cantidad)
    {
        armor += cantidad;
        if (armor > maxArmor) armor = maxArmor;
        ActualizarHUD();
    }

    private void ActualizarHUD()
    {
        // Esto cambia los números de la pantalla en tiempo real
        if (textoVida != null) textoVida.text = health.ToString();
        if (textoArmadura != null) textoArmadura.text = armor.ToString();
    }
}
