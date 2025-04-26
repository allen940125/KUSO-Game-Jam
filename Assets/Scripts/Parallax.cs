using UnityEngine;

public class Parallax : MonoBehaviour
{
    //Auto scrolling background script, using camera position to set texture offset
    [SerializeField] private Camera cam;

    private Vector3 camStartPos;
    private Vector3 offset;
    private Material mat;
    private float distance;

    [SerializeField] float speed = 0.5f;
    void Start()
    {
        camStartPos = cam.transform.position;
        offset = transform.position - cam.transform.position;
        mat = GetComponent<Renderer>().material;
        Debug.Log(mat);
    }

    // Update is called once per frame
    void Update()
    {
        distance = cam.transform.position.x - camStartPos.x;
        mat.SetTextureOffset("_MainTex", new Vector2(distance * speed, 0));

        transform.position = cam.transform.position + offset;
    }
}