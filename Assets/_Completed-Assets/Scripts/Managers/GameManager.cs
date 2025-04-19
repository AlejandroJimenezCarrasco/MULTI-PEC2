using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Complete;
using Mirror;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public int numberPlayers;
    public float m_StartDelay = 3f;
    public float m_EndDelay = 3f;
    
    public NPCManager[] m_NPCs;
    public GameObject m_NPCsPrefab;


    private int m_RoundNumber;
    private WaitForSeconds m_StartWait;
    private WaitForSeconds m_EndWait;
    private TankManager m_RoundWinner;
    private TankManager m_GameWinner;
	private List<NetworkPlayer> players = new List<NetworkPlayer>();
    private bool gameEnded = false;

    public override void OnStartServer()
    {

        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SpawnAllNPCs();
        StartCoroutine(GameLoop());
    }

	[Server]
    public void RegisterTank(NetworkPlayer tank)
    {
        if (!players.Contains(tank))
        {
            players.Add(tank);
        }
    }

    [Server]
    public void UnregisterTank(NetworkPlayer tank)
    {
        if (players.Contains(tank))
        {
            players.Remove(tank);

            if (players.Count == 1 && !gameEnded)
            {
                gameEnded = true;
                
                NetworkPlayer winner = players[0];

                winner.TargetShowEndMessage(winner.connectionToClient, "You Win");
                
                foreach (var player in FindObjectsOfType<NetworkPlayer>())
                {
                    if (player != winner)
                    {
                        player.TargetShowEndMessage(player.connectionToClient, "You Lose");
                    }
                }
            }
        }
    }

    [Server]
    private void SpawnAllNPCs()
    {
        CameraControl camControl = FindObjectOfType<CameraControl>();
        
        for (int i = 0; i < 4; i++)
        {
            m_NPCs[i].m_Instance =
                Instantiate(m_NPCsPrefab, m_NPCs[i].m_SpawnPoint.position, m_NPCs[i].m_SpawnPoint.rotation) as GameObject;
            NetworkServer.Spawn(m_NPCs[i].m_Instance);
            m_NPCs[i].Setup();
            
            if (camControl != null)
            {
                camControl.AddTarget(m_NPCs[i].m_Instance.transform); 
            }

			
        }
    }

    

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

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

}