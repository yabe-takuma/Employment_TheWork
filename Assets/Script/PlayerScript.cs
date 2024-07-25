using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody rb;
    public Animator animator;
    const float moveSpeed = 5.0f;

    private CharacterController characterController;
    private Vector3 velocity;
    [SerializeField]
    private float walkSpeed;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (characterController.isGrounded)
        {
            velocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            if (velocity.magnitude > 0.1f)
            {
                animator.SetFloat("Speed", velocity.magnitude);
                transform.LookAt(transform.position + velocity);
            }
            else
            {
                animator.SetFloat("Speed", 0f);
            }
        }
        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * walkSpeed * Time.deltaTime);
    }
}
