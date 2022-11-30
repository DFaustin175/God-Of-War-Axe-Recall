using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;
    public Transform cam;
    public static PlayerMovement movement;

    public float speed;
    [SerializeField] float mouseSens;
    float turnsmoothTime = 0.1f;
    float turnsmoothvel;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        movement = this;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(PlayerActions.instance.aiming)
        {
            float cAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cam.eulerAngles.y, ref turnsmoothvel, turnsmoothTime);
            transform.rotation = Quaternion.Euler(0f, cAngle, 0f);

            if(direction.magnitude > .1f)
            {
                controller.Move(direction * speed * Time.deltaTime);
                if (vertical == 1)
                {
                    animator.SetBool("movingForward", true);
                    animator.SetBool("movingBackward", false);
                    animator.SetBool("leftStrafe", false);
                    animator.SetBool("rightStrafe", false);
                }
                else if (vertical == -1)
                {
                    animator.SetBool("movingForward", false);
                    animator.SetBool("movingBackward", true);
                    animator.SetBool("leftStrafe", false);
                    animator.SetBool("rightStrafe", false);
                }
                else if (horizontal == 1 && vertical == 0)
                {
                    animator.SetBool("movingForward", false);
                    animator.SetBool("movingBackward", false);
                    animator.SetBool("leftStrafe", false);
                    animator.SetBool("rightStrafe", true);
                }
                else if (horizontal == -1 && vertical == 0)
                {
                    animator.SetBool("movingForward", false);
                    animator.SetBool("movingBackward", false);
                    animator.SetBool("leftStrafe", true);
                    animator.SetBool("rightStrafe", false);
                }
                
            }
            else
            {
                animator.SetBool("movingForward", false);
                animator.SetBool("movingBackward", false);
                animator.SetBool("leftStrafe", false);
                animator.SetBool("rightStrafe", false);
            }
        }
        else if(!PlayerActions.instance.aiming)
        {
                float targetangle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref turnsmoothvel, turnsmoothTime);


                transform.rotation = Quaternion.Euler(0f, angle, 0f);

            if(direction.magnitude > .1f)
            {
                animator.SetBool("movingForward", true);
                Vector3 moveDir = Quaternion.Euler(0f, targetangle, 0f) * Vector3.forward;
                controller.Move(moveDir * speed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("movingForward", false);
                animator.SetBool("movingBackward", false);
                animator.SetBool("leftStrafe", false);
                animator.SetBool("rightStrafe", false);
            }
        }
    }
}
