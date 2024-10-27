using UnityEngine;


[System.Serializable]
public class ChessBoardInfo
{
	/// <summary>
	/// [��� ȿ���� ���� ����] 
	/// �� ã�� �˰����� ���簢�� Board �׸���� ���, 
	/// Square Ȱ��ȭ�� ���簢�� Board �ȿ��� üũ�Ǳ� ����
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

