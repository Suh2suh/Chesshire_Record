using UnityEngine;


/// <summary> This class is for converting between Json File </summary>
[System.Serializable]
public class ChessSquareInfo
{

	[SerializeField] private Vector2Int grid = Vector2Int.zero;
	[SerializeField] private SquareType squareType = SquareType.Common;
	[SerializeField] private bool isActive = true;

	#region Property
	public Vector2Int Grid { get => grid; set => grid = value; }
	public SquareType SquareType 
	{
		get => squareType;
		set => squareType = value;
	}
	public bool IsActive { get => isActive; set => isActive = value; }

	#endregion


}