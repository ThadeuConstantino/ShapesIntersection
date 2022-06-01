using System;
using System.Collections.Generic;
using UnityEngine;

public class ShapesIntersection : MonoBehaviour
{
    string result;
    Dictionary<int, List<int>> dictCollided;

    public GameObject prefabRectangle;
    public GameObject prefabCircle;
    public GameObject container;

    public void Start()
    {
        List<Shape> shapes = new List<Shape>();

        shapes.Add(new Circle(0, -100, 50, 20));
        shapes.Add(new Circle(1, 50, 50, 100));
        shapes.Add(new Rectangle(2, 75, 75, 50, 50));
        shapes.Add(new Rectangle(3, -50, 25, 25, 10));
        shapes.Add(new Rectangle(4, 0, 0, 100, 100));
        shapes.Add(new Rectangle(5, 10, 10, 10, 10));

        dictCollided = new Dictionary<int, List<int>>();
        dictCollided = FindIntersections(shapes);

        Data.result = PrintResult();

        foreach(var shape in shapes)
            DrawShapes(shape);
    }

    private string PrintResult()
    {
        int i = 0;
        foreach (var id in dictCollided)
        {
            i++;
            result += id.Key.ToString() + "->(";

            int j = 0;
            foreach (var intersections in id.Value)
            {
                j++;
                result += intersections + (j == id.Value.Count ? "" : ",");
            }
            result += ")" + (i == dictCollided.Count ? "" : ", ");
        }
        return result;
    }

    private void DrawShapes(Shape shape)
    {
        switch(shape)
        {
            case Rectangle rectangle:
                {
                    GameObject cloneRect = Instantiate(prefabRectangle, new Vector3(shape.x, shape.y, 0), Quaternion.identity, container.transform);
                    cloneRect.GetComponent<DrawLine>().DrawShape(rectangle.id, rectangle.width, rectangle.height);
                    break;
                }

            case Circle circle:
                {
                    GameObject cloneCircle = Instantiate(prefabCircle, new Vector3(shape.x, shape.y, 0), Quaternion.identity, container.transform);
                    cloneCircle.GetComponent<DrawLine>().DrawShape(circle.id, circle.radius, circle.radius);
                    break;
                }
        }
    }

    public Dictionary<int, List<int>> FindIntersections(List<Shape> shapes)
    {
        Dictionary<int, List<int>> collidedShapes = new Dictionary<int, List<int>>();

        int numShapes = shapes.Count;
        for (int shapeAIndex = 0; shapeAIndex < numShapes; shapeAIndex++)
        {
            Shape shapeA = shapes[shapeAIndex];

            for (int shapeBIndex = shapeAIndex + 1; shapeBIndex < numShapes; shapeBIndex++)
            {
                Shape shapeB = shapes[shapeBIndex];
                if (Colliding(shapeA, shapeB))
                {
                    Debug.Log("\nShapes collision= true!");
                    InsertCollisionPair(shapeA.id, shapeB.id, collidedShapes);
                }
            }
        }

        return collidedShapes;
    }

    private void InsertCollisionPair(int idA, int idB, Dictionary<int, List<int>> collidedShapes)
    {
        if (!collidedShapes.TryGetValue(idA, out List<int> collidedIdsForA))
        {
            collidedIdsForA = new List<int>();
            collidedShapes[idA] = collidedIdsForA;
        }

        if (!collidedShapes.TryGetValue(idB, out List<int> collidedIdsForB))
        {
            collidedIdsForB = new List<int>();
            collidedShapes[idB] = collidedIdsForB;
        }

        collidedIdsForA.Add(idB);
        collidedIdsForB.Add(idA);
    }

    public bool Colliding(Shape shape_a, Shape shape_b)
    {
        switch (shape_a)
        {
            case Rectangle rectangleA:
                {
                    switch (shape_b)
                    {
                        case Rectangle rectangleB: return Colliding(rectangleA, rectangleB);
                        case Circle circleB: return Colliding(circleB, rectangleA);
                    }
                    break;
                }
            case Circle circleA:
                {
                    switch (shape_b)
                    {
                        case Rectangle rectangleB: return Colliding(circleA, rectangleB);
                        case Circle circleB: return Colliding(circleA, circleB);
                    }
                    break;
                }
        }

        return false;
    }

    //Rectangle -> Rectangle: Overlap
    private bool Colliding(Rectangle rectangleA, Rectangle rectangleB)
    {
        if (Math.Abs(rectangleA.x - rectangleB.x) > rectangleA.width + rectangleB.width)
            return false;

        if (Math.Abs(rectangleA.y - rectangleB.y) > rectangleA.height + rectangleB.height)
            return false;
        else
            return true;
    }

    //Circle -> Circle: Overlap
    private bool Colliding(Circle circleA, Circle circleB)
    {
        float dx = circleA.x - circleB.x;
        float dy = circleA.y - circleB.y;
        double distance = Math.Sqrt(dx * dx + dy * dy);

        return distance < circleA.radius + circleB.radius;
    }

    //Circle -> Rectangle: Overlap
    bool Colliding(Circle circle, Rectangle rectangle)
    {
        float circlex = Math.Abs(circle.x - rectangle.x - rectangle.width/2);
        float xDist = rectangle.width/2 + circle.radius;

        if (circlex > xDist)
            return false;

        float circley = Math.Abs(circle.y - rectangle.y - rectangle.height/2);
        float yDist = rectangle.height/2 + circle.radius;

        if (circley > yDist)
            return false;
        if (circlex <= rectangle.width/2 || circley <= rectangle.height/2)
            return true;

        float xCornerDist = circlex - rectangle.width/2;
        float yCornerDist = circley - rectangle.height/2;
        float xCornerDistSqrt = xCornerDist * xCornerDist;
        float yCornerDistSqrt = yCornerDist * yCornerDist;
        float maxCornerDistSqrt = circle.radius * circle.radius;
        return xCornerDistSqrt + yCornerDistSqrt <= maxCornerDistSqrt;
    }

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

    public class Circle : Shape
    {
        public float radius { get; }

        public Circle(int id, float x, float y, float radius) : base(id, x, y)
        {
            this.radius = radius;
        }
    }
}
