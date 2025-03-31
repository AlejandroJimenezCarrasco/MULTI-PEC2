using UnityEngine;
using Mirror;

public class ExplosionEffect : NetworkBehaviour
{
    public ParticleSystem explosionParticles;
    //public AudioSource explosionAudio;

    private void Start()
    {
        Debug.Log("llegamos");
        explosionParticles = GetComponent<ParticleSystem>();
        // Desacoplar el sistema de part�culas del objeto padre, en caso de que lo necesites
        explosionParticles.transform.parent = null;
        explosionParticles.Play();
        //explosionAudio.Play();

        // Destruir el prefab una vez que la duraci�n de las part�culas haya finalizado
        ParticleSystem.MainModule mainModule = explosionParticles.main;
        Destroy(gameObject, mainModule.duration);
    }
}

