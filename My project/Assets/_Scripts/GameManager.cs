using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Referencias")]
    public PlayerStats playerStats;
    public Transform spawnPoint;          // Punto de inicio (ver Paso 3)
    public CharacterController playerController;

    [Header("Respawn")]
    public float delayAntesDeRespawn = 1.5f; // Segundos de espera al morir

    private bool isRespawning = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (playerStats != null)
            playerStats.OnPlayerDied += HandlePlayerDeath;
    }

    private void OnDestroy()
    {
        if (playerStats != null)
            playerStats.OnPlayerDied -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath()
    {
        if (isRespawning) return;
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        isRespawning = true;

        Debug.Log("💀 Jugador muerto. Respawneando en " + delayAntesDeRespawn + "s...");

        // Aquí podrías mostrar pantalla de "HAS MUERTO"
        yield return new WaitForSeconds(delayAntesDeRespawn);

        // 1️⃣ Desactivar CharacterController (obligatorio para teletransportar)
        if (playerController != null) playerController.enabled = false;

        // 2️⃣ Mover al spawn
        if (spawnPoint != null && playerStats != null)
        {
            playerStats.transform.position = spawnPoint.position;
            playerStats.transform.rotation = spawnPoint.rotation;
        }

        // 3️⃣ Reactivar CharacterController
        if (playerController != null) playerController.enabled = true;

        // 4️⃣ Resetear vida y armadura
        playerStats.ResetearStats();

        // 5️⃣ (Opcional) Resetear jetpack
        PlayerMovement pm = playerStats.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.combustibleActual = pm.combustibleMaximo;
        }

        isRespawning = false;
        Debug.Log("✅ Respawn completado.");
    }
}