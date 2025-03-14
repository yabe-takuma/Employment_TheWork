using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePlayerScript : MonoBehaviour
{
    [SerializeField]
    private CharacterController characterController;
    private Vector3 velocity;
    [SerializeField]
    private Animator animator;
    //private bool isInput;
    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(0, -1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    isInput = true;
        //}
        //プレイヤーが指定した座標まで自動で向かう処理
        if (characterController.isGrounded)
        {
            if (transform.position.z < 192 /*&& isInput*/)
            {
                velocity.z = 1.0f;
                animator.SetFloat("Speed", velocity.magnitude);
            }
            else
            {
                velocity.z = 0.0f;
                animator.SetFloat("Speed", 0f);
            }
        }
        characterController.Move(velocity * Time.deltaTime);
    }
}
