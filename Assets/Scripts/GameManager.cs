using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Monedas")]
    public int totalCoins = 5;

    private int collectedCoins = 0;

    [Header("UI")]
    public TMP_Text coinText;
    public GameObject levelCompleteText;
    public GameObject startMessageText;

    [Header("Camino luminoso")]
    public GameObject luminousPath;

    void Start()
    {
        UpdateCoinText();

        levelCompleteText.SetActive(false);

        startMessageText.SetActive(true);
        StartCoroutine(HideStartMessage());

        luminousPath.SetActive(false);
    }

    IEnumerator HideStartMessage()
    {
        yield return new WaitForSeconds(5f);

        startMessageText.SetActive(false);
    }

    public void CollectCoin()
    {
        if (collectedCoins >= totalCoins)
            return;

        collectedCoins++;

        UpdateCoinText();

        if (collectedCoins >= totalCoins)
        {
            CompleteLevel();
        }
    }

    void UpdateCoinText()
    {
        coinText.text = "Monedas: " +
                        collectedCoins + " / " + totalCoins;
    }

    void CompleteLevel()
{
    // Mostrar mensaje
    levelCompleteText.SetActive(true);

    // Ocultar mensaje después de 3 segundos
    StartCoroutine(HideLevelCompleteText());

    // Activar camino luminoso
    luminousPath.SetActive(true);

    Debug.Log("Todas las monedas recolectadas");
}
IEnumerator HideLevelCompleteText()
{
    yield return new WaitForSeconds(3f);

    levelCompleteText.SetActive(false);
}

    public bool AllCoinsCollected()
    {
        return collectedCoins >= totalCoins;
    }
}