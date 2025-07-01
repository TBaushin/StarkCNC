namespace StarkCNC.Models
{
    public class User
    {
        public string Name { get; set; }

        public byte[] Image { get; set; }

        public User()
        {
            Name = string.Empty;
            Image = Array.Empty<byte>();
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
}
