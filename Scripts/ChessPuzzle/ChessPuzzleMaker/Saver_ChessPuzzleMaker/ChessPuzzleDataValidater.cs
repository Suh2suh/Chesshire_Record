

public static class ChessPuzzleDataValidater
{

	public static bool IsEntranceExitValid(ChessBoardInfo chessBoardInfo)
	{
		bool isEntranceValid = (chessBoardInfo.EntranceGrid.Grid.x >= 0 && chessBoardInfo.EntranceGrid.Grid.y >= 0);
		bool isExitValid = (chessBoardInfo.ExitGrid.Grid.x >= 0 && chessBoardInfo.ExitGrid.Grid.y >= 0);

		return (isEntranceValid && isExitValid);
	}


	public static bool IsChessPieceExist(ChessPieceBoxInfo chessPieceBoxInfo)
	{
		return (chessPieceBoxInfo.placedPieceCount > 0);
	}


}

