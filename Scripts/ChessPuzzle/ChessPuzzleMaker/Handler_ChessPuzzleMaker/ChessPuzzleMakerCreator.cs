using UnityEngine;


public class ChessPuzzleMakerCreator : MonoBehaviour
{

	[SerializeField] GameObject ChessPuzzleMaker4x4;
	[SerializeField] GameObject ChessPuzzleMaker6x6;
	[SerializeField] GameObject ChessPuzzleMaker8x8;
	[SerializeField] GameObject ChessPuzzleMaker10x10;


	public GameObject InstantiatePuzzleMaker(int boardLength)
	{
		switch (boardLength)
		{
			case 4:
				return Instantiate(ChessPuzzleMaker4x4, this.transform, false);

			case 6:
				return Instantiate(ChessPuzzleMaker6x6, this.transform, false);

			case 8:
				return Instantiate(ChessPuzzleMaker8x8, this.transform, false);

			case 10:
				return Instantiate(ChessPuzzleMaker10x10, this.transform, false);

			default:
				return null;
		}
	}


	public void Instantiate4x4PuzzleMaker()
	{
		Instantiate(ChessPuzzleMaker4x4, this.transform, false);
	}
	public void Instantiate6x6PuzzleMaker()
	{
		Instantiate(ChessPuzzleMaker6x6, this.transform, false);
	}
	public void Instantiate8x8PuzzleMaker()
	{
		Instantiate(ChessPuzzleMaker8x8, this.transform, false);
	}
	public void Instantiate10x10PuzzleMaker()
	{
		Instantiate(ChessPuzzleMaker10x10, this.transform, false);
	}


}
