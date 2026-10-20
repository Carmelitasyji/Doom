using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;

    public float gravity = -9.81f;

    public float movementSpeed;
    public float rotationSpeed;

    private float currentRotationPos;

    [Header("Configuración del Jetpack")]
    public float fuerzaJetpack = 5f;
    public float combustibleMaximo = 100f;
    public float consumoPorSegundo = 25f;
    public float recargaPorSegundo = 10f;
    public float combustibleActual;

    [Header("HUD del Jetpack")]
    public TextMeshProUGUI textoJetpack;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        combustibleActual = combustibleMaximo; // Inicia con el tanque lleno
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Rotation();
        ActualizarHUDJetpack();
    }

    public void Movement()
    {
        Vector3 inputVector = new Vector3(0, 0, InputController.Instance.movementVector.y);
        inputVector = transform.TransformDirection(inputVector);

        float verticalVelocity = gravity; 

        // Leemos directamente del InputController
        bool isJetpackPressed = InputController.Instance.isJetpackActive;

        if (isJetpackPressed && combustibleActual > 0)
        {
            verticalVelocity = fuerzaJetpack; 
            combustibleActual -= consumoPorSegundo * Time.deltaTime;
        }
        else
        {
            if (characterController.isGrounded && combustibleActual < combustibleMaximo)
            {
                combustibleActual += recargaPorSegundo * Time.deltaTime;
            }
        }

        combustibleActual = Mathf.Clamp(combustibleActual, 0, combustibleMaximo);

        Vector3 finalMovement = (inputVector * movementSpeed) + (Vector3.up * verticalVelocity);

        characterController.Move(finalMovement * Time.deltaTime);
    }

    public void Rotation()
    {
        float rotationInput = InputController.Instance.movementVector.x * rotationSpeed * Time.deltaTime;

        currentRotationPos += rotationInput;

        transform.localRotation = Quaternion.AngleAxis(currentRotationPos, transform.up);
    }

    private void ActualizarHUDJetpack()
    {
        if (textoJetpack != null)
        {
            // Convertimos la gasolina a un porcentaje entero (0 a 100)
            int porcentaje = Mathf.RoundToInt((combustibleActual / combustibleMaximo) * 100f);
            
            // Le pegamos el símbolo de % al final para que se vea profesional
            textoJetpack.text = porcentaje.ToString() + "%";
        }
    }

}
