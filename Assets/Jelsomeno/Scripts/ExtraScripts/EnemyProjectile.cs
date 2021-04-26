using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Jelsomeno
{
    public class EnemyProjectile : MonoBehaviour
    {
        private Vector3 velocity;
        private float lifespan = 3;

        private float age = 0;

        public float damageAmt = 20;

        public ParticleSystem bulletImpact;

        public void InitBullet(Vector3 vel)
        {
            velocity = vel; 
        }

        void Update()
        {

            age += Time.deltaTime; 
            if (age > lifespan)
            { 
                Destroy(gameObject);
            }


      
            transform.position += velocity * Time.deltaTime; 
        }


        private void OnTriggerEnter(Collider other)
        {
            HealthSystem healthOfThing = other.GetComponent<HealthSystem>(); // making a local variable from getting access from HealthScript
            if (healthOfThing)
            { // if the healthOfThing has data/storing something
                healthOfThing.DamageTaken(damageAmt); // damages the target
            }

            Instantiate(bulletImpact, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject); // destroys the projectile
            
        }
    }
}
