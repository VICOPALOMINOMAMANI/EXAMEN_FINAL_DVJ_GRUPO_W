
using System.Collections;
using UnityEngine;

public class EnemyContact : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;
    public Transform respawnPoint;

    [Header("Configuración")]
    public float hitDistance = 1.5f;
    public float hitCooldown = 2f;

    [Header("Interfaz")]
    public GameObject enemyWarningText;
    public GameObject damageFlash;

    [Header("Sonido")]
    public AudioClip hitSound;

    private float nextHitTime;
    private bool isRespawning = false;

    void Start()
    {
        if (enemyWarningText != null)
            enemyWarningText.SetActive(false);

        if (damageFlash != null)
            damageFlash.SetActive(false);
    }

    void Update()
    {
        if (player == null || respawnPoint == null)
            return;

        if (isRespawning || Time.time < nextHitTime)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distance <= hitDistance)
            StartCoroutine(RespawnPlayer());
    }

    IEnumerator RespawnPlayer()
    {
        isRespawning = true;
        nextHitTime = Time.time + hitCooldown;

        if (enemyWarningText != null)
            enemyWarningText.SetActive(true);

        if (damageFlash != null)
            damageFlash.SetActive(true);

        if (hitSound != null)
            AudioSource.PlayClipAtPoint(
                hitSound,
                player.position
            );

        yield return new WaitForSeconds(0.5f);

        CharacterController controller =
            player.GetComponent<CharacterController>();

        if (controller != null)
            controller.enabled = false;

        player.position = respawnPoint.position;
        player.rotation = respawnPoint.rotation;

        if (controller != null)
            controller.enabled = true;

        yield return new WaitForSeconds(1.5f);

        if (enemyWarningText != null)
            enemyWarningText.SetActive(false);

        if (damageFlash != null)
            damageFlash.SetActive(false);

        isRespawning = false;
    }
}
