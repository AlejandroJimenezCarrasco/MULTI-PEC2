using Mirror;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void OnHostButtonPressed()
    {
        NetworkManager.singleton.StartHost();
    }
}
