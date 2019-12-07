using System.Collections;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public IEnumerator MoveTo(ITraitBasedObjectData character, ITraitBasedObjectData target)
    {
	    var characterObject = (character.ParentObject as GameObject);
	    var targetObject = (target.ParentObject as GameObject);
	    
	    characterObject.transform.LookAt(targetObject.transform);

	    while (Vector3.Distance(characterObject.transform.position, targetObject.transform.position) > 0.1f)
	    {
		    characterObject.transform.position = Vector3.Lerp(characterObject.transform.position, targetObject.transform.position, 12 * Time.deltaTime);
		    yield return null;
	    }
    }
    
    public void PickItem(ITraitBasedObjectData character, ITraitBasedObjectData item)
    {
	    (item.ParentObject as GameObject)?.SetActive(false);
	    (character.ParentObject as GameObject)?.transform.Find("Key").gameObject.SetActive(true);
    }
}
