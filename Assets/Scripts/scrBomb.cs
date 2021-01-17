using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrBomb : MonoBehaviour
{
    //declare variables
    float timer;
    float duration;
    float radius;
    float power;
    ParticleSystem explosionParticles;

    //Initialise variables
    void Start() {
        duration = 5.0f;
        timer = 0.0f;
        radius = 5.0F;
        power = 1500.0F;
        explosionParticles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update() {
        timer += Time.deltaTime;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        if (timer >= duration) {
            foreach (Collider hit in colliders) {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                //Run Particle Explosion
                if (rb != null) {
                    explosionParticles.Play();
                    rb.AddExplosionForce(power, transform.position, radius, 3.0F);
                }
            }
            Destroy(gameObject, explosionParticles.main.duration);
        }
    }
}
