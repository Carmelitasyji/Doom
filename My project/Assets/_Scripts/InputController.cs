using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static InputController Instance;

    InputAction movementInput;
    InputAction shootInput;
    InputAction jetpackInput;

    [HideInInspector] public Vector2 movementVector;
    [HideInInspector] public bool isJetpackActive;

    private void Awake()
    {
       if (Instance == null)
        {
            Instance = this;
        }
       else
        {
            Destroy(this);
        }

        movementInput = InputSystem.actions.FindAction("Move");
        shootInput = InputSystem.actions.FindAction("Attack");
        jetpackInput = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        GetMoveInput();
        Shoot();
        Jetpack();
    }

    private void Shoot()
    {
        if (shootInput.WasPressedThisFrame())
        {
            GunController.Instance.Fire();
        }
    }

    public void GetMoveInput()
    {
        movementVector = movementInput.ReadValue<Vector2>();
    }
    private void Jetpack()
    {
        // Lee si el botón se mantiene presionado durante este frame
        if (jetpackInput != null)
        {
            isJetpackActive = jetpackInput.IsPressed();
        }
    }
}
