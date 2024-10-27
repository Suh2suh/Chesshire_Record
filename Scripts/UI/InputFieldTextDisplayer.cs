using UnityEngine;
using TMPro;


[RequireComponent(typeof(TextMeshProUGUI))]
public class InputFieldTextDisplayer : MonoBehaviour
{

	[SerializeField] TMP_InputField inputField;
	[SerializeField] string frontText;
	[SerializeField] string backText;


	private void OnEnable()
	{
		SetTextToInputField();
	}

	void SetTextToInputField()
	{
		this.transform.GetComponent<TextMeshProUGUI>().text = frontText + inputField.text + backText;
	}


}
