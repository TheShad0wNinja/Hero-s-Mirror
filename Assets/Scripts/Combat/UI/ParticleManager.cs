using System.Collections;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public Material hitMaterial;
    public float hitDuration = 0.02f;
    public int hitLoops = 2;
    public static ParticleManager Instance { get; private set; }
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }


    public static IEnumerator TriggerHitEffect(GameObject gameObject)
    {
        if (Instance != null)
        {
            var sr = gameObject.GetComponent<SpriteRenderer>();
            WaitForSeconds wfs = new WaitForSeconds(Instance.hitDuration);

            var ogMaterial = sr.material;
            for (int i = 0; i < Instance.hitLoops; i++)
            {
                yield return wfs;
                sr.material = Instance.hitMaterial;

                yield return wfs;
                sr.material = ogMaterial;

            }

        }
        yield return null;
    }
}
