using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class ChessPuzzleDataIO
{

	public static string GetChessPuzzleDirectory()
	{
#if UNITY_STANDALONE
#if UNITY_STANDALONE_WIN
		return Path.Combine(Application.persistentDataPath, "GameData", "ChessPuzzle");
#endif
#endif
	}

	public static string GetPuzzleDataFilePathWith(string puzzleName)
	{
		return Path.Combine(GetChessPuzzleDirectory(), puzzleName + ".json");
	}


	public static bool IsFilePathAlreadyExist(string puzzleName)
	{
		return File.Exists(GetPuzzleDataFilePathWith(puzzleName));
	}



}