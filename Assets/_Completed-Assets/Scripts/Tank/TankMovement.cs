using UnityEngine;
using Mirror;

namespace Complete
{
    public class TankMovement : NetworkBehaviour
    {
        [Header("Movimiento")]
        public int m_PlayerNumber = 1;            // Identifica el tanque del jugador.
        public float m_Speed = 12f;               // Velocidad de movimiento.
        public float m_TurnSpeed = 180f;          // Velocidad de giro (grados/segundo).

        [Header("Audio")]
        public AudioSource m_MovementAudio;       // Fuente de audio para el motor.
        public AudioClip m_EngineIdling;          // Sonido cuando el tanque está parado.
        public AudioClip m_EngineDriving;         // Sonido cuando el tanque se mueve.
        public float m_PitchRange = 0.2f;         // Rango de variación del pitch.

        private string m_MovementAxisName;        // Nombre del eje de movimiento.
        private string m_TurnAxisName;            // Nombre del eje de giro.
        private Rigidbody m_Rigidbody;            // Referencia al Rigidbody del tanque.
        private float m_MovementInputValue;       // Valor actual de movimiento.
        private float m_TurnInputValue;           // Valor actual de giro.
        private float m_OriginalPitch;            // Pitch original del audio.
        private ParticleSystem[] m_particleSystems; // Sistemas de partículas del tanque.

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            // Hacemos que el tanque no sea kinematic para que se mueva.
            m_Rigidbody.isKinematic = false;
            m_MovementInputValue = 0f;
            m_TurnInputValue = 0f;

            // Obtenemos los ParticleSystems hijos para controlarlos al activar/desactivar.
            m_particleSystems = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Play();
            }
        }

        private void OnDisable()
        {
            // Al desactivar, hacemos el Rigidbody kinematic y detenemos los ParticleSystems.
            m_Rigidbody.isKinematic = true;
            for (int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Stop();
            }
        }

        private void Start()
        {
            // Configuramos los nombres de los ejes de entrada basados en el número de jugador.
            m_MovementAxisName = "Vertical" + m_PlayerNumber;
            m_TurnAxisName = "Horizontal" + m_PlayerNumber;
            m_OriginalPitch = m_MovementAudio.pitch;
        }

        private void Update()
        {
            // Solo procesamos si la aplicación tiene foco.
            if (!Application.isFocused)
                return;

            // Solo el jugador local controla su tanque.
            if (isLocalPlayer)
            {
                m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
                m_TurnInputValue = Input.GetAxis(m_TurnAxisName);
                EngineAudio();
            }
        }

        private void FixedUpdate()
        {
            // Desde el cliente local, enviamos el input al servidor.
            if (isLocalPlayer)
            {
                CmdMoveTank(m_MovementInputValue, m_TurnInputValue);
            }
        }

        // Este Command se ejecuta en el servidor para mover el tanque
        [Command]
        void CmdMoveTank(float movementInput, float turnInput)
        {
            // Calculamos el vector de movimiento y la rotación.
            Vector3 movement = transform.forward * movementInput * m_Speed * Time.fixedDeltaTime;
            float turn = turnInput * m_TurnSpeed * Time.fixedDeltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

            // Actualizamos la posición y rotación en el servidor.
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
        }

        // Controla el audio del motor basado en la entrada.
        private void EngineAudio()
        {
            if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
            {
                // Si está inactivo y se está reproduciendo el audio de movimiento, cambiar a inactivo.
                if (m_MovementAudio.clip == m_EngineDriving)
                {
                    m_MovementAudio.clip = m_EngineIdling;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
            else
            {
                // Si se mueve y se está reproduciendo el audio inactivo, cambiar a movimiento.
                if (m_MovementAudio.clip == m_EngineIdling)
                {
                    m_MovementAudio.clip = m_EngineDriving;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
        }
    }
}
