using UnityEngine;
using UnityEngine.EventSystems;


public class ChessPuzzleLoadPanelController_Tester: ChessPuzzleLoadPanelController
{
	[SerializeField] private ChessPuzzleTesterHandler chessPuzzleTesterHandler;
	

	protected override void OnChessPuzzleLoadButtonClicked(PointerEventData pointerEventData)
	{
		string loadingPuzzleName = selectedPuzzleNameText.text;
		PrepareChessPuzzleTester(loadingPuzzleName);

		if (ChessPuzzleOnSelectPanel.activeSelf) ChessPuzzleOnSelectPanel.SetActive(false);
	}
	private void PrepareChessPuzzleTester(string loadingPuzzleName)
	{
		chessPuzzleTesterHandler.LoadPuzzle(loadingPuzzleName);

		gameObject.SetActive(false);
	}


}
