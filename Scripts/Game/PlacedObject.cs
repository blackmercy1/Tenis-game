using UnityEngine;

public class PlacedObject
{
    public readonly Component Prefab;
    public readonly Vector3 Position;
    public readonly Quaternion Rotation;

    public PlacedObject(Component prefab, Vector3 position, Quaternion rotation)
    {
        Prefab = prefab;
        Position = position;
        Rotation = rotation;
    }

    public Component Place()
    {
        return Object.Instantiate(Prefab, Position, Rotation);
    }
}