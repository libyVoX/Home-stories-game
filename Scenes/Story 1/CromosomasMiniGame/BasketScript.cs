using UnityEngine;
using TMPro;

public class BasketScript : MonoBehaviour
{
    public float speed = 3f;          // скорость движения
    public float minX = -6f;          // минимальная координата X
    public float maxX = 6f;           // максимальная координата X
    public int points = 0;
    
    public TMP_Text statsPoints;

    void FixedUpdate()
    {
        // --- Движение ---
        float moveX = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
            moveX = -5f;
        if (Input.GetKey(KeyCode.RightArrow))
            moveX = 5f;

        // перемещаем объект только по X
        Vector3 pos = transform.position;
        pos.x += moveX * speed * Time.deltaTime;

        // ограничиваем X
        pos.x = Mathf.Clamp(pos.x, minX, maxX);

        // сохраняем текущие Y и Z
        transform.position = pos;

        statsPoints.text = "Очки: " + points.ToString();
    }

    
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        points++;
    }
}
