using UnityEngine;
using Mirror;

namespace Complete
{
    public class ShellExplosion : NetworkBehaviour
    {
        public LayerMask m_TankMask;
        public GameObject m_ExplosionPrefab;  // Prefab con ParticleSystem, AudioSource y NetworkIdentity
        public float m_MaxDamage = 100f;
        public float m_ExplosionForce = 1000f;
        public float m_MaxLifeTime = 2f;
        public float m_ExplosionRadius = 5f;
        GameObject explosionInstance;

        private void Start()
        {
            Destroy(gameObject, m_MaxLifeTime);
        }

        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            // L�gica de da�o y f�sica en el servidor
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
                Debug.Log($"[ShellExplosion] Damaging {targetHealth.gameObject.name} | isServer={targetHealth.isServer} | isClient={targetHealth.isClient}");
                float damage = CalculateDamage(targetRigidbody.position);
                targetHealth.TakeDamage(damage);
            }

            // Instanciar y hacer spawn del prefab de la explosi�n en el servidor
            explosionInstance = Instantiate(m_ExplosionPrefab, transform.position, transform.rotation);
            NetworkServer.Spawn(explosionInstance);

           

            // Destruir la shell en el servidor
            Destroy(gameObject);
        }

        

        private float CalculateDamage(Vector3 targetPosition)
        {
            Vector3 explosionToTarget = targetPosition - transform.position;
            float explosionDistance = explosionToTarget.magnitude;
            float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;
            float damage = relativeDistance * m_MaxDamage;
            return Mathf.Max(0f, damage);
        }
    }
}
