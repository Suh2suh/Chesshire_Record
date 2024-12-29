using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public class ChessPiece_GamePlay : ChessPieceBase
{
	#region Private Variables
	private ChessPieceBoxBase parentChessPieceBox;
	private ChessPuzzleBase parentChessPuzzle;

	#endregion

	protected override bool IsChessPieceInteractable
	{
		get => parentChessPuzzle.IsChessInteractable;
	}


	#region Initialization
	protected override void Awake()
	{
		base.Awake();

		parentChessPuzzle = transform.GetComponentInParent<ChessPuzzleBase>();
		parentChessPieceBox = transform.GetComponentInParent<ChessPieceBoxBase>();
		chessBoardPlane = new Plane(Vector3.up, parentChessPuzzle.transform.position);
	}


	#endregion


	/// For ChessPiecePanel (Teeth)
	#region [Action: Force Chess Control]
	public void ForceHoldChessPiece()
	{
		HoldChessPiece();
	}
	public void ForceReleaseChessPiece()
	{
		ReleaseChessPiece();
	}
	public void ForceMoveChessPiece()
	{
		DragChessPiece();
	}


	#endregion

	#region [Action: Hold / Release]
	protected override void OnChessPieceHold()
	{
		AlertChessPieceHold();

		LitMaterialHandler.Emit(modelMeshRenderer, Color.white, false);
		modelParent.transform.localPosition = new Vector3(0, 5, 0);
	}
	protected override void OnChessPieceRelease()
	{
		modelParent.transform.localPosition = new Vector3(0, 0, 0);

		AlertChessPieceRelease();
	}

	private void AlertChessPieceHold()
	{
		parentChessPieceBox.HoldingChessPiece = ChessPieceType;
		parentChessPuzzle.OnChessPieceHold(this);
	}
	private void AlertChessPieceRelease()
	{
		parentChessPieceBox.HoldingChessPiece = PieceType.None;
		parentChessPuzzle.OnChessPieceRelease(this);
	}


	#endregion

	protected override void OnChessPieceReturn()
	{
		parentChessPieceBox.OnChessPieceReturn?.Invoke(this);
	}


	// PC(mouse), Switch(crosshair) Enviroment Only
	#region [Action: Emission On PointerEnter]
#if UNITY_STANDALONE

	private void OnMouseOver()
	{
		if (parentChessPuzzle.IsChessInteractable)
			if (!isHeld && parentChessPieceBox.HoldingChessPiece == PieceType.None)
				LitMaterialHandler.Emit(modelMeshRenderer, Color.white, true);
	}
	private void OnMouseExit()
	{
		LitMaterialHandler.Emit(modelMeshRenderer, Color.white, false);
	}


#endif
	#endregion

}
