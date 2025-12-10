using System.ComponentModel.DataAnnotations;

namespace StarkCNC.Core.Models;

public class User : ICloneable
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; }

    public IReadOnlyCollection<byte>? Image { get; private set; }

    public User() { }

    public User(Guid id, string name, IReadOnlyCollection<byte>? image)
    {
        Id = id;
        Name = name;
        Image = image;
    }

    public void SetImage(IReadOnlyCollection<byte> image)
    {
        Image = image;
    }

    public object Clone() =>
        new User(Id, Name, Image);
}
