﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public abstract class SignleSceneLoader : MonoBehaviour
{
	public void LoadSceneSigle(string sceneName, bool loadSceneInstantly = false)
	{
		var sceneAsyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
		sceneAsyncOperation.allowSceneActivation = loadSceneInstantly;

		StartCoroutine(LoadSceneSigle(sceneAsyncOperation));
	}
	private IEnumerator LoadSceneSigle(AsyncOperation sceneAsyncOperation)
	{
		yield return StartCoroutine(OperateSceneAsyncSigle(sceneAsyncOperation));

		if(sceneAsyncOperation.isDone.Equals(false))
			yield return StartCoroutine(CallbackSceneAsync(sceneAsyncOperation));
	}

	private IEnumerator OperateSceneAsyncSigle(AsyncOperation sceneAsyncOperation)
	{
		if(sceneAsyncOperation.allowSceneActivation.Equals(false))
		{
			while (sceneAsyncOperation.progress < 0.9f)
			{
				//loadingBar.value = (1 / 9) * (sceneAsyncOperation.progress * 10);

				yield return null;
			}
		}
		else
		{
			while (sceneAsyncOperation.isDone)
			{
				//loadingBar.value = sceneAsyncOperation.progress;

				yield return null;
			}
		}
	}

	private IEnumerator CallbackSceneAsync(AsyncOperation sceneAsyncOperation)
	{
		while(sceneAsyncOperation.isDone == false) 
		{
			if (IsSceneLoadable())
				sceneAsyncOperation.allowSceneActivation = true;
			 
			yield return null;
		}
	}
	protected abstract bool IsSceneLoadable();

}