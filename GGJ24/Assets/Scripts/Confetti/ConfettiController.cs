using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class ConfettiController: MonoBehaviour
{
    public SpriteRenderer sp;

    public Sprite[] sps;

    // The prefab to be instiated
    public GameObject block;
    // The range for the force;
    public float min_Force;
    public float max_Force;

    // random index of the prefabs in the list.
    int index;

    // The range for the torque; Not sure whether negative means clockwise or counter-clockwise.
    public float min_Torque;
    public float max_Torque;

    // Force actually work on the generated object;
    float force;

    // Initial force direction work on the generated object;
    float angle;

    // Torque actually work on the generated object;
    float torque;

    // called on button clicked;
    public void shoot_block()
    {
        force = Random.Range(min_Force, max_Force);
        torque = Random.Range(min_Torque, max_Torque);
        angle = Random.Range(0, Mathf.PI);
        GameObject g = Instantiate(block, transform);
        index = Random.Range(0, sps.Length);
        sp = g.GetComponent<SpriteRenderer>();
        sp.sprite = sps[index];
        g.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * force, ForceMode2D.Impulse);
        g.GetComponent<Rigidbody2D>().AddTorque(torque, ForceMode2D.Impulse);
    }
}
