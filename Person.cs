
public class Person
{
    public string Name { get; set; } = "";

    public override string ToString()
    {
        return $"My name is {Name}";
    }
}