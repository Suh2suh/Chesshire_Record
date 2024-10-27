
public static class ChessPuzzleLoader
{
	public static ChessPuzzleInfo LoadChessPuzzle(string puzzleName)
	{
		string dataPath = ChessPuzzleDataIO.GetPuzzleDataFilePathWith(puzzleName);

		ChessPuzzleInfo chessPuzzleData = DataIO.ReadDataObject<ChessPuzzleInfo>(dataPath);
		return chessPuzzleData;
	}

}

