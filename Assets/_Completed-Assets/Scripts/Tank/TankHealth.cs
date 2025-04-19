using UnityEngine;
using UnityEngine.UI;
using Mirror;

namespace Complete
{
    public class TankHealth : NetworkBehaviour
    {

        public float m_StartingHealth = 100f;               // The amount of health each tank starts with.
        public Slider m_Slider;                             // The slider to represent how much health the tank currently has.
        public Image m_FillImage;                           // The image component of the slider.
        public Color m_FullHealthColor = Color.green;       // The color the health bar will be when on full health.
        public Color m_ZeroHealthColor = Color.red;         // The color the health bar will be when on no health.
        public GameObject m_ExplosionPrefab;                // A prefab that will be instantiated in Awake, then used whenever the tank dies.


        private AudioSource m_ExplosionAudio;               // The audio source to play when the tank explodes.
        private ParticleSystem m_ExplosionParticles;        // The particle system the will play when the tank is destroyed.
        
        [SyncVar(hook = nameof(OnHealthChanged))] private float m_CurrentHealth;                      // How much health the tank currently has.
        private bool m_Dead;                                // Has the tank been reduced beyond zero health yet?


        private void Awake()
        {
            // Instantiate the explosion prefab and get a reference to the particle system on it.
            m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();

            // Get a reference to the audio source on the instantiated prefab.
            m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

            // Disable the prefab so it can be activated when it's required.
            m_ExplosionParticles.gameObject.SetActive(false);
            //gameManager = FindAnyObjectByType<GameManager>();

        }


        private void OnEnable()
        {
            // When the tank is enabled, reset the tank's health and whether or not it's dead.
            m_Dead = false;

        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            m_CurrentHealth = m_StartingHealth;
            SetHealthUI();
        }

        [Server]
        public void TakeDamage(float amount)
        {
            //Cambiamos este metodo para que OnHealthChange se llamase
           float newHealth = Mathf.Max(0, m_CurrentHealth - amount);

           if (Mathf.Approximately(newHealth, m_CurrentHealth))
               return; 

           m_CurrentHealth = newHealth;
           
           if(connectionToClient != null) ClientUpdateHealth(connectionToClient, newHealth);
        }
        [TargetRpc]
        void ClientUpdateHealth(NetworkConnection target, float newHealth)
        {
            m_CurrentHealth = newHealth;
            SetHealthUI();
        }
        private void OnHealthChanged(float oldHealth, float newHealth)
        {
            SetHealthUI();

            if (m_CurrentHealth <= 0f && !m_Dead)
            {
                OnDeath();
            }
        }

        
        private void SetHealthUI()
        {
            // Set the slider's value appropriately.
            m_Slider.value = m_CurrentHealth;

            // Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
            m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
        }


        private void OnDeath()
        {
            // Set the flag so that this function is only called once.
            m_Dead = true;

            // Move the instantiated explosion prefab to the tank's position and turn it on.
            m_ExplosionParticles.transform.position = transform.position;
            m_ExplosionParticles.gameObject.SetActive(true);

            // Play the particle system of the tank exploding.
            m_ExplosionParticles.Play();

            // Play the tank explosion sound effect.
            m_ExplosionAudio.Play();

            DeactivateTankVisuals(this.gameObject);
            ClientDeath();
			HandleActivePlayers();
        }
        public void DeactivateTankVisuals(GameObject completeTank)
        {
            Transform renderers = completeTank.transform.Find("TankRenderers");
            Transform canvas = completeTank.transform.Find("Canvas");
            if (renderers != null && canvas != null)
            {
                renderers.gameObject.SetActive(false);
                canvas.gameObject.SetActive(false);
            }
            var camControl = FindObjectOfType<CameraControl>();
            if (camControl != null)
            {
                camControl.RemoveTarget(transform); 
            }
        }
        [ClientRpc]
        void ClientDeath()
        {
            if (m_Dead) return;

            m_Dead = true;
            DeactivateTankVisuals(gameObject);
            
            if (m_ExplosionParticles != null)
            {
                m_ExplosionParticles.transform.position = transform.position;
                m_ExplosionParticles.gameObject.SetActive(true);
                m_ExplosionParticles.Play();
            }

            if (m_ExplosionAudio != null)
            {
                m_ExplosionAudio.Play();
            }

            
        }
		void HandleActivePlayers()
		{
			NetworkPlayer np = GetComponent<NetworkPlayer>();
            if (np != null)
            {
                GameManager gm = FindObjectOfType<GameManager>();
                if (gm != null)
                {
                    gm.UnregisterTank(np);
                }
            }       
    	}
    }
}