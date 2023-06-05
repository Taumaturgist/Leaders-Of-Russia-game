using UnityEngine;

public class PlayerChip : MonoBehaviour
{
    [SerializeField] private Sprite[] chipImageSprites;
    [SerializeField] private SpriteRenderer chipImageRenderer;

    private void Awake()
    {
        SetRandomImage();
    }

    private void SetRandomImage()
    {
        var randomInt = Random.Range(0, chipImageSprites.Length);
        chipImageRenderer.sprite = chipImageSprites[randomInt];
    }
}
