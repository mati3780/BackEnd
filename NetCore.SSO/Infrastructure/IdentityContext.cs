using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCore.SSO.Model;

namespace NetCore.SSO.Infrastructure
{
	public class IdentityContext : IdentityDbContext<User, Role, long, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
	{
		public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<User>().ToTable("User");
			builder.Entity<Role>().ToTable("Role");
			builder.Entity<UserRole>().ToTable("UserRole");
			builder.Entity<UserClaim>().ToTable("UserClaim");
			builder.Entity<UserLogin>().ToTable("UserLogin");
			builder.Entity<UserToken>().ToTable("UserToken");
			builder.Entity<RoleClaim>().ToTable("RoleClaim");
		}
	}
}