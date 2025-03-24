using UnityEngine;
using Unity.Cinemachine;

public class Camara : MonoBehaviour
{   
    public CinemachineCamera camera_1;
    public CinemachineCamera camera_2;

    // Method to switch cameras
    public void ChangeCamera(string zonaCamara)
    {
        if (zonaCamara == "Jugadores")
        {
            camera_1.Priority = 0;
            camera_2.Priority = 10;
        }
        else if (zonaCamara == "Home")
        {
            camera_1.Priority = 10;
            camera_2.Priority = 0;
        }
    }
}
