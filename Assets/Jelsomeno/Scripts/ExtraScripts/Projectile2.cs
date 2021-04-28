using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jelsomeno
{
    public class Projectile2 : MonoBehaviour
    {
        private Vector3 velocity;
        private float lifespan = 3;

        private float age = 0;

        public float damageAmt = 20;

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
            HealthSystem healthOfThing = other.GetComponent<HealthSystem>(); 
            if (healthOfThing)
            { // if the healthOfThing has data/storing something
                healthOfThing.DamageTaken(damageAmt); // damages the target
            }

            Destroy(this.gameObject); // destroys the projectile

        }
    }
}
