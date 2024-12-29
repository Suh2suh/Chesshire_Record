using UnityEngine;


public class Singleton<T> : MonoBehaviour where T: Singleton<T>, new()
{
	private static T instance;
	public static T Instance
	{
		get
		{
			if (instance == null)
				instance = new GameObject().AddComponent<T>();

			return instance;
		}
	}
	[SerializeField] private bool isDontDestroyOnLoad = false;


	private void Awake()
	{
		if (instance == null)
		{
			instance = gameObject.GetComponent<T>();
			if (isDontDestroyOnLoad)  DontDestroyOnLoad(gameObject);
		}
		else
		if (instance != gameObject.GetComponent<T>())
		{
			Destroy(gameObject);
		}
	}


}