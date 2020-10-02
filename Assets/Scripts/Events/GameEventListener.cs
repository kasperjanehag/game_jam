using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GenericEvent : UnityEvent<object> { }

public class GameEventListener : MonoBehaviour
{
    [SerializeField]
    private GameEvent gameEvent;
    [SerializeField]
    private GenericEvent response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(object param)
    {
        response.Invoke(param);
    }
}
