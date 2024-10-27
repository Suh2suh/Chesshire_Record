using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayingChessPuzzleData", menuName = "ScriptableObject/PlayingChessPuzzleData", order = 1)]
public class PlayingChessPuzzleData : ScriptableObject
{
	[SerializeField]
	private ChessPuzzleBase playingChessPuzzle = null;
	public ChessPuzzleBase PlayingChessPuzzle
	{
		get => playingChessPuzzle;
		set => playingChessPuzzle = value;
	}

}
