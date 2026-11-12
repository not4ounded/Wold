using UnityEngine;
using UnityEngine.Events;

public class VoidEventListenerSO : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO _channel;

    public UnityAction OnEventRaised;

    private void OnEnable()
    {
        if (_channel != null)
            _channel.OnEventRaised += RaiseEvent;
    }

    private void OnDisable()
    {
        if (_channel != null)
            _channel.OnEventRaised -= RaiseEvent;
    }

    private void RaiseEvent()
    {
        if (OnEventRaised != null) 
            OnEventRaised.Invoke();
    }
}
