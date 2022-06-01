public class Circle : Shape
{
    public float radius { get; }

    public Circle(int id, float x, float y, float radius) : base(id, x, y)
    {
        this.radius = radius;
    }
}