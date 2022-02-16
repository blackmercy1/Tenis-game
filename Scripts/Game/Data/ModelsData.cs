using UnityEngine;

[CreateAssetMenu(menuName = "Game/Models")]
public class ModelsData : ScriptableObject
{
    public PlayerModel[] Models => _models;
    [SerializeField] private PlayerModel[] _models;
}