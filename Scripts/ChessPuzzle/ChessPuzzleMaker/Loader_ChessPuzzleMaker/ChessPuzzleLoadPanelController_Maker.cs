using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;


public class ChessPuzzleLoadPanelController_Maker : ChessPuzzleLoadPanelController
{
	[SerializeField] private ChessPuzzleMakerHandler chessPuzzleMakerHandler;


	protected override void OnChessPuzzleLoadButtonClicked(PointerEventData pointerEventData)
	{
		string loadingPuzzleName = selectedPuzzleNameText.text;
		StartCoroutine(PrepareChessPuzzleMakerAsync(loadingPuzzleName));

		if (chessPuzzleOnSelectPanel.activeSelf) chessPuzzleOnSelectPanel.SetActive(false);
	}

	private IEnumerator PrepareChessPuzzleMakerAsync(string loadingPuzzleName)
	{
		ChessPuzzleInfo loadedChessPuzzleData = ChessPuzzleLoader.LoadChessPuzzle(loadingPuzzleName);
		
		yield return StartCoroutine(chessPuzzleMakerHandler.CreateChessPuzzleMakerAsync(loadedChessPuzzleData));
		chessPuzzleMakerHandler.ChessBoardCameraMover.ConvertCamTo(loadedChessPuzzleData.ChessBoardInfo.BoardLength);
		chessPuzzleMakerHandler.ChessPuzzleSaver.PuzzleNameInputFiled.text = loadingPuzzleName;

		this.gameObject.SetActive(false);
	}


}


