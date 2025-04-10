using Mirror;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnMaterialChanged))] private int materialIndex; // Índice del material sincronizado

    private Renderer[] tanqueRenderers; // Array de Renderers de las diferentes partes del tanque

    public Material[] materiales; // Array de materiales

    private void Start()
    {
        // Obtener todos los Renderers de las partes del tanque (chasis, cañón, ruedas, etc.)
        tanqueRenderers = GetComponentsInChildren<Renderer>();
    }

    // Método para cambiar el material de todas las partes del tanque
    public void SetTanqueMaterial(int nuevoMaterialIndex)
    {
        if (isLocalPlayer)
        {
            CmdSetTanqueMaterial(nuevoMaterialIndex);  // Llamar al comando del servidor para cambiar el material
        }
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

    // Método para obtener el índice del material del tanque (opcional si lo necesitas)
    public int GetTanqueMaterialIndex() => materialIndex;
}


