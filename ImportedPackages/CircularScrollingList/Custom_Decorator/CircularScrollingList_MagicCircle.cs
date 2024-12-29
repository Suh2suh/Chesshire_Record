using UnityEngine;


namespace AirFishLab.ScrollingList
{
	public class CircularScrollingList_MagicCircle : CircularScrollingList
	{
		[SerializeField] private RectTransform magicCircleRT;

		protected override void CallbackMovement(float movementValue)
		{
			RotateMagicCircle(movementValue);
		}

		private void RotateMagicCircle(float movementValue)
		{
			if (Mathf.Approximately(movementValue, 0f))
				return;

			magicCircleRT.Rotate(Vector3.forward, movementValue * 0.3f);
		}
	}
}