using UnityEngine;

[CreateAssetMenu(menuName = "Game/LevelGenerator")]
public class LevelGeneratorData : ScriptableObject
{
    public Platform StartPlatform => _startPlatformPrefab;
    public Platform PlatformSpace => _platformSpacePrefab;
    public Platform FinalPlatform => _finalPlatformPrefab;
    public Platform[] PlatformWithBlocksPrefabs => _platformWithBlocksPrefabs;

    public float DefaultSpaceBetweenPlatforms => _defaultSpaceBetweenPlatforms;
    public float AddSpacePerLevel => _addSpacePerLevel;
    public float DefaultCountPlatforms => _defaultCountPlatforms;
    public float AddCountPlatformsPerLevel => _addCountPlatformsPerLevel;

    public Wall[] WallLevels => _wallLevels;
    public float DefaultMinWallCountInPlatform => _defaultMinWallCountInPlatform;
    public float AddMinWallCountInPlatform => _addMinWallCountInPlatform;
    public float DefaultMaxWallCountInPlatform => _defaultMaxWallCountInPlatform;
    public float AddMaxWallCountInPlatfrom => _addMaxWallCountInPlatform;
    public float DefaultWallRatioDivPerWallLevel => _defaultWallRatioDivPerWallLevel;
    public float SubWallRatioDivPerWallLevel => _subWallRatioDivPerWallLevel;

    public Bonus[] Bonuses => _bonuses;
    public float DefaultMaxCountBonuses => _defaultMaxCountBonuses;
    public float AddMaxCountBonusesPerLevel => _addMaxCountBonuses;

    [Header("Platforms")]
    [Tooltip("Перфаб стартовой платформы")]
    [SerializeField] private Platform _startPlatformPrefab;
    [Tooltip("Перфаб промежуточной/пустой платформы с ячейками для генерации бонусов")]
    [SerializeField] private Platform _platformSpacePrefab;
    [Tooltip("Перфаб финальной платформы")]
    [SerializeField] private Platform _finalPlatformPrefab;
    [Tooltip("Перфабы платформ с ячейками для генерации стен")]
    [SerializeField] private Platform[] _platformWithBlocksPrefabs;
    [Header("Parameters")]
    [Tooltip("Количество пустых платформ между платформами со стенками для первого уровня (можно указать дробное число, которое потом в итоге будет округляться)")]
    [SerializeField] private float _defaultSpaceBetweenPlatforms;
    [Tooltip("Добавление количества пустых платформ за каждый уровень (можно указать дробное число, которое потом в итоге будет округляться)")]
    [SerializeField] private float _addSpacePerLevel;
    [Tooltip("Количество платформ со стенками для первого уровня (можно указать дробное число, которое потом в итоге будет округляться)")]
    [SerializeField] private float _defaultCountPlatforms;
    [Tooltip("Добавление количества платформ с стенками за каждый уровень (можно указать дробное число, которое потом в итоге будет округляться)")]
    [SerializeField] private float _addCountPlatformsPerLevel;
    [Space]
    [Header("Walls")]
    [Tooltip("Стены различаемые по уровням (слабые, средние, крепкие и тд), порядок должен быть от низкого уровня к высокому")]
    [SerializeField] private Wall[] _wallLevels;
    [Header("Parameters")]
    [Tooltip("Минимальное количество стен для первого уровня (можно указать дробное число)")]
    [SerializeField] private float _defaultMinWallCountInPlatform;
    [Tooltip("Добавление минимальног количества стен за уровень (можно указать дробное число)")]
    [SerializeField] private float _addMinWallCountInPlatform;
    [Tooltip("Максимальное количество стен для первого уровня (можно указать дробное число)")]
    [SerializeField] private float _defaultMaxWallCountInPlatform;
    [Tooltip("Добавление максимального количества стен за уровень (можно указать дробное число)")]
    [SerializeField] private float _addMaxWallCountInPlatform;
    [Tooltip("Коэффициент уменьшения количества стен определённого уровня, чем меньше, тем больше стен высокого уровня и меньше стен низкого уровня (можно указать дробное число)")]
    [SerializeField] private float _defaultWallRatioDivPerWallLevel;
    [Tooltip("Величина уменьшения коэффициента лёгкости стен за каждый уровень (можно указать дробное число)")]
    [SerializeField] private float _subWallRatioDivPerWallLevel;
    [Space]
    [Header("Bonuses")]
    [Tooltip("Бонусы")]
    [SerializeField] private Bonus[] _bonuses;
    [Header("Parameters")]
    [Tooltip("Максимальное количество бонусов для первого уровня (можно указать дробное число)")]
    [SerializeField] private float _defaultMaxCountBonuses;
    [Tooltip("Добавление количества бонусов за каждый уровень (можно указать дробное число)")]
    [SerializeField] private float _addMaxCountBonuses;
}