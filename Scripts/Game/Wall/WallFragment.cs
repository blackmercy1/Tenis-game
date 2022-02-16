using UnityEngine;

public class WallFragment : MonoBehaviour
{
    private const float DestroyTime = 5f;

    private Rigidbody _rigidbody;
    private bool _isHiding;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void ChangeColor(Color color)
    {
        var renderer = GetComponent<MeshRenderer>();
        foreach (var material in renderer.materials)
        {
            material.color = color;
            color -= new Color(0.1f, 0.1f, 0.1f, 0f);
        }
    }
    public void Explode(float force, float upwardsForceScale)
    {
        _rigidbody.AddExplosionForce(force, transform.position, force, upwardsForceScale, ForceMode.Impulse);
        Destroy(gameObject, DestroyTime);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (_isHiding) return;

        if (_rigidbody.velocity.sqrMagnitude < 1f)
            StartHide();
    }
    private void StartHide()
    {
        _isHiding = true;
        _rigidbody.isKinematic = true;
    }

    private void Update()
    {
        if (_isHiding == false) return;

        transform.position += Vector3.down * 3f * Time.deltaTime;
    }
}