using UnityEngine;

public class PickupOnTrigger : MonoBehaviour
{
    public void OnTriggerEnter(Collider collider)
    {
        var swatAgent = collider.GetComponent<SwatAgent>();
        
        if (swatAgent != null)
        {
            swatAgent.SetWeapon(true);
            Destroy(gameObject);
        }
    }
}
