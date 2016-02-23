using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2DCustom))]
    public class Platformer2DUserControlCustom : MonoBehaviour
    {
        private PlatformerCharacter2DCustom m_Character;
        private bool m_Jump;


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2DCustom>();
        }


        private void Update()
        {
            if (!m_Jump && !m_Character.Frozen)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }


        private void FixedUpdate()
        {
            if (m_Character.Frozen) {
                m_Character.Move(0, false, false, false);
                return;
            }

            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            bool down = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_Jump, down);
            m_Jump = false;
        }
    }
}