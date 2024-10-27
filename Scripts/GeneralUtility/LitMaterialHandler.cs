using System;
using UnityEngine;


public static class LitMaterialHandler
{

    public static void ChangeColor(MeshRenderer litMeshRenderer, Color newColor)
	{
        var litMaterial = litMeshRenderer.material;
        if (litMaterial == null)  return;

        litMaterial.SetColor("_BaseColor", newColor);
	}

    public static void Emit(MeshRenderer litMeshRenderer, Color emitColor, bool isOn)
    {
        var litMaterial = litMeshRenderer.material;
        if (litMaterial == null)  return;

        if (isOn == true && litMaterial.IsKeywordEnabled("_EMISSION") == false)
            litMaterial.EnableKeyword("_EMISSION");

        var finalEmitColor = emitColor * 0.5f * Convert.ToInt32(isOn);
        if(litMaterial.GetColor("_EmissionColor") != finalEmitColor)
            litMaterial.SetColor("_EmissionColor", finalEmitColor);
    }


}
