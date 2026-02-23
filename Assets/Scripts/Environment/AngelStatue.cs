using UnityEngine;

public class AngelStatue : MonoBehaviour
{
    [SerializeField] private float requiredExposureSeconds = 3f;
    [SerializeField] private float tileSize = 1f;
    [SerializeField] private float killDistanceTiles = 1f;
    [SerializeField] private bool onlyMoveHorizontally = true;

    private float exposureSeconds;

    public void ResetExposure()
    {
        exposureSeconds = 0f;
    }

    public void AddExposure(float deltaTime)
    {
        exposureSeconds += Mathf.Max(0f, deltaTime);
    }

    public bool ResolveBlink(Vector3 playerPosition)
    {
        float killDistance = tileSize * killDistanceTiles;
        if (Vector2.Distance(transform.position, playerPosition) <= killDistance)
        {
            return true;
        }

        int steps = GetStepCount();
        if (steps > 0)
        {
            MoveTowardsPlayer(playerPosition, steps);
        }

        return Vector2.Distance(transform.position, playerPosition) <= killDistance;
    }

    private int GetStepCount()
    {
        if (exposureSeconds >= requiredExposureSeconds)
        {
            return 0;
        }

        if (exposureSeconds <= 0.01f)
        {
            return 2;
        }

        return 1;
    }

    private void MoveTowardsPlayer(Vector3 playerPosition, int steps)
    {
        Vector3 currentPosition = transform.position;
        Vector3 nextPosition = currentPosition;

        if (onlyMoveHorizontally)
        {
            float direction = Mathf.Sign(playerPosition.x - currentPosition.x);
            if (direction == 0f)
            {
                direction = 1f;
            }

            nextPosition.x += direction * tileSize * steps;
        }
        else
        {
            Vector2 direction = playerPosition - currentPosition;
            if (direction.sqrMagnitude > 0.0001f)
            {
                direction.Normalize();
                nextPosition += (Vector3)(direction * (tileSize * steps));
            }
        }

        transform.position = nextPosition;
    }
}