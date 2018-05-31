using System.Collections.Generic;

namespace SamuraiApp.Domain
{
    public class Samurai
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Quote> Quotes { get; set; } = new List<Quote>();
        public List<SamuraiBattle> SamuraiBattles { get; set; } = new List<SamuraiBattle>();
        // Even if you put [Required] on this, EF will not recognize as required in EF Core
        // In order to make required, you can require the constructor to pass in a SecretIdentity
        // and set SecretIdentity = new SecretIdentity { RealName = name };
        public SecretIdentity SecretIdentity { get; set; }
    }
}
