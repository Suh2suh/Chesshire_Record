using System;
using UnityEngine;


public class ChessBoardCameraMover : MonoBehaviour
{
	[SerializeField] Camera mainCamera;

	[Space(15), SerializeField]
	UDictionary<BoardLength, Transform> CamPosPerBoardLength;


	public void ConvertCamTo(int boardLength)
	{
		if(Enum.IsDefined(typeof(BoardLength), boardLength))
		{
			ConvertCamTo(CamPosPerBoardLength[(BoardLength)boardLength]);
		}
		else
		{
			Debug.LogError("[Error][ChessBoard Width Selector] there is no matching puzzle with length " + boardLength + "!");
		}
	}

	private void ConvertCamTo(Transform cameraTransform)
	{
		mainCamera.transform.position = cameraTransform.position;
		mainCamera.transform.rotation = cameraTransform.rotation;
	}


}
