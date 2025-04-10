using Mirror;
using UnityEngine;
using TMPro;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnMaterialChanged))] private int materialIndex; // Índice del material sincronizado
    
    [SyncVar(hook = nameof(OnNameChanged))] private string playerName; // Nombre del jugador sincronizado
    public Renderer[] tanqueRenderers; // Array de Renderers de las diferentes partes del tanque

    public Material[] materiales; // Array de materiales
    public TextMeshProUGUI nameText;


    // Método para cambiar el material de todas las partes del tanque
    public void SetTanqueMaterial(int nuevoMaterialIndex)
    {
        if (isLocalPlayer)
        {
            CmdSetTanqueMaterial(nuevoMaterialIndex);  // Llamar al comando del servidor para cambiar el material
        }
    }

    public void SetPlayerName(string nuevoNombre)
    {
        if (isLocalPlayer)
        {
            CmdSetPlayerName(nuevoNombre);  // Llamar al comando del servidor para cambiar el nombre
        }
    }

    [Command]
    private void CmdSetPlayerName(string nuevoNombre)
    {
        playerName = nuevoNombre; // Cambiar el nombre del jugador en el servidor
    }

    // Comando ejecutado en el servidor para cambiar el material
    [Command]
    private void CmdSetTanqueMaterial(int nuevoMaterialIndex)
    {
        materialIndex = nuevoMaterialIndex; // Cambiar el índice del material en el servidor
    }

    // Hook para cuando el material cambie
    private void OnMaterialChanged(int oldIndex, int newIndex)
    {
        // Asegurarnos de que el array de materiales y los Renderers sean válidos
        if (tanqueRenderers != null && materiales != null && materiales.Length > newIndex)
        {
            // Aplicar el material en cada parte del tanque
            foreach (var renderer in tanqueRenderers)
            {
                renderer.material = materiales[newIndex]; // Cambiar el material de cada Renderer
            }
        }
    }

    // Hook para cuando el nombre cambie
    private void OnNameChanged(string oldName, string newName)
    {
        // Actualizar el nombre en la UI de todos los clientes
        if (nameText != null)
        {
            nameText.text = newName;
        }
    }

   
}




