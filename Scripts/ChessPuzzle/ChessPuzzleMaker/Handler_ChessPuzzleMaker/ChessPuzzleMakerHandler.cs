using System.Collections;
using UnityEngine;


[RequireComponent(typeof(ChessPuzzleMakerCreator), typeof(ChessBoardCameraMover), typeof(ChessPuzzleSaver_Maker))]
public class ChessPuzzleMakerHandler : MonoBehaviour
{
	[SerializeField] private ChessPuzzleMakerCreator chessPuzzleMakerCreator;
	[SerializeField] private ChessBoardCameraMover chessBoardCameraMover;
	[SerializeField] private ChessPuzzleSaver_Maker chessPuzzleSaver;

	#region Property
	public ChessPuzzleMakerCreator ChessPuzzleMakerCreator { get => chessPuzzleMakerCreator; }
	public ChessBoardCameraMover ChessBoardCameraMover { get => chessBoardCameraMover; }
	public ChessPuzzleSaver_Maker ChessPuzzleSaver { get => chessPuzzleSaver; }

	#endregion


	/// <summary>
	/// Used for creating new puzzleMaker
	/// </summary>
	/// <param name="boardLength"></param>
	public void SetupChessPuzzleMaker(int boardLength)
	{
		chessPuzzleMakerCreator.InstantiatePuzzleMaker(boardLength);
		chessBoardCameraMover.ConvertCamTo(boardLength);
	}


	/// <summary>
	/// Create ChessPuzzleMaker using ChessPuzzleInfo
	/// </summary>
	/// <param name="chessPuzzleInfo"> chessPuzzleMaker is initialized with this information </param>
	/// <returns></returns>
	public IEnumerator CreateChessPuzzleMakerAsync(ChessPuzzleInfo chessPuzzleInfo)
	{
		var chessBoardInfo = chessPuzzleInfo.ChessBoardInfo;
		var chessPieceBoxInfo = chessPuzzleInfo.ChessPieceBoxInfo;

		GameObject chessPuzzleMaker = chessPuzzleMakerCreator.InstantiatePuzzleMaker(chessBoardInfo.BoardLength);
		var makerChessPuzzle = chessPuzzleMaker.GetComponent<ChessPuzzle_Maker>();

		SetupChessPieceBox();
		void SetupChessPieceBox()
		{
			var makerChessPieceBox = makerChessPuzzle.transform.GetComponentInChildren<ChessPieceBox_Maker>();
			makerChessPieceBox.UpdateChessPieceBox(chessPieceBoxInfo);
			var chessPieceHandler = chessPuzzleMaker.GetComponentInChildren<ChessPieceHandler_Maker>();
			foreach (PieceType chessPieceType in chessPieceBoxInfo.ChessPieceBox.Values)
			{
				for (int pieceCount = 0; pieceCount < chessPieceBoxInfo.ChessPieceBox[chessPieceType]; pieceCount++)
					chessPieceHandler.AddNewChessPiece(chessPieceType);
			}
		}

		SetupChessBoard();
		void SetupChessBoard()
		{
			var makerChessBoard = makerChessPuzzle.transform.GetComponentInChildren<ChessBoard_Maker>();
			makerChessBoard.UpdateChessBoard(chessBoardInfo);
			var makerChessSquares = makerChessBoard.transform.GetComponentsInChildren<ChessSquare_Maker>();
			for (int i = 0; i < chessBoardInfo.BoardStructure.Length; i++)
			{
				var loadedChessSquareInfo = chessBoardInfo.BoardStructure[i];
				makerChessSquares[i].UpdateChessSquare(chessBoardInfo, loadedChessSquareInfo);
			}
		}

		makerChessPuzzle.LinkPuzzleInfoWithChildren();

		yield return null;
	}


	public void DestroyChessPuzzleMaker()
	{
		if (transform.childCount > 0)
		{
			chessPuzzleSaver.PuzzleNameInputFiled.text = "";
			Destroy(transform.GetChild(0).gameObject);
		}
	}



}
