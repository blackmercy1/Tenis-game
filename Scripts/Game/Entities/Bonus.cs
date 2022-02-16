using UnityEngine;

public abstract class Bonus : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private float _rotationSpeed = 50f;
    [SerializeField] private float _moveSpeed = 2f;

    private float _defaultY;
    private float _defaultYOffsetRadians;

    private void Start()
    {
        _defaultY = transform.position.y;
        var offset = Random.Range(-1f, 1f);
        _defaultYOffsetRadians = Mathf.Asin(offset);

        Move(offset);
        Rotate(Random.rotation.eulerAngles.y);
    }
    private void Update()
    {
        var yOffset = Mathf.Sin(_defaultYOffsetRadians + (Time.time * _moveSpeed));
        Move(yOffset);

        var angle = transform.rotation.eulerAngles.y + (_rotationSpeed * Time.deltaTime);
        Rotate(angle);
    }

    private void Move(float yOffset)
    {
        transform.position = new Vector3(transform.position.x, _defaultY + yOffset, transform.position.z);
    }
    private void Rotate(float angle)
    {
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}