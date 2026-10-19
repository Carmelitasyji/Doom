using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class Doors : MonoBehaviour
{
    private Animator animator;
    [Tooltip("0 Purpura, 1 verde, 2 azul")]
    public int keyID;
    private bool hasKey;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        hasKey = false;
    }

    void OnTriggerEnter(Collider other)
    {
        GotKey();
        if (other.CompareTag("Player") && hasKey)
        {
            animator.SetBool(name: "isOpen", true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool(name: "isOpen", false);
        }
    }
    private void GotKey()
    {
        switch (keyID)
        {
            case 0:
                hasKey = Keys.hasPurpleKey;
                break;
            case 1: 
                hasKey = Keys.hasGreenKey;
                break;
            case 2:
                hasKey = Keys.hasBlueKey;
                break;
            default:
                break;
        }
    }
}
