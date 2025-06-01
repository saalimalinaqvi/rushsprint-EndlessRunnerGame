using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private float score = 0;

    void Update()
    {
        score += Time.deltaTime * 10; // Score increases with time
        scoreText.text = "Score: " + ((int)score).ToString();
    }
}
