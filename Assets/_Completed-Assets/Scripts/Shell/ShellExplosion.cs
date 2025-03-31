using UnityEngine;
using Mirror;

namespace Complete
{
    public class ShellExplosion : NetworkBehaviour
    {
        public LayerMask m_TankMask;                        // Used to filter what the explosion affects, this should be set to "Players".
        public ParticleSystem m_ExplosionParticles;         // Reference to the particles that will play on explosion.
        public AudioSource m_ExplosionAudio;                // Reference to the audio that will play on explosion.
        public float m_MaxDamage = 100f;                    // The amount of damage done if the explosion is centred on a tank.
        public float m_ExplosionForce = 1000f;              // The amount of force added to a tank at the centre of the explosion.
        public float m_MaxLifeTime = 2f;                    // The time in seconds before the shell is removed.
        public float m_ExplosionRadius = 5f;                // The maximum distance away from the explosion tanks can be and are still affected.


        private void Start()
        {
            // If it isn't destroyed by then, destroy the shell after it's lifetime.
            Destroy(gameObject, m_MaxLifeTime);
        }

        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            // Lógica de daño y física (en el servidor)
            Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);
            for (int i = 0; i < colliders.Length; i++)
            {
                Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
                if (!targetRigidbody)
                    continue;

                targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

                TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();
                if (!targetHealth)
                    continue;

                float damage = CalculateDamage(targetRigidbody.position);
                targetHealth.TakeDamage(damage);
            }

            // Notificar a los clientes que reproduzcan los efectos de la explosión
            RpcPlayExplosion();

            // Destruir la shell en el servidor
            Destroy(gameObject);
        }

        [ClientRpc]
        private void RpcPlayExplosion()
        {
            // Desacoplar el sistema de partículas y reproducir efectos en cada cliente
            m_ExplosionParticles.transform.parent = null;
            m_ExplosionParticles.Play();
            m_ExplosionAudio.Play();

            ParticleSystem.MainModule mainModule = m_ExplosionParticles.main;
            Destroy(m_ExplosionParticles.gameObject, mainModule.duration);
        }



        private float CalculateDamage(Vector3 targetPosition)
        {
            // Create a vector from the shell to the target.
            Vector3 explosionToTarget = targetPosition - transform.position;

            // Calculate the distance from the shell to the target.
            float explosionDistance = explosionToTarget.magnitude;

            // Calculate the proportion of the maximum distance (the explosionRadius) the target is away.
            float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

            // Calculate damage as this proportion of the maximum possible damage.
            float damage = relativeDistance * m_MaxDamage;

            // Make sure that the minimum damage is always 0.
            damage = Mathf.Max(0f, damage);

            return damage;
        }
    }
}