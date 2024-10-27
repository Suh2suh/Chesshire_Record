using System.Collections.Generic;
using UnityEngine;


public class ChessBoard_Maker : MonoBehaviour
{
	[SerializeField] private ChessBoardInfo chessBoardInfo;
	public ChessBoardInfo ChessBoardInfo { get => chessBoardInfo; }


	private Dictionary<Vector2Int, ChessSquare_Maker> gridChessSquarePair = new();
	public Dictionary<Vector2Int, ChessSquare_Maker> GridChessSquarePair { get => gridChessSquarePair; }
	

	private void Awake()
	{
		InitializeChessSquares();
	}

	[ContextMenu("Initialize Chess Squares")]
	public void InitializeChessSquares()
	{
		var makerChessSquares = GetComponentsInChildren<ChessSquare_Maker>();
		var chessSquareInfos = new List<ChessSquareInfo>();
		foreach (var makerChessSquare in makerChessSquares)
		{
			makerChessSquare.AllignChessSquareUnderBoard();
			chessSquareInfos.Add(makerChessSquare.ChessSquareInfo);

			gridChessSquarePair[makerChessSquare.ChessSquareInfo.Grid] = makerChessSquare;
		}
		chessBoardInfo.BoardStructure = chessSquareInfos.ToArray();
	}

	public void UpdateChessBoard(ChessBoardInfo newChessBoardInfo)
	{
		chessBoardInfo = newChessBoardInfo;

		var makerChessSquares = GetComponentsInChildren<ChessSquare_Maker>();
		foreach (var makerChessSquare in makerChessSquares)
		{
			gridChessSquarePair[makerChessSquare.ChessSquareInfo.Grid] = makerChessSquare;
		}
	}


	public List<ChessSquare_Maker> GetChessSquaresOn(List<Vector2Int> grids)
	{
		var chessSquares = new List<ChessSquare_Maker>();

		foreach(var grid in grids)
		{
			if (TryGetChessSquareOn(grid, out var chessSquare))
				chessSquares.Add(chessSquare);
		}

		return chessSquares;
	}
	public bool TryGetChessSquareOn(Vector2Int grid, out ChessSquare_Maker chessSquare)
	{
		if(isGirdInChessBoard(grid))
		{
			chessSquare = gridChessSquarePair[grid];
			return true;
		}
		else
		{
			chessSquare = null;
			return false;
		}
	}


	public bool TrySetChessBoardEntranceAs(ChessSquareInfo chessSquare)
	{
		if (isGirdInChessBoard(chessSquare.Grid))
		{
			chessBoardInfo.EntranceGrid.Grid = chessSquare.Grid;
			return true;
		}

		return false;
	}
	public bool TrySetChessBoardExitAs(ChessSquareInfo chessSquare)
	{
		if (isGirdInChessBoard(chessSquare.Grid))
		{
			chessBoardInfo.ExitGrid.Grid = chessSquare.Grid;
			return true;
		}

		return false;
	}

	public bool IsChessBoardEntrance(ChessSquareInfo chessSquare)
	{
		return chessBoardInfo.EntranceGrid.Grid == chessSquare.Grid;
	}
	public bool IsChessBoardExit(ChessSquareInfo chessSquare)
	{
		return chessBoardInfo.ExitGrid.Grid == chessSquare.Grid;
	}

	public void ResetChessBoardEntrance()
	{
		chessBoardInfo.EntranceGrid.Grid = new Vector2Int(-1, -1);
	}
	public void ResetChessBoardExit()
	{
		chessBoardInfo.ExitGrid.Grid = new Vector2Int(-1, -1);
	}


	bool isGirdInChessBoard(Vector2Int grid)
	{
		return (grid.x >= 0 && grid.x < chessBoardInfo.BoardLength) && (grid.y >= 0 && grid.y < chessBoardInfo.BoardLength);
	}


}
