using UnityEngine;

public class CloudLoop : MonoBehaviour
{
    public float speed = 2f;
    public float limitX = -20f;
    public float respawnX = 20f;

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x < limitX)
        {
            transform.position = new Vector3(respawnX, transform.position.y, transform.position.z);
        }
    }
}