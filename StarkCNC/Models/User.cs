namespace StarkCNC.Models;

public class User
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public IReadOnlyCollection<byte> Image { get; }

    public User(Guid id, string name, byte[] image)
    {
        Id = id;
        Name = name;
        Image = image;
    }

    public User(User? user)
    {
        if (user is null)
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Image = Array.Empty<byte>();
        }
        else
        {
            Id = user.Id;
            Name = user.Name;
            Image = user.Image;
        }
    }
}