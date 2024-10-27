using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ObjectPool<T> where T: MonoBehaviour
{
	protected List<T> objectPool;

	public ObjectPool(List<T> initialPool)
	{
		objectPool = initialPool;
	}


	public T GetObject(T requestingObject)
	{
		var targetObj = objectPool.Where(obj => (obj == requestingObject)).FirstOrDefault();
		targetObj?.gameObject.SetActive(true);

		return targetObj;
	}
	public T GetObject(System.Func<T, bool> matchPredicate)
	{
		var targetObj = objectPool.Where(matchPredicate).FirstOrDefault();
		targetObj?.gameObject.SetActive(true);

		return targetObj;
	}
	public T GetObject(int objectIndex)
	{
		var targetObj = objectPool[objectIndex];
		targetObj?.gameObject.SetActive(true);

		return targetObj;
	}

	public void ReturnObject(T returningObject)
	{
		returningObject.gameObject.SetActive(false);
	}


}