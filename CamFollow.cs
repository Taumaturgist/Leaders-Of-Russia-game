using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float speed;
    [SerializeField] private Vector3 delta;

    [SerializeField] private Camera camera;

    private bool _isCamFar;
    private bool _isCamFollowing = true;

    private void Awake()
    {
        var position = target.position;
        transform.position = new Vector3(
            position.x + delta.x,
            position.y + delta.y,
            position.z + delta.z);
    }

    public void SetDeltaX(int offset)
    {
        delta.x = offset;
    }

    public void CamDistanse()
    {
        _isCamFar = !_isCamFar;

        if (_isCamFar)
        {
            CamFar();
        }
        else
        {
            CamClose();
        }
    }

    private void CamClose()
    {
        _isCamFollowing = true;     
        delta.y = 4;
        camera.orthographicSize = 8;
    }

    private void CamFar()
    {
        _isCamFollowing = false;    
        transform.position = new Vector3(0, 0, -10);
        camera.orthographicSize = 23;        
    }

    private void FixedUpdate()
    {
        if (_isCamFollowing)
        {
            var position = target.position;
            transform.position = Vector3.Lerp(transform.position, new Vector3(
                position.x + delta.x,
                position.y + delta.y,
                position.z + delta.z),
                speed * Time.deltaTime);
        }       
    }
}
