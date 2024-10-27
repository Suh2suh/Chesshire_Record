
using System.Collections.Generic;
using UnityEngine;

public static class ChessPieceGridCalculator
{

	public static List<Vector2Int> CalculateOccupiableChessGrids(Vector2Int targetedGrid, PieceType placingChessPiece, int chessBoardLength)
	{
		if ((targetedGrid.x < 0 || targetedGrid.y < 0) || chessBoardLength < 1)
			return null;

		switch (placingChessPiece)
		{
			case PieceType.King:
				return CalculateAsKing(targetedGrid, chessBoardLength);

			case PieceType.Queen:
				return CalculateAsQueen(targetedGrid, chessBoardLength);

			case PieceType.Bishop:
				return CalculateAsBishop(targetedGrid, chessBoardLength);

			case PieceType.Knight:
				return CalculateAsKnight(targetedGrid, chessBoardLength);

			case PieceType.Rook:
				return CalculateAsRook(targetedGrid, chessBoardLength);

			case PieceType.Pawn:
				return CalculateAsPawn(targetedGrid, chessBoardLength);

			case PieceType.None:
				return null;

			default:
				return null;
		}
	}


	private static List<Vector2Int> CalculateAsKing(Vector2Int targetedGrid, int chessBoardLength)
	{
		var calculatedGrids = new List<Vector2Int>
		{
			targetedGrid,
			targetedGrid + Vector2Int.up,
			targetedGrid + new Vector2Int(1, 1),
			targetedGrid + Vector2Int.right,
			targetedGrid + new Vector2Int(1, -1),
			targetedGrid + Vector2Int.down,
			targetedGrid + new Vector2Int(-1, -1),
			targetedGrid + Vector2Int.left,
			targetedGrid + new Vector2Int(-1, 1)
		};

		return calculatedGrids;
	}
	private static List<Vector2Int> CalculateAsQueen(Vector2Int targetedGrid, int chessBoardLength)
	{
		var calculatedGrids = new List<Vector2Int>
		{
			targetedGrid
		};
		calculatedGrids.AddRange(CalculateCrossedGridsFrom(targetedGrid, chessBoardLength));
		calculatedGrids.AddRange(CalculateDiagnolGridsFrom(targetedGrid, chessBoardLength));

		return calculatedGrids;
	}
	private static List<Vector2Int> CalculateAsBishop(Vector2Int targetedGrid, int chessBoardLength)
	{
		var calculatedGrids = new List<Vector2Int>
		{
			targetedGrid
		};
		calculatedGrids.AddRange(CalculateDiagnolGridsFrom(targetedGrid, chessBoardLength));

		return calculatedGrids;
	}
	private static List<Vector2Int> CalculateAsKnight(Vector2Int targetedGrid, int chessBoardLength)
	{
		var calculatedGrids = new List<Vector2Int>
		{
			targetedGrid,
			targetedGrid + Vector2Int.right + new Vector2Int(1, 1),
			targetedGrid + Vector2Int.right + new Vector2Int(1, -1),
			targetedGrid + Vector2Int.down + new Vector2Int(1, -1),
			targetedGrid + Vector2Int.down + new Vector2Int(-1, -1),
			targetedGrid + Vector2Int.left + new Vector2Int(-1, 1),
			targetedGrid + Vector2Int.left + new Vector2Int(-1, -1),
			targetedGrid + Vector2Int.up + new Vector2Int(1, 1),
			targetedGrid + Vector2Int.up + new Vector2Int(-1, 1)
		};

		return calculatedGrids;
	}
	private static List<Vector2Int> CalculateAsRook(Vector2Int targetedGrid, int chessBoardLength)
	{
		var calculatedGrids = new List<Vector2Int>
		{
			targetedGrid
		};
		calculatedGrids.AddRange(CalculateCrossedGridsFrom(targetedGrid, chessBoardLength));

		return calculatedGrids;
	}
	private static List<Vector2Int> CalculateAsPawn(Vector2Int targetedGrid, int chessBoardLength)
	{
		var calculatedGrids = new List<Vector2Int>
		{
			targetedGrid,
			targetedGrid + new Vector2Int(-1, 1),
			targetedGrid + new Vector2Int(1, 1),

			// Custom Added: leftBottom, rightBottom
			targetedGrid + new Vector2Int(-1, -1),
			targetedGrid + new Vector2Int(1, -1)
		};

		return calculatedGrids;
	}


	private static List<Vector2Int> CalculateCrossedGridsFrom(Vector2Int targetedGrid, int chessBoardLength)
	{
		var calculatedGrids = new List<Vector2Int>();

		for (int i = 0; i < targetedGrid.x; i++)
			calculatedGrids.Add(new Vector2Int(i, targetedGrid.y));

		for (int i = targetedGrid.x + 1; i < chessBoardLength; i++)
			calculatedGrids.Add(new Vector2Int(i, targetedGrid.y));

		for (int i = 0; i < targetedGrid.y; i++)
			calculatedGrids.Add(new Vector2Int(targetedGrid.x, i));

		for (int i = targetedGrid.y + 1; i < chessBoardLength; i++)
			calculatedGrids.Add(new Vector2Int(targetedGrid.x, i));

		return calculatedGrids;
	}
	private static List<Vector2Int> CalculateDiagnolGridsFrom(Vector2Int targetedGrid, int chessBoardLength)
	{
		var calculatedGrids = new List<Vector2Int>();

		for (int i = 1; i <= Mathf.Min(targetedGrid.x, targetedGrid.y); i++)
			calculatedGrids.Add(targetedGrid + (i * new Vector2Int(-1, -1)));

		for (int i = 1; i < Mathf.Min(chessBoardLength - targetedGrid.x, chessBoardLength - targetedGrid.y); i++)
			calculatedGrids.Add(targetedGrid + (i * new Vector2Int(1, 1)));

		for (int i = 1; i <= Mathf.Min(targetedGrid.x, chessBoardLength - targetedGrid.y); i++)
			calculatedGrids.Add(targetedGrid + (i * new Vector2Int(-1, 1)));

		for (int i = 1; i <= Mathf.Min(chessBoardLength - targetedGrid.x, targetedGrid.y); i++)
			calculatedGrids.Add(targetedGrid + (i * new Vector2Int(1, -1)));

		return calculatedGrids;
	}


}