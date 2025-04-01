using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void Update()
    {
        // Calcula la direcci�n desde el objeto hasta la c�mara
        Vector3 direction = transform.position - Camera.main.transform.position;
        // Actualiza la rotaci�n del objeto para que mire hacia la c�mara
        transform.rotation = Quaternion.LookRotation(direction);
    }
}

