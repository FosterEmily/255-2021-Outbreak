using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kortge
{
    public class PlayerAiming : MonoBehaviour
    {
        private Camera cam;
        public Transform debugObject;
        public Projectile prefabProjectile;
        private Animator animator;
        public bool stabbing;
        public int roses = 0;
        public Projectile beamPrefab;

        // Start is called before the first frame update
        void Start()
        {
            cam = Camera.main;
            animator = GetComponentInChildren<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            AimAtMouse();
            if (Input.GetButtonDown("Fire1") && !stabbing)
            {
                animator.SetTrigger("Stab");
                if (roses > 0)
                {
                    Projectile beam = Instantiate(beamPrefab, transform.position + transform.forward, transform.rotation);
                    beam.InitBullet(transform.forward * 20);
                    roses--;
                }
            }
        }

        private void AimAtMouse()
        {
            // make a ray and a plane:
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position);

            // does the ray hit the plane?
            if (plane.Raycast(ray, out float dis))
            {
                // find where the ray hit the plane:
                Vector3 hitPos = ray.GetPoint(dis);

                if (debugObject) debugObject.position = hitPos;

                Vector3 vectorToHitPos = hitPos - transform.position;

                float angle = Mathf.Atan2(vectorToHitPos.x, vectorToHitPos.z);

                angle /= Mathf.PI;
                angle *= 180; // Converts from radians to half-circles to degrees.
                transform.eulerAngles = new Vector3(0, angle, 0);
            }
        }
    }
}