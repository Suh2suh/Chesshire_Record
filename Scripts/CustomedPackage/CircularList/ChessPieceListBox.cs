using UnityEngine;
using AirFishLab.ScrollingList.ContentManagement;
using UnityEngine.UI;
using static AirFishLab.ScrollingList.Custom.ChessPieceListBank;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Unity.VisualScripting;


namespace AirFishLab.ScrollingList.Custom
{
	public class ChessPieceListBox : ListBox, IPointerDownHandler, IPointerClickHandler
	{
		#region Setting Variables
		[SerializeField, HideInInspector]
		private ChessPieceIconContainer chessPieceIconContainer;
		[SerializeField, HideInInspector]
		private PlayingChessPuzzleData playingChessPuzzleData;
		[SerializeField]
		private PieceTypeData chessPieceListBoxData;

		[Header("ChessPiece Creation Trigger")]
		[SerializeField]
		EventTriggerType eventTriggerType;

		#endregion

		#region Exposed Variables
		public int AssignedChessPieceIndex { get => chessPieceListBoxData.ChessPieceIndexPair.pieceIndex; }

		#endregion

		#region Private Variables
		private Image chessButtonImage;
		private bool shouldDragChessPiece = false;
		private ChessPiecePanelController chessPiecePanelController;

		#endregion

		/// <summary>
		/// Can create ChessPiece through this button?
		/// </summary>
		public bool IsInteractive
		{
			get
			{
				var targetChessPiece = chessPieceListBoxData.ChessPieceIndexPair.targetChessPiece;

				return (targetChessPiece == null ? true : !targetChessPiece.gameObject.activeSelf);
			}
		}


		protected override void OnInitialized()
		{
			chessPiecePanelController = transform.GetComponentInParent<ChessPiecePanelController>();
		}


		#region [Action (TeethPanel): On Button Drag Triggered]
		private void Update()
		{
			if (shouldDragChessPiece)
			{
				(chessPieceListBoxData.ChessPieceIndexPair.targetChessPiece as ChessPiece_GamePlay)?.ForceMoveChessPiece();

		#if UNITY_STANDALONE_WIN
				if (Input.GetMouseButtonUp(0))
					OnPointerUp();
		#endif

			}
		}

		/// <summary>
		/// On Button Drag Begin
		/// </summary>
		/// <param name="eventData"></param>
		public void OnPointerDown(PointerEventData eventData)
		{
			if (IsInteractive && eventTriggerType == EventTriggerType.BeginDrag)
			{
				CreateAssignedChessPiece();
				(chessPieceListBoxData.ChessPieceIndexPair.targetChessPiece as ChessPiece_GamePlay)?.ForceHoldChessPiece();
				shouldDragChessPiece = true;

				DisplayInteractiveStatus();
				chessPiecePanelController.OnChessPieceListBoxClicked(this);
			}
		}
		/// <summary>
		/// On Button Drag End
		/// </summary>
		private void OnPointerUp()
		{
			if (shouldDragChessPiece && eventTriggerType == EventTriggerType.BeginDrag)
			{
				(chessPieceListBoxData.ChessPieceIndexPair.targetChessPiece as ChessPiece_GamePlay)?.ForceReleaseChessPiece();

				shouldDragChessPiece = false;
			}
		}


		#endregion

		#region [Action (MagicCirclePanel): On Button Click Triggered]
		public void OnPointerClick(PointerEventData eventData)
		{
			if (IsInteractive && eventTriggerType == EventTriggerType.PointerClick)
			{
				CreateAssignedChessPiece();

				DisplayInteractiveStatus();
				chessPiecePanelController.OnChessPieceListBoxClicked(this);
			}
		}


		#endregion
		
		/// <summary>
		/// On ChessPieceListBox Clicked
		/// </summary>
		/// <returns></returns>
		private void CreateAssignedChessPiece()
		{
			var chessPieceBox = (playingChessPuzzleData.PlayingChessPuzzle?.ChessPieceBox) as ChessPieceBox_GamePlay;
			chessPieceBox?.CreateChessPiece(chessPieceListBoxData.ChessPieceIndexPair.pieceIndex);
		}


		#region [Action: ChessPieceListBox Graphic Display]

		protected override void UpdateDisplayContent(IListContent content)
		{
			var data = (PieceTypeData)content;
			chessPieceListBoxData = data;

			chessButtonImage = chessButtonImage ?? GetComponent<Image>();
			chessButtonImage.sprite = chessPieceIconContainer[data.PieceType];

			// On ListBox Activated each time
			DisplayInteractiveStatus();
		}

		public void DisplayInteractiveStatus()
		{
			ForceDisplayInteractiveStatus(IsInteractive);
		}
		public void ForceDisplayInteractiveStatus(bool isInteractive)
		{
			chessButtonImage = chessButtonImage ?? GetComponent<Image>();

			if (isInteractive)
				chessButtonImage.color = Color.white;
			else
				chessButtonImage.color = Color.grey;
		}


		#endregion


	}
}
