using System.Collections;
using Unity.AI.Planner.DomainLanguage.TraitBased;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public IEnumerator MoveTo(GameObject character, GameObject destination)
    {
	    character.transform.LookAt(destination.transform);

	    while (Vector3.Distance(character.transform.position, destination.transform.position) > 0.1f)
	    {
		    character.transform.position = Vector3.Lerp(character.transform.position, destination.transform.position, 12 * Time.deltaTime);
		    yield return null;
	    }
    }

    public void PickItem(GameObject character, GameObject item)
    {
	    item.SetActive(false);
	    character.transform.Find("Key").gameObject.SetActive(true);
    }
}
