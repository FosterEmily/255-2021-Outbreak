using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Miller
{
    public class PlayerMovement : MonoBehaviour
    {

        public enum MoveState
        {
            Regular, // 0
            Dashing, // 1
            Sprinting, // 2
            Sneaking // 3
        }


        public float playerSpeed = 10;
        private CharacterController pawn;
        MoveState currentMoveState = MoveState.Regular;

        /// <summary>
        /// How long a dash should take in seconds:
        /// </summary>
        public float dashDuration = 0.25f;

        private Vector3 dashDirection;
        /// <summary>
        /// This stores how many seconds are left:
        /// </summary>
        private float dashTImer = 0;

        void Start()
        {
            pawn = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {

            print(currentMoveState);

            switch (currentMoveState)
            {
                case MoveState.Regular:

                    // do behavior for this state:

                    MoveThePlayer(1);

                    // transitions to other states:
                    //if (Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking;
                    if (Input.GetButton("Fire3")) currentMoveState = MoveState.Sprinting;
                    if (Input.GetButton("Fire2"))
                    {
                        currentMoveState = MoveState.Dashing;
                        float h = Input.GetAxis("Horizontal");
                        float v = Input.GetAxis("Vertical");
                        dashDirection = new Vector3(h, 0, v);

                        //clamp the length of dashDir to 1:
                        if (dashDirection.sqrMagnitude > 1) dashDirection.Normalize();
                    }
                    break;
                case MoveState.Dashing:

                    // do behavior for this state:
                    DashThePlayer();

                    dashTImer -= Time.deltaTime;

                    // transitions to other states:
                    if (dashTImer <= 0) currentMoveState = MoveState.Regular;

                    break;
                case MoveState.Sprinting:

                    // do behavior for this state:

                    MoveThePlayer(2);


                    // transitions to other states:
                    if (!Input.GetButton("Fire3")) currentMoveState = MoveState.Regular;
                    if (!Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking;

                    break;
                case MoveState.Sneaking:

                    // do behavior for this state:

                    MoveThePlayer(0.5f);

                    // transitions to other states:
                    //if (!Input.GetButton("Fire1")) currentMoveState = MoveState.Regular;


                    break;
            }

        }

        /// <summary>
        /// this function moves the player while they are dashing
        /// </summary>
        private void DashThePlayer()
        {
            pawn.Move(dashDirection * Time.deltaTime * 25);
        }

        private void MoveThePlayer(float mult = 1)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 move = Vector3.right * h + Vector3.forward * v;

            if (move.sqrMagnitude > 1) move.Normalize(); // fix diagonal input vectors

            pawn.Move(move * Time.deltaTime * playerSpeed * mult);

        }
    }
}
