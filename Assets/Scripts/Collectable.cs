using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public new string name;
    public ParticleSystem particleEffect;

    public void OnDestroy()
    {
        ParticleSystem particle = Instantiate(particleEffect, transform.position, Quaternion.identity);
        Destroy(particle.gameObject, 1f);
    }
}
