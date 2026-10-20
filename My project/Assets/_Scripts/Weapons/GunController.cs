using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GunController : MonoBehaviour
{
    public static GunController Instance;

    private BoxCollider gunTrigger;
    
    [Header("Arma Equipada")]
    public Guns armaActual; // Ahora el jugador inicia desarmado
    private int idArmaActual = 0;

    [Header("Inventario de Armas y Munición")]
    public bool tieneArma1 = false;
    public int municionArma1 = 0;
    public Guns datosArma1; // Arrastra ScriptableObject del Arma 1 en el Inspector

    public bool tieneArma2 = false;
    public int municionArma2 = 0;
    public Guns datosArma2; // Arrastra ScriptableObject del Arma 2 en el Inspector
    [Header("Modelos en Pantalla")]
    public GameObject modeloArma1;//Cubo 1
    public GameObject modeloArma2;//Cubo 2

    [Header("Textos del HUD")]
    public TextMeshProUGUI textoNombreArma; // <-- Para mostrar cuál arma esta equipada
    public TextMeshProUGUI textoMunicion;   // <-- Para mostrar las balas

    public LayerMask raycastLayerMask;
    public AudioSource audioSource;
    private bool canFire;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        gunTrigger = GetComponent<BoxCollider>();
        armaActual = null; // Iniciamos con las manos vacías
        canFire = true;
        SetTriggers();
        ActualizarHUD(); // <-- Refresca la pantalla al iniciar
    }

    void Update()
    {
        // Sistema para cambiar de arma con los números 1 y 2
        if (Keyboard.current != null)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame && tieneArma1)
            {
                EquiparArma(datosArma1, 1);
            }
            if (Keyboard.current.digit2Key.wasPressedThisFrame && tieneArma2)
            {
                EquiparArma(datosArma2, 2);
            }
        }
    }

    public void EquiparArma(Guns nuevaArma, int id)
    {
        armaActual = nuevaArma;
        idArmaActual = id;
        SetTriggers();
        
        // Prendemos el arma visual que toca y apagamos la otra
        if (modeloArma1 != null) modeloArma1.SetActive(id == 1);
        if (modeloArma2 != null) modeloArma2.SetActive(id == 2);

        ActualizarHUD(); // <-- Refresca la pantalla al iniciar
    }

    IEnumerator CanFire()
    {
        canFire = false;
        yield return new WaitForSeconds(armaActual.fireRate);
        canFire = true;
    }

    public void SetTriggers()
    {
        if (armaActual == null) 
        {
            gunTrigger.enabled = false;
            return;
        }
        
        gunTrigger.enabled = true;
        gunTrigger.size = new Vector3(armaActual.horizontalRange, armaActual.verticalRange, armaActual.range);
        gunTrigger.center = new Vector3(0, (0.5f * armaActual.verticalRange - 1f), armaActual.range * 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Buscamos el componente en el padre por si el collider está en el hijo
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy) EnemyManager.Instance.AddEnemy(enemy);
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy) EnemyManager.Instance.RemoveEnemy(enemy);
    }

    public void Fire()
    {
        // Si no tiene arma equipada o está enfriando, no hace nada
        if (!canFire || armaActual == null) return;

        // Verificar si tiene munición
        if (idArmaActual == 1 && municionArma1 <= 0) return;
        if (idArmaActual == 2 && municionArma2 <= 0) return;

        // Restar munición
        if (idArmaActual == 1) municionArma1--;
        if (idArmaActual == 2) municionArma2--;

        ActualizarHUD(); // <-- Refresca la pantalla al iniciar

        audioSource.PlayOneShot(armaActual.sound);

        // Copiamos la lista para evitar errores si el enemigo muere a mitad de la lectura
        List<Enemy> enemigosActuales = new List<Enemy>(EnemyManager.Instance.enemiesInRange);

        foreach(Enemy enemy in enemigosActuales)
        {
            if (enemy == null) continue;

            var dir = (enemy.transform.position - transform.position).normalized;
            RaycastHit hit;

            // Arreglo del Raycast para impactar al clon inmortal sin importar dónde esté el collider
            if(Physics.Raycast(transform.position, dir, out hit, armaActual.range * 1.5f, raycastLayerMask))
            {
                Enemy hitEnemy = hit.transform.GetComponentInParent<Enemy>();
                if(hitEnemy != null && hitEnemy == enemy)
                {
                    Quaternion rot = Quaternion.LookRotation(-hit.normal);
                    hitEnemy.Damage(armaActual.damage, rot);
                }
            }
        }
        StartCoroutine(CanFire());
    }

    public void ActualizarHUD()
    {
        if (textoNombreArma != null && textoMunicion != null)
        {
            if (armaActual == null)
            {
                textoNombreArma.text = "Desarmado";
                textoMunicion.text = "0";
            }
            else
            {
                textoNombreArma.text = armaActual.name;
                if (idArmaActual == 1) textoMunicion.text = municionArma1.ToString();
                if (idArmaActual == 2) textoMunicion.text = municionArma2.ToString();
            }
        }
    }
}
