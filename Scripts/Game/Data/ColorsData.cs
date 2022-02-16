using UnityEngine;

[CreateAssetMenu(menuName = "Game/Colors")]
public class ColorsData : ScriptableObject
{
    public Color[] Colors => _colors;
    [SerializeField] private Color[] _colors;
}