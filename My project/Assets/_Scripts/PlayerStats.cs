using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Header("Estadísticas")]
    public int maxHealth = 100;
    public int health;
    public int maxArmor = 100;
    public int armor = 0;

    [Header("Conexión al HUD")]
    public TextMeshProUGUI textoVida;
    public TextMeshProUGUI textoArmadura;

    public System.Action OnPlayerDied;

    private bool yaMuerto = false;

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

        if (health < 0)
        {
            health = 0;
        }

        ActualizarHUD();

        // 👇 ESTE BLOQUE VA DENTRO DE RecibirDano
        if (health == 0 && !yaMuerto)
        {
            yaMuerto = true;
            Debug.Log("¡Te mataron:c");
            OnPlayerDied?.Invoke();
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

    public void ResetearStats()
    {
        health = maxHealth;
        armor = 0;
        yaMuerto = false;
        ActualizarHUD();
    }

    private void ActualizarHUD()
    {
        if (textoVida != null) textoVida.text = health.ToString();
        if (textoArmadura != null) textoArmadura.text = armor.ToString();
    }
}