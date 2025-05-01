using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerScript : MonoBehaviour
{
    public UnityEvent<GameObject> UE_event;
    public string S_requiredTag = "";
    public triggerEnum TriggerType = triggerEnum.onTriggerEnter;
    public enum triggerEnum { onTriggerEnter, onTriggerStay, onTriggerExit, onCollisionEnter, onCollisionStay, onCollisionExit};

    private void OnTriggerEnter(Collider other)
    {
        if (TriggerType == triggerEnum.onTriggerEnter)
        {
            if (S_requiredTag == "" || S_requiredTag == other.tag)
                UE_event.Invoke(other.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (TriggerType == triggerEnum.onTriggerStay)
        {
            if (S_requiredTag == "" || S_requiredTag == other.tag)
                UE_event.Invoke(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (TriggerType == triggerEnum.onTriggerExit)
        {
            if (S_requiredTag == "" || S_requiredTag == other.tag)
                UE_event.Invoke(other.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (TriggerType == triggerEnum.onCollisionEnter)
        {
            if (S_requiredTag == "" || S_requiredTag == collision.collider.tag)
                UE_event.Invoke(collision.gameObject);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (TriggerType == triggerEnum.onCollisionStay)
        {
            if (S_requiredTag == "" || S_requiredTag == collision.collider.tag)
                UE_event.Invoke(collision.gameObject);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (TriggerType == triggerEnum.onCollisionExit)
        {
            if (S_requiredTag == "" || S_requiredTag == collision.collider.tag)
                UE_event.Invoke(collision.gameObject);
        }
    }
}
