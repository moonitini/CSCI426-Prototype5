using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlinkFlashlightGameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Transform flashlightPivot;
    [SerializeField] private GameObject flashlightCone;
    [SerializeField] private AngelStatue[] statues;

    [Header("Flashlight Cycle")]
    [SerializeField] private float flashlightOnDuration = 10f;
    [SerializeField] private float blackoutDuration = 0.2f;

    [Header("Cone Visibility")]
    [SerializeField] private float coneRange = 8f;
    [SerializeField, Range(5f, 179f)] private float coneAngle = 45f;
    [SerializeField] private bool useLineOfSight = false;
    [SerializeField] private LayerMask obstacleMask;

    [Header("UI")]
    [SerializeField] private CanvasGroup blackoutOverlay;
    [SerializeField] private TMP_Text countdownText;

    private float cycleTimer;
    private bool isBlinking;
    private bool isGameOver;

    private void Awake()
    {
        if ((statues == null || statues.Length == 0))
        {
            statues = FindObjectsOfType<AngelStatue>();
        }
    }

    private void Start()
    {
        if (blackoutOverlay != null)
        {
            blackoutOverlay.alpha = 0f;
        }

        StartNewCycle();
    }

    private void Update()
    {
        if (isGameOver)
        {
            return;
        }

        UpdateFlashlightAim();

        if (isBlinking)
        {
            return;
        }

        float deltaTime = Time.deltaTime;
        cycleTimer -= deltaTime;

        for (int index = 0; index < statues.Length; index++)
        {
            AngelStatue statue = statues[index];
            if (statue == null)
            {
                continue;
            }

            if (IsVisibleInCone(statue.transform))
            {
                statue.AddExposure(deltaTime);
            }
        }

        UpdateCountdownText();

        if (cycleTimer <= 0f)
        {
            StartCoroutine(DoBlinkCheck());
        }
    }

    private void StartNewCycle()
    {
        cycleTimer = flashlightOnDuration;
        SetFlashlightActive(true);

        for (int index = 0; index < statues.Length; index++)
        {
            AngelStatue statue = statues[index];
            if (statue != null)
            {
                statue.ResetExposure();
            }
        }

        UpdateCountdownText();
    }

    private IEnumerator DoBlinkCheck()
    {
        isBlinking = true;
        SetFlashlightActive(false);

        if (blackoutOverlay != null)
        {
            blackoutOverlay.alpha = 1f;
        }

        bool playerKilled = false;
        for (int index = 0; index < statues.Length; index++)
        {
            AngelStatue statue = statues[index];
            if (statue == null)
            {
                continue;
            }

            if (statue.ResolveBlink(player.position))
            {
                playerKilled = true;
            }
        }

        if (playerKilled)
        {
            HandlePlayerDeath();
            yield break;
        }

        yield return new WaitForSeconds(blackoutDuration);

        if (blackoutOverlay != null)
        {
            blackoutOverlay.alpha = 0f;
        }

        StartNewCycle();
        isBlinking = false;
    }

    private void HandlePlayerDeath()
    {
        isGameOver = true;
        SetFlashlightActive(false);

        if (blackoutOverlay != null)
        {
            blackoutOverlay.alpha = 1f;
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (countdownText != null)
        {
            countdownText.text = "DEAD";
        }
    }

    private void SetFlashlightActive(bool active)
    {
        if (flashlightCone != null)
        {
            flashlightCone.SetActive(active);
        }
    }

    private void UpdateCountdownText()
    {
        if (countdownText != null)
        {
            countdownText.text = Mathf.CeilToInt(Mathf.Max(0f, cycleTimer)).ToString();
        }
    }

    private void UpdateFlashlightAim()
    {
        if (flashlightPivot == null)
        {
            return;
        }

        Camera mainCamera = Camera.main;
        Mouse mouse = Mouse.current;
        if (mainCamera == null || mouse == null)
        {
            return;
        }

        Vector3 pointerWorldPosition = mainCamera.ScreenToWorldPoint(mouse.position.ReadValue());
        pointerWorldPosition.z = flashlightPivot.position.z;

        Vector2 direction = pointerWorldPosition - flashlightPivot.position;
        if (direction.sqrMagnitude <= 0.0001f)
        {
            return;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        flashlightPivot.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private bool IsVisibleInCone(Transform target)
    {
        if (flashlightPivot == null || target == null)
        {
            return false;
        }

        Vector2 origin = flashlightPivot.position;
        Vector2 toTarget = (Vector2)target.position - origin;
        float distance = toTarget.magnitude;
        if (distance > coneRange || distance <= 0.001f)
        {
            return false;
        }

        Vector2 forward = flashlightPivot.right;
        float angle = Vector2.Angle(forward, toTarget.normalized);
        if (angle > coneAngle * 0.5f)
        {
            return false;
        }

        if (useLineOfSight)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, toTarget.normalized, distance, obstacleMask);
            if (hit.collider != null)
            {
                return false;
            }
        }

        return true;
    }
}