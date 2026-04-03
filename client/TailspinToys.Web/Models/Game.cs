namespace TailspinToys.Web.Models;

public class Game
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public Publisher? Publisher { get; set; }
    public Category? Category { get; set; }
    public double? StarRating { get; set; }
}

public class Publisher
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}
