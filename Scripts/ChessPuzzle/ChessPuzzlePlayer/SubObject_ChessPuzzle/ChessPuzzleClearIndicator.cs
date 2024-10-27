using System;
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(MeshRenderer))]
public class ChessPuzzleClearIndicator : MonoBehaviour
{
	private MeshRenderer modelMeshRenderer;
	private ChessPuzzle_GamePlay parentChessPuzzle;

	private bool isChessPuzzleClearable = false;
	public bool IsChessPuzzleClearable
	{
		get => isChessPuzzleClearable;
		private set
		{
			if(isChessPuzzleClearable != value)
			{
				isChessPuzzleClearable = value;
				LitMaterialHandler.Emit(modelMeshRenderer, Color.red, isChessPuzzleClearable);
			}
		}
	}


	#region Initialization
	private void Awake()
	{
		//modelMeshRenderer = transform.Find("Model").GetComponentInChildren<MeshRenderer>();
		modelMeshRenderer = transform.GetComponent<MeshRenderer>();
		parentChessPuzzle = GetComponentInParent<ChessPuzzle_GamePlay>();

		parentChessPuzzle.OnPathFindingFinished += IndicatePuzzleClearableStatus;
	}

	#endregion


	private void IndicatePuzzleClearableStatus(bool _isChessPuzzleClearable)
	{
		IsChessPuzzleClearable = _isChessPuzzleClearable;
	}


	// [Action]
	private void OnMouseDown()
	{
		if(parentChessPuzzle.IsChessInteractable)
			OnIndicatorClicked();
	}
	private void OnIndicatorClicked()
	{
		if (isChessPuzzleClearable)
			parentChessPuzzle.AlertOnClearIndicatorClicked();
	}


}
