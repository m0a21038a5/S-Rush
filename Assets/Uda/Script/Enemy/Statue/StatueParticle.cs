using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueParticle : MonoBehaviour
{
    private bool isPlaying;
    [SerializeField]
    ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("StatueRePop");
    }

    // Update is called once per frame
    private IEnumerator StatueRePop()
    {
        particle.Play(true);

        yield return new WaitForSeconds(0.5f);

        particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }
}
