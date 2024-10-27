using UnityEngine;


public class Singleton<T> : MonoBehaviour where T: Singleton<T>, new()
{
	private static T instance;
	public static T Instance
	{
		get
		{
			// 없다는 건, Awake가 실행되지 X, 즉 GameObject가 없다는 것.
			if (instance == null)
				instance = new GameObject().AddComponent<T>();
				// 그냥 T는 MonoBehaviour이라는 보장이 없기 때문에, Monobehabiour처리를 해줘야함.

			return instance;
		}
	}
	[SerializeField] private bool isDontDestroyOnLoad = false;


	private void Awake()
	{
		if (instance == null)
		{
			// 여기가 문제인 게, instance에 뭘 할당하려고 하니까 T타입은 객체가 아닌 타입형이라 할당이 불가
			// 즉, T타입의 객체를 받아와야 함 -> T가 현재 클래스의 자식이라면, T는 내 정보를 포함하고 있음.
			// instance = this as T;
			instance = this.gameObject.GetComponent<T>();
			if (isDontDestroyOnLoad)  DontDestroyOnLoad(this.gameObject);
		}
		else
		if (instance != this.gameObject.GetComponent<T>())
		{
			Destroy(this.gameObject);
		}
	}


}