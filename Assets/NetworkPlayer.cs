using Mirror;
using UnityEngine;
using TMPro;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnMaterialChanged))] private int materialIndex;

    [SyncVar(hook = nameof(OnNameChanged))] private string playerName;
    public Renderer[] tanqueRenderers;

    public Material[] materiales; // [0] = azul, [1] = rojo
    public TextMeshProUGUI nameText;

    public void SetTanqueMaterial(int nuevoMaterialIndex)
    {
        if (isLocalPlayer)
        {
            CmdSetTanqueMaterial(nuevoMaterialIndex);
        }
    }

    public void SetPlayerName(string nuevoNombre)
    {
        if (isLocalPlayer)
        {
            CmdSetPlayerName(nuevoNombre);
        }
    }

    [Command]
    private void CmdSetPlayerName(string nuevoNombre)
    {
        playerName = nuevoNombre;
    }

    [Command]
    private void CmdSetTanqueMaterial(int nuevoMaterialIndex)
    {
        materialIndex = nuevoMaterialIndex;
    }

    private void OnMaterialChanged(int oldIndex, int newIndex)
    {
        if (tanqueRenderers != null && materiales != null && materiales.Length > newIndex)
        {
            foreach (var renderer in tanqueRenderers)
            {
                renderer.material = materiales[newIndex];
            }
        }
    }

    private void OnNameChanged(string oldName, string newName)
    {
        if (nameText != null)
        {
            nameText.text = newName;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        Complete.CameraControl camControl = FindObjectOfType<Complete.CameraControl>();
        if (camControl != null)
        {
            camControl.AddTarget(this.transform);
        }

        // Si NO es el jugador local, forzar a rojo (índice 1)
        if (!isLocalPlayer)
        {
            // No tocamos el SyncVar para no sobrescribir materialIndex global
            if (tanqueRenderers != null && materiales.Length > 1)
            {
                foreach (var renderer in tanqueRenderers)
                {
                    renderer.material = materiales[4]; // Rojo
                }
            }
        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // Asignar nombre único
        SetPlayerName("Jugador " + netId);

        // Forzar color azul (índice 0)
        
    }
}





