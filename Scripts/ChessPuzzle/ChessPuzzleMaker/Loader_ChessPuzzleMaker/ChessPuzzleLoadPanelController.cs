using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public abstract class ChessPuzzleLoadPanelController : MonoBehaviour
{
	#region Setting Variables
	[SerializeField] private Transform loadableChessPuzzleContainer;
	[SerializeField] private GameObject loadableChessPuzzleButtonPrefab;

	[SerializeField, Space(15)] private GameObject nothingToLoadText;
	[SerializeField, Space(15)] private GameObject chessPuzzleOnSelectPanel;
	[SerializeField] private GameObject chessPuzzleLoadButton;

	#endregion

	protected GameObject ChessPuzzleOnSelectPanel { get => chessPuzzleOnSelectPanel; }
	protected TextMeshProUGUI selectedPuzzleNameText;


	#region Unity Methods

	private void Awake()
	{
		selectedPuzzleNameText = chessPuzzleOnSelectPanel.GetComponentInChildren<TextMeshProUGUI>();
	}
	private void Start()
	{
		EventTriggerLinker.LinkEventTriggerToTransform<PointerEventData>(chessPuzzleLoadButton.transform, 
																		 EventTriggerType.PointerClick,
																		 OnChessPuzzleLoadButtonClicked);
	}

	private void OnEnable()
	{
		DisplayLoadableChessPuzzles();
	}
	private void OnDisable()
	{
		Clear();
	}


	#endregion


	#region [Action]: Display Loadable ChessPuzzles in folder

	private void DisplayLoadableChessPuzzles()
	{
		var loadableChessPuzzleNames = DataIO.GetAllFileNamesInDirectory(ChessPuzzleDataIO.GetChessPuzzleDirectory());
		if (loadableChessPuzzleNames.Count > 0)
		{
			CreateLoadablePuzzleButtons();
		}


		void CreateLoadablePuzzleButtons()
		{
			foreach (var puzzleName in loadableChessPuzzleNames)
			{
				var loadableChessPuzzleButton = Instantiate(loadableChessPuzzleButtonPrefab, loadableChessPuzzleContainer, false);
				loadableChessPuzzleButton.GetComponentInChildren<TextMeshProUGUI>().text = puzzleName;
				EventTriggerLinker.LinkEventTriggerToTransform<PointerEventData, string>(loadableChessPuzzleButton.transform,
																						 EventTriggerType.PointerClick,
																						 OnLoadableChessPuzzleButtonClicked,
																						 puzzleName);
			}
			if (nothingToLoadText.activeSelf) nothingToLoadText.SetActive(false);
		}
	} 

	private void OnLoadableChessPuzzleButtonClicked(PointerEventData pointerEventData, string puzzleName)
	{
		selectedPuzzleNameText.text = puzzleName;
		if (!chessPuzzleOnSelectPanel.activeSelf) chessPuzzleOnSelectPanel.SetActive(true);
	}


	#endregion


	/// <summary>
	/// On ChessPuzzle Load Button Clicked
	/// </summary>
	/// <param name="pointerEventData"></param>
	protected abstract void OnChessPuzzleLoadButtonClicked(PointerEventData pointerEventData);


	private void Clear()
	{
		if (chessPuzzleOnSelectPanel.activeSelf) chessPuzzleOnSelectPanel.SetActive(false);
		if (!nothingToLoadText.activeSelf) nothingToLoadText.SetActive(true);

		for (int i = 0; i < loadableChessPuzzleContainer.childCount; i++)
		{
			Destroy(loadableChessPuzzleContainer.GetChild(i).gameObject);
		}
	}


}