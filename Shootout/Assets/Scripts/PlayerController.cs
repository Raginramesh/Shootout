using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerInputHandler m_InputHandler;
    CharacterController m_Controller;
    Animator m_anime;

    public float InputX;
    public float InputZ;
    public float playerSpeed;
    public float rotationSpeed;
    public float velocity;

    public Vector3 moveDirection;

    public bool isGrounded;

    public float verticalVel;
    private Vector3 moveVector;


    // Start is called before the first frame update
    void Start()
    {
        m_InputHandler = this.GetComponent<PlayerInputHandler>();
        m_Controller = this.GetComponent<CharacterController>();
        m_anime = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoveAndRotate();

        isGrounded = m_Controller.isGrounded;
        if (isGrounded)
        {
            verticalVel -= 0;
        }
        else
        {
            verticalVel -= 1;
        }
        moveVector = new Vector3(0, verticalVel * .2f * Time.deltaTime, 0);
        m_Controller.Move(moveVector);
    }

    private void PlayerMoveAndRotate()
    {
        if (m_InputHandler.currentTouchStatus == PlayerInputHandler.TouchStatus.Swipe)
        {
            InputX = m_InputHandler.direction.x;
            InputZ = m_InputHandler.direction.y;

            moveDirection = Vector3.forward * -InputZ + Vector3.right * -InputX;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotationSpeed);
            m_Controller.Move(moveDirection * Time.deltaTime * velocity);
        }
    }
}
