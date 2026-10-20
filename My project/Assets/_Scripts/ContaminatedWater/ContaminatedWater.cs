using UnityEngine;
using System.Collections.Generic;

public class ContaminatedWater : MonoBehaviour
{
    [Header("Configuración de Daño")]
    public float damagePerTick = 5f;   // Daño por tick
    public float tickInterval = 1f;    // Segundos entre cada tick

    [Header("Tags afectados")]
    public string playerTag = "Player";
    public string enemyTag = "Enemy";  // bórralo si aún no tienes enemigos

    // Lista manual (por seguridad, aunque con CharacterController no hace falta)
    private class TargetInfo
    {
        public GameObject obj;
        public float nextTickTime;
    }
    private List<TargetInfo> targetsInside = new List<TargetInfo>();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) || other.CompareTag(enemyTag))
        {
            if (targetsInside.Exists(t => t.obj == other.gameObject)) return;

            targetsInside.Add(new TargetInfo
            {
                obj = other.gameObject,
                nextTickTime = Time.time // Primer tick inmediato
            });

            Debug.Log($"[Agua] {other.name} entró al agua contaminada.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        targetsInside.RemoveAll(t => t.obj == other.gameObject);
    }

    void Update()
    {
        for (int i = targetsInside.Count - 1; i >= 0; i--)
        {
            TargetInfo info = targetsInside[i];

            if (info.obj == null)
            {
                targetsInside.RemoveAt(i);
                continue;
            }

            if (Time.time >= info.nextTickTime)
            {
                ApplyDamage(info.obj);
                info.nextTickTime = Time.time + tickInterval;
            }
        }
    }

    void ApplyDamage(GameObject target)
    {
        // CONEXIÓN CON EL SISTEMA DE VIDA
        PlayerStats stats = target.GetComponent<PlayerStats>();
        if (stats != null)
        {
            // RecibirDano recibe int → redondeamos
            stats.RecibirDano(Mathf.RoundToInt(damagePerTick));
            Debug.Log($"☠️ {target.name} recibió {damagePerTick} de daño (agua contaminada).");
        }
    }

    // Gizmo verde para ver la zona en la escena
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
        BoxCollider bc = GetComponent<BoxCollider>();
        if (bc != null)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(bc.center, bc.size);
        }
    }
}