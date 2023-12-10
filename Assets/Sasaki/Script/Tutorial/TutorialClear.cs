using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialClear : MonoBehaviour
{

    public float delayTime = 1.2f; 

    public BeamHPManager bhpm;
    void Start()
    {
        
    }

    void Update()
    {
        if (bhpm.HP <= 0)
        {
            StartCoroutine(BeforeLoading(delayTime)); ///深谷追加
            //SceneManager.LoadScene("Map 1");
        }
    }


    //////////////////深谷追加/////////////////////
    
    private IEnumerator BeforeLoading(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Map 1");
    }

    //ボス撃破後、ボス破壊アニメーションが動いてから
    //scene遷移するように追加しましたが、
    //フリーズする場合は「深谷追加」の行と
    //「//SceneManager.LoadScene("Map 1");」の「//」を
    //消してください。
    //タイミングが変だったら数値変えても大丈夫ですが、
    //2.0fなど値を大きくするとフリーズするようです

    /////////////////深谷ここまで/////////////////////
}
