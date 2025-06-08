using UnityEngine;

public class LevitationEffect : MonoBehaviour
{
    [Header("Levitation Settings")]
    [Tooltip("Height of the levitation movement")]
    public float levitationHeight = 0.1f;
    
    [Tooltip("Speed of the levitation movement")]
    public float levitationSpeed = 3f;
    
    [Tooltip("Offset for the starting position")]
    public float startOffset = 0f;

    [Header("Scale Settings")]
    [Tooltip("Minimum scale value")]
    public float minScale = 0.9f;
    
    [Tooltip("Maximum scale value")]
    public float maxScale = 1.1f;

    private Vector3 startPosition;
    private Vector3 startScale;

    private void Start()
    {
        // Сохраняем начальную позицию и размер объекта
        startPosition = transform.position;
        startScale = transform.localScale;
    }

    private void Update()
    {
        // Вычисляем новую позицию Y используя синусоиду
        float newY = startPosition.y + Mathf.Sin((Time.time + startOffset) * levitationSpeed) * levitationHeight;
        
        // Применяем новую позицию
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Вычисляем новый размер используя косинусоиду (сдвинутую на 90 градусов относительно синуса)
        float scaleMultiplier = Mathf.Lerp(minScale, maxScale, (Mathf.Cos((Time.time + startOffset) * levitationSpeed) + 1f) * 0.5f);
        transform.localScale = startScale * scaleMultiplier;
    }
} 