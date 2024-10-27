using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Chess Piece Manager
/// </summary>
public class ChessPieceBox_Tester : ChessPieceBoxBase
{
	#region Setting Variables
	[Space(15)]
	[SerializeField] private float gapBetweenChessPiece = 5.0f;

	#endregion


	protected override void CallbackInitialize()
	{
		CreateChessPieces();
		void CreateChessPieces()
		{
			CreateChessPiecesFrom(ChessPieceBoxInfo);
		}
	}


	#region ChessPiece Management

	private void CreateChessPiecesFrom(ChessPieceBoxInfo chessPieceBoxInfo)
	{
		foreach(var pieceTypeCountPair in chessPieceBoxInfo.ChessPieceBox)
		{
			int creatingPieceCount = pieceTypeCountPair.Value;
			if (creatingPieceCount <= 0)  continue;

			var creatingPieceType = pieceTypeCountPair.Key;

			CreateChessPiece(creatingPieceType, creatingPieceCount);
		}
	}
	private void CreateChessPiece(PieceType pieceType, int count)
	{
		for (int i = 0; i < count; i++)
		{
			var row = (transform.childCount - 1) / (ChessPieceBoxInfo.maxPieceCount / 2);
			var col = (transform.childCount - 1) % (ChessPieceBoxInfo.maxPieceCount / 2);

			CreateChessPiece(pieceType, new Vector3(row * gapBetweenChessPiece, 0, col * gapBetweenChessPiece));
		}
	}


	#endregion


}
