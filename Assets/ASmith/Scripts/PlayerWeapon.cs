using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASmith
{
    public class PlayerWeapon : MonoBehaviour
    {
        static class States
        {
            public class State
            {
                protected PlayerWeapon weapon;

                virtual public State Update() // virtual allows update to be overriden by the other states
                {
                    return null;
                }
                virtual public void OnStart(PlayerWeapon weapon)
                {
                    this.weapon = weapon;
                }
                virtual public void OnEnd()
                {

                }
            }

            public class Regular : State
            {
                public override State Update()
                {
                    // behavior:

                    // transitions:
                    if (Input.GetButton("Fire1"))
                    {
                        // if no ammo, go to cooldown:
                        if (PlayerWeapon.roundsInClip <= 0)
                        {
                            SoundBoard.PlayPlayerNoAmmo();
                            return new States.Cooldown(weapon.reloadTime); // Originally weapon.roundsInClip
                        }

                        // if ammo, go to shooting:
                        return new States.Attacking();
                    }
                    if (Input.GetButtonDown("Reload"))
                    {
                        // if clip is full, don't reload:
                        if (PlayerWeapon.roundsInClip > 9) return new States.Regular(); // Originally weapon.roundsInClip

                        // if clip is not full, reload
                        return new States.Cooldown(weapon.reloadTime);
                    }
                    return null;
                }
            }

            public class Attacking : State
            {
                public override State Update()
                {
                    // behavior:
                    weapon.SpawnGoodBullet();

                    // transitions:
                    if (!Input.GetButtonDown("Fire1")) return new States.Regular();

                    return null;
                }
            }

            public class Cooldown : State
            {
                float timeLeft = 3;

                public Cooldown(float time)
                {
                    timeLeft = time;
                }
                public override State Update()
                {
                    timeLeft -= Time.deltaTime;

                    if (timeLeft <= 0) return new States.Regular();

                    return null;
                }

                public override void OnEnd()
                {
                    PlayerWeapon.roundsInClip = weapon.maxRoundsInClip; // Originally weapon.roundsInClip
                }
            }
        }

        public GoodBullet prefabGoodBullet;
        private States.State state;

        public int maxRoundsInClip = 10;
        public static int roundsInClip;

        /// <summary>
        /// How many bullets to spawn per second
        /// Used to calculate timing between bullets
        /// </summary>
        public float roundsPerSecond = 5;

        /// <summary>
        /// How many seconds until player is able to fire again
        /// </summary>
        private float timerSpawnBullet = 0;

        public float reloadTime = 1;

        private void Start()
        {
            roundsInClip = maxRoundsInClip;
        }

        void Update()
        {
            if (timerSpawnBullet > 0) timerSpawnBullet -= Time.deltaTime;
            if (state == null) SwitchState(new States.Regular());

            // call state.Update()
            // switch to the returned state
            if (state != null) SwitchState(state.Update());            
        }

        void SwitchState(States.State newState)
        {
            if (newState == null) return;

            if (state != null) state.OnEnd();

            state = newState;
            state.OnStart(this);
        }

        void SpawnGoodBullet()
        {
            SoundBoard.PlayPlayerShoot();
            if (timerSpawnBullet > 0) return; // need to wait longer
            if (roundsInClip <= 0) return; // no ammo

            GoodBullet p = Instantiate(prefabGoodBullet, transform.position, Quaternion.identity);
            p.InitBullet(transform.forward * 20);

            roundsInClip--;
            timerSpawnBullet = 1 / roundsPerSecond;
        }
    }
}
