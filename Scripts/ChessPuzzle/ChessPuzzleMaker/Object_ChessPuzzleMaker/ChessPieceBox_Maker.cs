using UnityEngine;
using TMPro;


public class ChessPieceBox_Maker : MonoBehaviour
{
	// TODO: InEditable
	[SerializeField] private ChessPieceBoxInfo chessPieceBoxInfo;
	[SerializeField] private UDictionary<PieceType, TextMeshProUGUI> countTextPerPieceType = new()
	{
		{ PieceType.King, null },
		{ PieceType.Queen, null },
		{ PieceType.Bishop, null },
		{ PieceType.Knight, null },
		{ PieceType.Rook, null },
		{ PieceType.Pawn, null },
	};
	public ChessPieceBoxInfo ChessPieceBoxInfo { get => chessPieceBoxInfo; set => chessPieceBoxInfo = value; }


	public void UpdateChessPieceBox(ChessPieceBoxInfo newChessPieceBoxInfo)
	{
		chessPieceBoxInfo = newChessPieceBoxInfo;

		foreach (var pieceType in ChessPieceBoxInfo.ChessPieceBox.Keys)
			RefreshChessPieceCount(pieceType);
	}


	private void AdjustChessPieceCountByOne(PieceType pieceType, bool shouldIncrement)
	{
		int newPieceCount = ChessPieceBoxInfo.ChessPieceBox[pieceType] + (shouldIncrement ? 1 : -1);
		if (chessPieceBoxInfo.TrySetChessPieceCount(pieceType, newPieceCount))
			RefreshChessPieceCount(pieceType);
	}

	private void RefreshChessPieceCount(PieceType pieceType)
	{
		countTextPerPieceType[pieceType].text = chessPieceBoxInfo.ChessPieceBox[pieceType].ToString();
	}


	#region Chess Button Event Listener

	public void AdjustKingPieceCount(bool shouldIncrement)
	{
		AdjustChessPieceCountByOne(PieceType.King, shouldIncrement);
	}
	public void AdjustQueenPieceCount(bool shouldIncrement)
	{
		AdjustChessPieceCountByOne(PieceType.Queen, shouldIncrement);
	}
	public void AdjustBishopPieceCount(bool shouldIncrement)
	{
		AdjustChessPieceCountByOne(PieceType.Bishop, shouldIncrement);
	}
	public void AdjustKnightPieceCount(bool shouldIncrement)
	{
		AdjustChessPieceCountByOne(PieceType.Knight, shouldIncrement);
	}
	public void AdjustRookPieceCount(bool shouldIncrement)
	{
		AdjustChessPieceCountByOne(PieceType.Rook, shouldIncrement);
	}
	public void AdjustPawnPieceCount(bool shouldIncrement)
	{
		AdjustChessPieceCountByOne(PieceType.Pawn, shouldIncrement);
	}


	#endregion


}
