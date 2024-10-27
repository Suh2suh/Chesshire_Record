using UnityEngine;


public class ChessPiece_Maker : ChessPieceBase
{
	public ChessSquare_Maker toppingChessSquare = null;

	#region private variables
	private ChessPieceHandler chessPieceHandler;
	//private LineRenderer lineRenderer;

	protected override bool IsChessPieceInteractable
	{
		get => true;
	}

	#endregion


	#region Initialization
	protected override void Awake()
    {
		base.Awake();

        chessPieceHandler = transform.parent.GetComponentInParent<ChessPieceHandler>();
        //lineRenderer = transform.parent.GetComponentInParent<LineRenderer>();
        chessBoardPlane = new Plane(Vector3.up, new Vector3(0,0,0));   // 체스 보드판 Plane
    }

	#endregion


	protected override void OnChessPieceHold()
	{
		chessPieceHandler.HoldingChessPiece = ChessPieceType;
	}
	protected override void OnChessPieceRelease()
	{
		chessPieceHandler.HoldingChessPiece = PieceType.None;
	}

	protected override void OnChessPieceReturn()
	{
		//
	}



	#region Emission On Hover
#if UNITY_STANDALONE

	private void OnMouseOver()
	{
		if (!isHeld && chessPieceHandler.HoldingChessPiece == PieceType.None)
			LitMaterialHandler.Emit(modelMeshRenderer, Color.white, true);
	}

	private void OnMouseExit()
	{
		LitMaterialHandler.Emit(modelMeshRenderer, Color.white, false);
	}


#endif
	#endregion

	#region Chess Square Interaction

	private void OnTriggerEnter(Collider other)
	{
		if(other.TryGetComponent<ChessSquare_Maker>(out var makerChessSquare))
			toppingChessSquare = makerChessSquare;

	}
	private void OnTriggerStay(Collider other)
	{
		if (other.TryGetComponent<ChessSquare_Maker>(out var makerChessSquare) && toppingChessSquare != makerChessSquare)
			toppingChessSquare = makerChessSquare;
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.TryGetComponent<ChessSquare_Maker>(out var makerChessSquare))
			toppingChessSquare = null;
	}


	#endregion


}
