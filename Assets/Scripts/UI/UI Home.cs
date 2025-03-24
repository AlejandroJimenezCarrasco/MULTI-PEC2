using System.Collections;
using UnityEngine;

public class UIHome : MonoBehaviour
{
    public GameObject titleOptions;
    public GameObject playerOptions;
    public int indexColor;
    public GameObject[] tankColors;

    public IEnumerator ShowTitleOptions()
    {
        yield return new WaitForSeconds(1); // Espera 1 segundo
        titleOptions.SetActive(true); // Activa titleOptions
    }

    public IEnumerator ShowPlayerOptions()
    {
        yield return new WaitForSeconds(1); // Espera 1 segundo
        playerOptions.SetActive(true); // Activa playerOptions
    }

    // Métodos para iniciar las corutinas desde otro script o botón UI
    public void ActivateTitleOptions()
    {
        StartCoroutine(ShowTitleOptions());
    }

    public void ActivatePlayerOptions()
    {
        StartCoroutine(ShowPlayerOptions());
    }

    public void buttonColor()
    {
        ChangeTankColor();
    }
    public void ChangeTankColor()
    {
        // Avanza el índice y vuelve a 0 si supera el largo del array
        indexColor = (indexColor + 1) % tankColors.Length;

        // Activa sólo el tanque correspondiente, desactiva el resto
        for (int i = 0; i < tankColors.Length; i++)
        {
            tankColors[i].SetActive(i == indexColor);
        }
    }

    public void PreviousTankColor()
    {
        // Retrocede el índice y si es menor que 0 lo lleva al último elemento
        indexColor = (indexColor - 1 + tankColors.Length) % tankColors.Length;

        // Activa solo el tanque actual, desactiva el resto
        for (int i = 0; i < tankColors.Length; i++)
        {
            tankColors[i].SetActive(i == indexColor);
        }
    }
}


