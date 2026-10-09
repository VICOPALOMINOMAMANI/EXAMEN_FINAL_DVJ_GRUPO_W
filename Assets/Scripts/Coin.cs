using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Rotacion")]
    public float rotationSpeed = 100f;

    [Header("Particulas")]
    public ParticleSystem collectParticles;

    [Header("Sonido")]
    public AudioClip collectSound;

    [Range(0f, 1f)]
    public float soundVolume = 1f;

    private bool collected = false;

    void Update()
    {
        transform.Rotate(
            0f,
            rotationSpeed * Time.deltaTime,
            0f,
            Space.World
        );
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !collected)
        {
            collected = true;

            // Actualizar contador
            GameManager gameManager =
                FindFirstObjectByType<GameManager>();

            if (gameManager != null)
            {
                gameManager.CollectCoin();
            }

            // Reproducir sonido
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(
                    collectSound,
                    transform.position,
                    soundVolume
                );
            }

            // Reproducir particulas
            if (collectParticles != null)
            {
                collectParticles.Play();
            }

            // Ocultar moneda
            MeshRenderer meshRenderer =
                GetComponent<MeshRenderer>();

            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }

            // Desactivar collider
            Collider coinCollider =
                GetComponent<Collider>();

            if (coinCollider != null)
            {
                coinCollider.enabled = false;
            }

            // Destruir moneda
            Destroy(gameObject, 1.5f);
        }
    }
}