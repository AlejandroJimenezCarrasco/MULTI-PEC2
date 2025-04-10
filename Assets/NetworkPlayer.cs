using Mirror;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnMaterialChanged))] private int materialIndex; // �ndice del material sincronizado

    private Renderer tanqueRenderer; // Referencia al Renderer del tanque

    public Material[] materiales; // Array de materiales

    private void Start()
    {
        // Obtener el Renderer del tanque
        tanqueRenderer = GetComponentInChildren<Renderer>();
    }

    // M�todo para cambiar el material del tanque
    public void SetTanqueMaterial(int nuevoMaterialIndex)
    {
        if (isLocalPlayer)
        {
            CmdSetTanqueMaterial(nuevoMaterialIndex);  // Llamar al comando del servidor
        }
    }

    // Comando ejecutado en el servidor para cambiar el material
    [Command]
    private void CmdSetTanqueMaterial(int nuevoMaterialIndex)
    {
        materialIndex = nuevoMaterialIndex; // Cambiar el �ndice del material en el servidor
    }

    // Hook para cuando el material cambie
    private void OnMaterialChanged(int oldIndex, int newIndex)
    {
        if (tanqueRenderer != null && materiales != null && materiales.Length > newIndex)
        {
            tanqueRenderer.material = materiales[newIndex]; // Cambiar el material del tanque
        }
    }

    // M�todo para obtener el �ndice del material del tanque (opcional si lo necesitas)
    public int GetTanqueMaterialIndex() => materialIndex;
}
