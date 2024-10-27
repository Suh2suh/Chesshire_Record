using UnityEngine;


public abstract class ChessPieceBase : MonoBehaviour, IChessPiece
{
	public PieceType ChessPieceType { get; protected set; }

	#region Private Variables
	protected bool isHeld = false;
	protected bool isOnSquare = false;
	protected Plane chessBoardPlane = new Plane(Vector3.up, new Vector3(0, 0, 0));

	protected Transform modelParent;
	protected MeshRenderer modelMeshRenderer;

	protected Vector3 initialChessPiecePos;

	#endregion

	#region Exposed Variables
	public bool IsOnSquare { get => isOnSquare; }

	#endregion

	protected abstract bool IsChessPieceInteractable { get; }


	#region Initialization

	protected virtual void Awake()
	{
		modelParent = transform.Find("Model");
	}


	public void Initialize(PieceType pieceType)
	{
		ChessPieceType = pieceType;
		initialChessPiecePos = this.transform.position;
	}
	public void Initialize(PieceType pieceType, GameObject chessPieceModel)
	{
		ChessPieceType = pieceType;
		initialChessPiecePos = this.transform.position;

		CreateChessPieceModel(chessPieceModel);
	}

	public void CreateChessPieceModel(GameObject chessPieceModel)
	{
		modelParent = modelParent ?? transform.Find("Model");
		var chessPieceModelObj = Instantiate(chessPieceModel, modelParent.position, modelParent.rotation, modelParent);
		modelMeshRenderer = chessPieceModelObj.GetComponentInChildren<MeshRenderer>();

		var worldSpaceModelBound = MeshHandler.GetWorldMeshBound(chessPieceModel);
		SetChessPieceCollider(worldSpaceModelBound);
	}
	private void SetChessPieceCollider(Bounds worldSpaceModelBound)
	{
		var chessSquareCollider = transform.GetComponent<BoxCollider>();
		chessSquareCollider.center = worldSpaceModelBound.center;
		chessSquareCollider.size = worldSpaceModelBound.size;

		var squareTriggerCollider = transform.Find("ChessSquareTrigger").GetComponent<BoxCollider>();
		squareTriggerCollider.size *= chessSquareCollider.size.x;

	}


	#endregion


	#region Unity Events: Input
#if UNITY_STANDALONE

	private void OnMouseDown()
	{
		HoldChessPiece();
	}
	private void OnMouseUp()
	{
		ReleaseChessPiece();
	}
	private void OnMouseDrag()
	{
		DragChessPiece();
	}

#endif
	#endregion

	#region [Action: Hold / Release]

	protected void HoldChessPiece()
	{
		if (IsChessPieceInteractable)
		{
			isHeld = true;
			OnChessPieceHold();
		}
	}
	protected void ReleaseChessPiece()
	{
		if (IsChessPieceInteractable)
		{
			OnChessPieceRelease();
			isHeld = false;
		}
	}


	#endregion
	protected abstract void OnChessPieceHold();
	protected abstract void OnChessPieceRelease();

	#region [Action: Moving / Placing]

	protected void DragChessPiece()
	{
		if (IsChessPieceInteractable)
		{
			MoveChessPieceAlongMouse();
		}
	}

	protected void MoveChessPieceAlongMouse()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float distance;

		if (chessBoardPlane.Raycast(ray, out distance))
		{
			MoveChessPieceOnPlane(ray, distance);
		}
	}
	protected virtual void MoveChessPieceOnPlane(Ray ray, float distance)
	{
		Vector3 mouseWorldPosition = ray.GetPoint(distance);
		transform.position = mouseWorldPosition;
	}


	public void PlaceChessPieceOn(Vector3 chessSquarePos)
	{
		isOnSquare = true;
		transform.position = chessSquarePos;
	}
	public void ReturnChessPiece()
	{
		isOnSquare = false;
		transform.position = initialChessPiecePos;

		OnChessPieceReturn();
	}


	#endregion
	protected abstract void OnChessPieceReturn();


}
