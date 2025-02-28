using UnityEngine;

public class FacingCamera : MonoBehaviour
{
    private Transform[] childs;

    void Start()
    {
        childs = new Transform[transform.childCount];
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i] = transform.GetChild(i);
        }
    }

    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
        foreach (Transform t in childs)
        {
            t.rotation = Camera.main.transform.rotation;
        }
    }
}
