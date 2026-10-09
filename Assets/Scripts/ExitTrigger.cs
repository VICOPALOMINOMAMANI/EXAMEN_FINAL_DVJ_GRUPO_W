using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    [Header("Siguiente escenario")]
    public string nextSceneName = "Escenario2";

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (gameManager != null && gameManager.AllCoinsCollected())
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.Log("Primero debes recoger las 15 monedas");
        }
    }
}