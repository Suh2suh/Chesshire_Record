using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;


public class SceneLoadingManager_Lobby : MonoBehaviour
{
	[SerializeField] VideoPlayer lobbyVideoPlayer;
	[SerializeField] private Slider lobbyLoadingBar;
	[SerializeField] private GameObject pressToStartText;


	private void Start()
	{
		RefreshLobby();
		StartCoroutine(LoadSceneSingle("Stage1Copy"));  // this can be loaded from serialized name maybe? later
	}
	private void RefreshLobby()
	{
		lobbyLoadingBar.value = 0;
		lobbyLoadingBar.gameObject.SetActive(true);

		pressToStartText.SetActive(false);

		lobbyVideoPlayer.Pause();
	}


	private IEnumerator LoadSceneSingle(string sceneName)
	{
		var sceneAsyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
		sceneAsyncOperation.allowSceneActivation = false;

		while (sceneAsyncOperation.progress < 0.9f)
		{
			lobbyLoadingBar.value = (1 / 9) * (sceneAsyncOperation.progress * 10);

			yield return null;
		}
		CallBack();

		while(sceneAsyncOperation.isDone.Equals(false))
		{
			if (Input.GetMouseButtonDown(0))
				sceneAsyncOperation.allowSceneActivation = true;

			yield return null;
		}
	}

	private void CallBack()
	{
		lobbyVideoPlayer.Play();

		lobbyLoadingBar.gameObject.SetActive(false);

		pressToStartText.SetActive(true);

		lobbyVideoPlayer.Play();

	}

}
