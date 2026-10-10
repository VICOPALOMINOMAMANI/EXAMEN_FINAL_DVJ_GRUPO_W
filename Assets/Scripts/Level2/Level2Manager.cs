
using UnityEngine;
using TMPro;
using System.Collections;

public class Level2Manager : MonoBehaviour
{
    [Header("Fragmentos")]
    public int totalFragments = 3;

    private int collectedFragments = 0;

    [Header("Interfaz")]
    public TMP_Text fragmentText;
    public GameObject startMessageText;
    public GameObject levelCompleteText;

    [Header("Iluminación de la salida")]
    public GameObject exitLightBeam;
    public GameObject exitLightBeamCore;

    void Start()
    {
        UpdateFragmentText();

        // Mostrar mensaje inicial
        if (startMessageText != null)
            startMessageText.SetActive(true);

        // Ocultar mensaje de llave completada
        if (levelCompleteText != null)
            levelCompleteText.SetActive(false);

        // Mantener apagada la iluminación de salida
        if (exitLightBeam != null)
            exitLightBeam.SetActive(false);

        if (exitLightBeamCore != null)
            exitLightBeamCore.SetActive(false);

        StartCoroutine(HideStartMessage());
    }

    IEnumerator HideStartMessage()
    {
        yield return new WaitForSeconds(5f);

        if (startMessageText != null)
            startMessageText.SetActive(false);
    }

    public void CollectFragment()
    {
        if (collectedFragments >= totalFragments)
            return;

        // Aumentar contador
        collectedFragments++;

        // Actualizar interfaz
        UpdateFragmentText();

        // Verificar si completó la llave
        if (collectedFragments >= totalFragments)
        {
            CompleteKey();
        }
    }

    void UpdateFragmentText()
    {
        if (fragmentText != null)
        {
            fragmentText.text =
                "Fragmentos: " +
                collectedFragments +
                " / " +
                totalFragments;
        }
    }

    void CompleteKey()
    {
        // Mostrar mensaje de llave completada
        if (levelCompleteText != null)
            levelCompleteText.SetActive(true);

        // Encender la columna de luz de salida
        if (exitLightBeam != null)
            exitLightBeam.SetActive(true);

        if (exitLightBeamCore != null)
            exitLightBeamCore.SetActive(true);

        // Ocultar mensaje después de 3 segundos
        StartCoroutine(HideCompleteMessage());

        Debug.Log(
            "¡Llave completada! La salida del bosque está iluminada."
        );
    }

    IEnumerator HideCompleteMessage()
    {
        yield return new WaitForSeconds(3f);

        if (levelCompleteText != null)
            levelCompleteText.SetActive(false);
    }

    public bool AllFragmentsCollected()
    {
        return collectedFragments >= totalFragments;
    }
}
