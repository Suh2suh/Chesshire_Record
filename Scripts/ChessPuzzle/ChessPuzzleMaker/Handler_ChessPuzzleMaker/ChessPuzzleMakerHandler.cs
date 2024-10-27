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


	public void DestroyChessPuzzleMaker()
	{
		if(transform.childCount > 0)
		{
			chessPuzzleSaver.PuzzleNameInputFiled.text = "";
			Destroy(transform.GetChild(0).gameObject);
		}
	}


	public IEnumerator CreateChessPuzzleMakerAsync(ChessPuzzleInfo chessPuzzleInfo)
	{
		var chessBoardInfo = chessPuzzleInfo.ChessBoardInfo;
		var chessPieceBoxInfo = chessPuzzleInfo.ChessPieceBoxInfo;

		GameObject chessPuzzleMaker = chessPuzzleMakerCreator.InstantiatePuzzleMaker(chessBoardInfo.BoardLength);
		var makerChessPuzzle = chessPuzzleMaker.GetComponent<ChessPuzzle_Maker>();
		var makerChessPieceBox = makerChessPuzzle.transform.GetComponentInChildren<ChessPieceBox_Maker>();
		var makerChessBoard = makerChessPuzzle.transform.GetComponentInChildren<ChessBoard_Maker>();
		var makerChessSquares = makerChessBoard.transform.GetComponentsInChildren<ChessSquare_Maker>();

		makerChessPieceBox.UpdateChessPieceBox(chessPieceBoxInfo);
		makerChessBoard.UpdateChessBoard(chessBoardInfo);
		for(int i = 0; i < chessBoardInfo.BoardStructure.Length; i++)
		{
			var loadedChessSquareInfo = chessBoardInfo.BoardStructure[i];
			makerChessSquares[i].UpdateChessSquare(chessBoardInfo, loadedChessSquareInfo);
		}

		makerChessPuzzle.AssignChessPuzzleInfoFromChildren();

		yield return null;
	}


}
