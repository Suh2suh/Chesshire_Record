using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPieceCreator : MonoBehaviour
{
    [SerializeField] GameObject makerChessPiecePrefab;
    [SerializeField] private UDictionary<PieceType, GameObject> modelPerPieceType = new()
                     {
                         { PieceType.King, null },   { PieceType.Queen, null },
                         { PieceType.Bishop, null }, { PieceType.Knight, null },
                         { PieceType.Rook, null },   { PieceType.Pawn, null }
                     };


    public GameObject CreateChessPiece(PieceType pieceType)
	{
        var makerChessPiece = Instantiate(makerChessPiecePrefab, this.transform, false);
        makerChessPiece.GetComponent<ChessPiece_Maker>().CreateChessPieceModel(modelPerPieceType[pieceType]);

        return makerChessPiece;
    }


}
