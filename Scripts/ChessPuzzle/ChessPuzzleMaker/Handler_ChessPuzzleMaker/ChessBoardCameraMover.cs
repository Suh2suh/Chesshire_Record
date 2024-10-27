using UnityEngine;


public class ChessBoardCameraMover : MonoBehaviour
{
	[SerializeField] Camera mainCamera;

	[Space(15), SerializeField] Transform ChessBoardPos4x4;
	[SerializeField] Transform ChessBoardPos6x6;
	[SerializeField] Transform ChessBoardPos8x8;
	[SerializeField] Transform ChessBoardPos10x10;


	public void ConvertCamTo(int boardLength)
	{
		switch (boardLength)
		{
			case 4:
				ConvertCamTo4x4();

				break;
			case 6:
				ConvertCamTo6x6();

				break;
			case 8:
				ConvertCamTo8x8();

				break;
			case 10:
				ConvertCamTo10x10();

				break;
			default:

				break;
		}
	}


	public void ConvertCamTo4x4()
	{
		ConvertCamTo(ChessBoardPos4x4);
	}
	public void ConvertCamTo6x6()
	{
		ConvertCamTo(ChessBoardPos6x6);
	}
	public void ConvertCamTo8x8()
	{
		ConvertCamTo(ChessBoardPos8x8);
	}
	public void ConvertCamTo10x10()
	{
		ConvertCamTo(ChessBoardPos10x10);
	}

	void ConvertCamTo(Transform cameraTransform)
	{
		mainCamera.transform.position = cameraTransform.position;
		mainCamera.transform.rotation = cameraTransform.rotation;
	}


}
