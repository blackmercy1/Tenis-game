using DG.Tweening;
using Cinemachine;

public static class CinemachineVirtualCameraExtensions
{
    public static Tween DOShake(this CinemachineVirtualCamera camera, float force, float intencity, float duration)
    {
        var shake = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        shake.m_AmplitudeGain = 0;
        shake.m_FrequencyGain = intencity;

        return DOTween.Sequence()
            .Append(DOTween.To(() => shake.m_AmplitudeGain, (value) => shake.m_AmplitudeGain = value, force, duration / 2f))
            .Append(DOTween.To(() => shake.m_AmplitudeGain, (value) => shake.m_AmplitudeGain = value, 0, duration / 2f))
            .AppendCallback(() => shake.m_FrequencyGain = 0f);
    }
}