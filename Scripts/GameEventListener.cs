using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
	[SerializeField]
	List<EventResponsePair> eventResponsePair = new List<EventResponsePair>();


	private void OnEnable()
	{
		foreach (var pair in eventResponsePair) 
		{
			pair.TargetEvent?.LinkResponse(pair.Responses);
		}
	}

	private void OnDisable()
	{
		foreach (var pair in eventResponsePair)
		{
			pair.TargetEvent?.UnLinkResponse(pair.Responses);
		}
	}

	[System.Serializable]
	public struct EventResponsePair
	{
		[SerializeField] private GameEvent targetEvent;
		[SerializeField] private UnityEvent responses;

		public GameEvent TargetEvent { get => targetEvent; }
		public UnityEvent Responses { get => responses; }
	}

}

