using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerActions : MonoBehaviour
{

    public static PlayerActions instance;
    public bool aiming;
    public Rigidbody axeRB;

    public Transform target;
    public Transform hand;
    public Transform curveposition;
    private Vector3 oldpos;
    private bool isReturning = false;
    bool axeinHand = true;
    private float time = 0.0f;
    [SerializeField] float throwPower;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !axeinHand)
        { returnAxe(); }
        if (Input.GetMouseButton(0) && axeinHand)
        {
            PlayerMovement.movement.speed = 1.5f;
            axeinHand = false;
            PlayerMovement.movement.animator.SetTrigger("throw");
        }

        if (isReturning)
        {
            if (time < 1.0f)
            {
                axeRB.position = getBQCPoint(time, oldpos, curveposition.position, target.position);
                axeRB.rotation = Quaternion.Slerp(axeRB.rotation, target.rotation, 35 * Time.deltaTime);
                time += Time.deltaTime;
            }
            else
            {
                resetAxe();
            }
        }
    }

    public void AxeThrow()
    {
        axeRB.isKinematic = false;
        axeRB.transform.parent = null;
        axeRB.AddForce(PlayerMovement.movement.cam.transform.forward * throwPower, ForceMode.Impulse);
        AxeScript.instance.activated = true;
    }

    void returnAxe()
    { 
        isReturning = true;
        oldpos = axeRB.position;
        PlayerMovement.movement.animator.Play("Recall");
        AxeScript.instance.activated = false;
        axeRB.velocity = Vector3.zero;
        axeRB.isKinematic = true;
    }

    public void resetAxe()
    {
        axeinHand = true;
        time = 0.0f;
        isReturning = false;
        axeRB.transform.parent = hand;
        axeRB.transform.rotation = target.rotation;
        axeRB.transform.position = target.position;
        PlayerMovement.movement.animator.SetTrigger("Caught");
        Debug.Log("Reset");
    }

    Vector3 getBQCPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = (uu * p0) + (2 * u * t * p1) + (tt * p2);
        return p;
    }
}
