using UnityEngine;
using UnityEngine.UI; // Necesario para interactuar con la UI
using Mirror;
using TMPro;

public class Atributos : MonoBehaviour
{
    public TMP_InputField nombreInputField;
    public GameManager gameManager;
    public int colorIndexSelected = 0;
    // Referencias a los componentes UI
  
     // Array para los botones de colores
        // Bot�n para aplicar los cambios

    // Referencia al material del tanque (puedes asignar el material o cambiar el color directamente)
    //public Renderer tanqueRenderer;    // Aseg�rate de asignar el Renderer del tanque

    // Colores predefinidos para los botones


    private void Start()
    {
   

       
       
    }

    // Cambiar el color del tanque basado en el bot�n presionado
    public void CambiarColor(int colorIndex)
    {
        colorIndexSelected = colorIndex;
    }

    // Aplicar los cambios (nombre y color) cuando el bot�n de aplicar sea presionado
    public void AplicarCambios()
    {
        // Verificar que el jugador est� conectado
        if (NetworkClient.isConnected && NetworkClient.localPlayer != null)
        {
            // Obtener el jugador local desde NetworkManager
            NetworkPlayer localPlayer = NetworkClient.localPlayer.GetComponent<NetworkPlayer>();

            if (localPlayer != null)
            {
                // Cambiar el nombre del tanque (si es necesario)

                string nuevoNombre = nombreInputField.text; // Obtener el nombre del InputField
                localPlayer.SetPlayerName(nuevoNombre); // Llamar al m�todo de NetworkPlayer para cambiar el nombre

                // Cambiar el color del tanque (sincronizado a trav�s de la red)
                localPlayer.SetTanqueMaterial(colorIndexSelected);
            }
        }
    }
}
