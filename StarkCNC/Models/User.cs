namespace StarkCNC.Models;

public class User
{
    public string Name { get; set; }

    public IReadOnlyCollection<byte> Image { get; }

    public User(string name, byte[] image)
    {
        Name = name;
        Image = image;
    }

    public User(User? user)
    {
        if (user is null)
        {
            Name = string.Empty;
            Image = Array.Empty<byte>();
        }
        else
        {
            Name = user.Name;
            Image = user.Image;
        }
    }
}