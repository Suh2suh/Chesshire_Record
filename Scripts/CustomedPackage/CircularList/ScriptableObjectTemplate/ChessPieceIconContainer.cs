using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ChessPieceIconContainer", menuName = "ScriptableObject/ChessPieceIconContainer", order = 1)]
public class ChessPieceIconContainer : ScriptableObject
{
	public Sprite this[PieceType pieceType]
	{
		get => iconPerChessPiece[pieceType];
	}


	[SerializeField]
	UDictionary<PieceType, Sprite> iconPerChessPiece;
}
