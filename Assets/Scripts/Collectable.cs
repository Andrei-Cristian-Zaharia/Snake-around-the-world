using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Collectable : MonoBehaviour
{
    public new string name;
    public ParticleSystem particleEffect;

    public bool isDebuff = false;

    protected bool hit = false;

    protected AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnDestroy()
    {
        if (!isDebuff)
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
                    Destroy(particle.gameObject, 1f);
                }
            }
        }
        else
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
                    Destroy(particle.gameObject, 1f);
                }
            }
            else
            if (particleEffect != null)
            {
                ParticleSystem particle = Instantiate(particleEffect, transform.position, Quaternion.identity);
                Destroy(particle.gameObject, 1f);
            }

            GameManager GM = (GameManager)GameObject.FindObjectOfType(typeof(GameManager));
            GM.SpawnNewDebuff();
        }
    }
}
