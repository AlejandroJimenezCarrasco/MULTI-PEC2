using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void Update()
    {
        // Calcula la dirección desde el objeto hasta la cámara
        Vector3 direction = transform.position - Camera.main.transform.position;
        // Actualiza la rotación del objeto para que mire hacia la cámara
        transform.rotation = Quaternion.LookRotation(direction);
    }
}

