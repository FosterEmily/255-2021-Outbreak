using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Foster
{
    public class PlayerMovement : MonoBehaviour
    {
        public enum MoveState 
        {
                Regular,//0
                Dashing,//1
                Sprinting,//2
                Sneaking,//3
                Shielding,//4
        }

        public float playerSpeed = 10;

        private CharacterController pawn;

        MoveState currentMoveState = MoveState.Regular;

        /// <summary>
        /// How long a dash should take in seconds
        /// </summary>
        public float dashDuration = 0.25f;
        public float dashSpeed = 50;

        private Vector3 dashDirection;

        /// <summary>
        /// This stores how many seconds are left
        /// </summary>
        private float dashTimer = 0;


        //health and mana 
        public static float health { get; set; }
        public float healthMax = 100f;

        public static float mana { get; set; }
        public float manaMax = 100;
        public float manaRegenTimer = 0f;

        private bool pain;

        void Start()
        { 
            pawn = GetComponent<CharacterController>();

            health = healthMax;
            mana = manaMax;


        }

        // Update is called once per frame
        void Update()
        {
            if (health >= 100) health = 100;
            ManaRegen();
            if (manaRegenTimer >= 0) manaRegenTimer -= Time.deltaTime;
            //print(health);
            //print(mana);

            if (pain) health -= .1f;

            switch (currentMoveState)
            {
                case MoveState.Regular:

                    //do behavior for this state;
                    MoveThePlayer(1);

                    //transition to other states;
                    //if (Input.GetButtonDown("Fire1")) currentMoveState = MoveState.Sneaking;
                    if (Input.GetButton("Fire3")) currentMoveState = MoveState.Sprinting;

                    if (Input.GetButtonDown("Fire2")) //transition into dashing
                    {

                        currentMoveState = MoveState.Dashing;
                        float h = Input.GetAxisRaw("Horizontal");
                        float v = Input.GetAxisRaw("Vertical");
                        dashDirection = new Vector3(h, 0, v);
                        dashDirection.Normalize();
                        dashTimer = .5f;

                        //clamps the length of dashDir to 1
                        if (dashDirection.sqrMagnitude > 1) dashDirection.Normalize();

                    }

                    break;

                case MoveState.Dashing:

                    //do behavioyr for this state;
                    DashThePlayer();

                    dashTimer -= Time.deltaTime;

                    //transition to other states;
                    if (dashTimer <= 0) currentMoveState = MoveState.Regular;



                    break;
                case MoveState.Sprinting:

                    //do behavioyr for this state;
                    MoveThePlayer(2);


                    //transition to other states;
                    if (!Input.GetButton("Fire3")) currentMoveState = MoveState.Regular;
                    //if (Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking;


                    break;

                case MoveState.Sneaking:

                    //do behavioyr for this state;
                    MoveThePlayer(0.5f);


                    //transition to other states;
                    //if (!Input.GetButton("Fire1")) currentMoveState = MoveState.Regular;

                    break;

            }

        }
        public void OnTriggerEnter(Collider other)
        {
            if (this.tag == ("Player") & other.tag == ("EnemyBullet"))
            {
                TakeDamage(50);
                print("Player Damaged");
               // Destroy(gameObject);
            }
            if (this.tag == ("Player") & other.tag == ("DamageCircle"))
            {
                pain = true;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (this.tag == ("Player") & other.tag == ("DamageCircle"))
            {
                pain = false;
            }
        }
        //called when the player/boss takes damage
        public void TakeDamage(int amt)
        {
            if (amt <= 0) return;
            health -= amt;

            if (health <= 0) Die();
        }

        //mana regeneration over time when the player/boss uses spells 
        public void ManaRegen()
        {
            if (mana <= 99)
            {
                if (manaRegenTimer <= 0)
                {
                    mana += 1;
                    manaRegenTimer = .06f;
                }
            }
            if (mana == 100) return;
        }

        private void DashThePlayer()
        {
            pawn.Move(dashDirection * Time.deltaTime * dashSpeed);
        }
        private void MoveThePlayer(float mult = 1)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 move = Vector3.right * h + Vector3.forward * v;
            if(move.sqrMagnitude > 1) move.Normalize(); // fix bug with diaginal input vectors

            //pawn.Move(move * Time.deltaTime * playerSpeed * mult);
            pawn.SimpleMove(move * playerSpeed * mult);


        }
        public void Die()
        {
            print("DEAD");
            Destroy(gameObject);
            Outbreak.Game.GameOver();
        }
    }
}