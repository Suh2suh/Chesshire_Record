using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class ChessPuzzleSaver_Maker : MonoBehaviour
{

	[SerializeField] GameObject TryToSaveButton;
	[SerializeField] GameObject SureToSavePanel;
	[SerializeField] GameObject warningPanel;

	[SerializeField] TMP_InputField puzzleNameInputField;
	public TMP_InputField PuzzleNameInputFiled { get => puzzleNameInputField; }


	private void Start()
	{
		EventTriggerLinker.LinkEventTriggerToTransform<PointerEventData>(TryToSaveButton.transform, EventTriggerType.PointerClick, OnTryToSaveButtonClicked);
	}


	public void OnTryToSaveButtonClicked(PointerEventData pointerEventData)
	{
		var savingPuzzleInfo = transform.GetComponentInChildren<ChessPuzzle_Maker>().ChessPuzzleInfo;


		if (ChessPuzzleDataValidater.IsEntranceExitValid(savingPuzzleInfo.ChessBoardInfo) == false)
		{
			warningPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Must Make Entrance & Exit!\nClick Chess Square while Pressing 'E'/'S' ";
			warningPanel.SetActive(true);
			return;
		}
		if(ChessPuzzleDataValidater.IsChessPieceExist(savingPuzzleInfo.ChessPieceBoxInfo) == false)
		{
			warningPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Must Apply at least '1' ChessPiece!";
			warningPanel.SetActive(true);
			return;
		}
		if(puzzleNameInputField.text == null || puzzleNameInputField.text.Length == 0)
		{
			warningPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Chess Puzzle Name must contains at least '1' Character!";
			warningPanel.SetActive(true);
			return;
		}

		SureToSavePanel.GetComponentInChildren<TextMeshProUGUI>().text = "Are you sure to save as\n<color=red>" + puzzleNameInputField.text + "</color>?";
		SureToSavePanel.SetActive(true);
	}


	public void OnSureToSaveButtonClicked()
	{
		string puzzleName = puzzleNameInputField.text;
		if (ChessPuzzleDataIO.IsFilePathAlreadyExist(puzzleName))
		{
			string wantToOverwriteText = "Chess Puzzle named as\n<color=red>" + puzzleName + "</color> already exists.\n" + "Want to Overwrite?";

			if(SureToSavePanel.GetComponentInChildren<TextMeshProUGUI>().text != wantToOverwriteText)
			{
				SureToSavePanel.GetComponentInChildren<TextMeshProUGUI>().text = wantToOverwriteText;
				return;
			}
		}

		SureToSavePanel.SetActive(false);
		StartCoroutine(SaveChessPuzzle(transform.GetComponentInChildren<ChessPuzzle_Maker>().ChessPuzzleInfo, puzzleNameInputField.text));
	}

	private IEnumerator SaveChessPuzzle(ChessPuzzleInfo savingPuzzleInfo, string puzzleName)
	{
		string dataPath = ChessPuzzleDataIO.GetPuzzleDataFilePathWith(puzzleName);
		yield return StartCoroutine(DataIO.WriteDataObjectAsync(savingPuzzleInfo, dataPath));

		warningPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Save Complete!";
		warningPanel.SetActive(true);
	}


}