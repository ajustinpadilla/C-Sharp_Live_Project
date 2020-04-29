using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Web;
using TheatreCMS.Models;
using TheatreCMS.Controllers;
using System;
using System.Linq;
using System.Drawing;

[assembly: OwinStartupAttribute(typeof(TheatreCMS.Startup))]
namespace TheatreCMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
            SeedCastMembers();
			SeedProductions();
            SeedProductionPhotos();
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
                user.Role = "Admin";

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
                    ZipCode = "12345",
                    Role = "Member"
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
                    ZipCode = "54321",
                    Role = "Subscriber"
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
        private void SeedCastMembers()
        {
            
            var castMembers = new List<CastMember>
            {
                new CastMember{Name = "Aaron Rodgers", YearJoined= 2005, MainRole = Enum.PositionEnum.Actor,
                Bio = "Aaron was a highly sought after actor who we were thrilled to have join us in 2005.",
                CurrentMember = true, },

                new CastMember{Name = "Davante Adams", YearJoined= 2014, MainRole = Enum.PositionEnum.Actor,
                Bio = "Davante is a big part of our team.",
                CurrentMember = true, },

                new CastMember{Name = "Matt Lafluer", YearJoined= 2019, MainRole = Enum.PositionEnum.Director,
                Bio = "Matt became one of the youngest directors in the business when he joined us in 2019",
                CurrentMember = true, }

            };

            castMembers.ForEach(castMember => context.CastMembers.AddOrUpdate(c => c.Name, castMember));
            context.SaveChanges();
        }

		//Seeding database with dummy Productions
		private void SeedProductions()
		{
            var stupid = context.ProductionPhotos.Where(photo => photo.Description.Contains("Hamilton")).FirstOrDefault();
            var productions = new List<Production>
            {
                new Production{Title = "Hamilton", Playwright = "Lin-Manuel Miranda", Description = "This is a musical inspired by the biography " +
                "Alexander Hamilton by historian Ron Chernow. This musical tells the story of American Founding Father Alexander Hamilton through music " +
                "that draws heavily from hip hop, as well as R&B, pop, soul, and traditional-style show tune. ", ProductionPhotos = context.ProductionPhotos.Where(photo => photo.Description.Contains("Hamilton")).ToArray(), OpeningDay = new DateTime(2020, 01, 02, 19, 30, 00),
                ClosingDay = new DateTime(2020, 01, 30, 19, 30, 00), ShowtimeEve = new DateTime(2020, 01, 02, 19, 30, 00) , ShowtimeMat = new DateTime(2020, 01, 02, 22, 30, 00),
                TicketLink = "ticketsforyou.com", Season = 1, IsCurrent = true},

                new Production{Title = "Phantom of the Opera", Playwright = "Andrew Lloyd Webber & Charles Hart", Description = "Based on a French " +
                "novel of the same name by Gaston Leroux, its central plot revolves around a beautiful soprano, Christine Daae, who becomes the obesession " +
                "of a mysterious, disfigured musical genius living in the subterranean labyrinth beneath the Paris Opera House.", OpeningDay = new DateTime(2020, 01, 04, 17, 30, 00),
                ClosingDay = new DateTime(2020, 02, 02, 17, 30, 00), ShowtimeEve = new DateTime(2020, 01, 04, 17, 30, 00), ShowtimeMat = new DateTime(2020, 01, 04, 19, 30, 00),
                TicketLink = "ticketsforyou.com", Season = 1, IsCurrent = true},

                new Production{Title = "The Book of Mormon", Playwright = "Trey Parker, Robert, Lopez, & Matt Stone", Description = "The Book of Mormon " +
                "follows two Latter-Day Saints missionaries as they attempt to preach the faith of the Church of Jesus Christ of Latter-Day Saints to the " +
                "inhabitants of a remote Ugandan village.", OpeningDay = new DateTime(2020, 04, 02, 19, 30, 00), ClosingDay = new DateTime(2020, 04, 27, 19, 30, 00),
                ShowtimeEve = new DateTime(2020, 04, 02, 19, 30, 00), ShowtimeMat = new DateTime(2020, 04, 02, 22, 30, 00), TicketLink = "ticketsforyou.com", Season = 2,
                IsCurrent = true},

				new Production{Title = "Wicked", Playwright = "Stephen Schwartz", Description = "This musical is told from the perspective of the witches of " +
				"the Land of Oz; its plot begins before and continues after Dorothy Gale arrives in Oz from Kansas, and includes several references to the 1939 film.",
				OpeningDay = new DateTime(2019, 10, 01, 19, 30, 00), ClosingDay = new DateTime(2019, 10, 25, 19, 30, 00), ShowtimeEve = new DateTime(2019, 10, 01, 19, 30, 00),
				ShowtimeMat = new DateTime(2019, 10, 01, 23, 30, 00), TicketLink = "ticketsforyou.com", Season = 4, IsCurrent = false},

				new Production{Title = "How to Succeed in Business Without Really Trying", Playwright = "Frank Loesser", Description = "This story concerns young, " +
                "ambitious J. Pierrepont Finch, who, with the help of the book How to Succeed in Business Without Really Trying, rises from window washer to chairman of " +
                "the board of the World Wide Wicket Company.", OpeningDay = new DateTime(2019, 12, 01, 19, 30, 00), ClosingDay = new DateTime(2019, 12, 22, 19, 30, 00),
                ShowtimeEve = new DateTime(2019, 12, 01, 19, 30, 00), ShowtimeMat = new DateTime(2019, 12, 01, 23, 30, 00), TicketLink = "ticketsforyou.com", Season = 4,
                IsCurrent = false}
			};



            //added if statement to ensure that Startup.cs will not override additions to the productions table on start up.
            var currentList = context.Productions.ToList();

            if (currentList.Count == 0)
            {
               
                productions.ForEach(Production => context.Productions.AddOrUpdate(d => d.Title, Production));
                context.SaveChanges();
            }
		}

        
        private void SeedProductionPhotos()
        {
            var converter = new ImageConverter();
            // create images first
            string imagesRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\Content\Images");

            Image image1 = Image.FromFile(Path.Combine(imagesRoot, @"hamiltonFc.png"));
            Image image2 = Image.FromFile(Path.Combine(imagesRoot, @"hamilton2.png"));
            Image image3 = Image.FromFile(Path.Combine(imagesRoot, @"phantom1.png"));
            Image image4 = Image.FromFile(Path.Combine(imagesRoot, @"phantom2.png"));
            Image image5 = Image.FromFile(Path.Combine(imagesRoot, @"bookofmormon1.png"));
            Image image6 = Image.FromFile(Path.Combine(imagesRoot, @"bookofmormon2.png"));
            Image image7 = Image.FromFile(Path.Combine(imagesRoot, @"wicked1.png"));
            Image image8 = Image.FromFile(Path.Combine(imagesRoot, @"wicked2.png"));
            Image image9 = Image.FromFile(Path.Combine(imagesRoot, @"howtosucceedinbusinesswithoutreallytrying.png"));
            Image image10 = Image.FromFile(Path.Combine(imagesRoot, @"howtosucceedinbusinesswithoutreallytrying2.png"));

            var photos = new List<Photo>
            {
                new Photo
                {
                    OriginalHeight = image1.Height,
                    OriginalWidth = image1.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image1, typeof(byte[])),
                    Title = "Hamilton Image 1"
                },
                new Photo
                {
                    OriginalHeight = image2.Height,
                    OriginalWidth = image2.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image2, typeof(byte[])),
                    Title = "Hamilton Image 2"
                },
                new Photo
                {
                    OriginalHeight = image3.Height,
                    OriginalWidth = image3.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image3, typeof(byte[])),
                    Title = "Phantom Of The Opera Image 1"
                },
                new Photo
                {
                    OriginalHeight = image4.Height,
                    OriginalWidth = image4.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image4, typeof(byte[])),
                    Title = "Phantom Of The Opera Image 2"
                },
                new Photo
                {
                    OriginalHeight = image5.Height,
                    OriginalWidth = image5.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image5, typeof(byte[])),
                    Title = "The Book of Mormon Image 1"
                },
                new Photo
                {
                    OriginalHeight = image6.Height,
                    OriginalWidth = image6.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image6, typeof(byte[])),
                    Title = "The Book of Mormon Image 2"
                },
                new Photo
                {
                    OriginalHeight = image7.Height,
                    OriginalWidth = image7.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image7, typeof(byte[])),
                    Title = "Wicked Image 1"
                },
                new Photo
                {
                    OriginalHeight = image8.Height,
                    OriginalWidth = image8.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image8, typeof(byte[])),
                    Title = "Wicked Image 2"
                },
                new Photo
                {
                    OriginalHeight = image9.Height,
                    OriginalWidth = image9.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image9, typeof(byte[])),
                    Title = "How to Succeed in Business Without Really Trying Image 1"
                },
                new Photo
                {
                    OriginalHeight = image10.Height,
                    OriginalWidth = image10.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image10, typeof(byte[])),
                    Title = "How to Succeed in Business Without Really Trying Image 2"
                },

            };
            photos.ForEach(Photo => context.Photo.AddOrUpdate(p => p.PhotoFile, Photo));
            context.SaveChanges();



            var productionphoto = new List<ProductionPhotos>
            {
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Hamilton Image 1").FirstOrDefault().PhotoId,
                    Title = "Hamilton",
                    Description = "Actors performing \"The Story Of Tonight\".",
                    Production = context.Productions.Where(name => name.Title == "Hamilton").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Hamilton Image 2").FirstOrDefault().PhotoId,
                    Title = "Hamilton",
                    Description = "Hamilton Cover.",
                    Production = context.Productions.Where(name => name.Title == "Hamilton").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Phantom Of The Opera Image 1").FirstOrDefault().PhotoId,
                    Title = "Phantom Of The Opera",
                    Description = "Phantom Of The Opera Cover.",
                    Production = context.Productions.Where(name => name.Title == "Phantom of the Opera").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Phantom Of The Opera Image 2").FirstOrDefault().PhotoId,
                    Title = "Phantom Of The Opera",
                    Description = "The Phantom and Christine embrace.",
                    Production = context.Productions.Where(name => name.Title == "Phantom of the Opera").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "The Book of Mormon Image 1").FirstOrDefault().PhotoId,
                    Title = "The Book of Mormon",
                    Description = "Kevin Price main stage.",
                    Production = context.Productions.Where(name => name.Title == "The Book of Mormon").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "The Book of Mormon Image 2").FirstOrDefault().PhotoId,
                    Title = "The Book of Mormon",
                    Description = "General Butt-F******-Naked and Kevin Price singing together.",
                    Production = context.Productions.Where(name => name.Title == "The Book of Mormon").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Wicked Image 1").FirstOrDefault().PhotoId,
                    Title = "Wicked",
                    Description = "Wicked Cover",
                    Production = context.Productions.Where(name => name.Title == "Wicked").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Wicked Image 2").FirstOrDefault().PhotoId,
                    Title = "Wicked",
                    Description = "Elphaba Thropp in the spotlight.",
                    Production = context.Productions.Where(name => name.Title == "Wicked").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "How to Succeed in Business Without Really Trying Image 1").FirstOrDefault().PhotoId,
                    Title = "How to Succeed in Business Without Really Trying",
                    Description = "Smitty and the other secretaries read up on \"How to Hook a Tycoon and Take Him For All He’s Worth\".",
                    Production = context.Productions.Where(name => name.Title == "How to Succeed in Business Without Really Trying").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "How to Succeed in Business Without Really Trying Image 2").FirstOrDefault().PhotoId,
                    Title = "How to Succeed in Business Without Really Trying",
                    Description = "Daniel Radcliffe as Pierrepont Finch.",
                    Production = context.Productions.Where(name => name.Title == "How to Succeed in Business Without Really Trying").FirstOrDefault()
                }
            };
            productionphoto.ForEach(prodphoto => context.ProductionPhotos.AddOrUpdate(p => p.PhotoId, prodphoto));
            context.SaveChanges();

            // get all production titles
            // foreach production title in production titles
            // get production
            // assign production's default photo to a production photo that contains that title

            var productions = context.Productions.ToList();
            var productionPhotos = context.ProductionPhotos.ToList();
            foreach (var production in productions)
            {
                production.DefaultPhoto = productionPhotos.Where(productionPhoto => productionPhoto.Production == production).FirstOrDefault();
            }
            context.SaveChanges();
        }
    }
}
