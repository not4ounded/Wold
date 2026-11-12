using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "HealthEventChannelSO", menuName = "Scriptable Objects/HealthEventChannelSO")]
public class HealthEventChannelSO : ScriptableObject
{
    public UnityAction<float, float> OnEventRaised;

    public void RaiseEvent(float current, float max)
    {
        OnEventRaised?.Invoke(current, max);
    }
}
