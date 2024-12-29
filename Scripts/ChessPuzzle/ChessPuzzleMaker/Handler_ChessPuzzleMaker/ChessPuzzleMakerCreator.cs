using System;
using UnityEngine;


public class ChessPuzzleMakerCreator : MonoBehaviour
{
	[SerializeField]
	private UDictionary<BoardLength, GameObject> chessPuzzlePerBoardLength;

	public GameObject InstantiatePuzzleMaker(int boardLength)
	{
		if(Enum.IsDefined(typeof(BoardLength), boardLength))
		{
			var chessPuzzleMaker = chessPuzzlePerBoardLength[(BoardLength)boardLength];
			return Instantiate(chessPuzzleMaker, this.transform, false);
		}
		else
		{
			Debug.LogError("[Error][ChessBoard Width Selector] there is no matching puzzle with length " + boardLength + "!");
			return null;
		}
	}
}
