using UnityEngine;

public class Street
{
    public int length;
    public Vector2Int direction;
    public Street crossRight;
    public Street crossLeft;

    public Street(
        int minLength,
        int maxLength,
        Vector2Int direction,
        float crossProbability
    )
    {
        length = Random.Range(minLength, maxLength + 1) * 2;
        this.direction = direction;
        if(Random.value < crossProbability){
            crossRight = new Street(minLength, maxLength, new Vector2Int(direction.y, -direction.x), 0);
        }
        if(Random.value < crossProbability){
            crossLeft = new Street(minLength, maxLength, new Vector2Int(-direction.y, direction.x), 0);
        }
    }
}
