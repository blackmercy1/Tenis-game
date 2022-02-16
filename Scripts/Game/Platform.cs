using System.Collections.Generic;

using UnityEngine;

public class Platform : MonoBehaviour
{
    public Mesh Mesh => _meshFilter.sharedMesh;

    public Vector3 Position => transform.position;
    public Quaternion Rotation => transform.rotation;

    public Vector3 StartPoint => _startPoint.position;
    public Vector3 EndPoint => _endPoint.position;

    public float Width => EndPoint.x - StartPoint.x;
    public float Height => EndPoint.z - StartPoint.z;

    public List<PlacedObject> PlacedPrefabs { get; } = new List<PlacedObject>();

    [Header("References")]
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private Transform[] _wallPlaces;
    [SerializeField] private Mesh _wallMesh;

    public void PlaceObjects(Component[] objects)
    {
        for (var i = 0; i < _wallPlaces.Length && i < objects.Length; i++)
            PlaceObject(i, objects[i]);
    }
    public void PlaceObjectsRandom(Component[] objects)
    {
        var places = new List<Transform>(_wallPlaces);
        for (var i = 0; i < objects.Length && places.Count > 0; i++)
        {
            var randomIndex = Random.Range(0, places.Count);
            var place = places[randomIndex];

            PlaceObject(place, objects[i]);
            places.RemoveAt(randomIndex);
        }
    }
    
    public void PlaceObject(int placeIndex, Component @object)
    {
        var place = _wallPlaces[placeIndex];
        PlaceObject(place, @object);
    }
    public void PlaceObject(Transform place, Component @object)
    {
        var placedPrefab = new PlacedObject(@object, place.position, place.rotation);
        placedPrefab.Place();

        PlacedPrefabs.Add(placedPrefab);
    }

    public void Destroy()
    {
        Destroy(_startPoint.gameObject);
        Destroy(_endPoint.gameObject);

        foreach (var wallPlace in _wallPlaces)
            Destroy(wallPlace.gameObject);

        transform.DetachChildren();
        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_wallPlaces == null) return;

        Gizmos.color = Color.blue;

        for (var i = 0; i < _wallPlaces.Length; i++)
        {
            var wallPlace = _wallPlaces[i];
            Gizmos.DrawMesh(_wallMesh, wallPlace.position, wallPlace.rotation);
        }
    }
#endif
}