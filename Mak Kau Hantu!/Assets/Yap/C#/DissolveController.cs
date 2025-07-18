using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveController : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public VisualEffect DissolveParticles;
    public float dissolveRate = 0.025f;
    public float refreshRate = 0.05f;
    private Material[] meshMaterials;

    // Start is called before the first frame update
    void Start()
    {
        if (skinnedMesh != null)
        {
            meshMaterials = skinnedMesh.materials;
        }
        if (DissolveParticles != null)
        {
            DissolveParticles.Stop();
        }
    }

    // Update is called once per frame
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

        if (meshMaterials.Length > 0)
        {

            float counter = 0;

            while (meshMaterials[0].GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate;
                for (int i = 0; i < meshMaterials.Length; i++)
                {
                    meshMaterials[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}