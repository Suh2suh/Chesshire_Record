using System;
using UnityEngine;


public class MouseInputHandler : MonoBehaviour
{
	[Header("Check Function/MouseButton you want to check")]
	[SerializeField] private MouseInputStatus mouseDownTargets;
	[SerializeField] private MouseInputStatus mouseUpTargets;
	[SerializeField] private  MouseInputStatus mousePressedTargets;
	private MouseInputStatus mouseDown = new();
	private MouseInputStatus mouseUp = new();
	private MouseInputStatus mousePressed = new();


	#region Unity Methods

	private void Update()
	{
		foreach(var downTargetButton in mouseDownTargets.ButtonInputStatus.Keys)
		{
			if (mouseDownTargets.ButtonInputStatus[downTargetButton] == false)  continue;
			mouseDown.ButtonInputStatus[downTargetButton] = Input.GetMouseButton(Convert.ToInt32(downTargetButton));
		}
		foreach (var upTargetButton in mouseUpTargets.ButtonInputStatus.Keys)
		{
			if (mouseUpTargets.ButtonInputStatus[upTargetButton] == false)  continue;
			mouseUp.ButtonInputStatus[upTargetButton] = Input.GetMouseButtonUp(Convert.ToInt32(upTargetButton));
		}
		foreach (var pressedTargetButton in mousePressedTargets.ButtonInputStatus.Keys)
		{
			if (mousePressedTargets.ButtonInputStatus[pressedTargetButton] == false)  continue;
			mousePressed.ButtonInputStatus[pressedTargetButton] = Input.GetMouseButtonDown(Convert.ToInt32(pressedTargetButton));
		}
	}


	#endregion


	public bool IsButtonDown(MouseButton downTargetButton)
	{
		return mouseDown.ButtonInputStatus[downTargetButton];
	}
	public bool IsButtonUp(MouseButton upTargetButton)
	{
		return mouseUp.ButtonInputStatus[upTargetButton];
	}
	public bool IsButtonPressed(MouseButton pressedTargetButton)
	{
		return mousePressed.ButtonInputStatus[pressedTargetButton];
	}




	[System.Serializable]
	private class MouseInputStatus
	{
		[SerializeField]
		private UDictionary<MouseButton, bool> buttonInputStatus = new()
		{
			{ MouseButton.Left, false },
			{ MouseButton.Middle, false },
			{ MouseButton.Right, false }
		};
		public UDictionary<MouseButton, bool> ButtonInputStatus
		{
			get => buttonInputStatus;
			set => buttonInputStatus = value;
		}
	}

}


public enum MouseButton
{
	Left = 0,
	Middle = 1,
	Right = 2
}