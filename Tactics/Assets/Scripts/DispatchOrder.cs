using System;
using UnityEngine;

public class DispatchOrder : MonoBehaviour
{
    public void PickupItem(GameObject agent, GameObject item)
    {
        agent.GetComponent<SwatAgent>().GoTo(item);
    }

    public void TakeCover(GameObject agent, GameObject cover)
    {
        agent.GetComponent<SwatAgent>().GoTo(cover);
    }

    public void SkipTurn(GameObject agent)
    {
        Debug.Log($"{agent.name} Skip Turn");
    }
}
