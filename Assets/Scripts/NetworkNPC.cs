using UnityEngine;
using Mirror;
using TMPro;
public class NetworkNPC : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnColorChanged))] public Color npcColor;
    [SyncVar(hook = nameof(OnNameChanged))] public string npcName;

    private MeshRenderer[] renderers;
    private TMP_Text nameText;

    private void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
        nameText = GetComponentInChildren<Canvas>().GetComponentInChildren<TMP_Text>();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        // Apply initial state
        OnColorChanged(Color.clear, npcColor);
        OnNameChanged("", npcName);
		
		Complete.CameraControl camControl = FindObjectOfType<Complete.CameraControl>();
        if (camControl != null)
        {
            camControl.AddTarget(transform);
        }
    }

    void OnColorChanged(Color _, Color newColor)
    {
        if (renderers == null) return;
        foreach (var rend in renderers)
        {
            rend.material.color = newColor;
        }
    }

    void OnNameChanged(string _, string newName)
    {
        if (nameText != null)
        {
            nameText.text = newName;
        }
    }
}
