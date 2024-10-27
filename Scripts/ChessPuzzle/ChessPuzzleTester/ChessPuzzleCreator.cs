using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPuzzleCreator : MonoBehaviour
{
	[SerializeField] GameObject ChessPuzzlePrefab;

	public ChessPuzzleBase CreateChessPuzzle(string puzzleName)
	{
		var chessPuzzleObj = Instantiate(ChessPuzzlePrefab, Vector3.zero, Quaternion.identity, this.transform);
		var chessPuzzle = chessPuzzleObj.GetComponent<ChessPuzzleBase>();
		chessPuzzle.LoadNewChessPuzzle(puzzleName);

		return chessPuzzle;
	}
}
