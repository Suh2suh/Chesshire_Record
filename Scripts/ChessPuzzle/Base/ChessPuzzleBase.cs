
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ChessPuzzleBase : MonoBehaviour, IChessPuzzle
{
	#region Setting variables
	[SerializeField] protected string loadingPuzzleName;
	[SerializeField] protected bool LoadPuzzleOnAwake = false;
	[SerializeField] protected PlayingChessPuzzleData playingChessPuzzleData;

	[Space(15), Header("No need to assign")]
	[SerializeField, HideInInspector] protected ChessBoardBase chessBoard;
	[SerializeField, HideInInspector] protected ChessPieceBoxBase chessPieceBox;

	#endregion

	#region Exposed variables
	public ChessPuzzleInfo ChessPuzzleInfo { get; private set; }
	public ChessBoardBase ChessBoard { get => chessBoard; }
	public ChessPieceBoxBase ChessPieceBox { get => chessPieceBox; }
	public bool IsChessInteractable { get; set; } = true;

	public Action<ChessPieceBase> OnChessPieceHold;
	public Action<ChessPieceBase> OnChessPieceRelease;
	public event Action<bool> OnPathFindingFinished;

	#endregion

	protected Vector3 entranceBridgePos;
	protected Vector3 exitBridgePos;


	#region private variables
	private bool isFindingPath = false;
	private List<Node> passableChessPuzzlePath;

	private event Action OnChessPiecePlaced;

	#endregion


	#region Unity Methods: Link Variables & Events
	protected virtual void Awake()
	{
		chessBoard = chessBoard ?? GetComponentInChildren<ChessBoardBase>();
		chessPieceBox = chessPieceBox ?? GetComponentInChildren<ChessPieceBoxBase>();

		// 고칠 점: 현재 Play 중인 체스 아래에서만
		// ChessPuzzleLinker을 해서 해당 스크립트가 대신 Linking해주면?
		OnChessPieceRelease += TryPlaceChessPieceOnChessSquare;
		OnChessPiecePlaced += AlertOnPathFindingFinished;   // 길 가로막았을 때 대비
		OnChessPieceHold += AlertOnPathFindingFinished;

		PlayerController.OnPlayerMoveStart += SetChessPuzzleNonInteractive;
		PlayerController.OnPlayerMoveEnd += SetChessPuzzleInteractive;
	}

	private void Start()
	{
		if (LoadPuzzleOnAwake && loadingPuzzleName.Length > 0 && chessBoard && chessPieceBox)
			LoadNewChessPuzzle();
	}

	protected virtual void OnDestroy()
	{
		OnChessPieceRelease -= TryPlaceChessPieceOnChessSquare;
		OnChessPiecePlaced -= AlertOnPathFindingFinished;
		OnChessPieceHold -= AlertOnPathFindingFinished;

		PlayerController.OnPlayerMoveStart -= SetChessPuzzleNonInteractive;
		PlayerController.OnPlayerMoveEnd -= SetChessPuzzleInteractive;
	}

	#endregion

	#region Initialization

	public void LoadNewChessPuzzle(string _loadingPuzzleName)
	{
		loadingPuzzleName = _loadingPuzzleName;
		LoadNewChessPuzzle();
	}
	protected void LoadNewChessPuzzle()
	{
		var loadedChessPuzzle = ChessPuzzleLoader.LoadChessPuzzle(loadingPuzzleName);
		ChessPuzzleInfo = loadedChessPuzzle;
		
		if(playingChessPuzzleData)
			playingChessPuzzleData.PlayingChessPuzzle = this;

		chessBoard?.Initialize(loadedChessPuzzle.ChessBoardInfo);
		chessPieceBox?.Initialize(loadedChessPuzzle.ChessPieceBoxInfo);

		InitializeBridges();

		CallbackInitialize();
	}


	#endregion
	/// <summary>
	/// Initialize entranceBridge/exitBridge
	/// </summary>
	abstract protected void InitializeBridges();
	abstract protected void CallbackInitialize();


	#region ChessPuzzle Management

	/// <summary>
	/// When: On ChessPiece Targeting ChessSquare
	/// <para> Returns occupiable grids of holding ChessPiece, with currently targeting ChessSquare's grid </para>
	/// </summary>
	public bool TryCalculateOccupiableGrids(out List<Vector2Int> occupiableGrids)
	{
		if (chessBoard == null || chessBoard.TargetedChessSquare == null || chessPieceBox == null)
			occupiableGrids = null;
		else
			occupiableGrids = ChessPieceGridCalculator.CalculateOccupiableChessGrids(chessBoard.TargetedChessSquare.ChessSquareInfo.Grid,
																					 chessPieceBox.HoldingChessPiece,
																					 ChessPuzzleInfo.ChessBoardInfo.BoardLength);
		return (occupiableGrids != null);
	}


	/// <summary>
	/// When: On ChessPiece Release
	/// <para> Put ChessPiece on targeting ChessSquare if it is placeable </para>
	/// </summary>
	private void TryPlaceChessPieceOnChessSquare(ChessPieceBase releasedChessPiece)
	{
		if (playingChessPuzzleData.PlayingChessPuzzle != this)
			return;

		bool isChessPiecePlaceable = (chessBoard.TargetedChessSquare != null &&
									  chessBoard.TargetedChessSquare.ChessSquareInfo.SquareType == SquareType.Common);
		if (isChessPiecePlaceable)
		{
			var placingChessPiece = releasedChessPiece;
			var targetChessSquare = chessBoard.TargetedChessSquare;

			placingChessPiece.PlaceChessPieceOn(targetChessSquare.transform.position);
			chessBoard.OccupyChessSquaresBy(placingChessPiece);

			OnChessPiecePlaced?.Invoke();
		}
		else
		{
			releasedChessPiece.ReturnChessPiece();
			chessBoard.TargetedChessSquare = null;
		}
	}


	protected Vector3[] GetChessSquarePositionsWith(Vector2Int[] grids)
	{
		List<Vector3> positions = new();
		foreach (var grid in grids)
		{
			chessBoard.TryGetChessSquareOn(grid, out var chessSquare);
			positions.Add(chessSquare.transform.position);
		}

		return positions.ToArray();
	}


	private void SetChessPuzzleInteractive()
	{
		if (playingChessPuzzleData.PlayingChessPuzzle != this)
			return;

		IsChessInteractable = false;
	}
	private void SetChessPuzzleNonInteractive()
	{
		if (playingChessPuzzleData.PlayingChessPuzzle != this)
			return;

		IsChessInteractable = false;
	}


	#endregion

	#region On PuzzleClearableIndictor Clicked

	/// <summary>
	/// When: EntranceBridge is clicked with pathPassableStatus
	/// <para> Move player through the chess puzzle with chessObjects inactive  </para>
	/// </summary>
	public void AlertOnClearIndicatorClicked()
	{
		if (isFindingPath == false && passableChessPuzzlePath != null)
		{
			var player = GameObject.FindGameObjectWithTag("Player");
			player?.GetComponent<PlayerController>().StartMoveThroughChessPuzzle(entranceBridgePos,
																				 GetChessSquarePositionsWith(passableChessPuzzlePath
																											 .Select(node => node.Grid).ToArray()),
																				 exitBridgePos);
		}
	}


	#endregion

	#region Path Finding

	// [Action: ChessPuzzle Path Finding]
	/// <summary>
	/// When: On ChessBoard Moving End
	/// </summary>
	protected void AlertOnPathFindingFinished()
	{
		if (playingChessPuzzleData.PlayingChessPuzzle != this)
			return;

		if (isFindingPath) return;

		if (TryFindChessPuzzlePath(out var chessPuzzlePath) == true)
		{
			passableChessPuzzlePath = chessPuzzlePath;
			OnPathFindingFinished?.Invoke(true);
		}
		else
		{
			passableChessPuzzlePath = null;
			OnPathFindingFinished?.Invoke(false);
		}

		isFindingPath = false;
	}
	protected void AlertOnPathFindingFinished(ChessPieceBase chessPiece)
	{
		AlertOnPathFindingFinished();
	}


	protected bool TryFindChessPuzzlePath(out List<Node> resultPathList)
	{
		resultPathList = OptimalPathCalculator.FindPath(ChessPuzzleInfo.ChessBoardInfo.EntranceGrid.Grid,
														ChessPuzzleInfo.ChessBoardInfo.ExitGrid.Grid,
														ChessPuzzleInfo.ChessBoardInfo.BoardLength,
														ValidateEntranceGrid, ValidatePassableGrid);
		if (resultPathList != null && resultPathList.Count > 0 &&
			resultPathList[0].Grid == ChessPuzzleInfo.ChessBoardInfo.EntranceGrid.Grid &&
			resultPathList[^1].Grid == ChessPuzzleInfo.ChessBoardInfo.ExitGrid.Grid)
		{

			return true;
		}

		return false;
	}


	#endregion
	abstract protected bool ValidateEntranceGrid(Vector2Int entranceGrid);
	abstract protected bool ValidatePassableGrid(Vector2Int checkingGird);


}