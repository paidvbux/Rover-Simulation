using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetScript : MonoBehaviour
{
    public enum Path { Flower, Crown, Circle, Star, None };

    public TextMeshProUGUI pathText;
    public Path currentPath;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            GoPreviousPath();
        if (Input.GetKeyDown(KeyCode.RightArrow))
            GoNextPath();

        float t = Time.time % (2 * Mathf.PI);

        Vector3 position = Vector3.zero;
        switch (currentPath)
        {
            case Path.Flower:
                position = new Vector3(
                    Mathf.Cos(t) * Mathf.Sin(3 * t) + 2,
                    Mathf.Sin(t) * Mathf.Sin(3 * t) + 2,
                    Mathf.Sin(3 * t) / 4 + 2
                    );
                pathText.text = "Flower";
                break;
            case Path.Crown:
                position = new Vector3(
                    Mathf.Cos(2 * t),
                    3 + Mathf.Sin(6 * t) / 3,
                    Mathf.Sin(2 * t)
                    );
                pathText.text = "Crown";
                break;
            case Path.Circle:
                position = new Vector3(
                    Mathf.Cos(3 * t),
                    Mathf.Sin(3 * t) / 4 + 3,
                    Mathf.Sin(3 * t)
                    );
                pathText.text = "Circle";
                break;
            case Path.Star:
                t *= 3;
                position = new Vector3(
                    2f * Mathf.Cos(t) + 5f * Mathf.Cos(2f / 3f * t),
                    9,
                    2f * Mathf.Sin(t) - 5f * Mathf.Sin(2f / 3f * t)
                    ) / 3;
                pathText.text = "Star";
                break;
            case Path.None:
                position = new Vector3(1, Input.mousePosition.y / 108f / 3f + 0.25f, Input.mousePosition.x / 216f / 3f - 10f / 6f);
                pathText.text = "None";
                break;
        }

        transform.position = Vector3.Lerp(transform.position, position, 5 * Time.deltaTime);
    }

    public void GoNextPath()
    {
        switch (currentPath)
        {
            case Path.Flower:
                currentPath = Path.Crown;
                break;
            case Path.Crown:
                currentPath = Path.Circle;
                break;
            case Path.Circle:
                currentPath = Path.Star;
                break;
            case Path.Star:
                currentPath = Path.None;
                break;
            case Path.None:
                currentPath = Path.Flower;
                break;
        }
    }

    public void GoPreviousPath()
    {
        switch (currentPath)
        {
            case Path.Flower:
                currentPath = Path.None;
                break;
            case Path.Crown:
                currentPath = Path.Flower;
                break;
            case Path.Circle:
                currentPath = Path.Crown;
                break;
            case Path.Star:
                currentPath = Path.Circle;
                break;
            case Path.None:
                currentPath = Path.Flower;
                break;
        }
    }
}