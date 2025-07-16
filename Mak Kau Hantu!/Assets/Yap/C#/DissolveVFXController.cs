using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveVFXController : MonoBehaviour
{
    public SkinnedMeshRenderer[] skinnedMeshes;     // Support multiple skinned meshes
    public VisualEffect DissolveParticles;
    public float dissolveRate = 0.025f;
    public float refreshRate = 0.05f;

    private List<Material> meshMaterials = new List<Material>();

    void Start()
    {
        // Collect all materials from all skinned meshes
        foreach (var mesh in skinnedMeshes)
        {
            if (mesh != null)
            {
                meshMaterials.AddRange(mesh.materials); // material instances
            }
        }

        if (DissolveParticles != null)
        {
            DissolveParticles.Stop();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Dissolving());
        }
    }

    IEnumerator Dissolving()
    {
        if (DissolveParticles != null)
        {
            DissolveParticles.Play();
        }

        float counter = 0;

        if (meshMaterials.Count > 0)
        {
            while (meshMaterials[0].GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate;
                foreach (var mat in meshMaterials)
                {
                    mat.SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
