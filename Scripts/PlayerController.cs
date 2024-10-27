using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// Only Position-to-Position 이동 뿐이기 때문에, 자연스러운 조작 필요 X
/// 그래서 FixedUpdate보다는 유동적으로 처리 가능한 Corotuine을 사용
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	[SerializeField] private float moveSpeed;
	[SerializeField] private float jumpHeight;

	[Space(20)]
	[SerializeField] Transform entranceBridge;
	[SerializeField] private List<Transform> paths;
	[SerializeField] Transform exitBridge;

	private new Rigidbody rigidbody;
	private Transform modelTransform;

	private bool canMove = false;

	public static event Action OnPlayerMoveStart;
	public static event Action OnPlayerMoveEnd;


	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		modelTransform = transform.Find("Model");
	}


	[ContextMenu("test")]
	public void MoveTest()
	{
		StartMoveThroughChessPuzzle(entranceBridge.position, paths.Select(tr => tr.position).ToArray(), exitBridge.position);
	}
	/*
	public void MoveAlong(Vector3[] _orderedPaths)
	{
		if(_orderedPaths.Length > 0)
		{
			InitializePath();
			void InitializePath()
			{
				orderedPaths = _orderedPaths;
				currentPathIndex = 0;
				initialPosition = transform.position;
				headingPosition = _orderedPaths[currentPathIndex];
			}

			canMove = true;
			// shouldJump = if current is chess && next is chess
		}
	}
	private void FixedUpdate()
	{
		// canMove -> shouldGoJump
		if (canMove)
		{
			StepForwardToXZ(headingPosition);

			if(shouldJump)
			{
				float reach = InterpolationFactorCalculator.GetTFromZero(transform.position, initialPosition, headingPosition);
				if (reach <= 0.5)
					Jump(reach / 0.5f);
				else
					Jump((1 - reach) * 2);
			}

			if (IsSameInXZSpace(transform.position, headingPosition))
			{
				transform.position = headingPosition;

				currentPathIndex++;
				if (currentPathIndex < orderedPaths.Length)
					TryHeadToNextPosition();
				else
					canMove = false;
			}
		}


		void TryHeadToNextPosition()
		{
			try
			{
				initialPosition = transform.position;
				headingPosition = orderedPaths[currentPathIndex];
				// shouldJump = if current is chess && next is chess
				// Debug.Log("<" + headingPosition + ">");
			}
			catch (System.Exception)
			{
				canMove = false;
				throw;
			}
		}

	}
	*/


	public void StartMoveThroughChessPuzzle(Vector3 entranceBridge, Vector3[] chessSquarePath, Vector3 exitBridge)
	{
		// 이거 말고, static event.
		OnPlayerMoveStart();
		canMove = true;

		if (chessSquarePath.Length > 0)
			StartCoroutine(MoveThroughChessPuzzle(entranceBridge, chessSquarePath, exitBridge));
		else
			Debug.Log("Moving start failed");
	}
	private IEnumerator MoveThroughChessPuzzle(Vector3 entranceBridge, Vector3[] chessSquarePath, Vector3 exitBridge)
	{
		if (canMove == false)
			yield break;
		yield return MoveToPosition3D(entranceBridge, shouldJump: false);
		//Debug.Log("Entrance");

		if (canMove == false)
			yield break;
		yield return MoveToPosition3D(chessSquarePath[0], shouldJump: false);
		int pathIndex = 1;
		while(pathIndex < chessSquarePath.Length)
		{
			if (canMove == false)
				yield break;
			yield return MoveToPosition3D(chessSquarePath[pathIndex], shouldJump: true);
			//Debug.Log("Path " + pathIndex);
			pathIndex++;
		}

		if (canMove == false)
			yield break;
		yield return MoveToPosition3D(exitBridge, shouldJump: false);
		//Debug.Log("Exit");

		OnPlayerMoveEnd();
		canMove = false;
	}


	// Time.deltatTime -> Coroutine 내부라서 이상하게 되는 것은 아님.
	private IEnumerator MoveToPosition3D(Vector3 targetPosition, bool shouldJump)
	{
		Vector3 initialPosition = transform.position;
		Quaternion initialRotation = transform.rotation;

		float reach = InterpolationFactorCalculator.GetTFromZero(transform.position, initialPosition, targetPosition);
		while (reach < 1f) 
		{
			if (canMove == false)
				yield break;

			if(shouldJump)
			{
				if (reach <= 0.5)
					Jump(reach / 0.5f);
				else
					Jump((1 - reach) * 2);
			}

			LookAt(initialRotation, LookRotation(initialPosition, targetPosition), reach);
			StepForwardTo3D(targetPosition);
			reach = InterpolationFactorCalculator.GetTFromZero(transform.position, initialPosition, targetPosition);

			yield return null;
		}

		rigidbody.MovePosition(targetPosition);
	}


	/// <summary>
	/// Looks Like Jump: It moves player's model
	/// </summary>
	/// <param name="intensity"></param>
	private void Jump(float intensity)
	{
		intensity = Mathf.Clamp(intensity, 0, 1);
		modelTransform.localPosition = new Vector3(modelTransform.localPosition.x, intensity * jumpHeight,
																				 modelTransform.localPosition.z);
	}

	private void LookAt(Quaternion from, Quaternion lookAt, float intensity)
	{
		intensity = Mathf.Clamp(intensity, 0, 1);
		rigidbody.MoveRotation(Quaternion.Lerp(from, lookAt, intensity));
	}
	private Quaternion LookRotation(Vector3 initialPosition, Vector3 targetPosition)
	{
		Vector3 direction = (targetPosition - initialPosition).normalized;
		direction.y = 0;
		Quaternion goalRotation = Quaternion.LookRotation(direction, Vector3.up);

		return goalRotation;
	}

	private void StepForwardTo3D(Vector3 targetPosition)
	{
		Vector3 direction = (targetPosition - transform.position).normalized;

		Vector3 nextPos = transform.position + direction * moveSpeed * Time.deltaTime;
		//Vector3 nextPos = transform.position + (direction * moveSpeed * 0.016f);

		rigidbody.MovePosition(nextPos);   // frame 속도에 맞춰서 곱해줌
	}
	private void StepForwardToXZ(Vector3 targetPosition)
	{
		Vector3 direction = new Vector3(targetPosition.x - transform.position.x, 0, targetPosition.z - transform.position.z).normalized;

		rigidbody.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
	}

	private bool IsSameIn3DSpace(Vector3 position1, Vector3 position2)
	{
		return (position1 == position2);
	}
	private bool IsSameInXZSpace(Vector3 position1, Vector3 position2)
	{
		// pop move
		//return (Mathf.Round(position1.x - position2.x) == 0 && Mathf.Round(position1.z - position2.z) == 0);

		// smooth move
		return (Mathf.Abs(position1.x - position2.x) <= (moveSpeed+10)/100 && 
					Mathf.Abs(position1.z - position2.z) <= (moveSpeed+10)/100);
	}


}
