using System.Collections.Generic;

using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance { get; private set; }

    [Header("References")]
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshCollider _meshCollider;
    [SerializeField] private LevelGeneratorData _data;

    private List<Platform> _generatedPlatforms = new List<Platform>();
    private List<PlacedObject> _generatedObjects = new List<PlacedObject>();

    private Vector3 _finalPosition;

    private int _countPlatforms;
    private int _spaceBetweenPlatforms;
    private int _minWallCountInPlatform;
    private int _maxWallCountInPlatform;
    private int _wallRatioDivPerWallLevel;
    private int _maxCountBonuses;

    private int _countBonuses;
    private int _generationLevel = -1;

    private bool IsGenerated
        => _generationLevel == Level.Value;
    private Platform LastPlatform =>
        _generatedPlatforms[_generatedPlatforms.Count - 1];

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Generate()
    {
        if (IsGenerated)
        {
            RestoreObjects();
            return;
        }

        var level = Level.Value;
        _generationLevel = level;
        _countBonuses = 0;

        _countPlatforms = Mathf.FloorToInt(_data.DefaultCountPlatforms + (level * _data.AddCountPlatformsPerLevel));
        _spaceBetweenPlatforms = Mathf.FloorToInt(_data.DefaultSpaceBetweenPlatforms + (level * _data.AddSpacePerLevel));
        _wallRatioDivPerWallLevel = Mathf.FloorToInt(_data.DefaultWallRatioDivPerWallLevel - (level * _data.SubWallRatioDivPerWallLevel));
        _minWallCountInPlatform = Mathf.FloorToInt(_data.DefaultMinWallCountInPlatform + (level * _data.AddMinWallCountInPlatform));
        _maxWallCountInPlatform = Mathf.FloorToInt(_data.DefaultMaxWallCountInPlatform + (level * _data.AddMaxCountBonusesPerLevel));
        _maxCountBonuses = Mathf.FloorToInt(_data.DefaultMaxCountBonuses + (level * _data.AddMaxCountBonusesPerLevel));

        PlacePlatform(_data.StartPlatform);
        PlacePlatforms();

        var finalPlatform = PlacePlatform(_data.FinalPlatform);
        _finalPosition = finalPlatform.Position;

        Bake();
    }

    private void RestoreObjects()
    {
        foreach (var generatedObject in _generatedObjects)
            generatedObject.Place();

        var finalPlatform = SpawnPlatform(_data.FinalPlatform, _finalPosition);
        finalPlatform.Destroy();

        _generatedPlatforms.Clear();
    }
    private void PlacePlatforms()
    {
        for (var i = 0; i < _countPlatforms; i++)
        {
            for (var j = 0; j < _spaceBetweenPlatforms; j++)
                PlaceSpacePlatform();

            PlaceRandomPlatform();
        }
    }
    
    private void PlaceRandomPlatform()
    {
        var randomIndex = Random.Range(0, _data.PlatformWithBlocksPrefabs.Length);
        var randomPlatformPrefab = _data.PlatformWithBlocksPrefabs[randomIndex];

        var platform = PlacePlatform(randomPlatformPrefab);
        platform.PlaceObjects(GenerateWalls());
    }
    private Wall[] GenerateWalls()
    {
        var walls = new List<Wall>();
        var index = 0;
        var countWall = Random.Range(_minWallCountInPlatform, _maxWallCountInPlatform + 1);

        for (var i = 0; i < _data.WallLevels.Length; i++)
        {
            for (var j = 0; j < countWall; j++, index++)
                walls.Insert(index, _data.WallLevels[i]);

            countWall = (int)(countWall / _wallRatioDivPerWallLevel);
        }

        walls.Reverse();
        return walls.ToArray();
    }

    private void PlaceSpacePlatform()
    {
        var platform = PlacePlatform(_data.PlatformSpace);
        platform.PlaceObjectsRandom(GenerateBonuses());
    }
    private Bonus[] GenerateBonuses()
    {
        var left = _maxCountBonuses - _countBonuses;
        if (left > 1) left = Mathf.CeilToInt((float)left / 2);

        var count = Random.Range(0, left + 1);
        var bonuses = new Bonus[count];

        _countBonuses += count;

        for (var i = 0; i < count; i++)
            bonuses[i] = GetRandomBonus();

        return bonuses;
    }
    private Bonus GetRandomBonus()
    {
        var randomIndex = Random.Range(0, _data.Bonuses.Length);
        return _data.Bonuses[randomIndex];
    }

    private Platform PlacePlatform(Platform platformPrefab)
    {
        var position = GetNextPlatformPosition(platformPrefab);
        var platform = SpawnPlatform(platformPrefab, position);

        _generatedPlatforms.Add(platform);

        return platform;
    }
    private Platform SpawnPlatform(Platform platformPrefab, Vector3 position)
    {
        return Instantiate(platformPrefab, position, platformPrefab.Rotation);
    }
    private Vector3 GetNextPlatformPosition(Platform nextPlatformPrefab)
    {
        var nextPlatformZPosition = 0f;

        if (_generatedPlatforms.Count != 0)
            nextPlatformZPosition = LastPlatform.EndPoint.z + (nextPlatformPrefab.Height / 2f);

        var yOffset = Vector3.up * nextPlatformPrefab.Position.y;
        var zOffset = Vector3.forward * nextPlatformZPosition;

        var position = yOffset + zOffset;
        return position;
    }

    private void Bake()
    {
        CombineObjects();
        CombineMeshes();
    }
    private void CombineObjects()
    {
        _generatedObjects.Clear();

        foreach (var platform in _generatedPlatforms)
            _generatedObjects.AddRange(platform.PlacedPrefabs);
    }
    private void CombineMeshes()
    {
        var combines = new CombineInstance[_generatedPlatforms.Count];

        for (var i = 0; i < _generatedPlatforms.Count; i++)
        {
            var platform = _generatedPlatforms[i];
            var combine = new CombineInstance();

            combine.mesh = platform.Mesh;
            combine.transform = platform.transform.localToWorldMatrix;
            combines[i] = combine;

            platform.Destroy();
        }
        _generatedPlatforms.Clear();

        var mesh = new Mesh();
        mesh.CombineMeshes(combines, true, true, true);
        mesh.OptimizeIndexBuffers();
        mesh.OptimizeReorderVertexBuffer();
        mesh.Optimize();
        mesh.RecalculateBounds();

        _meshFilter.mesh = mesh;
        _meshCollider.sharedMesh = mesh;
    }
}