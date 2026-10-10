
using UnityEngine;

public class KeyFragment : MonoBehaviour
{
    [Header("Rotación")]
    public Transform rotatingVisual;
    public float rotationSpeed = 100f;

    [Header("Efectos de recolección")]
    public ParticleSystem collectParticles;
    public AudioClip collectSound;

    [Range(0f, 1f)]
    public float soundVolume = 1f;

    [Header("Configuración")]
    public float particleDestroyDelay = 3f;

    private bool collected = false;
    private Level2Manager levelManager;

    void Start()
    {
        levelManager = FindFirstObjectByType<Level2Manager>();

        if (rotatingVisual == null)
        {
            Debug.LogWarning(
                "No se asignó KeyVisual en " + gameObject.name
            );
        }

        // Las partículas de recolección
        // deben permanecer apagadas inicialmente.
        if (collectParticles != null)
        {
            collectParticles.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );
        }
    }

   void Update()
{
    if (collected || rotatingVisual == null)
        return;

    Renderer[] renderers =
        rotatingVisual.GetComponentsInChildren<Renderer>();

    if (renderers.Length == 0)
        return;

    // Calcular el centro visual de la llave
    Bounds bounds = renderers[0].bounds;

    for (int i = 1; i < renderers.Length; i++)
    {
        bounds.Encapsulate(renderers[i].bounds);
    }

    Vector3 center = bounds.center;

    // Girar alrededor del centro visual
    rotatingVisual.RotateAround(
        center,
        Vector3.up,
        rotationSpeed * Time.deltaTime
    );
}

    void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        if (!other.CompareTag("Player"))
            return;

        CollectFragment();
    }

    void CollectFragment()
    {
        collected = true;

        // 1. Actualizar el contador.
        if (levelManager != null)
        {
            levelManager.CollectFragment();
        }
        else
        {
            Debug.LogWarning(
                "No se encontró Level2Manager."
            );
        }

        // 2. Reproducir sonido.
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(
                collectSound,
                transform.position,
                soundVolume
            );
        }

        // 3. Reproducir partículas de recolección.
        if (collectParticles != null)
        {
            collectParticles.transform.SetParent(
                null,
                true
            );

            collectParticles.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );

            collectParticles.Play();

            Destroy(
                collectParticles.gameObject,
                particleDestroyDelay
            );
        }

        // 4. Ocultar el fragmento y todos sus hijos:
        // modelo, luz, halo y partículas decorativas.
        gameObject.SetActive(false);
    }
}
