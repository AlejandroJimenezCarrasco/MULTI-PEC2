using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using Complete;

public class CameraManager : MonoBehaviour
{
    public GameManager gameManager;
    public CinemachineBrain cinemachineBrain;
    public CinemachineCamera[] cameras;  // Array de c�maras de Cinemachine
    public Camera[] cams;
    public Complete.TankManager[] playersToFollow;  // Array de jugadores (TankManager)
    public CinemachineCamera auxiliaryCamera;  // C�mara auxiliar ortogr�fica
    public string channel;


    // Configur
    // a las c�maras en funci�n del n�mero de jugadores

    public void SetGameManager(GameManager manager)
    {
        gameManager = manager;
        Debug.Log("GameManager asignado al CameraManager.");
    }

    public void resetCameras()
    {
        foreach (var camera in cameras)
        {
            camera.gameObject.SetActive(false);
        }

        foreach (var cam in cams)
        {
            cam.gameObject.SetActive(false);
        }
    }
    public void SetCameras(int numPlayers, Complete.TankManager[] players)
    {
        // Aseg�rate de que las c�maras est�n activadas o desactivadas correctamente
       

        // Actualiza la lista de jugadores
        playersToFollow = players;

        

        // L�gica para diferentes configuraciones de c�maras
        switch (numPlayers)
        {
            case 2:
                SetHorizontalSplit();
                break;

            case 3:
                SetThreePlayers();
                break;

            case 4:
                SetFourPlayers();
                break;

            
        }
    }

    // Divide la c�mara en dos partes horizontales
    private void SetHorizontalSplit()
    {
        cameras[1].gameObject.SetActive(true);
        cameras[1].Follow = playersToFollow[0].m_Instance.transform;

        cameras[2].gameObject.SetActive(true);
        cameras[2].Follow = playersToFollow[1].m_Instance.transform;

        // Ajustar Viewport Rect para pantalla dividida verticalmente (lado a lado)
        cams[1].gameObject.SetActive(true);
        cams[1].rect = new Rect(0, 0, 0.5f, 1f); // C�mara izquierda

        cams[2].gameObject.SetActive(true);
        cams[2].rect = new Rect(0.5f, 0, 0.5f, 1f); // C�mara derecha
    }

    // Configura las c�maras para 3 jugadores
    private void SetThreePlayers()
    {
        cameras[1].gameObject.SetActive(true);
        cameras[1].Follow = playersToFollow[0].m_Instance.transform;

        cameras[2].gameObject.SetActive(true);
        cameras[2].Follow = playersToFollow[1].m_Instance.transform;

        cameras[3].gameObject.SetActive(true);
        cameras[3].Follow = playersToFollow[2].m_Instance.transform;

        

        cameras[0].gameObject.SetActive(true);


        cams[1].gameObject.SetActive(true);
        cams[1].rect = new Rect(0, 0.5f, 0.5f, 0.5f);

        cams[2].gameObject.SetActive(true);
        cams[2].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);

        cams[3].gameObject.SetActive(true);
        cams[3].rect = new Rect(0, 0, 0.5f, 0.5f);

        cams[0].gameObject.SetActive(true);
        cams[0].rect = new Rect(0.5f, 0, 0.5f, 0.5f);
    }

    // Configura las c�maras para 4 jugadores
    private void SetFourPlayers()
    {
        cameras[1].gameObject.SetActive(true);
        cameras[1].Follow = playersToFollow[0].m_Instance.transform;

        cameras[2].gameObject.SetActive(true);
        cameras[2].Follow = playersToFollow[1].m_Instance.transform;

        cameras[3].gameObject.SetActive(true);
        cameras[3].Follow = playersToFollow[2].m_Instance.transform;

        cameras[4].gameObject.SetActive(true);
        cameras[4].Follow = playersToFollow[3].m_Instance.transform;

        

        // Ajustar Viewport Rect para dividir la pantalla en 4 jugadores
        cams[1].gameObject.SetActive(true);
        cams[1].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
        
        cams[2].gameObject.SetActive(true);
        cams[2].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);

        cams[3].gameObject.SetActive(true);
        cams[3].rect = new Rect(0, 0, 0.5f, 0.5f);

        cams[4].gameObject.SetActive(true);
        cams[4].rect = new Rect(0.5f, 0, 0.5f, 0.5f);

    }





    int newTarget = 0;


    public void ChangeChannel(int index)
    {
        
        newTarget++;
        
        if(gameManager.numberPlayers == 2)
        {
            if ((newTarget >= playersToFollow.Length) || newTarget == 2)
            {
                newTarget = 0;
            }
        }

        if (gameManager.numberPlayers == 3)
        {
            if ((newTarget >= playersToFollow.Length) || newTarget == 3)
            {
                newTarget = 0;
            }
        }

        if (gameManager.numberPlayers == 4)
        {
            if ((newTarget >= playersToFollow.Length) || newTarget == 4)
            {
                newTarget = 0;
            }
        }



        cameras[index].Follow = playersToFollow[newTarget].m_Instance.transform;

    }
}
