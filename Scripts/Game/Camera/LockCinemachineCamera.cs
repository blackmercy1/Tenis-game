using UnityEngine;

using Cinemachine;
 
/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Z co-ordinate
/// </summary>
[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")]
public class LockCinemachineCamera : CinemachineExtension
{
    [Header("Parameters")]
    [SerializeField] private Vector3 _lockingPosition;
    [SerializeField] private bool _lockX;
    [SerializeField] private bool _lockY;
    [SerializeField] private bool _lockZ;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (enabled && stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;

            if (_lockX)
                pos.x = _lockingPosition.x;
            if (_lockY)
                pos.y = _lockingPosition.y;
            if (_lockZ)
                pos.z = _lockingPosition.z;

            state.RawPosition = pos;
        }
    }
}