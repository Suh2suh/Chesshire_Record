using UnityEditor.XR;
using UnityEngine;


public class ChessPuzzle_GamePlay : ChessPuzzleBase
{
	#region Setting Variables

	[SerializeField] GameEvent chessPuzzleStartEvent;

	[Space(15)]
	[SerializeField] GameObject entranceBridge;
	[SerializeField] GameObject exitBridge;

	#endregion


	#region Unity Methods: Link Variables & Events

	protected override void Awake()
	{
		base.Awake();

		// When ChessSquares Moving Finished
		(chessBoard as ChessBoard_GamePlay).ChessSquareCoroutineManager.OnChessBoardMoveEnd += AlertOnPathFindingFinished;

	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		// When ChessSquares Moving Finished
		(chessBoard as ChessBoard_GamePlay).ChessSquareCoroutineManager.OnChessBoardMoveEnd -= AlertOnPathFindingFinished;
	}


	#endregion

	#region Initialization

	protected override void InitializeBridges()
	{
		chessBoard.TryGetChessSquareOn(ChessPuzzleInfo.ChessBoardInfo.EntranceGrid.Grid, out var entranceChessSquare);
		entranceBridge.transform.localPosition = new Vector3(entranceBridge.transform.localPosition.x,
															 entranceBridge.transform.localPosition.y,
															 chessBoard.transform.localPosition.z + entranceChessSquare.transform.localPosition.z);

		chessBoard.TryGetChessSquareOn(ChessPuzzleInfo.ChessBoardInfo.ExitGrid.Grid, out var exitChessSquare);
		exitBridge.transform.localPosition = new Vector3(exitBridge.transform.localPosition.x,
														 exitBridge.transform.localPosition.y,
														 chessBoard.transform.localPosition.z + exitChessSquare.transform.localPosition.z);

		entranceBridgePos = entranceBridge.transform.position;
		exitBridgePos = exitBridge.transform.position;
	}


	protected override void CallbackInitialize()
	{
		chessPuzzleStartEvent?.Raise();
	}


	#endregion


	#region Passable Path Finding in ChessBoard

	protected override bool ValidateEntranceGrid(Vector2Int entranceGrid)
	{
		if (chessBoard.TryGetChessSquareOn(entranceGrid, out var entranceChessSquare))
		{
			return (entranceChessSquare.ChessSquareInfo.SquareType == SquareType.Common &&
					entranceChessSquare.OccupyingChessPieces.Count > 0 &&
				    (entranceChessSquare as ChessSquare_GamePlay).IsOnPeak);
		}

		return false;
	}
	protected override bool ValidatePassableGrid(Vector2Int checkingGird)
	{
		if (chessBoard.TryGetChessSquareOn(checkingGird, out var checkingChessSquare))
		{
			return (checkingChessSquare.ChessSquareInfo.SquareType == SquareType.Common &&
						checkingChessSquare.OccupyingChessPieces.Count > 0 &&
						(checkingChessSquare as ChessSquare_GamePlay).IsOnPeak);
		}

		return false;
	}


	#endregion


}
