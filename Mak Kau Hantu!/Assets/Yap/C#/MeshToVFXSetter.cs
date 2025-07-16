using UnityEngine;
using UnityEngine.VFX;

public class MeshToVFXSetter : MonoBehaviour
{
    public VisualEffect vfxGraph;                  // Reference to the VisualEffect component
    public MeshFilter[] staticMeshSources;         // Array of static mesh filters

    private Mesh combinedMesh;

    void Start()
    {
        if (vfxGraph == null)
        {
            Debug.LogWarning("VFX Graph not assigned.");
            return;
        }

        if (staticMeshSources == null || staticMeshSources.Length == 0)
        {
            Debug.LogWarning("No static mesh sources assigned.");
            return;
        }

        CombineMeshesFromFilters(staticMeshSources);
        vfxGraph.SetMesh("TargetMesh", combinedMesh); // 'TargetMesh' must match the VFX Graph mesh property name
        Debug.Log("Setting mesh to VFX: " + combinedMesh.name);
    }

void CombineMeshesFromFilters(MeshFilter[] meshFilters)
{
    combinedMesh = new Mesh();
    var combineInstances = new System.Collections.Generic.List<CombineInstance>();

    for (int i = 0; i < meshFilters.Length; i++)
    {
        var meshFilter = meshFilters[i];
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogWarning($"[Combine] Skipped null mesh at index {i}");
            continue;
        }

        if (!meshFilter.sharedMesh.isReadable)
        {
            Debug.LogWarning($"[Combine] Mesh '{meshFilter.name}' is not readable. Enable Read/Write in import settings.");
            continue;
        }

        CombineInstance instance = new CombineInstance();
        instance.mesh = meshFilter.sharedMesh;
        instance.transform = meshFilter.transform.localToWorldMatrix;
        combineInstances.Add(instance);

        Debug.Log($"[Combine] Adding mesh: {meshFilter.name}");
    }

    if (combineInstances.Count > 0)
    {
        combinedMesh.CombineMeshes(combineInstances.ToArray(), true, true);
        combinedMesh.name = "CombinedMesh_" + combineInstances.Count;
        Debug.Log($"✅ Combined {combineInstances.Count} mesh(es).");
    }
    else
    {
        Debug.LogError("❌ No meshes were combined. Ensure all meshes are readable and assigned.");
    }
}
}
