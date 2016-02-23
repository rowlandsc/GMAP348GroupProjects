using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2DCustom : Character {
        [SerializeField]
        private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField]
        private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [Range(0, 1)]
        [SerializeField]
        private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField]
        private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField]
        private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;

        public float FallTimer = 0.5f;
        private float _currentFallTimer = 0;
        public float KnockbackForce;

        private void Awake() {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }
        


        private void FixedUpdate() {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++) {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }
            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
        }


        public void Move(float move, bool crouch, bool jump, bool down) {
            // If crouching, check to see if the character can stand up
            /*if (!crouch && m_Anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    crouch = true;
                }
            }*/

            // Set whether or not the character is crouching in the animator
            //m_Anim.SetBool("Crouch", crouch);

            if (down && m_Rigidbody2D.velocity.y <= 0 && _currentFallTimer <= 0) {
                _currentFallTimer = FallTimer;
            }
            if (_currentFallTimer > 0) {
                _currentFallTimer -= Time.deltaTime;
            }
            if (_currentFallTimer > 0) {
                gameObject.layer = LayerMask.NameToLayer("PlayerPassthrough");
            }
            else if (m_Rigidbody2D.velocity.y > 0) {
                gameObject.layer = LayerMask.NameToLayer("PlayerJump");
            }
            else {
                gameObject.layer = LayerMask.NameToLayer("PlayerWalkAndFall");
            }

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl) {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move * m_CrouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && CurrentFacing == Facing.LEFT) {
                    // ... flip the player.
                    Flip();
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && CurrentFacing == Facing.RIGHT) {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground")) {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }


        private void Flip() {
            // Switch the way the player is labelled as facing.
            if (CurrentFacing == Facing.LEFT)
                CurrentFacing = Facing.RIGHT;
            else
                CurrentFacing = Facing.LEFT;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        protected override void UpdateFacing() {
            // do nothing here, it's done already
        }


        public override IEnumerator Knock(bool forward, float duration, float driftSpeed, bool killOnFinish) {
            int currentMask = gameObject.layer;
            SetLayerRecursively(gameObject, LayerMask.NameToLayer("Knockback"));
            Frozen = true;

            if ((CurrentFacing == Facing.RIGHT && forward) || (CurrentFacing == Facing.LEFT && !forward)) {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, -45));
            }
            else {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
            }

            float startY = transform.position.y;
            float gravity = 0;
            if (_rigidbody) {
                gravity = _rigidbody.gravityScale;
                _rigidbody.gravityScale = 0;
            }

            transform.position += new Vector3(0, 1, 0);

            int direction = 1;
            if ((CurrentFacing == Facing.RIGHT && forward) || (CurrentFacing == Facing.LEFT && !forward)) {
                direction = 1;
            }
            else {
                direction = -1;
            }

            float timer = 0;
            _rigidbody.AddForce(new Vector2(1, 0) * direction * KnockbackForce);
            while (timer < duration) {
                timer += Time.deltaTime;

                yield return null;
            }

            if (killOnFinish) {
                yield return StartCoroutine(Kill(forward, driftSpeed));
            }
            else {
                //transform.position = new Vector3(transform.position.x, startY, transform.position.z);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                Frozen = false;
                SetLayerRecursively(gameObject, currentMask);
                if (_rigidbody) {
                    _rigidbody.gravityScale = gravity;
                }
            }
        }
    }
}
