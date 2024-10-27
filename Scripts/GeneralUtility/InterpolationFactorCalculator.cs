using System.Collections;
using UnityEngine;


//public static class InterpolationFactorCalculator
public static class InterpolationFactorCalculator
{
	
	/// <summary>
	/// 
	/// </summary>
	public static float GetTFromZero(Vector3 currentPos, Vector3 initialPos, Vector3 goalPos)
	{
		if (initialPos == goalPos) return 0;

		//float bias = (goalPos - initialPos).magnitude;
		float bias = Vector3.Distance(initialPos, goalPos);
		//float mine = (initialPos - currentPos).magnitude;
		float mine = Vector3.Distance(initialPos, currentPos);

		//Debug.Log(mine + " / " + bias);

		return Mathf.Clamp(mine / bias, 0, 1);
	}
	public static float GetTFromOne(Vector3 currentPos, Vector3 initialPos, Vector3 goalPos)
	{
		if (initialPos == goalPos) return 1;

		float bias = (goalPos - initialPos).magnitude;
		float mine = (goalPos - currentPos).magnitude;

		return Mathf.Clamp(mine / bias, 0, 1);
	}
}