using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{

    public int A;
    public int B;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(A % B);
        }
    }
}
