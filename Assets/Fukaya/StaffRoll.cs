using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffRoll : MonoBehaviour
{
    public GameObject staffCanvas;
    public GameObject Rusult;
 //   public GameObject ButtonNo;
 //   public GameObject ButtonStaff;
   // public GameObject Continue;

    // Start is called before the first frame update
    void Start()
    {
        staffCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonStaffpush()
    {
        staffCanvas.SetActive(true);
        


    }
}
