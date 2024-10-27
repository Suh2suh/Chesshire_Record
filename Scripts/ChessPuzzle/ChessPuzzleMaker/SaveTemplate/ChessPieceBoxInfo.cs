using UnityEngine;


[System.Serializable]
public class ChessPieceBoxInfo
{
	readonly public static int maxPieceCount = 10;
	public int placedPieceCount { get; set;  } = 0;

	[SerializeField]
	private UDictionary<PieceType, int> chessPieceBox = new()
	{
		{ PieceType.King, 0 },
		{ PieceType.Queen, 0 },
		{ PieceType.Bishop, 0 },
		{ PieceType.Knight, 0 },
		{ PieceType.Rook, 0 },
		{ PieceType.Pawn, 0 }
	};

	#region Property

	public UDictionary<PieceType, int> ChessPieceBox { get => chessPieceBox; private set => chessPieceBox = value; }

	#endregion


	public bool TrySetChessPieceCount(PieceType pieceType, int newPieceCount)
	{
		int previousPieceCount = chessPieceBox[pieceType];
		int pieceCountChange = newPieceCount - previousPieceCount;

		if (newPieceCount >= 0 && placedPieceCount + pieceCountChange <= maxPieceCount)
		{
			placedPieceCount += pieceCountChange;

			chessPieceBox[pieceType] = newPieceCount;
			return true;
		}
		
		return false;
	}


}
