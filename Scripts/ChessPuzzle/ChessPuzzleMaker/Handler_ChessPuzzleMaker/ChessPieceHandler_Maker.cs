using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


[RequireComponent(typeof(ChessPieceCreator))]
public class ChessPieceHandler_Maker : MonoBehaviour
{
	[SerializeField] private ChessPieceCreator chessPieceCreator;
	[SerializeField] private PieceType holdingChessPiece = PieceType.None;

	private ChessPuzzle_Maker parentChessPuzzle;
	private Dictionary<PieceType, List<GameObject>> makerChessPieceContainer =  new Dictionary<PieceType, List<GameObject>>() { 
														{PieceType.King, new List<GameObject>() },
														{PieceType.Queen, new List<GameObject>() }, 
														{PieceType.Knight, new List<GameObject>() },																												   
														{PieceType.Bishop, new List<GameObject>() }, 
														{PieceType.Rook, new List<GameObject>()}, 
														{PieceType.Pawn, new List<GameObject>() } };
	private int currentChessPieceCount { get => transform.childCount; }
	public PieceType HoldingChessPiece { get => holdingChessPiece; set => holdingChessPiece = value; }


	private void Awake()
	{
		parentChessPuzzle = GetComponentInParent<ChessPuzzle_Maker>();
	}


	/// <summary>
	/// Add/Destroy ChessPiece On ChessPieceButton Pressed
	/// </summary>
	/// <param name="pieceType">pressed chesspiece button type</param>
	/// <param name="isIncrement">whether user add or remove chesspiece</param>
	private void OnChessPieceButtonPressed(PieceType pieceType, bool isIncrement)
	{
		if (isIncrement)
		{
			AddNewChessPiece(pieceType);
		}
		else
		{
			DestroyChessPiece(pieceType);
			AlignChessPieces();
		}
	}
	#region OnChessButtonPressed - EventHandler
	public void OnKingPieceButtonOn(bool isIncrement)
	{
		OnChessPieceButtonPressed(PieceType.King, isIncrement);
	}
	public void OnQueenPieceButtonOn(bool isIncrement)
	{
		OnChessPieceButtonPressed(PieceType.Queen, isIncrement);
	}
	public void OnBishopPieceButtonOn(bool isIncrement)
	{
		OnChessPieceButtonPressed(PieceType.Bishop, isIncrement);
	}
	public void OnKnightPieceButtonOn(bool isIncrement)
	{
		OnChessPieceButtonPressed(PieceType.Knight, isIncrement);
	}	
	public void OnRookPieceButtonOn(bool isIncrement)
	{
		OnChessPieceButtonPressed(PieceType.Rook, isIncrement);
	}	
	public void OnPawnPieceButtonOn(bool isIncrement)
	{
		OnChessPieceButtonPressed(PieceType.Pawn, isIncrement);
	}
	#endregion


	public void AddNewChessPiece(PieceType newPieceType)
	{
		if (currentChessPieceCount < ChessPieceBoxInfo.maxPieceCount)
		{
			var newChessPiece = chessPieceCreator.CreateChessPiece(newPieceType);
			newChessPiece.GetComponent<ChessPiece_Maker>().Initialize(newPieceType);

			makerChessPieceContainer[newPieceType].Add(newChessPiece);
			AlignChessPieces();
		}
	}

	private void DestroyChessPiece(PieceType targetPieceType)
	{
		var destroyableCandidates = makerChessPieceContainer[targetPieceType];
		if (destroyableCandidates.Count <= 0) return;

		bool isFreePieceExists = false;
		for (int i = destroyableCandidates.Count-1; i >= 0; i--)
		{
			var targetMakerChessPiece = destroyableCandidates[i];
			if (targetMakerChessPiece.TryGetComponent<ChessPiece_Maker>(out var makerChessPiece) 
				&& makerChessPiece.toppingChessSquare == null)
			{
				isFreePieceExists = true;

				destroyableCandidates.RemoveAt(i);
				Destroy(targetMakerChessPiece);
				break;
			}
		}

		if (! isFreePieceExists)
		{
			var destroyingChessPiece = destroyableCandidates[^1];

			parentChessPuzzle.DeOccupyChessSquareOf(destroyingChessPiece.GetComponent<ChessPiece_Maker>());
			destroyableCandidates.RemoveAt(destroyableCandidates.IndexOf(destroyingChessPiece));
			Destroy(destroyingChessPiece);
		}
	}


	private void AlignChessPieces()
	{
		int i = 0;
		foreach (var makerChessPiece in GetComponentsInChildren<ChessPiece_Maker>())
		{
			if (makerChessPiece.toppingChessSquare != null)
				continue;

			makerChessPiece.transform.localPosition = new Vector3(5 * i, 0, 0);
			i++;
		}
	}


}
