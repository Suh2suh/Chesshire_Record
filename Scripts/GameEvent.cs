using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvent", order = 0)]
public class GameEvent : ScriptableObject
{
	[SerializeField]
	private List<UnityEvent> responses;


	public void Raise()
	{
		//Debug.Log("Response Count: " + responses.Count);

		for (int i = responses.Count - 1; i >= 0; i--)
		{
			//Debug.Log("Raise ," + i);
			responses[i]?.Invoke();
		}
	}

	public void LinkResponse(UnityEvent response)
	{
		responses.Add(response);
	}
	public void UnLinkResponse(UnityEvent response)
	{
		responses.Remove(response);
	}


}