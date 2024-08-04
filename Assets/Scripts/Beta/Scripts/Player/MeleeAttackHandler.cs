using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZombieSpace;

// Author : Tarif Khan
// This script is used for melee attacks
public class MeleeAttackHandler : MonoBehaviour
{
    public static void PerformMeleeAttack()
    {
        Transform contactPoint = WeaponManager.contactPoint;
        float attackRange = 2f;
        float attackAngle = 60f;
        Collider[] hitColliders = Physics.OverlapSphere(contactPoint.position, attackRange);

        foreach (Collider hitCollider in hitColliders)
        {
            Vector3 directionToTarget = hitCollider.transform.position - contactPoint.position;
            float angle = Vector3.Angle(contactPoint.forward, directionToTarget);

            if (angle <= attackAngle * 0.5f)
            {
                ZombieHealth zombieHealth = hitCollider.GetComponent<ZombieHealth>();
                if (zombieHealth != null)
                {
                    zombieHealth.TakeDamage(WeaponManager.meleeDamage);
                }
            }
        }
    }

    // Use this to draw out if hits are working.
    void OnDrawGizmosSelected()
    {
        if (WeaponManager.contactPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(WeaponManager.contactPoint.position, 2f);

            Vector3 rightDir = Quaternion.AngleAxis(30f, Vector3.up) * WeaponManager.contactPoint.forward;
            Vector3 leftDir = Quaternion.AngleAxis(-30f, Vector3.up) * WeaponManager.contactPoint.forward;

            Gizmos.DrawRay(WeaponManager.contactPoint.position, rightDir * 2f);
            Gizmos.DrawRay(WeaponManager.contactPoint.position, leftDir * 2f);
        }
    }
}