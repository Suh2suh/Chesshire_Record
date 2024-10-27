using UnityEngine;

public static class MissingObjectChecker
{
	private static bool IsMissing<T>(T checkingObj) where T : MonoBehaviour
	{
		return ReferenceEquals(checkingObj, null) ? false : (checkingObj == null ? true : false);
	}
}