using UnityEngine;
using AirFishLab.ScrollingList.ContentManagement;
using System.Collections.Generic;


namespace AirFishLab.ScrollingList.Custom
{
	public class ChessPieceListBank : BaseListBank
	{
		[SerializeField] private List<PieceTypeData> _contents;
		public List<PieceTypeData> Contents { get => _contents; }


		public void AddChessPiece(PieceType pieceType)
		{
			_contents.Add(new PieceTypeData(pieceType));
		}

		public void ClearChessPiece()
		{
			_contents.Clear();
		}


		public override int GetContentCount()
		{
			return _contents.Count;
		}

		public override IListContent GetListContent(int index)
		{
			return _contents[index];
		}




		[System.Serializable]
		public class PieceTypeData : IListContent
		{
			[SerializeField] private PieceType pieceType;
			[SerializeField] private ChessPieceIndexPair chessPieceIndexPair;

			public PieceType PieceType { get => pieceType; }
			public ChessPieceIndexPair ChessPieceIndexPair { get => chessPieceIndexPair; }


			public PieceTypeData(PieceType _pieceType)
			{
				pieceType = _pieceType;
			}

			public void LinkTargetPiece(ChessPieceBase targetChessPiece, int pieceIndex)
			{
				chessPieceIndexPair.targetChessPiece = targetChessPiece;
				chessPieceIndexPair.pieceIndex = pieceIndex;
			}

		}


		[System.Serializable]
		public struct ChessPieceIndexPair
		{
			public ChessPieceBase targetChessPiece;
			public int pieceIndex;
		}


	}

}
