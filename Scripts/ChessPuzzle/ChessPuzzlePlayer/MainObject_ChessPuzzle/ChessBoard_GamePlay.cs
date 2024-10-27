using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Chess Square Square Manager
/// </summary>
[RequireComponent(typeof(ChessSquareCoroutineManager))]
public class ChessBoard_GamePlay : ChessBoardBase
{
	#region Setting Variables
	[SerializeField] private ChessSquareCoroutineManager chessSquareCoroutineManager;
	[SerializeField] private float chessSquareMovingDuration;

	#endregion

	#region [Property]
	public ChessSquareCoroutineManager ChessSquareCoroutineManager
	{
		get => chessSquareCoroutineManager;
		private set => chessSquareCoroutineManager = value;
	}

	public float ChessSquareMovingDuration 
	{
		get => chessSquareMovingDuration; 
		private set => chessSquareMovingDuration = value; 
	}

	#endregion


	#region Unity Methods
	private void Awake()
	{
		chessSquareCoroutineManager = GetComponent<ChessSquareCoroutineManager>();
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
