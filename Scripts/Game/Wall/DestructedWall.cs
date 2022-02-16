using UnityEngine;

public class DestructedWall : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WallFragment[] _wallFragments;

    public void ChangeColor(Color color)
    {
        //foreach (var wallFragment in _wallFragments)
        //    wallFragment.ChangeColor(color);
    }
    public void Explode(float force, float upwardsForceScale)
    {
        transform.DetachChildren();

        foreach (var wallFragment in _wallFragments)
            wallFragment.Explode(force, upwardsForceScale);

        Destroy(gameObject);
    }
}