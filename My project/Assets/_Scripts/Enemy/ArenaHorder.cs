using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaHorder : MonoBehaviour
{
    [Header("Configuración de la Arena")]
    public GameObject[] barreras; 
    public Transform[] puntosDeSpawn; 
    public GameObject prefabEnemigo; 

    private List<GameObject> enemigosActivos = new List<GameObject>();
    private bool hordaIniciada = false;
    private bool hordaCompletada = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hordaIniciada && !hordaCompletada)
        {
            IniciarHorda();
        }
    }

    private void IniciarHorda()
    {
        hordaIniciada = true;

        //Activar las barreras
        foreach (GameObject barrera in barreras)
        {
            if(barrera != null) barrera.SetActive(true);
        }

        //Aparecer UN enemigo en cada punto de spawn de la lista
        for (int i = 0; i < puntosDeSpawn.Length; i++)
        {
            Transform puntoElegido = puntosDeSpawn[i];
            GameObject nuevoEnemigo = Instantiate(prefabEnemigo, puntoElegido.position, puntoElegido.rotation);
            enemigosActivos.Add(nuevoEnemigo);
        }
    }

    private void Update()
    {
        if (hordaIniciada && !hordaCompletada)
        {
            // Revisamos y limpiamos la lista de enemigos destruidos
            for (int i = enemigosActivos.Count - 1; i >= 0; i--)
            {
                if (enemigosActivos[i] == null || !enemigosActivos[i].activeInHierarchy)
                {
                    enemigosActivos.RemoveAt(i);
                }
            }

            if (enemigosActivos.Count == 0)
            {
                TerminarHorda();
            }
        }
    }

    private void TerminarHorda()
    {
        hordaCompletada = true;

        // Apagar las barreras al terminar
        foreach (GameObject barrera in barreras)
        {
            if(barrera != null) barrera.SetActive(false);
        }
    }
}
