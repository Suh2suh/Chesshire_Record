using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ChessPuzzleInfo
{
	[SerializeField] private ChessBoardInfo chessBoardInfo;
	[SerializeField] private ChessPieceBoxInfo chessPieceBoxInfo;

	#region Property
	public ChessBoardInfo ChessBoardInfo { get => chessBoardInfo; set => chessBoardInfo = value; }
	public ChessPieceBoxInfo ChessPieceBoxInfo { get => chessPieceBoxInfo; set => chessPieceBoxInfo = value; }

	#endregion


}
