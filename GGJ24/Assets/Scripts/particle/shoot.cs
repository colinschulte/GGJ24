using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class shoot : MonoBehaviour
{

    public GameObject block;
    // Start is called before the first frame update

    public float min_Force;

    public float max_Force;

    public float min_Torque;

    public float max_Torque;

    float force;

    float angle;

    float torque;


    void Start()
    {
        
    }

    public void shoot_block()
    {
        force = Random.Range(min_Force, max_Force);
        torque = Random.Range(min_Torque, max_Torque);
        angle = Random.Range(-Mathf.PI, Mathf.PI);
        GameObject g = Instantiate(block, transform);
        g.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * force, ForceMode2D.Impulse);
        g.GetComponent<Rigidbody2D>().AddTorque(torque, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
