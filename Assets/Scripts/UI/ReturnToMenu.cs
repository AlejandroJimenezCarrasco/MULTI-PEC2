using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
public class ReturnToMenu : MonoBehaviour
{
    public void ReturnToMenuButton()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
        }
        else if (NetworkServer.active)
        {
            NetworkManager.singleton.StopServer();
        }
        else
        {
            SceneManager.LoadScene("Scene0"); 
        }
    }
}
