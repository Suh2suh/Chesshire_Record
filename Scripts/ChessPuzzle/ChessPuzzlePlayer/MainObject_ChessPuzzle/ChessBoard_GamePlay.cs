using System.Collections.Generic;
using UnityEngine;


public class ChessBoard_GamePlay : ChessBoardBase
{
	#region Setting Variables
	[SerializeField] private float chessSquareMovingDuration;

	#endregion

	#region [Property]
	public float ChessSquareMovingDuration 
	{
		get => chessSquareMovingDuration; 
		private set => chessSquareMovingDuration = value; 
	}

	#endregion


	protected override void ProcessPrevTargetedSquares(List<ChessSquareBase> prevTargetedSquares)
	{
		EmitChessSquares(prevTargetedSquares, false);
	}
	protected override void ProcessNewTargetedSquares(List<ChessSquareBase> newTargetedSquares)
	{
		EmitChessSquares(newTargetedSquares, true);
	}

	protected override void CallbackInitialize()
	{
		//
	}

}
