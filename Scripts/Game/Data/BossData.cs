using UnityEngine;

[CreateAssetMenu(menuName = "Game/Boss")]
public class BossData : ScriptableObject
{
    public int DefaultEnergy => _defaultEnergy;
    public int AddEnergyPerLevel => _addEnergyPerLevel;

    [Header("Parameters")]
    [Tooltip("Количество энергии для первого уровня")]
    [SerializeField] private int _defaultEnergy;
    [Tooltip("Добавление количества энергии за каждый уровень")]
    [SerializeField] private int _addEnergyPerLevel;
}