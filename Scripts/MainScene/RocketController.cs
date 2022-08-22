using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RocketController : MonoBehaviour
{
    //Constants
    private Rigidbody rb;
    public Transform bottomCheck;
    public RocketInfo rocketInfo;
    public Universe universe;
    public GameObject gimball;


    //Control Parameters
    private float thrust;
    private Vector3 tilt;

    private bool unityPhysics;

    //Read Only Parameters
    public Vector3 a;
    public Vector3 s;
    public Vector3 v;
    public Vector3 aA;
    public Vector3 aV;
    public Vector3 aS;
    public float mass;

    public float rotationalIntertia;

    //Gizmos
    public Vector3 centerOfMass;
    public float r;

    void Start()
    {
        //Settings
        thrust = 30f;
        unityPhysics = true;
        mass = 1f;
        rotationalIntertia = 10000f;

        //Constants
        rb = GetComponent<Rigidbody>();
        rocketInfo = GetComponent<RocketInfo>();
        universe = FindObjectOfType<Universe>();
        s = this.transform.position;
        rb.mass = mass;
    }

    void FixedUpdate()
    {
        //Reset
        a = new Vector3(0f, 0f, 0f);

        //Tilt
        tilt += new Vector3(Input.GetAxis("Horizontal"), 0f, -Input.GetAxis("Vertical"));
        gimball.transform.localRotation = Quaternion.Euler(tilt);

        //Thrust
        if (Input.GetKey("space"))
        {
            //acceleration
            Vector3 dir = gimball.transform.rotation * Vector3.up;
            Vector3 vec = dir * thrust;
 
            //torque
            float twoPi = 2 * Mathf.PI;
            Vector3 T;
            T.x = r * thrust * Mathf.Sin(gimball.transform.localRotation.x * twoPi);
            T.y = 0f;
            T.z = r * thrust * Mathf.Sin(gimball.transform.localRotation.z * twoPi);

            //apply
            a += vec / mass;
            aA = T / rotationalIntertia;
        }

        //Fall Correction
        if (rocketInfo.height > 0f)
        {
            SimulateGravity();
        }
        else
        {
            HitFloor();
        }

        ApplyTransform(universe.timeStep);
    }

    void ApplyTransform(float timeStep)
    {
        float t = timeStep;

        if (unityPhysics)
        {
            rb.AddForce(a, ForceMode.Acceleration);
            rb.AddTorque(aA, ForceMode.Acceleration);
        }
        else
        {
            v += (a * t);
            s += (v * t) + (0.5f * t * t * a);
            this.transform.position = s;

            aV += (aA * t);
            aS += (aV * t) + (0.5f * t * t * aA);
            this.transform.rotation = Quaternion.Euler(aS);
        }

    }

    void HitFloor()
    {
        if (unityPhysics)
        {
            rb.velocity += new Vector3(0f, 0.01f, 0f);
        }
        else
        {
            s.y += 0.01f;
        }

    }

    void SimulateGravity()
    {
        a.y -= rocketInfo.gravity;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(centerOfMass, r);
    }



}
