using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public abstract class ChessBoardBase : MonoBehaviour, IChessBoard
{
	#region Setting Variables

	[SerializeField] private GameObject chessSquarePrefab;
	[SerializeField] protected GameObject chessSquareModel;
	[SerializeField] private float gapBetweenChessSquare;
	[SerializeField] private int maxBoardLength = 8;

	// Square 이동 부분은 고민 좀 필요
	[SerializeField] private float initialChessSquareHeight;
	//[SerializeField] private float chessSquareMovingDuration;

	#endregion

	#region Properties
	public ChessBoardInfo ChessBoardInfo { get; private set; }
	public ChessPuzzleBase ParentChessPuzzle { get; private set; }
	private ChessSquareBase targetedChessSquare;
	public ChessSquareBase TargetedChessSquare
	{
		get => targetedChessSquare;
		set
		{
			var newTargetedSquare = value;
			if (targetedChessSquare != newTargetedSquare)
			{
				targetedChessSquare = newTargetedSquare;

				OnChessSquareTargeted(newTargetedSquare);
			}
		}
	}

	#endregion

	private List<ChessSquareBase> occupiableChessSquares;
	private Dictionary<Vector2Int, ChessSquareBase> validChessSquarePerGrid;


	#region Initialization
	public void Initialize(ChessBoardInfo chessBoardInfo)
	{
		ChessBoardInfo = chessBoardInfo;
		ParentChessPuzzle = transform.GetComponentInParent<ChessPuzzleBase>();
		if (gapBetweenChessSquare < 1) gapBetweenChessSquare = 1;

		validChessSquarePerGrid = new();
		occupiableChessSquares = new();

		CreateBoardSquares(shouldCreateBridge: true);

		PositionBoardOnCenter();
		void PositionBoardOnCenter()
		{
			transform.localPosition = CalculateSquarePosition((4 - ChessBoardInfo.BoardLength) / 2, 0, MeshHandler.GetWorldMeshBoundSize(chessSquareModel));
		}

		CallbackInitialize();
	}

	private void CreateBoardSquares(bool shouldCreateBridge = true, int maxBoardLength = 8)
	{
		CreateChessSquaresFrom(ChessBoardInfo, MeshHandler.GetWorldMeshBoundSize(chessSquareModel));

		if (shouldCreateBridge)
		{
			CreateEntranceSquares(validChessSquarePerGrid[ChessBoardInfo.EntranceGrid.Grid]);
			CreateExitSquares(validChessSquarePerGrid[ChessBoardInfo.ExitGrid.Grid]);
		}
	}
	private void CreateChessSquaresFrom(ChessBoardInfo chessBoardInfo, Vector3 chessSquareModelBound)
	{
		foreach (var chessSquareInfo in chessBoardInfo.BoardStructure)
		{
			if (chessSquareInfo.IsActive == false) continue;

			var chessSquareObj = Instantiate(chessSquarePrefab, Vector3.zero, Quaternion.identity, this.transform);
			chessSquareObj.transform.localPosition = CalculateSquarePosition(chessSquareInfo.Grid.x, chessSquareInfo.Grid.y, chessSquareModelBound);
			chessSquareObj.name = "ChessSquare_" + (chessSquareInfo.Grid.x + "-" + chessSquareInfo.Grid.y);

			var chessSquare = chessSquareObj.GetComponent<ChessSquareBase>();
			chessSquare.Initialize(chessSquareInfo, chessSquareModel, initialChessSquareHeight);

			validChessSquarePerGrid[chessSquareInfo.Grid] = chessSquare;
		}
	}

	private void CreateEntranceSquares(ChessSquareBase entranceChessSquare)
	{
		entranceChessSquare.ChangeModelColor(Color.red);

		Vector2Int entranceSquareGrid = entranceChessSquare.ChessSquareInfo.Grid;
		int bridgeCount = (maxBoardLength - ChessBoardInfo.BoardLength) / 2 +
						  ((ChessBoardInfo.BoardLength - 1) - entranceChessSquare.ChessSquareInfo.Grid.x);
		if (bridgeCount <= 0) return;

		for (int gridX = entranceSquareGrid.x + 1; gridX <= entranceSquareGrid.x + bridgeCount; gridX++)
		{
			var chessBridgeObj = Instantiate(chessSquareModel, this.transform);
			chessBridgeObj.name = "EntranceSquare_" + gridX + "-" + entranceSquareGrid.y;
			chessBridgeObj.transform.localPosition = CalculateSquarePosition(gridX, entranceSquareGrid.y, MeshHandler.GetWorldMeshBoundSize(chessSquareModel));
		}
	}
	private void CreateExitSquares(ChessSquareBase exitChessSquare)
	{
		exitChessSquare.ChangeModelColor(Color.green);

		Vector2Int exitSquareGrid = exitChessSquare.ChessSquareInfo.Grid;
		int bridgeCount = (maxBoardLength - ChessBoardInfo.BoardLength) / 2 +
						  (exitChessSquare.ChessSquareInfo.Grid.x);
		if (bridgeCount <= 0) return;

		for (int gridX = exitSquareGrid.x - 1; gridX >= exitSquareGrid.x - bridgeCount; gridX--)
		{
			var chessBridgeObj = Instantiate(chessSquareModel, this.transform);
			chessBridgeObj.name = "ExitSquare_" + gridX + "-" + exitSquareGrid.y;
			chessBridgeObj.transform.localPosition = CalculateSquarePosition(gridX, exitSquareGrid.y, MeshHandler.GetWorldMeshBoundSize(chessSquareModel));
		}
	}

	#endregion
	protected abstract void CallbackInitialize();


	#region OnChessSquareTargeted
	protected void OnChessSquareTargeted(ChessSquareBase newTargetedChessSquare)
	{
		ClearPrevTargetedSquares();
		void ClearPrevTargetedSquares()
		{
			var prevOccupiableChessSquares = occupiableChessSquares;
			if (prevOccupiableChessSquares.Count > 0)
			{
				ProcessPrevTargetedSquares(prevOccupiableChessSquares);
			}
		}

		if (newTargetedChessSquare == null)
		{
			occupiableChessSquares.Clear();
			return;
		}

		UpdateOccupiableChessSquares();
		void UpdateOccupiableChessSquares()
		{
			if (ParentChessPuzzle.TryCalculateOccupiableGrids(out var occupiableGrids))
			{
				occupiableChessSquares = occupiableGrids.Where(grid => validChessSquarePerGrid.ContainsKey(grid))
															.Select(validGrid => validChessSquarePerGrid[validGrid])
															.ToList();
			}
			else
			{
				occupiableChessSquares.Clear();
			}
		}

		ProcessNewTargetedSquares(occupiableChessSquares);
	}

	#endregion
	protected abstract void ProcessPrevTargetedSquares(List<ChessSquareBase> prevTargetedSquares);
	protected abstract void ProcessNewTargetedSquares(List<ChessSquareBase> newTargetedSquares);


	/// <summary>
	/// Occupy all occupialbe chess squares by holding chessPiece
	/// </summary>
	public void OccupyChessSquaresBy(ChessPieceBase placedChessPiece)
	{
		foreach (var occupiableChessSquare in occupiableChessSquares)
			occupiableChessSquare.OccupyChessSquareBy(placedChessPiece);
		TargetedChessSquare.SetToppedChessPiece(placedChessPiece);

		TargetedChessSquare = null;
	}


	#region Chess Square Management Utility

	/// <summary>
	/// Emit target chess squares: On/Off
	/// </summary>
	protected void EmitChessSquares(List<ChessSquareBase> targetChessSquares, bool isOn)
	{
		foreach (var targetChessSquare in targetChessSquares)
			targetChessSquare.ActivateTransparentGrid(isOn);
	}


	/// <summary>
	/// Get ChessSquare by grid
	/// </summary>
	public bool TryGetChessSquareOn(Vector2Int grid, out ChessSquareBase chessSquare)
	{
		if (!validChessSquarePerGrid.ContainsKey(grid))
		{
			chessSquare = null;
			return false;
		}
		else
		{
			chessSquare = validChessSquarePerGrid[grid];
			return true;
		}
	}


	public Vector3 CalculateSquarePosition(int gridX, int gridY)
	{
		return CalculateSquarePosition(gridX, gridY, MeshHandler.GetWorldMeshBoundSize(chessSquareModel));
	}
	protected Vector3 CalculateSquarePosition(int gridX, int gridY, Vector3 squareBound)
	{
		return new Vector3(gridX * squareBound.x * gapBetweenChessSquare,
						   0,
						   gridY * squareBound.z * gapBetweenChessSquare);
	}


	#endregion


}