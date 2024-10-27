using System.Collections.Generic;
using UnityEngine;


public class ActivationHandlerOnStart : MonoBehaviour
{
	[SerializeField] List<GameObject> enableTargetObjects;
	[SerializeField] List<GameObject> disableTargetObjects;

	private void Start()
	{
		Initialize();
	}
	
	// TODO: 이름 바꾸기
	public void Initialize()
	{
		foreach (var targetObj in enableTargetObjects)
		{
			if (targetObj.activeSelf != true) targetObj.SetActive(true);
		}
		foreach (var targetObj in disableTargetObjects)
		{
			if (targetObj.activeSelf != false) targetObj.SetActive(false);
		}
	}


}
