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
            //Add photos of cast members
            string imagesRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\Content\Images");

            var castMembers = new List<CastMember>
            {
                new CastMember{Name = "London Bauman", MainRole = Enum.PositionEnum.Actor,
                Bio = "London Bauman is an actor, writer, sound designer, and Theatre Vertigo company member. " +
                "As an artist, he is interested in immersive physical theatre, magical realism, and new collaborative works." +
                " Selected work includes the role of Torch in Beirut (The Steep & Thorny Way to Heaven), " +
                "Barnaby the Barkeep in the devised Western Melodrama Bang! (Action/Adventure’s pilot season) " +
                "and sound design / original compositions for The Romeo and Juliet Project (Enso Theatre Ensemble). " +
                "In August, London will be traveling to the Edinburgh Fringe Festival in Scotland as Robert in Chet Wilson’s new play Gayface.",
                Photo = ImageUploadController.InsertPhoto(Image.FromFile(Path.Combine(imagesRoot, @"London_Bauman.png"))),
                CurrentMember = true,},

                new CastMember{Name = "Jacquelle Davis", MainRole = Enum.PositionEnum.Actor,
                Bio = "Jacquelle Davis is a proud Portland native and member of Theatre Vertigo. " +
                "She studied acting at Willamette University. Jacquelle performs regularly with her beloved improv group, " +
                "No Filter. Her favorite roles include Jane Fonda in That Pretty Pretty; " +
                "Or, The Rape Play, and Box Worker 2 in Box. Jacquelle loves puns and pickles..",
                Photo = ImageUploadController.InsertPhoto(Image.FromFile(Path.Combine(imagesRoot, @"JacQuelle_Davis.jpg"))),
                CurrentMember = true, },

                new CastMember{Name = "Adriana Gantzer", MainRole = Enum.PositionEnum.Actor,
                Bio = "Adriana has been a huge fan of Theatre Vertigo for many years and feels so fortunate to become " +
                "a part of this incredible company. She has been acting on stage for over a decade and has been a " +
                "full-time voiceover actor for over 4 years. Some favorite past roles include: Andy in A Dark Sky " +
                "Full of Stars, Adriana’s Theatre Vertigo debut; Matilde in The Clean House, Germaine in Picasso at " +
                "the Lapin Agile, and Georgeanne in Five Women Wearing the Same Dress. In her four years in Portland " +
                "she has worked with Milagro, NORTHWEST THEATRE WORKSHOP, Mask & Mirror, and Twilight theaters, " +
                "and at Prospect Theater Project in her hometown of Modesto, CA.",
                Photo = ImageUploadController.InsertPhoto(Image.FromFile(Path.Combine(imagesRoot, @"Adriana_Gantzer.jpg"))),
                CurrentMember = true, },

                new CastMember{Name = "Clara-Liis Hillier", MainRole = Enum.PositionEnum.Actor,
                Bio = "Clara-Liis is a graduate of Reed College. A proud company member of Theatre Vertigo, " +
                "she is also a past resident actor for Bag&Baggage. She was last seen in Caucasian Chalk Circle " +
                "at Shaking the Tree, Gyspy at Broadway Rose, Godspell at Lakewood Theater " +
                "(Drammy Award for Supporting Actress), world premiere Carnivora as Woodwoman " +
                "(Theatre Vertigo) and as the Wicked Witch of the West in The Wizard of Oz (NW Children's Theater). " +
                "Favorite roles: Graeae Sister in Up The Fall with PHAME, Wait Until Dark (Susan Hendrix) with NWCTC, " +
                "Our Country's Good (Liz Morden) and Julius Caesar (Casca) with Bag&Baggage; The Seagull (Masha) with " +
                "NWCTC. When she's not onstage, Clara-Liis works for Portland Center Stage at The Armory as their" +
                " Education & Community Programs Associate and teaches Dance and Theater for NW Children's Theater " +
                "and Riverdale High School. Thank you to Heath K. for his love and patience and Mom and " +
                "Kaia for their strength and inspiration. For Ted.",
                Photo = ImageUploadController.InsertPhoto(Image.FromFile(Path.Combine(imagesRoot, @"Clara_Liis_Hillier.jpg"))),
                CurrentMember = true, },

                new CastMember{Name = "Kaia Maarja Hillier", MainRole = Enum.PositionEnum.Actor,
                Bio = "Kaia is a current Theatre Vertigo company member, a Board and Company member of the Pulp Stage, " +
                "a No Filter Improv Troupe member, and Costume Designer. Past acting credits include: Jessica in A Maze" +
                " (Theatre Vertigo); Nora in Assistance (Theatre Vertigo); and April in The Best of Everything (Bag &Baggage)." +
                " Kaia has had SO much fun performing with the new ensemble in readings that celebrate Vertigo's " +
                "rich artistic past. Thank you to everyone who has come out to support the new ensemble and to " +
                "helping keep Theatre Vertigo and the Shoebox thriving-we need these space to stay alive and " +
                "let our community grow and share their art. Much love to Mom, the Ensemble, the Associate Artists, " +
                "Clara, and JQ.",
                Photo = ImageUploadController.InsertPhoto(Image.FromFile(Path.Combine(imagesRoot, @"Kaia_Maarja_Hillier.jpg"))),
                CurrentMember = true, },

                new CastMember{Name = "Heath Hyun Houghton", MainRole = Enum.PositionEnum.Actor,
                Bio = "A Korean American actor, writer and director.  He previously appeared with Theatre Vertigo in Assistance;" +
                " other Portland credits include work with Imago Theatre, Portland Shakespeare Project, Broadway Rose Theatre" +
                ", and many more.  Exploring the relationships between the sciences and the arts is a focal point of his work" +
                " as a collaborator and educator.",
                Photo = ImageUploadController.InsertPhoto(Image.FromFile(Path.Combine(imagesRoot, @"Heath_Hyun_Houghton.jpg"))),
                CurrentMember = true, },

                new CastMember{Name = "Tom Mounsey", YearJoined= 2012, MainRole = Enum.PositionEnum.Actor,
                Bio = "Tom found a passion for theatre and performance in his late 20s thanks to a class at Portland" +
                " Actors Conservatory, and has been acting in and around Portland since his graduation in 2008. " +
                "You might have seen him on stage at places like defunkt theatre, Imago Theatre, Northwest Classical " +
                "Theatre Collaborative, Action/Adventure Theatre, Lakewood Center for the Arts, Clackamas Repertory " +
                "Theatre, and of course, Theatre Vertigo. Tom was a member of Theatre Vertigo from 2012 to 2017, " +
                "and is very excited to be back as part of this amazing company.",
                Photo = ImageUploadController.InsertPhoto(Image.FromFile(Path.Combine(imagesRoot, @"Tom_Mounsey.jpg"))),
                CurrentMember = true, },

                new CastMember{Name = "Devon Roberts", MainRole = Enum.PositionEnum.Actor,
                Bio = "Devon Roberts is a born and raised Portland director and actor. He holds a BA of Theater Arts " +
                "from Portland State University and is an alumnus of the Orchard Project Core Company. He has worked" +
                " with local companies: Boom Arts, Fuse Theatre Ensemble, Portland Center Stage at The Armory and out" +
                " of state: such as The Civilians, Tectonic Theater Project, Pig Iron and at the Edinburgh Fringe Festival." +
                " When Devon isn’t working on and off stage, he can be found enjoying the local cuisine, or soaking up" +
                " the natural beauty of Oregon. Devon is thankful for the opportunity to join the Vertigo Ensemble!",
                Photo = ImageUploadController.InsertPhoto(Image.FromFile(Path.Combine(imagesRoot, @"Devon_Roberts.jpg"))),
                CurrentMember = true, },

            };

            castMembers.ForEach(castMember => context.CastMembers.AddOrUpdate(c => c.Name, castMember));
            context.SaveChanges();
        }

		//Seeding database with dummy Productions
		private void SeedProductions()
		{
            
            var productions = new List<Production>
            {
                new Production{Title = "Hamilton", Playwright = "Lin-Manuel Miranda", Description = "This is a musical inspired by the biography " +
                "Alexander Hamilton by historian Ron Chernow. This musical tells the story of American Founding Father Alexander Hamilton through music " +
                "that draws heavily from hip hop, as well as R&B, pop, soul, and traditional-style show tune. ", OpeningDay = new DateTime(2020, 01, 01, 19, 30, 00),
                ClosingDay = new DateTime(2020, 02, 29, 19, 30, 00), ShowtimeEve = new DateTime(2020, 01, 02, 19, 30, 00) , ShowtimeMat = new DateTime(2020, 01, 02, 14, 30, 00),
                TicketLink = "ticketsforyou.com", Season = 23, IsCurrent = true},

                new Production{Title = "Phantom of the Opera", Playwright = "Andrew Lloyd Webber & Charles Hart", Description = "Based on a French " +
                "novel of the same name by Gaston Leroux, its central plot revolves around a beautiful soprano, Christine Daae, who becomes the obesession " +
                "of a mysterious, disfigured musical genius living in the subterranean labyrinth beneath the Paris Opera House.", OpeningDay = new DateTime(2019, 10, 01, 17, 30, 00),
                ClosingDay = new DateTime(2019, 11, 30, 17, 30, 00), ShowtimeEve = new DateTime(2019, 10, 04, 17, 30, 00), ShowtimeMat = new DateTime(2019, 10, 04, 12, 30, 00),
                TicketLink = "ticketsforyou.com", Season = 23, IsCurrent = true},

                new Production{Title = "The Book of Mormon", Playwright = "Trey Parker, Robert, Lopez, & Matt Stone", Description = "The Book of Mormon " +
                "follows two Latter-Day Saints missionaries as they attempt to preach the faith of the Church of Jesus Christ of Latter-Day Saints to the " +
                "inhabitants of a remote Ugandan village.", OpeningDay = new DateTime(2021, 01, 01, 19, 30, 00), ClosingDay = new DateTime(2021, 02, 28, 19, 30, 00),
                ShowtimeEve = new DateTime(2021, 01, 02, 19, 30, 00), TicketLink = "ticketsforyou.com", Season = 24,
                IsCurrent = false},

				new Production{Title = "Wicked", Playwright = "Stephen Schwartz", Description = "This musical is told from the perspective of the witches of " +
				"the Land of Oz; its plot begins before and continues after Dorothy Gale arrives in Oz from Kansas, and includes several references to the 1939 film.",
				OpeningDay = new DateTime(2020, 10, 01, 19, 30, 00), ClosingDay = new DateTime(2020, 11, 30, 19, 30, 00),
				ShowtimeMat = new DateTime(2020, 10, 01, 14, 30, 00), TicketLink = "ticketsforyou.com", Season = 24, IsCurrent = false},

				new Production{Title = "How to Succeed in Business Without Really Trying", Playwright = "Frank Loesser", Description = "This story concerns young, " +
                "ambitious J. Pierrepont Finch, who, with the help of the book How to Succeed in Business Without Really Trying, rises from window washer to chairman of " +
                "the board of the World Wide Wicket Company.", OpeningDay = new DateTime(2020, 04, 01, 19, 30, 00), ClosingDay = new DateTime(2020, 05, 31, 19, 30, 00),
                ShowtimeEve = new DateTime(2020, 04, 01, 19, 30, 00), ShowtimeMat = new DateTime(2020, 04, 01, 14, 30, 00), TicketLink = "ticketsforyou.com", Season = 23,
                IsCurrent = true}
			};
            
            productions.ForEach(Production => context.Productions.AddOrUpdate(d => d.Title, Production));
            context.SaveChanges();

        }

        
        private void SeedProductionPhotos()
        {
            var converter = new ImageConverter();
            // create images first
            string imagesRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\Content\Images");

            Image image1 = Image.FromFile(Path.Combine(imagesRoot, @"hamilton1.png"));
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
