using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Collectable : MonoBehaviour
{
    public new string name;
    public ParticleSystem particleEffect;
    
    protected bool hit = false;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnDestroy()
    {
        if (hit)
        {
            if (AudioManager.toogleAudio)
            {
                audioSource.Play();
            }

            if (particleEffect != null)
            {
                ParticleSystem particle = Instantiate(particleEffect, transform.position, Quaternion.identity);
                Destroy(particle.gameObject, 0.2f);
            }
        }
    }
}
