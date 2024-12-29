using System.Collections;
using UnityEngine;


public class ChessPuzzle_Maker : MonoBehaviour
{
	[SerializeField] ChessPuzzleInfo chessPuzzleInfo;
	public ChessPuzzleInfo ChessPuzzleInfo { get => chessPuzzleInfo; }
	public ChessBoard_Maker childChessBoard { get; private set; }
	public ChessPieceBox_Maker childChessPieceBox { get; private set; }


	private void Awake()
	{
		childChessBoard = GetComponentInChildren<ChessBoard_Maker>();
		childChessPieceBox = GetComponentInChildren<ChessPieceBox_Maker>();

		LinkPuzzleInfoWithChildren();
	}

	public void LinkPuzzleInfoWithChildren()
	{
		chessPuzzleInfo.ChessBoardInfo = transform.GetComponentInChildren<ChessBoard_Maker>().ChessBoardInfo;
		chessPuzzleInfo.ChessPieceBoxInfo = transform.GetComponentInChildren<ChessPieceBox_Maker>().ChessPieceBoxInfo;
	}


	public void DeOccupyChessSquareOf(ChessPiece_Maker owner)
	{
		var occupyingGrids = ChessPieceGridCalculator.CalculateOccupiableChessGrids(owner.toppingChessSquare.ChessSquareInfo.Grid,
																					owner.ChessPieceType,
																					childChessBoard.ChessBoardInfo.BoardLength);
		var chessSquaresPerGrids = childChessBoard.GetChessSquaresOn(occupyingGrids);
		foreach (var chessSquare in chessSquaresPerGrids)
			chessSquare.OccupiedCount--;
	}




}