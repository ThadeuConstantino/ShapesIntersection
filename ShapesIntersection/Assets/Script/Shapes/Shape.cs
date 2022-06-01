public class Shape
{
    public int id { get; }
    public float x { get; }
    public float y { get; }

    protected Shape(int id, float x, float y)
    {
        this.id = id;
        this.x = x;
        this.y = y;
    }
}
