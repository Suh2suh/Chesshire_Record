using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public static class EventTriggerLinker
{

	/// <summary>  Action Parameter's form: void Action(T BaseEventData)  </summary>
	public static void LinkEventTriggerToTransform<T>(Transform targetTransform, EventTriggerType eventTriggerType, UnityAction<T> action) where T : BaseEventData
	{
		var targetEventTrigger = targetTransform.GetComponent<EventTrigger>() ?? targetTransform.gameObject.AddComponent<EventTrigger>();
		LinkEventTriggerToTrigger(targetEventTrigger, eventTriggerType, action);
	}
	/// <summary>  Action Parameter's form: void Action(T1 BaseEventData, T2 parameter1)  </summary>
	public static void LinkEventTriggerToTransform<T1, T2>(Transform targetTransform, EventTriggerType eventTriggerType, UnityAction<T1, T2> action, T2 actionParameter) where T1 : BaseEventData
	{
		var targetEventTrigger = targetTransform.GetComponent<EventTrigger>() ?? targetTransform.gameObject.AddComponent<EventTrigger>();
		LinkEventTriggerToTrigger(targetEventTrigger, eventTriggerType, action, actionParameter);
	}

	/// <summary>  Action Parameter's form: void Action(T BaseEventData)  </summary>
	public static void LinkEventTriggerToTrigger<T>(EventTrigger targetEventTrigger, EventTriggerType eventTriggerType, UnityAction<T> action) where T : BaseEventData
	{
		var newEntry = new EventTrigger.Entry { eventID = eventTriggerType };
		newEntry.callback.AddListener((data) => { action((T)data); });

		targetEventTrigger.triggers.Add(newEntry);
	}
	/// <summary>  Action Parameter's form: void Action(T1 BaseEventData, T2 parameter1)  </summary>
	public static void LinkEventTriggerToTrigger<T1, T2>(EventTrigger targetEventTrigger, EventTriggerType eventTriggerType, UnityAction<T1, T2> action, T2 actionParameter) where T1 : BaseEventData
	{
		var newEntry = new EventTrigger.Entry { eventID = eventTriggerType };
		newEntry.callback.AddListener((data) => { action((T1)data, actionParameter); });

		targetEventTrigger.triggers.Add(newEntry);
	}


}