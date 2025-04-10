using UnityEngine;
using UnityEngine.UI; // Necesario para interactuar con la UI
using Mirror;

public class Atributos : MonoBehaviour
{
    public GameManager gameManager;
    public int colorIndexSelected = 3;
    // Referencias a los componentes UI
    public InputField nombreInputField; // Input Field para el nombre del tanque
    public Button[] botonesColores;    // Array para los botones de colores
    public Button botonAplicar;        // Botón para aplicar los cambios

    // Referencia al material del tanque (puedes asignar el material o cambiar el color directamente)
    //public Renderer tanqueRenderer;    // Asegúrate de asignar el Renderer del tanque

    // Colores predefinidos para los botones
    private Color[] colores = new Color[6];

    private void Start()
    {
        // Asignar los colores de los botones
        colores[0] = Color.red;
        colores[1] = Color.blue;
        colores[2] = Color.green;
        colores[3] = Color.yellow;
        colores[4] = Color.cyan;
        colores[5] = Color.magenta;

       

       
       
    }

    // Cambiar el color del tanque basado en el botón presionado
    private void CambiarColor(int colorIndex)
    {
        colorIndexSelected = colorIndex;
    }

    // Aplicar los cambios (nombre y color) cuando el botón de aplicar sea presionado
    public void AplicarCambios()
    {
        // Verificar que el jugador está conectado
        if (NetworkClient.isConnected && NetworkClient.localPlayer != null)
        {
            // Obtener el jugador local desde NetworkManager
            NetworkPlayer localPlayer = NetworkClient.localPlayer.GetComponent<NetworkPlayer>();

            if (localPlayer != null)
            {
                // Cambiar el nombre del tanque (si es necesario)
                
                

                // Cambiar el color del tanque (sincronizado a través de la red)
                localPlayer.SetTanqueMaterial(colorIndexSelected);
            }
        }
    }
}
