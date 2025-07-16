using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveController : MonoBehaviour
{
    public MeshRenderer[] Meshes;                    // All mesh renderers
    public VisualEffect DissolveParticles;
    public float dissolveRate = 0.025f;
    public float refreshRate = 0.05f;

    private Material[][] allMaterials;               // Store material instances per mesh renderer

    void Start()
    {
        if (Meshes != null && Meshes.Length > 0)
        {
            allMaterials = new Material[Meshes.Length][];

            for (int i = 0; i < Meshes.Length; i++)
            {
                // Make sure we use unique instances of materials
                allMaterials[i] = Meshes[i].materials;
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

        bool dissolving = true;
        while (dissolving)
        {
            counter += dissolveRate;

            dissolving = false;

            for (int i = 0; i < allMaterials.Length; i++)
            {
                for (int j = 0; j < allMaterials[i].Length; j++)
                {
                    float current = allMaterials[i][j].GetFloat("_Dissolve_Amount");
                    if (current < 1f)
                    {
                        allMaterials[i][j].SetFloat("_Dissolve_Amount", counter);
                        dissolving = true;
                    }
                }
            }

            yield return new WaitForSeconds(refreshRate);
        }
    }
}
