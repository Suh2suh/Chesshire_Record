using System.Collections;
using UnityEngine;


[RequireComponent(typeof(ChessPuzzleCreator))]
public class ChessPuzzleTesterHandler : MonoBehaviour
{
	[SerializeField] ChessPuzzleCreator chessPuzzleCreator;

	public void LoadPuzzle(string loadingPuzzleName)
	{
		var chessPuzzle = chessPuzzleCreator.CreateChessPuzzle(loadingPuzzleName);
	}
}