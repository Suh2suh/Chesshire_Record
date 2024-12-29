using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


public class ChessPieceBox_GamePlay : ChessPieceBoxBase
{
	[SerializeField, ReadOnly] private ChessPieceBase spawnedChessPiece = null;

	#region Private Variables
	private ObjectPool<ChessPieceBase> chessPiecePool;
	private Dictionary<int, ChessPieceBase> chessPieceIndexPair = new();

	#endregion


	#region Unity Methods: Link Events

	private void Awake()
	{
		OnChessPieceReturn += DestroyChessPiece;
	}
	private void OnDestroy()
	{
		OnChessPieceReturn -= DestroyChessPiece;
	}


	#endregion


	protected override void CallbackInitialize()
	{
		List<ChessPieceBase> chessPieces = new();

		int index = 0;
		foreach(var pieceType in ChessPieceBoxInfo.ChessPieceBox.Keys)
		{
			for(int i = 0; i < ChessPieceBoxInfo.ChessPieceBox[pieceType]; i++)
			{
				var spawnedPiece = CreateChessPiece(pieceType, Vector3.zero);
				spawnedPiece.gameObject.SetActive(false);
				chessPieces.Add(spawnedPiece);

				chessPieceIndexPair.Add(index, spawnedPiece);
				index++;
			}
		}

		chessPiecePool = new ObjectPool<ChessPieceBase>(chessPieces);
	}


	public void CreateChessPiece(int chessPieceIndex)
	{
		if (spawnedChessPiece != null && spawnedChessPiece.IsOnSquare == false)
		{
			chessPiecePool.ReturnObject(spawnedChessPiece);
		}

		spawnedChessPiece = chessPiecePool.GetObject(chessPieceIndex);
	}
	public void DestroyChessPiece(ChessPieceBase chessPiece)
	{
		chessPiecePool.ReturnObject(chessPiece);
	}


}