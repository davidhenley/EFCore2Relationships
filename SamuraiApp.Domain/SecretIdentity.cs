namespace SamuraiApp.Domain
{
    public class SecretIdentity
    {
        public int Id { get; set; }
        public string RealName { get; set; }
        // If the FK is nullable (int?), the child does not need a parent
        public int SamuraiId { get; set; }
        // As long as the SamuraiId is here, EF can work out that the foreign key is the Samurai
        // You can however add public Samurai Samurai { get; set; } if you want to navigate that way
        // If no SamuraiId you have to tell EF Core which is the principle and which is the dependent
        // It's anyones guess what EF Core will pick on its own without the key
        // You must specify in the OnModelCreating
        // modelBuilder.Entity<Samurai>().HasOne(s => s.SecretIdentity).WithOne(i => i.Samurai).IsRequired();
        // or shadow property
        // modelBuilder.Entity<Samurai>().HasOne(s => s.SecretIdentity).WithOne(i => i.Samurai).HasForeignKey<SecretIdentity>("SamuraiId");
    }
}
