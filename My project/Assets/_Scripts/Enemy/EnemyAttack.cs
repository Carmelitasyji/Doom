
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private EnemyAggro enemyAggro;
    [HideInInspector] public bool isAttacking;

    private void Start()
    {
        enemyAggro = GetComponentInParent<EnemyAggro>();
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //enemyAggro.animator.SetBool("isAttacking", true);
            isAttacking = true;
            Debug.Log("El jugador ha entrado al área de daño del enemigo.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //enemyAggro.animator.SetBool("isAttacking", false);
            isAttacking = false;
            Debug.Log("El jugador ha salido del área de daño del enemigo.");
        }
    }

}
