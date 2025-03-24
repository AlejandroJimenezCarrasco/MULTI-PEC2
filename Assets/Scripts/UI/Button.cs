using UnityEngine;
using UnityEngine.EventSystems; // Necesario para IPointerEnterHandler

public class Button : MonoBehaviour, IPointerEnterHandler
{
    public enum Players { Two = 2, Three = 3, Four = 4 } // Enumeración para los jugadores
    public Players currentPlayers; // Variable para definir el número de jugadores

    public GameObject tankPlayer3;
    public GameObject tankPlayer4;

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Dependiendo del número de jugadores, activamos o desactivamos los tanques
        switch (currentPlayers)
        {
            case Players.Two:
                tankPlayer3.SetActive(false);
                tankPlayer4.SetActive(false);
                break;

            case Players.Three:
                tankPlayer3.SetActive(true);
                tankPlayer4.SetActive(false);
                break;

            case Players.Four:
                tankPlayer3.SetActive(true);
                tankPlayer4.SetActive(true);
                break;
        }
    }
}