using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupArma : MonoBehaviour
{
    [Header("Configuración del Recolectable")]
    [Tooltip("Escribe 1 si es tu arma primaria, o 2 si es la secundaria")]
    public int idArma = 1; 
    
    [Tooltip("El ScriptableObject de esta arma")]
    public Guns datosDelArma; 
    
    [Tooltip("Cuántas balas te da al recogerla")]
    public int municionQueDa = 30;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Verificamos cuál arma es y se la pasamos al inventario
            if (idArma == 1)
            {
                GunController.Instance.tieneArma1 = true;
                GunController.Instance.municionArma1 += municionQueDa;
                GunController.Instance.EquiparArma(datosDelArma, 1);
            }
            else if (idArma == 2)
            {
                GunController.Instance.tieneArma2 = true;
                GunController.Instance.municionArma2 += municionQueDa;
                GunController.Instance.EquiparArma(datosDelArma, 2);
            }
            
            // Destruimos el objeto del piso
            Destroy(gameObject); 
        }
    }
}
