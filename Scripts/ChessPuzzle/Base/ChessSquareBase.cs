using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


public abstract class ChessSquareBase : MonoBehaviour, IChessSquare
{
	[ReadOnly, SerializeField] private ChessSquareInfo chessSquareInfo;
	[ReadOnly, SerializeField] private List<ChessPieceBase> occupyingChessPieces;
	private ChessPieceBase toppedChessPiece;

	protected ChessBoardBase parentChessBoard;

	private Transform transparentGrid;
	protected Transform modelParent;
	private MeshRenderer modelMeshRenderer;

	protected Vector3 initialModelPos;
	private SquareType initialSquareType;

	#region Properties

	public ChessSquareInfo ChessSquareInfo { get => chessSquareInfo; private set => chessSquareInfo = value; }
	public List<ChessPieceBase> OccupyingChessPieces { get => occupyingChessPieces; protected set => occupyingChessPieces = value; }

	#endregion


	#region Initialization
	protected virtual void Awake()
	{
		modelParent = transform.Find("Model");
		transparentGrid = transform.Find("TransparentGrid");

		parentChessBoard = transform.GetComponentInParent<ChessBoardBase>();
	}


	public void Initialize(ChessSquareInfo chessSquareInfo, GameObject chessSquareModel, float initialChessBoardHeight)
	{
		ChessSquareInfo = chessSquareInfo;
		initialSquareType = chessSquareInfo.SquareType;
		occupyingChessPieces = new();

		CreateChessSquareModel(chessSquareModel, initialChessBoardHeight);

		parentChessBoard.ParentChessPuzzle.OnChessPieceHold += VacateChessSquareFrom;
	}

	private void CreateChessSquareModel(GameObject chessSquareModel, float initialChessSquareHeight)
	{
		var squareModelObj = Instantiate(chessSquareModel, modelParent.position, modelParent.rotation, modelParent);
		modelMeshRenderer = squareModelObj.GetComponentInChildren<MeshRenderer>();

		initialModelPos = new Vector3(0, initialChessSquareHeight, 0);
		modelParent.localPosition = initialModelPos;

		var worldSpaceModelBound = MeshHandler.GetWorldMeshBoundSize(chessSquareModel);
		SetChessSquareColliderSize(worldSpaceModelBound);
		SetTransparentGridSize(worldSpaceModelBound);
		SetOutlineGridSize(worldSpaceModelBound);

		CallbackInitialize();
	}
	private void SetChessSquareColliderSize(Vector3 worldSpaceModelBound)
	{
		var chessSquareCollider = transform.GetComponent<BoxCollider>();
		chessSquareCollider.size = new Vector3(worldSpaceModelBound.x, chessSquareCollider.size.y, worldSpaceModelBound.z);
	}
	private void SetTransparentGridSize(Vector3 worldSpaceModelBound)
	{
		transparentGrid.localScale = new Vector3(worldSpaceModelBound.x, 1, worldSpaceModelBound.z);
	}
	private void SetOutlineGridSize(Vector3 worldSpaceModelBound)
	{
		transform.Find("OutlineGrid").localScale = new Vector3(worldSpaceModelBound.x, 0.5f, worldSpaceModelBound.z);
	}


	private void OnDestroy()
	{
		parentChessBoard.ParentChessPuzzle.OnChessPieceHold -= VacateChessSquareFrom;
	}


	#endregion
	abstract protected void CallbackInitialize();


	#region OnTargetedByChessPiece
	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "ChessSquareTrigger")
		{
			AlertChessSquareTargeted(true);
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.name == "ChessSquareTrigger")
		{
			AlertChessSquareTargeted(false);
		}
	}


	// [Action: On Targeted by chesspiece / Occupied by chesspiece / On Vacate from chesspiece]
	private void AlertChessSquareTargeted(bool isTargetOn)
	{
		if (isTargetOn)
			parentChessBoard.TargetedChessSquare = this;
		else
		if (parentChessBoard.TargetedChessSquare == this)
			parentChessBoard.TargetedChessSquare = null;
	}

	#endregion

	#region OnOccupiedByChessPiece / OnVacateFromChessPiece

	/// <summary>  
	/// OnChessPieceRelease && CanPlaceOn  
	/// </summary>
	public void OccupyChessSquareBy(ChessPieceBase placedChessPiece)
	{
		if (!OccupyingChessPieces.Contains(placedChessPiece))
		{
			OccupyingChessPieces.Add(placedChessPiece);

			if (OccupyingChessPieces.Count == 1)
				OnChessSquareOccupied();
		}
	}

	/// <summary>  
	/// OnChesssPieceHold  
	/// </summary>
	public void VacateChessSquareFrom(ChessPieceBase holdedChessPiece)
	{
		if (OccupyingChessPieces.Contains(holdedChessPiece))
		{
			OccupyingChessPieces.Remove(holdedChessPiece);

			if (ChessSquareInfo.SquareType == SquareType.Piece && toppedChessPiece == holdedChessPiece)
				ChessSquareInfo.SquareType = initialSquareType;

			if (OccupyingChessPieces.Count == 0)
				OnChessSquareVacated();
				//parentChessBoard.GetComponent<ChessSquareCoroutineManager>().StartSquareMoveCoroutine(SmoothMoveChessSquareModel, moveDown: true);
		}
	}

	public void SetToppedChessPiece(ChessPieceBase toppedChessPiece)
	{
		this.toppedChessPiece = toppedChessPiece;
		ChessSquareInfo.SquareType = SquareType.Piece;
	}


	#endregion
	abstract protected void OnChessSquareOccupied();
	abstract protected void OnChessSquareVacated();


	#region Model Utility
	public void ChangeModelColor(Color newColor)
	{
		LitMaterialHandler.ChangeColor(modelMeshRenderer, newColor);
	}

	public void EmitModel(bool isOn)
	{
		LitMaterialHandler.Emit(modelMeshRenderer, Color.white, isOn);
	}

	#endregion

	#region TransparentGrid Management
	public void ActivateTransparentGrid(bool activateStatus)
	{
		if (transparentGrid.gameObject.activeSelf != activateStatus)
			transparentGrid.gameObject.SetActive(activateStatus);
	}

	#endregion


	#region Descripted

	/*
	private void SetLineRendererPos(Vector3 worldSpaceModelBound)
	{
		var lineRenderer = transform.GetComponent<LineRenderer>();
		for (int i = 0; i < lineRenderer.positionCount; i++)
		{
			lineRenderer.SetPosition(i, lineRenderer.GetPosition(i) * worldSpaceModelBound.z);
		}
	}*/


	#endregion


}