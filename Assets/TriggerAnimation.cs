using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    public GameObject menu;

    public void Trigger()
    {
        menu.SetActive(true);
    }
}
