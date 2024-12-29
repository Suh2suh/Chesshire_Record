using UnityEngine;


public class InputHandler : MonoBehaviour
{
	[SerializeField] private KeyInputHandler keyInputHander;
	[SerializeField] private MouseInputHandler mouseInputHandler;

	public KeyInputHandler KeyInputHandler { get => keyInputHander; }
	public MouseInputHandler MouseInputHandler { get => mouseInputHandler; }


	private void Reset()
	{
		keyInputHander = GetComponent<KeyInputHandler>() ?? gameObject.AddComponent<KeyInputHandler>();
		mouseInputHandler = GetComponent<MouseInputHandler>() ?? gameObject.AddComponent<MouseInputHandler>();
	}


}