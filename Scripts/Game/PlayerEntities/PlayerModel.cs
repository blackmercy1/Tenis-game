using System;

using UnityEngine;

[Serializable]
public class PlayerModel
{
    public GameObject Model => _model;
    public Avatar Avatar => _avatar;

    [SerializeField] private GameObject _model;
    [SerializeField] private Avatar _avatar;
}