using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerNumber : MonoBehaviour
{
    public static PlayerNumber Instance { get; private set; }
    public int playerNumber = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    


}