using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChessSquare_GamePlay : ChessSquareBase
{
	public bool IsOnPeak { get; private set; }
	private ChessBoard_GamePlay chessBoard_GamePlay;


	protected override void Awake()
	{
		base.Awake();

		chessBoard_GamePlay = parentChessBoard as ChessBoard_GamePlay;
	}

	protected override void CallbackInitialize(){}


	protected override void OnChessSquareOccupied()
	{
		chessBoard_GamePlay.GetComponent<ChessSquareElevator>().StartMoveChessSquare(SmoothMoveChessSquareModel, moveDown: false);
	}

	protected override void OnChessSquareVacated()
	{
		chessBoard_GamePlay.GetComponent<ChessSquareElevator>().StartMoveChessSquare(SmoothMoveChessSquareModel, moveDown: true);
	}


	// MovingChessBoard 아래에 있는 애들 한해서만.
	// [Action: Model - Move, Color, Emit]
	private async UniTask SmoothMoveChessSquareModel(bool moveDown)
	{
		Debug.Log("Down");

		Vector3 startPos = modelParent.localPosition;
		Vector3 goalPos = (moveDown ? initialModelPos : Vector3.zero); 
		float movingDuration = (chessBoard_GamePlay.ChessSquareMovingDuration * Mathf.Abs(goalPos.y - startPos.y)) / Mathf.Abs(initialModelPos.y);   // 3 : x = | highPoint - floorPoint | : | goalPos - startPos |
		if (moveDown.Equals(true))  IsOnPeak = false;

		float time = 0f, t = 0f;
		while(t < 1)
	    {
			time += Time.deltaTime;

			t = time / movingDuration;
			modelParent.localPosition = Vector3.Lerp(startPos, goalPos, t);

			bool shouldStopMoving = (moveDown == true && OccupyingChessPieces.Count > 0) || (moveDown == false && OccupyingChessPieces.Count == 0);
			if (shouldStopMoving)
				return;

			await UniTask.Yield();
	    }

		InstantMoveChessSquareModel(moveDown);
		if (moveDown.Equals(false))  IsOnPeak = true;
	}
	private void InstantMoveChessSquareModel(bool moveDown)
	{
		if (moveDown)
			modelParent.localPosition = initialModelPos;
		else
			modelParent.localPosition = Vector3.zero;
	}


}
