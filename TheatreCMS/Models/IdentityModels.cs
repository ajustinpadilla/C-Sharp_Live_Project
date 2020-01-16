using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TheatreCMS.Areas.Subscribers.Models;
using TheatreCMS.Models;


namespace TheatreCMS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Role { get; set; }
        public virtual Subscriber SubscriberPerson { get; set; }
        public virtual SeasonManager SeasonManagerPerson { get; set; }

        /* Need to find a way to explicitly match a CastMember's User account to their ApplicationUser object, 
        If an app-user becomes a Castmember, ensure that for ApplicationUser user "=" CastMember castMember,
        user.CastMemberPersonID = castMembe.CastMemberPersonID */
        //public virtual CastMember CastMemberUser { get; set; }
        public int CastMemberUserID { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<ContentSection> ContentSections { get; set; }
        public DbSet<CurrentProduction> CurrentProductions { get; set; }
        public DbSet<RentalRequest> RentalRequests { get; set; }
        public DbSet<CastMember> CastMembers { get; set; }

        /* We'll be using the fluent API to create a 0..1 to 0..1 relationship
         * between a CastMember and an ApplicationUser, since some cast-members 
         * won't have logins, and not all app-users will be cast members */
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{

        //    modelBuilder.Entity<CastMember>()
        //        .HasOptional(u => u.CastMemberPerson)
        //        .WithRequired(p => p.CastMemberUser);

        //    modelBuilder.Entity<ApplicationUser>()
        //        .HasOptional(p => p.CastMemberUser)
        //        .WithRequired(u => u.CastMemberPerson);
        //}
        /* The above code doesn't jell with update-database due to 
         * "conflicting multiplicities," but I think it's close. 
         * The documentation on Fluent API and code first all seem to 
         * suggest that this kind of implicitly loaded zero-or-one-
         * to-zero-or-one relationship doesn't really exist... 
         * 
         * This code has been commented out along with the two commented 
         * virtual methods in CastMember and ApplicationUser, and should only
         * serve as a starting point for later stories that might want to 
         * tackle this further. */

        public DbSet<Part> Parts { get; set; }
        public DbSet<Production> Productions { get; set; }
        public DbSet<DisplayLinks> DisplayLinks { get; set; }
        public DbSet<DisplayInfo> DisplayInfo { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<SeasonManager> SeasonManagers { get; set; }
        public DbSet<CalendarEvent> CalendarEvent { get; set; }
    }
    // IdentityManager will be able to edit, add and delete roles
    public class IdentityManager 
    {
        public bool RoleExists(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            return rm.RoleExists(name);
        }

        public bool CreateRole(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var idResult = rm.Create(new IdentityRole(name));
            return idResult.Succeeded;
        }

        public bool AddUserToRole(string userId, string roleName)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var idResult = um.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }

        public void ClearUserRoles(string userId)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = um.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();
            currentRoles.AddRange(user.Roles);
            foreach(var role in currentRoles)
            {
                um.RemoveFromRole(userId, role.RoleId);
            }
        }
    }
}