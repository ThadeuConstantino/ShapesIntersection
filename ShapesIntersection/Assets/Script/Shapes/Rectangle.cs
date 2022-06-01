public class Rectangle : Shape
{
    public float width { get; }
    public float height { get; }

    public Rectangle(int id, float x, float y, float width, float height) : base(id, x, y)
    {
        this.width = width;
        this.height = height;
    }
}