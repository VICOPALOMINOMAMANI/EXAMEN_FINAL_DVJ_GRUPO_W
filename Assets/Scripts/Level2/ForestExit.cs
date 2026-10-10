
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForestExit : MonoBehaviour
{
    [Header("Referencias")]
    public Level2Manager level2Manager;

    [Header("Configuración")]
    public string nextScene = "Escenario3";

    [Header("Interfaz")]
    public GameObject lockedMessage;

    private void Start()
    {
        if (lockedMessage != null)
            lockedMessage.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (level2Manager == null)
        {
            Debug.LogWarning(
                "No se asignó Level2Manager."
            );
            return;
        }

        if (level2Manager.AllFragmentsCollected())
        {
            Debug.Log("Llave completa. Salida habilitada.");

            if (Application.CanStreamedLevelBeLoaded(nextScene))
            {
                SceneManager.LoadScene(nextScene);
            }
            else
            {
                Debug.Log(
                    "Victoria: completaste el bosque."
                );
            }
        }
        else
        {
            if (lockedMessage != null)
            {
                lockedMessage.SetActive(true);
            }

            Debug.Log(
                "Necesitas los 3 fragmentos de la llave."
            );
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") &&
            lockedMessage != null)
        {
            lockedMessage.SetActive(false);
        }
    }
}
