using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    public TriggerType triggerType;
    public delegate void OnTrigger(TriggerType triggerType);
    public static event OnTrigger onTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            HandleTriggers();
            onTrigger.Invoke(triggerType);
        }
    }

    //Disable colliders for spawnpoint triggers to set it once only
    private void HandleTriggers()
    {
        if (triggerType == TriggerType.SpawnPointOneTrigger || triggerType == TriggerType.SpawnPointTwoTrigger || triggerType == TriggerType.SpawnPointTwoTrigger)
        {
            transform.GetComponent<Collider>().enabled = false;
        }
    }
}
