using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 얘를 static으로 박을지 아니면 Board에서 squares를 관리하니까 맥락상 board에 넣을지 고민해보기
/// <summary> ROLE: Check Chess Square Mover Coroutine is over, Execute Chess Square Mover Coroutine </summary>
public class ChessSquareCoroutineManager : MonoBehaviour
{
	public Action OnChessBoardMoveStart;   // = ( () => Debug.Log("Board Move Start") );
	public Action OnChessBoardMoveEnd;   // = ( () => Debug.Log("Board Move End") );
	public Action OnChessSquareMoveDown;   // = ( () => Debug.Log(" ChessSquare Move Down Start") );

	public delegate IEnumerator squareMoveCoroutine(bool moveDown);

	private List<IEnumerator> chessSquareCoroutineContainer = new();
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
	public void StartSquareMoveCoroutine(squareMoveCoroutine moveCoroutine, bool moveDown)
	{
		IEnumerator coroutineInstance = moveCoroutine(moveDown);
		chessSquareCoroutineContainer.Add(coroutineInstance);
		StartCoroutine(ProcessCoroutineWithCallBack( coroutineInstance, moveDown));

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
	private IEnumerator ProcessCoroutineWithCallBack(IEnumerator coroutine, bool moveDown)
	{
		yield return RemoveCoroutineAfterExecute(coroutine);

		CallBack();

		void CallBack()
		{
			if (IsBoardMoving == true && chessSquareCoroutineContainer.Count == 0)
				IsBoardMoving = false;
		}
	}
	private IEnumerator RemoveCoroutineAfterExecute(IEnumerator watingCoroutine)
	{
		yield return StartCoroutine(watingCoroutine);

		chessSquareCoroutineContainer.Remove(watingCoroutine);
	}

}