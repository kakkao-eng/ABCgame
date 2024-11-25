using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1;
        Health target = null;
        float damage = 0;
        Vector3 direction;

        // Update is called once per frame
        void Update()
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }

        public void SetTarget(Health target, float damage) 
        {
            this.target = target;
            this.damage = damage;
            direction = (GetAimLocation() - transform.position).normalized;
            
            // Rotate the projectile to face the initial direction
            transform.rotation = Quaternion.LookRotation(direction);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
