using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ChessPuzzleLoadPanelController_Tester: ChessPuzzleLoadPanelController
{
	[SerializeField] private ChessPuzzleTesterHandler chessPuzzleTesterHandler;
	

	protected override void OnChessPuzzleLoadButtonClicked(PointerEventData pointerEventData)
	{
		string loadingPuzzleName = selectedPuzzleNameText.text;
		PrepareChessPuzzleMaker(loadingPuzzleName);

		if (chessPuzzleOnSelectPanel.activeSelf) chessPuzzleOnSelectPanel.SetActive(false);
	}
	private void PrepareChessPuzzleMaker(string loadingPuzzleName)
	{
		chessPuzzleTesterHandler.LoadPuzzle(loadingPuzzleName);

		this.gameObject.SetActive(false);
	}


}
