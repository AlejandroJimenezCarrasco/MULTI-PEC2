using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Complete;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public int numberPlayers;
    public int m_NumRoundsToWin = 5;
    public float m_StartDelay = 3f;
    public float m_EndDelay = 3f;
    public CameraControl m_CameraControl;
    public Text m_MessageText;
    public GameObject m_TankPrefab;
    public Complete.TankManager[] m_Tanks;
    
    public NPCManager[] m_NPCs;
    public GameObject m_NPCsPrefab;
    public CameraManager cameraManager;


    private int m_RoundNumber;
    private WaitForSeconds m_StartWait;
    private WaitForSeconds m_EndWait;
    private TankManager m_RoundWinner;
    private TankManager m_GameWinner;


    public override void OnStartServer()
    {

        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SpawnAllTanks();
        //SetCameraTargets();
        SpawnAllNPCs();

        StartCoroutine(GameLoop());
    }


    private void SpawnAllTanks()
    {
        for (int i = 0; i < numberPlayers; i++)
        {
            m_Tanks[i].m_Instance =
                Instantiate(m_TankPrefab, m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
            m_Tanks[i].m_PlayerNumber = i + 1;
            m_Tanks[i].Setup();
        }
    }
    [Server]
    private void SpawnAllNPCs()
    {
        Complete.CameraControl camControl = FindObjectOfType<Complete.CameraControl>();

        
        for (int i = 0; i < 4; i++)
        {
            m_NPCs[i].m_Instance =
                Instantiate(m_NPCsPrefab, m_NPCs[i].m_SpawnPoint.position, m_NPCs[i].m_SpawnPoint.rotation) as GameObject;
            //m_NPCs[i].m_PlayerNumber = i + 1;
            NetworkServer.Spawn(m_NPCs[i].m_Instance);
            m_NPCs[i].Setup();
            
            
            
            if (camControl != null)
            {
                camControl.AddTarget(m_NPCs[i].m_Instance.transform); // Afegir aquest tank com a target per la càmera
            }

			
        }
    }


    private void SetCameraTargets()
    {
        /*Transform[] targets = new Transform[m_Tanks.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = m_Tanks[i].m_Instance.transform;
        }

        m_CameraControl.m_Targets = targets;*/
        cameraManager.SetCameras(PlayerNumber.Instance.playerNumber, m_Tanks);
    }


    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        /*        if (m_GameWinner != null)
                {
                    SceneManager.LoadScene(0);
                }
                else
                {
                    StartCoroutine(GameLoop());
                }
        */
    }


    private IEnumerator RoundStarting()
    {
        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        yield return null;
    }


    private IEnumerator RoundEnding()
    {
        yield return m_EndWait;
    }


    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }

    /*
        private TankManager GetRoundWinner()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                if (m_Tanks[i].m_Instance.activeSelf)
                    return m_Tanks[i];
            }

            return null;
        }


        private TankManager GetGameWinner()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                if (m_Tanks[i].m_Wins == m_NumRoundsToWin)
                    return m_Tanks[i];
            }

            return null;
        }


        private string EndMessage()
        {
            string message = "DRAW!";

            if (m_RoundWinner != null)
                message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

            message += "\n\n\n\n";

            for (int i = 0; i < m_Tanks.Length; i++)
            {
                message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + " WINS\n";
            }

            if (m_GameWinner != null)
                message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

            return message;
        }
    */

    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].Reset();
        }
    }

    private void ResetAllNPCs()
    {
        for (int i = 0; i < 4; i++)
        {
            m_NPCs[i].Reset();
        }
    }


    private void EnableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].EnableControl();
        }
    }


    private void DisableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].DisableControl();
        }
    }
}