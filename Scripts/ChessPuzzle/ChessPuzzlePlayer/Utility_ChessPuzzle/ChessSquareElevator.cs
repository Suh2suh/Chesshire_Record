using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;


public class ChessSquareElevator : Singleton<ChessSquareElevator>
{
	public Action OnChessBoardMoveStart;   // = ( () => Debug.Log("Board Move Start") );
	public Action OnChessBoardMoveEnd;   // = ( () => Debug.Log("Board Move End") );
	public Action OnChessSquareMoveDown;  //= ( () => Debug.Log(" ChessSquare Move Down Start") );

	public delegate UniTask SquareElevateFunc(bool moveDown);
	private List<Func<UniTask>> squareElevateTaskContainer = new();

	private bool isBoardMoving = false;
	public bool IsBoardMoving
	{
		get => isBoardMoving;
		private set
		{
			if (value == true)
				OnChessBoardMoveStart?.Invoke();
			else
				OnChessBoardMoveEnd?.Invoke();

			isBoardMoving = value;
		}
	}


	/// <summary>
	/// Put SquareMove coroutine to the list, alert "BoardMoveStart" when first square moving starts
	/// </summary>
	public void StartMoveChessSquare(SquareElevateFunc squareElevateFunc, bool moveDown)
	{
		Debug.Log("Task Created");
		Func<UniTask> squareElevateTask = (() => squareElevateFunc(moveDown));
		squareElevateTaskContainer.Add(squareElevateTask);   // TODO: Alternative: Queue에 넣기만 하고 Update에서 실행하는 방법
		ElevateAndPostProcess(squareElevateTask).Forget();

		CallBack();
		void CallBack()
		{
			if (moveDown)
				OnChessSquareMoveDown?.Invoke();

			if (IsBoardMoving == false)
				IsBoardMoving = true;
		}
	}


	/// <summary>
	/// Execute SquareMove coroutine, alert "BoardMoveEnd" when last coroutine execution finished
	/// </summary>
	private async UniTaskVoid ElevateAndPostProcess(Func<UniTask> squareElevateTask)
	{
		await InvokeAndPopAsync(squareElevateTask);

		CallBack();
		void CallBack()
		{
			if (IsBoardMoving == true && squareElevateTaskContainer.Count == 0)
				IsBoardMoving = false;
		}
	}

	private async UniTask InvokeAndPopAsync(Func<UniTask> squareElevateTask)
	{
		Debug.Log("Should now!");
		await squareElevateTask().AttachExternalCancellation(this.GetCancellationTokenOnDestroy());

		squareElevateTaskContainer.Remove(squareElevateTask);
	}



}