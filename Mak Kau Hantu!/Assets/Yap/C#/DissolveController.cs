using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveController : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public VisualEffect DissolveParticles;
    public float dissolveRate = 0.025f;
    public float refreshRate = 0.05f;
    private Material[] skinnedMaterials;

    // Start is called before the first frame update
    void Start()
    {
        if (skinnedMesh != null)
        {
            skinnedMaterials = skinnedMesh.materials;
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

        if (skinnedMaterials.Length > 0)
        {

            float counter = 0;

            while (skinnedMaterials[0].GetFloat("_Dissolve_Amount") < 1)
            {
                counter += dissolveRate;
                for (int i = 0; i < skinnedMaterials.Length; i++)
                {
                    skinnedMaterials[i].SetFloat("_Dissolve_Amount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
