
using System.Data.Entity;

namespace Messenger
{
    internal class AppContext: DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppContext() : base("DefaultConnection") { }

    }
}
