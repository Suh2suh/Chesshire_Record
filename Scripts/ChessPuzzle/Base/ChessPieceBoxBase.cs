using System;
using UnityEngine;


public abstract class ChessPieceBoxBase : MonoBehaviour, IChessPieceBox
{
	#region Setting Variables

	[SerializeField] protected Transform chessPieceBoxAnchor;
	[SerializeField] private GameObject chessPiecePrefab;
	[SerializeField]
	private UDictionary<PieceType, GameObject> modelPerPieceType = new()
	{
		{ PieceType.King, null},
		{ PieceType.Queen, null },
		{ PieceType.Bishop, null },
		{ PieceType.Knight, null },
		{ PieceType.Rook, null },
		{ PieceType.Pawn, null }
	};

	#endregion

	#region Private Variables
	protected PieceType holdingChessPiece = PieceType.None;

	#endregion

	#region Exposed Variables
	public ChessPieceBoxInfo ChessPieceBoxInfo { get; private set; }
	public PieceType HoldingChessPiece
	{
		get => holdingChessPiece;
		set
		{
			if (holdingChessPiece != value)
			{
				holdingChessPiece = value;
			}
		}
	}

	#endregion

	public Action<ChessPieceBase> OnChessPieceReturn;


	#region Initialization
	public void Initialize(ChessPieceBoxInfo chessPieceBoxInfo)
	{
		ChessPieceBoxInfo = chessPieceBoxInfo;

		CallbackInitialize();
	}

	#endregion
	/// <summary>
	/// Create Chess Pieces Here
	/// </summary>
	abstract protected void CallbackInitialize();


	#region Chess Piece Management

	protected ChessPieceBase CreateChessPiece(PieceType pieceType, Vector3 _localPosition)
	{
		var chessPieceObj = Instantiate(chessPiecePrefab, Vector3.zero, chessPieceBoxAnchor.rotation, this.transform);
		chessPieceObj.transform.localPosition = _localPosition;
		chessPieceObj.transform.localRotation = Quaternion.Euler(0, 180, 0);
		chessPieceObj.name = pieceType.ToString();

		var chessPiece = chessPieceObj.GetComponent<ChessPieceBase>();
		//chessPiece.Initialize(pieceType);
		//chessPiece.CreateChessPieceModel(modelPerPieceType[pieceType]);
		chessPiece.Initialize(pieceType, modelPerPieceType[pieceType]);

		return chessPiece;
	}

	protected void DestroyAllChessPiece()
	{
		for (int i = 0; i < transform.childCount; i++)
			Destroy(transform.GetChild(i));
	}

	#endregion



}