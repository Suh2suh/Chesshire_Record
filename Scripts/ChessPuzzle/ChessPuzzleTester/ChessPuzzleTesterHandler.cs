using System.Collections;
using UnityEngine;


[RequireComponent(typeof(ChessPuzzleCreator), typeof(ChessBoardCameraMover))]
public class ChessPuzzleTesterHandler : MonoBehaviour
{
	[SerializeField] ChessPuzzleCreator chessPuzzleCreator;
	[SerializeField] ChessBoardCameraMover chessBoardCameraMover;

	public void LoadPuzzle(string loadingPuzzleName)
	{
		var chessPuzzle = chessPuzzleCreator.CreateChessPuzzle(loadingPuzzleName);
	}
}