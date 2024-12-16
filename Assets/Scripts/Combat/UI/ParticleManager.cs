using System.Collections;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public Material hitMaterial;
    public GameObject hitEffectPrefab;
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

            Instance.StartCoroutine(Instance.CreateHitEffect(gameObject));

            var ogMaterial = sr.material;
            for (int i = 0; i < Instance.hitLoops; i++)
            {
                sr.material = Instance.hitMaterial;

                yield return wfs;
                sr.material = ogMaterial;

                yield return wfs;

            }

        }
        yield return null;
    }

    IEnumerator CreateHitEffect(GameObject origin)
    {
        var sfx = Instantiate(hitEffectPrefab, origin.transform);
        yield return Helper.WaitForAnimation(sfx.GetComponent<Animator>(), 0, "main");
        Destroy(sfx);
    }
}
