using UnityEngine;

public class CloudLoopPerfect : MonoBehaviour
{
    public float speed = 2f;
    public float width = 7680f; 

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        if (transform.position.x <= -width)
        {
            transform.position += new Vector3(width * 2, 0, 0);
        }
    }
}