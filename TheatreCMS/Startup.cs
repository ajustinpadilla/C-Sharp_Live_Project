using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Web;
using TheatreCMS.Models;
using TheatreCMS.Controllers;
using System;

[assembly: OwinStartupAttribute(typeof(TheatreCMS.Startup))]
namespace TheatreCMS
{
    public partial class Startup
    {

        List<CastMember> castMembers;
        List<Production> productions;


        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
            SeedCastMembers(ref castMembers);
            SeedProductions(ref productions);
            SeedParts(productions, castMembers);
        }

        private ApplicationDbContext context = new ApplicationDbContext();

        //create method for default roles and Admin users for login
        private void createRolesandUsers()
        {


            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //create Admin roll on strartup if none exist
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "test";
                user.FirstName = "Queen";
                user.LastName = "LaSneefa";
                user.Email = "test@gmail.com";
                user.StreetAddress = "420 Beefsteak Ln";
                user.City = "Atlantis";
                user.State = "OR";
                user.ZipCode = "97212";

                string userPWD = "Passw0rd!";

                var chkUser = userManager.Create(user, userPWD);
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "Admin");
                }
            }

            //Creating Member role
            if (!roleManager.RoleExists("Member"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Member";
                roleManager.Create(role);

                var user = new ApplicationUser()
                {
                    UserName = "memberTest",
                    FirstName = "God",
                    LastName = "Skrilla",
                    Email = "member.test@gmail.com",
                    StreetAddress = "314 Pi Ct",
                    City = "Detroit",
                    //State could be Enum later
                    State = "FL",
                    ZipCode = "12345"
                };

                string userPWD = "Ih@ve12cats";

                var chkUser = userManager.Create(user, userPWD);
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "Member");
                }
            }

            //Creating Subscriber role
            if (!roleManager.RoleExists("Subscriber"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Subscriber";
                roleManager.Create(role);

                var user = new ApplicationUser()
                {
                    UserName = "subscriberTest",
                    FirstName = "Pamela",
                    LastName = "Flanderson",
                    Email = "subscriber.test@gmail.com",
                    StreetAddress = "100 100th St",
                    City = "Shire",
                    State = "MA",
                    ZipCode = "54321"
                };

                string userPWD = "100100St!";

                var chkUser = userManager.Create(user, userPWD);
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "Subscriber");
                }
            }
        }

        //Seeding database with dummy CastMembers
        private void SeedCastMembers(ref List<CastMember> castMembers)
        {
            castMembers = new List<CastMember>
            {
                     new CastMember
                {
                    CastMemberID = 1,
                    Name = "Aaron Rodgers",
                    YearJoined = 2005,
                    MainRole = Enum.PositionEnum.Actor,
                    Bio = "Aaron was a highly sought after actor who we were thrilled to have join us in 2005.",
                    CurrentMember = true,
                },

                new CastMember
                {
                    CastMemberID = 2,
                    Name = "Davante Adams",
                    YearJoined = 2014,
                    MainRole = Enum.PositionEnum.Actor,
                    Bio = "Davante is a big part of our team.",
                    CurrentMember = true,
                },

                new CastMember
                {
                    CastMemberID = 2,
                    Name = "Matt Lafluer",
                    YearJoined = 2019,
                    MainRole = Enum.PositionEnum.Director,
                    Bio = "Matt became one of the youngest directors in the business when he joined us in 2019",
                    CurrentMember = true,
                }
            };

            //c => c.Name, castMember
            castMembers.ForEach(castMember => context.CastMembers.AddOrUpdate(c => c.Name, castMember));
            context.SaveChanges();

        }

        //Seeding database with dummy Productions
        private void SeedProductions(ref List<Production> productions)
        {
            productions = new List<Production>
            {
                new Production { ProductionId = 1, Title = "Hamilton", Playwright = "Lin-Manuel Miranda", Description = "This is a musical inspired by the biography " +
                "Alexander Hamilton by historian Ron Chernow. This musical tells the story of American Founding Father Alexander Hamilton through music " +
                "that draws heavily from hip hop, as well as R&B, pop, soul, and traditional-style show tune. ", OpeningDay = new DateTime(2020, 01, 02, 19, 30, 00),
                    ClosingDay = new DateTime(2020, 01, 30, 19, 30, 00), ShowtimeEve = new DateTime(2020, 01, 02, 19, 30, 00), ShowtimeMat = new DateTime(2020, 01, 02, 22, 30, 00),
                    TicketLink = "ticketsforyou.com", Season = 1, IsCurrent = true },

                new Production { ProductionId = 2,  Title = "Phantom of the Opera", Playwright = "Andrew Lloyd Webber & Charles Hart", Description = "Based on a French " +
                "novel of the same name by Gaston Leroux, its central plot revolves around a beautiful soprano, Christine Daae, who becomes the obesession " +
                "of a mysterious, disfigured musical genius living in the subterranean labyrinth beneath the Paris Opera House.", OpeningDay = new DateTime(2020, 01, 04, 17, 30, 00),
                    ClosingDay = new DateTime(2020, 02, 02, 17, 30, 00), ShowtimeEve = new DateTime(2020, 01, 04, 17, 30, 00), ShowtimeMat = new DateTime(2020, 01, 04, 19, 30, 00),
                    TicketLink = "ticketsforyou.com", Season = 1, IsCurrent = true },

                new Production { ProductionId = 3, Title = "The Book of Mormon", Playwright = "Trey Parker, Robert, Lopez, & Matt Stone", Description = "The Book of Mormon " +
                "follows two Latter-Day Saints missionaries as they attempt to preach the faith of the Church of Jesus Christ of Latter-Day Saints to the " +
                "inhabitants of a remote Ugandan village.", OpeningDay = new DateTime(2020, 04, 02, 19, 30, 00), ClosingDay = new DateTime(2020, 04, 27, 19, 30, 00),
                    ShowtimeEve = new DateTime(2020, 04, 02, 19, 30, 00), ShowtimeMat = new DateTime(2020, 04, 02, 22, 30, 00), TicketLink = "ticketsforyou.com", Season = 2,
                    IsCurrent = true },

                new Production { ProductionId = 4, Title = "Wicked", Playwright = "Stephen Schwartz", Description = "This musical is told from the perspective of the witches of " +
                "the Land of Oz; its plot begins before and continues after Dorothy Gale arrives in Oz from Kansas, and includes several references to the 1939 film.",
                    OpeningDay = new DateTime(2019, 10, 01, 19, 30, 00), ClosingDay = new DateTime(2019, 10, 25, 19, 30, 00), ShowtimeEve = new DateTime(2019, 10, 01, 19, 30, 00),
                    ShowtimeMat = new DateTime(2019, 10, 01, 23, 30, 00), TicketLink = "ticketsforyou.com", Season = 4, IsCurrent = false },

                new Production
                {
                    ProductionId = 5,
                    Title = "How to Succeed in Business Without Really Trying",
                    Playwright = "Frank Loesser",
                    Description = "This story concerns young, " +
                "ambitious J. Pierrepont Finch, who, with the help of the book How to Succeed in Business Without Really Trying, rises from window washer to chairman of " +
                "the board of the World Wide Wicket Company.",
                    OpeningDay = new DateTime(2019, 12, 01, 19, 30, 00),
                    ClosingDay = new DateTime(2019, 12, 22, 19, 30, 00),
                    ShowtimeEve = new DateTime(2019, 12, 01, 19, 30, 00),
                    ShowtimeMat = new DateTime(2019, 12, 01, 23, 30, 00),
                    TicketLink = "ticketsforyou.com",
                    Season = 4,
                    IsCurrent = false
                }
            };
            //d => d.Title, Production
            productions.ForEach(Production => context.Productions.AddOrUpdate(d => d.Title, Production));
            context.SaveChanges();

        }

        //Seeding database with dummy roles.
        private void SeedParts(List<Production> productions, List<CastMember> castMembers)
        {

            var parts = new List<Part>
            {
                new Part
                {
                Production = productions[2] ,
                Character = "Elder Kevin Price",
                Type = Enum.PositionEnum.Actor,
                Person = castMembers[0],
                Details = "Elder (Kevin) Price is a young Mormon missionary. " +
                "He is seen by most, as the perfect, all American, son of god."
                },

                new Part {Production = productions[2], Type = Enum.PositionEnum.Director, Person = castMembers[2],
                Details = "We Thought Matt would make the perfect director for this performance." },

                new Part {Production = productions[1], Character = "Erik", Type = Enum.PositionEnum.Actor,
                Person = castMembers[1], Details = "Erik is born in a small town outside of Rouen, France. " +
                "Born hideously deformed, he is a \"subject of horror\" for his family"}
            };

            parts.ForEach(Part => context.Parts.AddOrUpdate(p => p.Character, Part));
            context.SaveChanges();
        }
    }
}