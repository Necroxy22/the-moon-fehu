using UnityEngine;

public class BackgroundDecorMover : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float despawnX = -15f;
    [SerializeField] private bool scaleWithGameSpeed = false;

    public void Init(float moveSpeed, float destroyX, bool useSpeedScale = false)
    {
        speed = moveSpeed;
        despawnX = destroyX;
        scaleWithGameSpeed = useSpeedScale;
    }

    void Update()
    {
        float currentSpeed = speed;
        if (scaleWithGameSpeed && DifficultyManager.Instance != null && DifficultyManager.Instance.baseSpeed > 0f)
        {
            float ratio = DifficultyManager.Instance.CurrentSpeed / DifficultyManager.Instance.baseSpeed;
            currentSpeed *= ratio;
        }

        transform.Translate(Vector3.left * currentSpeed * Time.deltaTime, Space.World);

        if (transform.position.x <= despawnX)
        {
            Destroy(gameObject);
        }
    }
}
