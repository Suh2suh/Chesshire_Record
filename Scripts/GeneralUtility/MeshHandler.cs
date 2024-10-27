using UnityEngine;


public static class MeshHandler
{


	public static Bounds GetWorldMeshBound(GameObject model)
	{
		var meshRenderer = model.GetComponentInChildren<MeshRenderer>();
		return meshRenderer.bounds;
	}
	public static Bounds GetLocalMeshBound(GameObject model)
	{
		var meshRenderer = model.GetComponentInChildren<MeshRenderer>();
		return meshRenderer.localBounds;
	}


	public static Vector3 GetWorldMeshBoundSize(GameObject model)
	{
		var meshRenderer = model.GetComponentInChildren<MeshRenderer>();
		return meshRenderer.bounds.size;
	}
	public static Vector3 GetLocalMeshBoundSize(GameObject model)
	{
		var meshRenderer = model.GetComponentInChildren<MeshRenderer>();
		return meshRenderer.localBounds.size;
	}


}