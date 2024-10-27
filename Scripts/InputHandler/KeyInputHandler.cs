using System.Collections.Generic;
using UnityEngine;


public class KeyInputHandler : MonoBehaviour
{

	[SerializeField] private List<KeyCode> downCheckKeys;
	[SerializeField] private List<KeyCode> upCheckKeys;
	[SerializeField] private List<KeyCode> pressedCheckKeys;

	private Dictionary<KeyCode, bool> keyDownStatus = new();
	private Dictionary<KeyCode, bool> keyUpStatus = new();
	private Dictionary<KeyCode, bool> keyPressedStatus = new();


	#region Unity Methods

	private void Awake()
	{
		if (downCheckKeys != null)
			foreach (var downTargetKey in downCheckKeys)
				keyDownStatus[downTargetKey] = false;
		if (upCheckKeys != null)
			foreach (var upTargetKey in upCheckKeys)
				keyUpStatus[upTargetKey] = false;
		if (pressedCheckKeys != null)
			foreach (var pressedTargetKey in pressedCheckKeys)
				keyPressedStatus[pressedTargetKey] = false;
	}


	private void Update()
	{
		foreach(var downTargetKey in downCheckKeys)
			keyDownStatus[downTargetKey] = Input.GetKey(downTargetKey);
		foreach (var upTargetKey in upCheckKeys)
			keyUpStatus[upTargetKey] = Input.GetKeyUp(upTargetKey);
		foreach (var pressedTargetKey in pressedCheckKeys)
			keyPressedStatus[pressedTargetKey] = Input.GetKeyDown(pressedTargetKey);
	}


	#endregion


	public bool IsKeyDown(KeyCode downTargetKey)
	{
		if (keyDownStatus.ContainsKey(downTargetKey))
			return keyDownStatus[downTargetKey];
		else
			return false;
	}
	public bool IsKeyUp(KeyCode upTargetKey)
	{
		if (keyUpStatus.ContainsKey(upTargetKey))
			return keyUpStatus[upTargetKey];
		else
			return false;
	}
	public bool IsKeyPressed(KeyCode pressedTargetKey)
	{
		if (keyPressedStatus.ContainsKey(pressedTargetKey))
			return keyPressedStatus[pressedTargetKey];
		else
			return false;
	}


}
