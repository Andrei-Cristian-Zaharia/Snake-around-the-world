using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    const float G = 667.4f;
    public bool isPlanet;

    public Rigidbody rb;

    public static List<Attractor> Attractors;

    private void FixedUpdate()
    {
        foreach (Attractor attractor in Attractors)
        {
            if (attractor.isPlanet && !isPlanet || !attractor.isPlanet && isPlanet && attractor != this)
                Attract(attractor);
        }
    }

    void OnEnable()
    {
        if (Attractors == null)
            Attractors = new List<Attractor>();

        Attractors.Add(this);
    }

    void OnDisable()
    {
        Attractors.Remove(this);
    }

    void Attract (Attractor objToAttract)
    {
        Rigidbody rbToAttract = objToAttract.rb;

        Vector3 direction = rb.position - rbToAttract.position;
        float distance = direction.magnitude;

        if (distance == 0) return;

        float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }
}
