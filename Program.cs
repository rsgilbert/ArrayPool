// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

class Person
{
    public string Name { get; set; } = "";

    public override string ToString()
    {
        return $"My name is {Name}";
    }
}
public class Program
{
    public static void Main(string[] args)
    {
        MyPool<Person> personPool = new();
        Person person = personPool.Rent();
        Console.WriteLine(person);
        person.Name = "Jessie";
        Console.WriteLine(person);

        Console.WriteLine("******** 2 ************");
        Person person2 = personPool.Rent();
        Console.WriteLine(person2);
        person2.Name = "Happie";
        Console.WriteLine(person2);
        Console.WriteLine(person);

          Console.WriteLine("******** 3 ************");
        personPool.Return(person);
        Person person3 = personPool.Rent();
        Console.WriteLine(person3);
        person3.Name = "Mike";
        Console.WriteLine(person3);
        Console.WriteLine(person);

    }
}

