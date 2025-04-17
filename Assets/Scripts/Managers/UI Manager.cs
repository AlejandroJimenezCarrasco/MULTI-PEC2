using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;
using Mirror; 
using Mirror.Discovery;
using UnityEngine.UI;

namespace UIManager // Cambia el namespace si lo necesitas
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkManager))]
    public class CustomNetworkManager : MonoBehaviour
    {
        public NetworkDiscovery networkDiscovery;
        
        readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();

        public GameObject ServersFound;
        
        void Awake()
        {
            networkDiscovery = GetComponent<NetworkDiscovery>();
        }

        /// <summary>
        /// Inicia la partida en modo host (servidor + cliente).
        /// </summary>
        public void CrearPartida()
        {
            discoveredServers.Clear();
            NetworkManager.singleton.StartHost();
            networkDiscovery.AdvertiseServer();
        }
        
        /// <summary>
        /// Se conecta como cliente a una partida existente.
        /// </summary>
        public void UnirseAPartida()
        {
            discoveredServers.Clear();
            networkDiscovery.StartDiscovery();
            
            ServersFound.SetActive(true);
        }
        /// <summary>
        /// Inicia el servidor sin un cliente.
        /// </summary>
        public void CrearServidor()
        {
            discoveredServers.Clear();
            NetworkManager.singleton.StartServer();
            networkDiscovery.AdvertiseServer();
        }
        
        void Connect(ServerResponse info)
        {
            networkDiscovery.StopDiscovery();
            NetworkManager.singleton.StartClient(info.uri);
        }
        
        public void OnDiscoveredServer(ServerResponse info)
        {
            discoveredServers[info.serverId] = info;
            
            UnityEngine.UI.Button connectButton = ServersFound.GetComponentInChildren<UnityEngine.UI.Button>();
            connectButton.GetComponentInChildren<Text>().text = info.EndPoint.Address.ToString();
            
            connectButton.onClick.RemoveAllListeners();
            connectButton.onClick.AddListener(() => Connect(info));
        }

    }
}
