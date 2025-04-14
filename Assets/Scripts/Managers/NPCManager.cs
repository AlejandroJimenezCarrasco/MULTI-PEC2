using System;
using TMPro;
using UnityEngine;



    [Serializable]
    public class NPCManager
    {
        // This class is to manage various settings on a tank.
        // It works with the GameManager class to control how the tanks behave
        // and whether or not players have control of their tank in the 
        // different phases of the game.

        public Color m_PlayerColor;                             // This is the color this tank will be tinted.
        public Transform m_SpawnPoint;                          // The position and direction the tank will have when it spawns.
       
        [HideInInspector] public GameObject m_Instance;         // A reference to the instance of the tank when it is created.
        [HideInInspector] public string m_ColoredPlayerText;
        

        
        private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.


        public void Setup ()
        {
            m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas> ().gameObject;


            // Create a string using the correct color that says 'PLAYER 1' etc based on the tank's color and the player's number.
            //m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";
            m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">NPC 1" +  "</color>";
            m_CanvasGameObject.GetComponentInChildren<TMP_Text>().text = m_ColoredPlayerText;
            // Get all of the renderers of the tank.
            MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer> ();

            // Go through all the renderers...
            for (int i = 0; i < renderers.Length; i++)
            {
                // ... set their material color to the color specific to this tank.
                renderers[i].material.color = m_PlayerColor;
            }
			
			var sync = m_Instance.GetComponent<NetworkNPC>();
			if (sync != null)
    		{
        		sync.npcColor = m_PlayerColor;
        		sync.npcName = m_ColoredPlayerText;
    		}
            
            
        }


        


        // Used at the start of each round to put the tank into it's default state.
        public void Reset ()
        {
            m_Instance.transform.position = m_SpawnPoint.position;
            m_Instance.transform.rotation = m_SpawnPoint.rotation;

            m_Instance.SetActive (false);
            m_Instance.SetActive (true);
        }
    }

