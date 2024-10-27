using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ChessPuzzleLoadPanelController : MonoBehaviour
{

	[SerializeField] private Transform loadableChessPuzzleContainer;
	[SerializeField] private GameObject loadableChessPuzzleButtonPrefab;

	[SerializeField, Space(15)] private GameObject nothingToLoadText;
	[SerializeField] protected GameObject chessPuzzleOnSelectPanel;
	[SerializeField, Space(15)] private GameObject chessPuzzleLoadButton;
	protected TextMeshProUGUI selectedPuzzleNameText;

	private List<string> existingPuzzleNames;
	public List<string> ExistingPuzzleNames { get => existingPuzzleNames; }


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
		Initialize();
	}


	#endregion


	// [Action: On PuzzleLoadButton Clicked]
	protected abstract void OnChessPuzzleLoadButtonClicked(PointerEventData pointerEventData);


	// [Action: Display LoadableChessPuzzles | On LoadableChessPuzzleButton Clicked]
	private void DisplayLoadableChessPuzzles()
	{
		existingPuzzleNames = new();
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
				existingPuzzleNames.Add(puzzleName);
			}
			if (nothingToLoadText.activeSelf) nothingToLoadText.SetActive(false);
		}
	} 

	private void OnLoadableChessPuzzleButtonClicked(PointerEventData pointerEventData, string puzzleName)
	{
		selectedPuzzleNameText.text = puzzleName;
		if (!chessPuzzleOnSelectPanel.activeSelf) chessPuzzleOnSelectPanel.SetActive(true);
	}


	// [Initialization]
	private void Initialize()
	{
		if (chessPuzzleOnSelectPanel.activeSelf) chessPuzzleOnSelectPanel.SetActive(false);
		if (!nothingToLoadText.activeSelf) nothingToLoadText.SetActive(true);

		for (int i = 0; i < loadableChessPuzzleContainer.childCount; i++)
		{
			Destroy(loadableChessPuzzleContainer.GetChild(i).gameObject);
		}
	}


}