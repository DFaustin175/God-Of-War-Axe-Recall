using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeScript : MonoBehaviour
{
    public static AxeScript instance;
    [SerializeField] Material HitTarget;


    public GameObject rotationPoint;
    public GameObject explosion;
    public ParticleSystem hitEffect;

    public TrailRenderer trail;
    public bool activated;
    public float rotationSpeed;

    public bool itemHit;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(activated)
        {
            rotationPoint.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
            trail.enabled = true;
        }
        else
        {
            trail.enabled = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject != null)
        {
            activated = false;
            rotationPoint.GetComponent<Rigidbody>().isKinematic = true;
            hitEffect.Play();
            if (collision.gameObject.tag == "Normal Target")
            {
                
                GameManager.instance.Score += 300;
                GameManager.instance.comboMeter++;
                GameManager.instance.comboTimer = 5;
                itemHit = true;
                collision.gameObject.GetComponent<MeshRenderer>().material = HitTarget;
                collision.gameObject.tag = "Hit Target";
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pass Through Target")
        {
            GameManager.instance.Score += 150;
            GameManager.instance.comboMeter++;
            GameManager.instance.comboTimer = 5;
            itemHit = true;
            other.gameObject.GetComponent<MeshRenderer>().material = HitTarget;
            other.gameObject.tag = "Hit Target";
        }
        else if(other.gameObject.tag == "Explosive Target")
        {
            GameManager.instance.Score += 100;
            GameManager.instance.comboMeter++;
            GameManager.instance.comboTimer = 5;
            itemHit = true;
            Instantiate(explosion, other.gameObject.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
    }
}
