using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

namespace UIManager // Cambia el namespace si lo necesitas
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkManager))]
    public class CustomNetworkManager : MonoBehaviour
    {
        private NetworkManager manager;

        void Awake()
        {
            manager = GetComponent<NetworkManager>();
        }

        /// <summary>
        /// Inicia la partida en modo host (servidor + cliente).
        /// </summary>
        public void CrearPartida()
        {
            manager.StartHost();
            manager.ServerChangeScene("_Complete - Game");
        }

        /// <summary>
        /// Se conecta como cliente a una partida existente.
        /// </summary>
        public void UnirseAPartida()
        {
            manager.StartClient();
        }

        /// <summary>
        /// Inicia el servidor sin un cliente.
        /// </summary>
        public void CrearServidor()
        {
            manager.StartServer();
        }
    }
}
