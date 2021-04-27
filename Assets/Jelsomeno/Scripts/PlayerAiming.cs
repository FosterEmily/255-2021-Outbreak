using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Jelsomeno
{
    /// <summary>
    /// this class will help the player get a better control for aiming at the boss
    /// </summary>
    public class PlayerAiming : MonoBehaviour
    {
        /// <summary>
        /// camera used for the raycast
        /// </summary>
        private Camera cam;

        public Transform debugObject;
        //public Transform PartToRotate;
        //public float turnSpeed = 10;


        // Start is called before the first frame update
        void Start()
        {
            cam = Camera.main; // assigns the camera
        }

        // Update is called once per frame
        void Update()
        {
            AimAtMouse(); // references the AimAtMouse method and runs it once per frame

        }

        private void AimAtMouse()
        {
            // make a ray and a plane 
            Ray ray = cam.ScreenPointToRay(Input.mousePosition); // creates a ray point
            Plane plane = new Plane(Vector3.up, transform.position); // Vector3 up = new Vector3(0, 1, 0)


            // does the ray hit the plane?
            if (plane.Raycast(ray, out float dis))
            {

                // find where the ray hit the plane
                Vector3 hitPos = ray.GetPoint(dis);

                debugObject.position = hitPos;

                Vector3 vectorToHitPos = hitPos - transform.position;

                float angle = Mathf.Atan2(vectorToHitPos.x, vectorToHitPos.z);

                angle /= Mathf.PI; // converts from radians to half circles 
                angle *= 180; // converts to half circles to degrees

                transform.eulerAngles = new Vector3(0, angle, 0);

            }

            //Vector3 dir = debugObject.position - transform.position;
            //Quaternion lookRotation = Quaternion.LookRotation(dir);
            //Vector3 rotation = Quaternion.Lerp(PartToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            //PartToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
        }
    }
}
