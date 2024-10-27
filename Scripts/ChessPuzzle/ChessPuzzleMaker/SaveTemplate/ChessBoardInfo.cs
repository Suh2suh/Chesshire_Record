using UnityEngine;


[System.Serializable]
public class ChessBoardInfo
{
	/// <summary>
	/// [계산 효율을 위해 저장] 
	/// 길 찾기 알고리즘이 정사각형 Board 그리드로 계산, 
	/// Square 활성화가 정사각형 Board 안에서 체크되기 때문
	/// </summary>
	[SerializeField] private int boardLength;
	[SerializeField] private ChessSquareInfo[] boardStructure;

	[SerializeField] private BridgeGridInfo entranceGrid;
	[SerializeField] private BridgeGridInfo exitGrid;

	#region Property
	public int BoardLength { get => boardLength; set => boardLength = value; }
	public ChessSquareInfo[] BoardStructure { get => boardStructure; set => boardStructure = value; }
	public BridgeGridInfo EntranceGrid { get => entranceGrid; set => entranceGrid = value; }
	public BridgeGridInfo ExitGrid { get => exitGrid; set => exitGrid = value; }

	#endregion


	[System.Serializable]
	public class BridgeGridInfo
	{
		[SerializeField] Vector2Int grid = new Vector2Int(-1, -1);
		[SerializeField] Vector2Int bridgeDirection;
		public Vector2Int Grid { get => grid; set => grid = value; }
		public Vector2Int BridgeDirection { get => bridgeDirection; set => bridgeDirection = value; }
	}


}

