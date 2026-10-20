using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAmmo : MonoBehaviour
{
    [Header("Caja de Munición")]
    public int idArma = 1; // 1 para dar balas al arma principal, 2 para la secundaria
    public int municionQueDa = 15;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Solo da balas si ya recogiste el arma correspondiente antes
            if (idArma == 1 && GunController.Instance.tieneArma1)
            {
                GunController.Instance.municionArma1 += municionQueDa;
                GunController.Instance.ActualizarHUD(); //Llama a la pantalla
                Destroy(gameObject);
            }
            else if (idArma == 2 && GunController.Instance.tieneArma2)
            {
                GunController.Instance.municionArma2 += municionQueDa;
                GunController.Instance.ActualizarHUD(); //Llama a la pantalla
                Destroy(gameObject);
            }
        }
    }
}
