using AirFishLab.ScrollingList;
using AirFishLab.ScrollingList.Custom;
using DG.Tweening;
using System;
using System.Linq;
using Unity.Collections;
using UnityEngine;


public class ChessPiecePanelController : MonoBehaviour
{
	#region Setting Variables
	[SerializeField] private PlayingChessPuzzleData playingChessPuzzleData;
	[SerializeField] private CircularScrollingList chessPieceCircularPanel;
	[SerializeField] private ChessPieceListBank chessPieceListBank;
	[SerializeField, ReadOnly] private PieceType currentSpawnedPiece = PieceType.None;

	[Space(15)]
	[SerializeField] private float panelMoveOffset = 100f;
	[SerializeField] private float panelMoveDuration = 1.5f;

	#endregion

	#region Private Variables
	private ChessPieceListBox prevClickedListBox;
	private bool isMagicCirclePanel = false;
	private Vector3 initialPos;

	#endregion


	#region Initialization

	private void Awake()
	{
		initialPos = transform.position;
	}


	/// <summary>
	/// For PuzzleOnStart's UnityEvent
	/// </summary>
	public void InitializeChessPiecePanel()
	{
		if (playingChessPuzzleData.PlayingChessPuzzle == null)
			return;

		var chessPieceBoxData = playingChessPuzzleData.PlayingChessPuzzle.ChessPuzzleInfo.ChessPieceBoxInfo.ChessPieceBox;

		int pieceIndex = 0;
		foreach (var pieceType in chessPieceBoxData.Keys)
			InitializeListBank(pieceType, count: chessPieceBoxData[pieceType]);
		void InitializeListBank(PieceType pieceType, int count)
		{
			var chessPieceBox = playingChessPuzzleData.PlayingChessPuzzle?.ChessPieceBox;
			for (int i = 0; i < count; i++)
			{
				chessPieceListBank.AddChessPiece(pieceType);
				chessPieceListBank.Contents[pieceIndex].LinkTargetPiece(chessPieceBox.transform.GetChild(pieceIndex).GetComponent<ChessPieceBase>(),
																		pieceIndex);

				pieceIndex++;
			}
		}

		InitializeCircularPanel();
		void InitializeCircularPanel()
		{
			chessPieceCircularPanel.Initialize();
		}


		isMagicCirclePanel = (chessPieceCircularPanel.ListBoxes.Length != 10);
		playingChessPuzzleData.PlayingChessPuzzle.OnChessPieceHold += HideChessPiecePanel;
		playingChessPuzzleData.PlayingChessPuzzle.OnChessPieceRelease += ShowChessPiecePanel;
		playingChessPuzzleData.PlayingChessPuzzle.ChessPieceBox.OnChessPieceReturn += UpdateChessPieceListBox;
	}

	/// <summary>
	/// For PuzzleOnClear's UnityEvent
	/// </summary>
	public void ClearChessPiecePanel()
	{
		playingChessPuzzleData.PlayingChessPuzzle.OnChessPieceHold -= HideChessPiecePanel;
		playingChessPuzzleData.PlayingChessPuzzle.OnChessPieceRelease -= ShowChessPiecePanel;
		playingChessPuzzleData.PlayingChessPuzzle.ChessPieceBox.OnChessPieceReturn -= UpdateChessPieceListBox;
	}


	#endregion


	#region [Action: Hide/Show ChessPiecePanel]

	/// <summary>
	/// On ChessPiece Hold
	/// </summary>
	/// <param name="holdingChessPiece"></param>
	private void HideChessPiecePanel(ChessPieceBase holdingChessPiece)
	{
		transform.DOLocalMoveY(panelMoveOffset, panelMoveDuration);
	}

	/// <summary>
	/// On ChessPiece Release
	/// </summary>
	/// <param name="releasedChessPiece"></param>
	private void ShowChessPiecePanel(ChessPieceBase releasedChessPiece)
	{
		transform.DOLocalMove(Vector3.zero, panelMoveDuration);
	}


	#endregion

	#region [Action: ChessPieceListBox Graphic Update]

	/// <summary>
	/// On Some Pieces Clicked
	/// </summary>
	public void OnChessPieceListBoxClicked(ChessPieceListBox clickedBox)
	{
		prevClickedListBox?.DisplayInteractiveStatus();
		prevClickedListBox = clickedBox;
	}


	/// <summary>
	/// On ChessPiece Return to ChessPieceBox
	/// </summary>
	/// <param name="assignedChessPiece"></param>
	private void UpdateChessPieceListBox(ChessPieceBase assignedChessPiece)
	{
		if(isMagicCirclePanel)
		{
			GetListBoxByIndex(assignedChessPiece.transform.GetSiblingIndex())?.ForceDisplayInteractiveStatus(true);
		}
		else
		{
			var targetListBox = chessPieceCircularPanel.ListBoxes[9 - assignedChessPiece.transform.GetSiblingIndex()];
			targetListBox.GetComponent<ChessPieceListBox>().ForceDisplayInteractiveStatus(true);
		}
	}


	private ChessPieceListBox GetListBoxByIndex(int chessPieceSiblingIndex)
	{
		var chessPieceListBoxes = Array.ConvertAll(chessPieceCircularPanel.ListBoxes, (listBox) => listBox as ChessPieceListBox);
		ChessPieceListBox match = null;

		foreach(var chessPieceListBox in chessPieceListBoxes) 
		{
			if(chessPieceListBox.gameObject.activeSelf &&
			   chessPieceListBox.AssignedChessPieceIndex == chessPieceSiblingIndex)
			{
				match = chessPieceListBox;
				break;
			}
		}

		return match;
	}


	#endregion


	/// <summary>
	/// For bag button UnityEvent
	/// </summary>
	public void ToggleChessPiecePanelActivation()
	{
		gameObject.SetActive(! gameObject.activeSelf);
	}


}