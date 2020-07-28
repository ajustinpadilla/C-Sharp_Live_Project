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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Ajax.Utilities;
using System.Web.Caching;

[assembly: OwinStartupAttribute(typeof(TheatreCMS.Startup))]
namespace TheatreCMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
            SeedCastPhotos();
            SeedCastMembers();
            SeedProductions();
            SeedProductionPhotos();
            SeedParts();
            SeedAwards();
        }


        private ApplicationDbContext context = new ApplicationDbContext();

        //create method for default roles and Admin users for login
        private void createRolesandUsers()
        {


            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //create Admin role on startup if none exist
            
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
        // I created a new method to seed the castmember images. I placed it before seedCastMembers() in order for those objects to reference the Photo database table
        private void SeedCastPhotos()
        {
            var converter = new ImageConverter();
            // create images first
            string imagesRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\Content\Images");

            Image image1 = Image.FromFile(Path.Combine(imagesRoot, @"London_Bauman.png"));
            Image image2 = Image.FromFile(Path.Combine(imagesRoot, @"JacQuelle_Davis.jpg"));
            Image image3 = Image.FromFile(Path.Combine(imagesRoot, @"Adriana_Gantzer.jpg"));
            Image image4 = Image.FromFile(Path.Combine(imagesRoot, @"Clara_Liis_Hillier.jpg"));
            Image image5 = Image.FromFile(Path.Combine(imagesRoot, @"Kaia_Maarja_Hillier.jpg"));
            Image image6 = Image.FromFile(Path.Combine(imagesRoot, @"Heath_Hyun_Houghton.jpg"));
            Image image7 = Image.FromFile(Path.Combine(imagesRoot, @"Tom_Mounsey.jpg"));
            Image image8 = Image.FromFile(Path.Combine(imagesRoot, @"Devon_Roberts.jpg"));

            var photos = new List<Photo>
            {
               new Photo
                {
                    OriginalHeight = image1.Height,
                    OriginalWidth = image1.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image1, typeof(byte[])),
                    Title = "London Bauman"
                },
                new Photo
                {
                    OriginalHeight = image2.Height,
                    OriginalWidth = image2.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image2, typeof(byte[])),
                    Title = "Jacquelle Davis"
                },
                new Photo
                {
                    OriginalHeight = image3.Height,
                    OriginalWidth = image3.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image3, typeof(byte[])),
                    Title = "Adriana Gantzer"
                },
                new Photo
                {
                    OriginalHeight = image4.Height,
                    OriginalWidth = image4.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image4, typeof(byte[])),
                    Title = "Clara-Liis Hillier"
                },
                new Photo
                {
                    OriginalHeight = image5.Height,
                    OriginalWidth = image5.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image5, typeof(byte[])),
                    Title = "Kaia Maarja Hillier"
                },
                new Photo
                {
                    OriginalHeight = image6.Height,
                    OriginalWidth = image6.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image6, typeof(byte[])),
                    Title = "Heath Hyun Houghton"
                },
                new Photo
                {
                    OriginalHeight = image7.Height,
                    OriginalWidth = image7.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image7, typeof(byte[])),
                    Title = "Tom Mounsey"
                },
                new Photo
                {
                    OriginalHeight = image8.Height,
                    OriginalWidth = image8.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image8, typeof(byte[])),
                    Title = "Devon Roberts"
                }
            };
            photos.ForEach(Photo => context.Photo.AddOrUpdate(p => p.PhotoFile, Photo));
            context.SaveChanges();
        }



        //Seeding database with dummy CastMembers
        private void SeedCastMembers()
        {
            //Add photos of cast members
            //string imagesRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\Content\Images");

            var castMembers = new List<CastMember>
            {
                // For each cast member I replaced CastMember.Photo with CastMember.PhotoId and assigned it the value of the corresponding Photo.PhotoId 
                new CastMember{Name = "London Bauman", MainRole = Enum.PositionEnum.Actor,
                Bio = "London Bauman is an actor, writer, sound designer, and Theatre Vertigo company member. " +
                "As an artist, he is interested in immersive physical theatre, magical realism, and new collaborative works." +
                " Selected work includes the role of Torch in Beirut (The Steep & Thorny Way to Heaven), " +
                "Barnaby the Barkeep in the devised Western Melodrama Bang! (Action/Adventure’s pilot season) " +
                "and sound design / original compositions for The Romeo and Juliet Project (Enso Theatre Ensemble). " +
                "In August, London will be traveling to the Edinburgh Fringe Festival in Scotland as Robert in Chet Wilson’s new play Gayface.",
                PhotoId = context.Photo.Where(photo => photo.Title == "London Bauman").FirstOrDefault().PhotoId, 
                CurrentMember = true,},

                new CastMember{Name = "Jacquelle Davis", MainRole = Enum.PositionEnum.Actor,
                Bio = "Jacquelle Davis is a proud Portland native and member of Theatre Vertigo. " +
                "She studied acting at Willamette University. Jacquelle performs regularly with her beloved improv group, " +
                "No Filter. Her favorite roles include Jane Fonda in That Pretty Pretty; " +
                "Or, The Rape Play, and Box Worker 2 in Box. Jacquelle loves puns and pickles..",
                PhotoId = context.Photo.Where(photo => photo.Title == "Jacquelle Davis").FirstOrDefault().PhotoId,
                CurrentMember = true, },

                new CastMember{Name = "Adriana Gantzer", MainRole = Enum.PositionEnum.Actor,
                Bio = "Adriana has been a huge fan of Theatre Vertigo for many years and feels so fortunate to become " +
                "a part of this incredible company. She has been acting on stage for over a decade and has been a " +
                "full-time voiceover actor for over 4 years. Some favorite past roles include: Andy in A Dark Sky " +
                "Full of Stars, Adriana’s Theatre Vertigo debut; Matilde in The Clean House, Germaine in Picasso at " +
                "the Lapin Agile, and Georgeanne in Five Women Wearing the Same Dress. In her four years in Portland " +
                "she has worked with Milagro, NORTHWEST THEATRE WORKSHOP, Mask & Mirror, and Twilight theaters, " +
                "and at Prospect Theater Project in her hometown of Modesto, CA.",
                PhotoId = context.Photo.Where(photo => photo.Title == "Adriana Gantzer").FirstOrDefault().PhotoId,
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
                PhotoId = context.Photo.Where(photo => photo.Title == "Clara-Liis Hillier").FirstOrDefault().PhotoId,
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
                PhotoId = context.Photo.Where(photo => photo.Title == "Kaia Maarja Hillier").FirstOrDefault().PhotoId,
                CurrentMember = true, },

                new CastMember{Name = "Heath Hyun Houghton", MainRole = Enum.PositionEnum.Actor,
                Bio = "A Korean American actor, writer and director.  He previously appeared with Theatre Vertigo in Assistance;" +
                " other Portland credits include work with Imago Theatre, Portland Shakespeare Project, Broadway Rose Theatre" +
                ", and many more.  Exploring the relationships between the sciences and the arts is a focal point of his work" +
                " as a collaborator and educator.",
                PhotoId = context.Photo.Where(photo => photo.Title == "Heath Hyun Houghton").FirstOrDefault().PhotoId,
                CurrentMember = true, },

                new CastMember{Name = "Tom Mounsey", YearJoined= 2012, MainRole = Enum.PositionEnum.Actor,
                Bio = "Tom found a passion for theatre and performance in his late 20s thanks to a class at Portland" +
                " Actors Conservatory, and has been acting in and around Portland since his graduation in 2008. " +
                "You might have seen him on stage at places like defunkt theatre, Imago Theatre, Northwest Classical " +
                "Theatre Collaborative, Action/Adventure Theatre, Lakewood Center for the Arts, Clackamas Repertory " +
                "Theatre, and of course, Theatre Vertigo. Tom was a member of Theatre Vertigo from 2012 to 2017, " +
                "and is very excited to be back as part of this amazing company.",
                PhotoId = context.Photo.Where(photo => photo.Title == "Tom Mounsey").FirstOrDefault().PhotoId,
                CurrentMember = true, },

                new CastMember{Name = "Devon Roberts", MainRole = Enum.PositionEnum.Actor,
                Bio = "Devon Roberts is a born and raised Portland director and actor. He holds a BA of Theater Arts " +
                "from Portland State University and is an alumnus of the Orchard Project Core Company. He has worked" +
                " with local companies: Boom Arts, Fuse Theatre Ensemble, Portland Center Stage at The Armory and out" +
                " of state: such as The Civilians, Tectonic Theater Project, Pig Iron and at the Edinburgh Fringe Festival." +
                " When Devon isn’t working on and off stage, he can be found enjoying the local cuisine, or soaking up" +
                " the natural beauty of Oregon. Devon is thankful for the opportunity to join the Vertigo Ensemble!",
                PhotoId = context.Photo.Where(photo => photo.Title == "Devon Roberts").FirstOrDefault().PhotoId,
                CurrentMember = true, },

                };

            castMembers.ForEach(castMember => context.CastMembers.AddOrUpdate(c => c.Name, castMember));
            context.SaveChanges();
        }

        //Seeding database with dummy SeedAwards
        private void SeedAwards()
        {
            var awards = new List<Award>
            {
                #region SeedAwardsData
                new Award
                {
                    Year = 2017, Name = "Drammy", Type = AwardType.Finalist, Category = "Sound Design", Recipient = "Andrew Bray",
                          ProductionId = context.Productions.Where(prod => prod.Title == "Assistance").FirstOrDefault().ProductionId,
                          //CastMemberId = context.CastMembers.Where(p => p.Name == "London Bauman").FirstOrDefault().CastMemberID,



                },
                 new Award
                {
                    Year = 2016, Name = "Drammy", Type = AwardType.Award, Category = "Outstanding Achievement", Recipient = "Scenic Artist,Mindy Barker",
                          ProductionId = context.Productions.Where(prod => prod.Title == "The Drunken City").FirstOrDefault().ProductionId,
                          
                },
                new Award
                {
                    Year = 2016, Name = "Drammy", Type = AwardType.Finalist, Category = "Sound Design", Recipient = "Richard E.Moore",
                          ProductionId = context.Productions.Where(prod => prod.Title == "The Drunken City").FirstOrDefault().ProductionId,
                },
                new Award
                {
                    Year = 2015, Name = "Drammy", Type = AwardType.Award, Category = "Ensemble in a Play", 
                          ProductionId = context.Productions.Where(prod => prod.Title == "Bob: A Life in Five Acts").FirstOrDefault().ProductionId,
                },
                new Award
                {
                    Year = 2015, Name = "Drammy", Type = AwardType.Award, Category = "Direction", Recipient = "Matthew B.Zrebski",
                          ProductionId = context.Productions.Where(prod => prod.Title == "Bob: A Life in Five Acts").FirstOrDefault().ProductionId ,
                },
                new Award
                {
                    Year = 2015, Name = "Drammy", Type = AwardType.Finalist, Category = "Ensemble in a Play",
                          ProductionId = context.Productions.Where(prod => prod.Title == "The School for Lies").FirstOrDefault().ProductionId,
                },
                new Award
                {
                    Year = 2014, Name = "Drammy", Type = AwardType.Award, Category = "Sound Design", Recipient = "Annalise Albright Woods",
                          ProductionId = context.Productions.Where(prod => prod.Title == "pool (no water)").FirstOrDefault().ProductionId,
                },
                new Award
                {
                    Year = 2014, Name = "Drammy", Type = AwardType.Finalist, Category = "Ensemble in a play ",
                          ProductionId = context.Productions.Where(prod => prod.Title == "pool (no water)").FirstOrDefault().ProductionId,
                },
                new Award
                {
                    Year = 2013, Name = "Drammy", Type = AwardType.Award, Category = "Actress in a supporting role", Recipient ="Brooke Calcagno",
                          ProductionId = context.Productions.Where(prod => prod.Title == "Mother Courage & Her Children").FirstOrDefault().ProductionId,
                },
                new Award
                {
                    Year = 2013, Name = "Drammy", Type = AwardType.Award, Category = "Sound design", Recipient ="Richard Moore",
                          ProductionId = context.Productions.Where(prod => prod.Title == "The Velvet Sky").FirstOrDefault().ProductionId,
                },
                new Award
                {
                    Year = 2012, Name = "Drammy", Type = AwardType.Award, Category = "Actor in a land role", Recipient ="Mario Calcagno",
                          ProductionId = context.Productions.Where(prod => prod.Title == "The American Pilot").FirstOrDefault().ProductionId,
                },
                new Award
                {
                    Year = 2012, Name = "Drammy", Type = AwardType.Award, Category = "Sound design", Recipient ="Em Gustason",
                          ProductionId = context.Productions.Where(prod => prod.Title == "The American Pilot").FirstOrDefault().ProductionId,
                },
                 new Award
                {
                    Year = 2010, Name = "Drammy", Type = AwardType.Award, Category = "Supporting Actress", Recipient ="Amy Newman",
                          ProductionId = context.Productions.Where(prod => prod.Title == "God's Ear").FirstOrDefault().ProductionId,
                },
                new Award
                {
                    Year = 2009, Name = "Drammy", Type = AwardType.Award, Category = "Supporting Actor", Recipient ="Garland Lyons",
                          ProductionId = context.Productions.Where(prod => prod.Title == "Romance").FirstOrDefault().ProductionId,
                },
                new Award
                {
                    Year = 2008, Name = "Drammy", Type = AwardType.Award, Category = "Outstanding puppeteering", OtherInfo = "in collaboration with Tears of Joy Theatre",
                          ProductionId = context.Productions.Where(prod => prod.Title == "The Long Christmas Ride Home").FirstOrDefault().ProductionId,
                },
                new Award
                {
                    Year = 2007, Name = "Drammy", Type = AwardType.Award, Category = "Set design", Recipient ="Ben Plont",
                          ProductionId = context.Productions.Where(prod => prod.Title == "Escape from Happiness").FirstOrDefault().ProductionId,
                },
                //new Award
                //{
                //    Year = 2001, Name = "Drammy", Type = AwardType.Award, Category = "Supporting Actress", Recipient ="Andrea White",
                //          ProductionId = context.Productions.Where(prod => prod.Title == "Hellcab").FirstOrDefault().ProductionId,
                //},
                //new Award
                //{
                //    Year = 2001, Name = "Drammy", Type = AwardType.Award, Category = "Supporting Actress", Recipient ="Lorraine Bahr",
                //          ProductionId = context.Productions.Where(prod => prod.Title == "Lion in the Streets").FirstOrDefault().ProductionId,
                //},
                //new Award
                //{
                //    Year = 2001, Name = "Drammy", Type = AwardType.Award, Category = "Supporting Actress", Recipient ="Ted Schulz",
                //          ProductionId = context.Productions.Where(prod => prod.Title == "The Grey Zone").FirstOrDefault().ProductionId,
                //},
                //new Award
                //{
                //    Year = 2000, Name = "Drammy", Type = AwardType.Award, Category = "Outstanding direction", Recipient ="Michael Griggs",
                //          ProductionId = context.Productions.Where(prod => prod.Title == "The Grey Zone").FirstOrDefault().ProductionId,
                //},
                #endregion
            };

            //awards.ForEach(award => context.Awards.AddOrUpdate(aw => aw.AwardId, award));

            awards.ForEach(award => context.Awards.AddOrUpdate(a => new { a.Year, a.Name, a.Type, a.Category }, award));
            context.SaveChanges();
        }

        //Seeding database with dummy Productions
        private void SeedProductions()
        {
            var productions = new List<Production>
            {
                #region SeedProductionsData
                new Production{Title = "Hamilton", Playwright = "Lin-Manuel Miranda", Description = "This is a musical inspired by the biography " +
                "Alexander Hamilton by historian Ron Chernow. This musical tells the story of American Founding Father Alexander Hamilton through music " +
                "that draws heavily from hip hop, as well as R&B, pop, soul, and traditional-style show tune. ", Runtime=90, OpeningDay = new DateTime(2020, 01, 01, 19, 30, 00),
                ClosingDay = new DateTime(2020, 02, 29, 19, 30, 00), ShowtimeEve = new DateTime(2020, 01, 02, 19, 30, 00) , ShowtimeMat = new DateTime(2020, 01, 02, 14, 30, 00),
                TicketLink = "ticketsforyou.com", Season = 23, IsCurrent = true},

                new Production{Title = "Phantom of the Opera", Playwright = "Andrew Lloyd Webber & Charles Hart", Description = "Based on a French " +
                "novel of the same name by Gaston Leroux, its central plot revolves around a beautiful soprano, Christine Daae, who becomes the obesession " +
                "of a mysterious, disfigured musical genius living in the subterranean labyrinth beneath the Paris Opera House.", Runtime=90, OpeningDay = new DateTime(2019, 10, 01, 17, 30, 00),
                ClosingDay = new DateTime(2019, 11, 30, 17, 30, 00), ShowtimeEve = new DateTime(2019, 10, 04, 17, 30, 00), ShowtimeMat = new DateTime(2019, 10, 04, 12, 30, 00),
                TicketLink = "ticketsforyou.com", Season = 23, IsCurrent = true},

                new Production{Title = "The Book of Mormon", Playwright = "Trey Parker, Robert Lopez, & Matt Stone", Description = "The Book of Mormon " +
                "follows two Latter-Day Saints missionaries as they attempt to preach the faith of the Church of Jesus Christ of Latter-Day Saints to the " +
                "inhabitants of a remote Ugandan village.", Runtime=90, OpeningDay = new DateTime(2021, 01, 01, 19, 30, 00), ClosingDay = new DateTime(2021, 02, 28, 19, 30, 00),
                ShowtimeEve = new DateTime(2021, 01, 02, 19, 30, 00), ShowtimeMat = new DateTime(2021, 01, 02, 14, 30, 00), TicketLink = "ticketsforyou.com", Season = 24,
                IsCurrent = false},

				new Production{Title = "Wicked", Playwright = "Stephen Schwartz", Description = "This musical is told from the perspective of the witches of " +
				"the Land of Oz; its plot begins before and continues after Dorothy Gale arrives in Oz from Kansas, and includes several references to the 1939 film.", Runtime=90,
				OpeningDay = new DateTime(2020, 10, 01, 19, 30, 00), ClosingDay = new DateTime(2020, 11, 30, 19, 30, 00),
				ShowtimeEve = new DateTime(2020, 10, 01, 19, 30, 00), ShowtimeMat = new DateTime(2020, 10, 01, 14, 30, 00), TicketLink = "ticketsforyou.com", Season = 24, IsCurrent = false},

                new Production{Title = "How to Succeed in Business Without Really Trying", Playwright = "Frank Loesser", Description = "This story concerns young, " +
                "ambitious J. Pierrepont Finch, who, with the help of the book How to Succeed in Business Without Really Trying, rises from window washer to chairman of " +
                "the board of the World Wide Wicket Company.", Runtime=90, OpeningDay = new DateTime(2020, 04, 01, 19, 30, 00), ClosingDay = new DateTime(2020, 05, 31, 19, 30, 00),
                ShowtimeEve = new DateTime(2020, 04, 01, 19, 30, 00), ShowtimeMat = new DateTime(2020, 04, 01, 14, 30, 00), TicketLink = "ticketsforyou.com", Season = 23, IsCurrent = true},

                // productions for season 20
                new Production{Title = "Assistance", Playwright = "Leslye Headland", Description = "The small army of administrative assistants belonging to Daniel " +
                "Weisinger, a famous and powerful business mogul, are a mixed bag of motives: suave, aggressive Vince despises his boss; the underdogs, clumsy Heather " +
                "and bitter Justin, are devoted to their jobs; obnoxiously competent Jenny is eager and confident that she will surpass her co-workers. Nick and Nora flirt " +
                "and joke to distract themselves from their unhappiness. But each assistant shares a deep ambition, desire for status, and twisted allegiance to their abusive" +
                " boss, even in the face of volatile power plays, unreasonable demands, and physical injury. Leslye Headland’s biting, fast-paced comedy Assistance, part of " +
                "her “Seven Deadly Sins” play cycle, explores greed and power both professional and personal, and the terrible, if hilarious, toll they can take on those who " +
                "are caught up in their pursuit and practice.",
                OpeningDay = new DateTime(2012, 05, 30, 14, 30, 00), ClosingDay = new DateTime(2012, 08, 30, 19, 30, 00), ShowtimeEve = new DateTime(2020, 05, 30, 19, 30, 00),
                ShowtimeMat = new DateTime(2012, 05, 30, 15, 30, 00), TicketLink = "ticketsforyou.com", Season = 20, IsCurrent = false, Runtime = 120},

                new Production{Title = "Carnivora", Playwright = "Matthew B. Zrebski", Description = "Some of the dangerous seductions in “Carnivora” come courtesy of a very" +
                " un-mythic, albeit charismatic, cult leader (played by Dunkin) whose obsession is yet another fear, of sorts, that Zrebski has addressed. “Texting the Sun,”" +
                " a 2010 piece for the OCT/Kaiser Permanente Educational Theatre Program, dealt with concerns about a generation growing up fully immersed in interactive " +
                "technology and social media. In “Carnivora,” add in suspicions about the egocentric dynamics of an all-pervasive celebrity culture, and you have a " +
                "semi-coherent vision of society on the brink. Serve that up with an unhealthy dose of messianism, to folks frustrated by weakening economic and social-value" +
                " structures around them, and a bizarre cult web snags even the well-educated and well-meaning Lorraine.",
                OpeningDay = new DateTime(2012, 09, 01, 14, 30, 00), ClosingDay = new DateTime(2012, 12, 30, 19, 30, 00), ShowtimeEve = new DateTime(2012, 09, 01, 19, 30, 00),
                ShowtimeMat = new DateTime(2012, 12, 30, 15, 30, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 20, IsCurrent = false},

                new Production{Title = "A Maze", Playwright = "Rob Handel", Description = "There are two kinds of mazes - a fairy tale-like king explains to his queen in " +
                "Rob Handel's \"A Maze\" - the kind you try to get through from one side to the other, and the kind where you either try to get from the entrance to the " +
                "center or from the center to the exit. The confounding and stimulating beauty of Handel's \"Maze\" is that it manages to be all those at once, and something" +
                " altogether different - a maze in which three paths may intersect or even merge.",
                OpeningDay = new DateTime(2013, 01, 02, 14, 30, 00), ClosingDay = new DateTime(2013, 03, 30, 15, 30, 00), ShowtimeEve = new DateTime(2012, 01, 02, 19, 30, 00),
                ShowtimeMat = new DateTime(2013, 01, 02, 15, 30, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 20, IsCurrent = false},

                // productions for season 19
                new Production{Title = "The Drunken City", Playwright = "Adam Bock", Description = "Off on the bar crawl to end all crawls, three twenty-something brides-to-be" +
                " find their lives going topsy-turvy when one of them begins to question her future after a chance encounter with a recently jilted handsome stranger. " +
                "The Drunken City is a wildly theatrical take on the mystique of marriage and the ever-shifting nature of love and identity in a city that never sleeps.",
                OpeningDay = new DateTime(2013, 05, 30, 14, 30, 00), ClosingDay = new DateTime(2013, 08, 30, 19, 30, 00), ShowtimeEve = new DateTime(2013, 05, 30, 19, 30, 00),
                ShowtimeMat = new DateTime(2013, 05, 30, 15, 30, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 19, IsCurrent = false},

                new Production{Title = "I Want to Destroy You", Playwright = "Rob Handel", Description = "Life’s not going especially well for Harold, a sort-of-famous artist" +
                " who’s now teaching grad students at a richer-than-thou private college, and he really likes it, actually, but there are … problems. His ex-wife is out of " +
                "the picture somewhere – California, it sounds like – and his teenage daughter, Micki, comes to visit, prodding him for more of a relationship than he seems" +
                " willing to commit to. His roof’s got a bad leak, and he’s unfortunately seriously ticked off the roofer, Andy. He’s up for tenure, but his friend and mentor" +
                " Bob is in a hospital, dying, and the school dean, a crafty-smooth politico named Stephanie (everyone’s on a chummy first-name basis around here, even when " +
                "they’re decidedly not chums), seems strangely unsympathetic: downright threatening, you might say. Then there’s Mark, the weirdo grad student, who comes to" +
                " class to give a presentation on a conceptual piece, and in the process starts waving a handgun around. Which very much freaks out the other students, earnest" +
                " Ilich and leafy Leaf, and throws a serious scare into Harold, which is both completely understandable and a tad ironic, because, after all, the work that " +
                "made Harold famous and a prize catch for the richer-than-thou college in the first place was the performance piece where he had himself shot. With a rifle." +
                " What goes around, as they say, comes around. And on the other end of things, it looks scary.",
                OpeningDay = new DateTime(2013, 09, 01, 14, 30, 00), ClosingDay = new DateTime(2013, 12, 30, 19, 30, 00), ShowtimeEve = new DateTime(2013, 09, 01, 19, 30, 00),
                ShowtimeMat = new DateTime(2013, 09, 01, 15, 30, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 19, IsWorldPremiere= true, IsCurrent = false},

                new Production{Title = "Love and Information", Playwright = "Caryl Churchill", Description = "Someone sneezes. Someone can’t get a signal. Someone won’t " +
                "answer the door. Someone put an elephant on the stairs. Someone’s not ready to talk. Someone is her brother’s mother. Someone hates irrational numbers. " +
                "Someone told the police. Someone got a message from the traffic light. Someone’s never felt like this before.\n In this fast moving kaleidoscope, more " +
                "than a hundred characters try to make sense of what they know.",
                OpeningDay = new DateTime(2014, 01, 02, 14, 30, 00), ClosingDay = new DateTime(2014, 03, 30, 19, 30, 00), ShowtimeEve = new DateTime(2014, 01, 02, 19, 30, 00),
                ShowtimeMat = new DateTime(2014, 01, 02, 15, 30, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 19, IsCurrent = false},

                // productions for season 18
                new Production{Title = "Bob: A Life in Five Acts", Playwright = "Peter Sinn Nachtrieb", Description = "BOB chronicles the highly unusual life of Bob and his" +
                " lifelong quest to become a “Great Man.” Born and abandoned in the bathroom of a fast food restaurant, Bob energetically embarks on an epic journey across" +
                " America and encounters inspiring generosity, crushing hardships, blissful happiness, stunning coincidences, wrong turns, lucky breaks, true love and " +
                "heartbreaking loss. Along the way, Bob meets a myriad of fellow countrymen all struggling to find their own place in the hullaballoo of it all. Will Bob’s" +
                " real life ever be able to live up to his dream? BOB is a comedic exploration of American mythology and values, the treacherous pursuit of happiness, and" +
                " discovering what it means to be truly “great.\"",
                OpeningDay = new DateTime(2014, 05, 30, 14, 30, 00), ClosingDay = new DateTime(2014, 08, 30, 19, 30, 00), ShowtimeEve = new DateTime(2014, 05, 30, 19, 30, 00),
                ShowtimeMat = new DateTime(2014, 05, 30, 15, 30, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 18, IsCurrent = false},

                new Production{Title = "Sexual Neuroses of Our Parents", Playwright = "(translated from Lukas Bärfuss) by Neil Blackadder", Description = "Dora has been " +
                "taking tranquilizers for years because otherwise her behavior was uncontrollably wild. The drugs have kept her in a semi-comatose state, and now Dora’s" +
                " mother wants to know what her daughter is really like and convinces their doctor to take Dora off the drugs. Dora develops an enormous hunger for life," +
                " demonstrates her own will, and, above all, discovers her sexuality—to a degree which far exceeds adult ideas about how she should live.",
                OpeningDay = new DateTime(2014, 09, 01, 14, 30, 00), ClosingDay = new DateTime(2014, 12, 30, 19, 30, 00), ShowtimeEve = new DateTime(2012, 09, 01, 19, 30, 00),
                ShowtimeMat = new DateTime(2014, 09, 01, 15, 30, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 18, IsCurrent = false},

                new Production{Title = "The School for Lies", Playwright = "David Ives", Description = "Adapted from Le Misanthrope by Molière. It’s 1666 and the brightest," +
                " wittiest salon in Paris is that of Celimene, a beautiful young widow so known for her satiric tongue she’s being sued for it. Surrounded by shallow " +
                "suitors, whom she lives off of without surrendering to, Celimene has managed to evade love since her beloved husband died—until today, when Frank appears. " +
                "A traveler from England known for his own coruscating wit and acidic misanthropy, Frank turns Celimene’s world upside-down, taking on her suitors," +
                " matching her barb for barb, and teaching her how to live again. (Never mind that their love affair has been engineered by a couple of well-placed lies.) " +
                "This wild farce of furious tempo and stunning verbal display, all in very contemporary couplets, runs variations on Molière’s The Misanthrope, which " +
                "inspired it. Another incomparable romp from the brilliant author of All in the Timing.",
                OpeningDay = new DateTime(2015, 01, 02, 14, 30, 00), ClosingDay = new DateTime(2015, 03, 30, 19, 30, 00), ShowtimeEve = new DateTime(2015, 01, 02, 19, 30, 00),
                ShowtimeMat = new DateTime(2015, 01, 02, 15, 30, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 18, IsCurrent = false},

                // productions for season 17
                new Production{Title = "Dr. Jekyll and Mr. Hyde", Playwright = "Jeffrey Hatcher", Description = "Some of the strongest scenes here are between Hyde and his" +
                " love interestt, a fiery chambermaid named Elizabeth. In a twist on the Jekyll and Hyde story added by playwright Jeffrey Hatcher, Elizabeth sees through" +
                " Hyde’s anger and falls hopelessly in love with him. Their romantic scenes let us see a new and highly relatable side of Jekyll’s alter ego as the character " +
                "struggles to overcome his fears to be with the person he loves. It helps that Wennstrom and Koerschgen, whose interactions are backed only by the sound of" +
                " a pounding heartbeat, have palpable, natural chemistry.",
                OpeningDay = new DateTime(2015, 05, 30, 14, 30, 00), ClosingDay = new DateTime(2015, 08, 30, 19, 30, 00), ShowtimeEve = new DateTime(2015, 05, 30, 19, 30, 00),
                ShowtimeMat = new DateTime(2015, 05, 30, 15, 30, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 17, IsCurrent = false},

                new Production{Title = "The End of Sex", Playwright = "Craig Jessen", Description = "The story centers around Sam (Stephanie Cordell) who is the head of " +
                "one team in a research lab, attempting to find a safe cream or pill that will increase sexual activity for those having trouble in that area.  She is " +
                "aided by her able assistant, Zoe (Beth Thompson) and, together, are trying to be the first team to find that secret, so that they will get the fame and " +
                "monies due them.",
                OpeningDay = new DateTime(2015, 09, 01, 14, 30, 00), ClosingDay = new DateTime(2015, 12, 30, 19, 30, 00), ShowtimeEve = new DateTime(2015, 09, 01, 19, 30, 00),
                ShowtimeMat = new DateTime(2015, 09, 01, 15, 30, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 17, IsCurrent = false},

                new Production{Title = "pool (no water)", Playwright = "Mark Ravenhill", Description = "A famous artist invites her old friends to her luxurious new home." +
                " For one night only, the group is back together. But celebrations come to an abrupt end when the host suffers an horrific accident. As the victim lies in" +
                " a coma, an almost unthinkable plan starts to take shape: could her suffering be their next work of art? Pool (No Water) is a visceral and shocking new" +
                " play about the fragility of friendship and the jealousy and resentment inspired by success.",
                OpeningDay = new DateTime(2016, 01, 02, 14, 30, 00), ClosingDay = new DateTime(2016, 03, 30, 19, 30, 00), ShowtimeEve = new DateTime(2016, 01, 02, 19, 30, 00),
                ShowtimeMat = new DateTime(2016, 01, 02, 15, 30, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 17, IsCurrent = false},

                new Production{Title = "Anonymous Theatre:  The Crucible", Playwright = "Arthur Miller", Description = "Salem, Massachusetts—1692.  Rigid piety huddles on" +
                " the edge of the new world’s wilderness.\nIts inhabitants believe unquestioningly in their own sanctity, but in Arthur Miller’s edgy masterpiece, that very" +
                " belief will have poisonous consequences when a vengeful teenager accuses a rival of witchcraft—and then those accusations multiply to consume the entire village.",
                OpeningDay = new DateTime(2016, 04, 01, 14, 30, 00), ClosingDay = new DateTime(2016, 05, 29, 19, 30, 00), ShowtimeEve = new DateTime(2016, 04, 01, 19, 30, 00),
                ShowtimeMat = new DateTime(2016, 04, 01, 15, 30, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 17, IsCurrent = false},

                // productions for season 16
                new Production{Title = "Mother Courage & Her Children", Playwright = "(translated from Bertolt Brecht) by Tony Kushner", Description = "The indomitable" +
                " Mother Courage follows one luckless army after another across a war-torn world in her canteen wagon. She’ll do anything to hold onto her money-making " +
                "wagon, even if it means the loss of her children.",
                OpeningDay = new DateTime(2016, 05, 30, 14, 30, 00), ClosingDay = new DateTime(2016, 08, 30, 19, 30, 00), ShowtimeEve = new DateTime(2020, 08, 30, 19, 30, 00),
                ShowtimeMat = new DateTime(2016, 05, 30, 15, 30, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 16, IsCurrent = false},

                new Production{Title = "The Velvet Sky", Playwright = "Roberto Aguirre-Sacasa", Description = "Poor Bethany Palmer hasn’t slept in thirteen years. When her" +
                " husband, Warren, steals their son, Andrew, away in the middle of the night, her already fragile grip on reality starts to weaken—even as she sets off after" +
                " them on a nightmarish phantasmagoria through an urban dreamscape. Chronicling Bethany’s desperate flight, THE VELVET SKY is a dark fairytale for grown-ups," +
                " about the stories and lies adults tell children to keep them safe from the things that lurk in the dark. Things like the macabre Sandman, who is hungry to" +
                " steal the innocent gleam from young Andrew’s eyes…",
                OpeningDay = new DateTime(2016, 09, 01, 14, 30, 00), ClosingDay = new DateTime(2016, 12, 30, 19, 30, 00), ShowtimeEve = new DateTime(2016, 09, 01, 19, 30, 00),
                ShowtimeMat = new DateTime(2016, 09, 01, 15, 30, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 16, IsCurrent = false},

                new Production{Title = "Aloha, Say the Pretty Girls", Playwright = "Naomi Iizuka", Description = "Aloha, Say the Pretty Girls is a quirky journey that " +
                "follows a bunch of twenty-somethings in their quest for love and identity. Strangers, friends, lovers, and acquaintances travel the globe from Alaska to" +
                " Hawaii and from New York City to Inner Borneo, exploring themes of migration, evolution, and interconnectivity. Babies, wild dogs, komodo dragons, and" +
                " hula dancers abound in this hilarious play about finding your tribe in a world gone haywire.",
                OpeningDay = new DateTime(2017, 01, 02, 14, 30, 00), ClosingDay = new DateTime(2017, 03, 30, 19, 30, 00), ShowtimeEve = new DateTime(2017, 01, 02, 19, 30, 00),
                ShowtimeMat = new DateTime(2017, 01, 02, 15, 30, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 16, IsCurrent = false},

                new Production{Title = "Anonymous Theatre:  The Skin of Our Teeth", Playwright = "Thornton Wilder", Description = "This comedic masterpiece spans the " +
                "entirety of history, with one ordinary American family who lives through it all. Dad’s just invented the wheel, Cain is throwing rocks at the neighbor " +
                "kid, mammoths and dinosaurs lounge in the family room and mom frets about how to get all those animals on the boat two by two.\nThrough Ice Ages, biblical " +
                "floods and political conventions, the Antrobus family of Excelsior, New Jersey perseveres. With a giant cast and time-set across the ages, this theatrical" +
                " allegory captures the human spirit – of brilliance, idiocy and ultimately sweet survival.",
                OpeningDay = new DateTime(2017, 04, 01, 14, 30, 00), ClosingDay = new DateTime(2017, 05, 28, 19, 30, 00), ShowtimeEve = new DateTime(2017, 04, 01, 19, 30, 00),
                ShowtimeMat = new DateTime(2017, 04, 01, 15, 30, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 16, IsCurrent = false},

                //productions for season 15

                new Production{Title = "Cloud 9", Playwright = "Caryl Churchill", Description = "Clive, A British colonial administrator, lives with his family, " +
                "a governess and servant during turbulent times in Africa. The natives are rioting and Mrs Saunders, a widow, comes to them to seek safety. "+
                "Her arrival is soon followed by Harry Bagley, an explorer. The governess Ellen, who reveals herself to be a lesbian, is forced into marriage " +
                "with Harry after his sexuality is discovered and condemned by Clive.",
                OpeningDay = new DateTime(2011, 06, 01, 14, 00, 00), ClosingDay = new DateTime(2011, 06, 30, 19, 00, 00), ShowtimeEve = new DateTime(2011, 06, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2011, 06, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 165, Season = 15, IsCurrent = false},

                new Production{Title = "Hunter Gatherers", Playwright = "Peter Sinn Nachtrieb", Description = "Pam and Richard are hosting their best friends, " +
                "Wendy and Tom, for an annual dinner get-together. An animal sacrifice kicks off the evening, followed by a little more sex, violence, deception, " +
                "wrestling, and dancing than at previous parties. A darkly comic evening where the line between civilized and primal man is blurred, " +
                "and where not everyone will survive long enough to enjoy the brownies for dessert." ,
                OpeningDay = new DateTime(2011, 08, 01, 14, 00, 00), ClosingDay = new DateTime(2011, 08, 30, 19, 00, 00), ShowtimeEve = new DateTime(2011, 08, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2011, 08, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime =120, Season = 15, IsCurrent = false},

                new Production{Title = "The American Pilot", Playwright = "David Greig", Description = "An American pilot has crash-landed in a distant country "+ 
                "rent by civil war. Since the Americans fund the country's oppressive government and the pilot has landed in rural, rebel territory, " +
                "he represents both a temptation and an opportunity." ,
                OpeningDay = new DateTime(2011, 10, 01, 14, 00, 00), ClosingDay = new DateTime(2011, 11, 01, 19, 00, 00), ShowtimeEve = new DateTime(2011, 10, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2011, 10, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 90, Season = 15, IsCurrent = false},

                new Production{Title = "Anonymous Theatre: The Good Doctor", Playwright = "Neil Simon", Description = "A combination of Neil Simon and Chekhov, "+ 
                "The Good Doctor, a comedy with music, is, by turns, charming, hilarious, sad, and touching. It centers around a writer, "+ 
                "who speaks to the audience and shares his writing with them, throughout one day. He presents them with a plethora of scenes: some are from his childhood, " +
                "others are his family and friends, and still others are his own life experiences. The audience meets a variety of characters, all of whom " +
                "are immediately relatable and strikingly human." ,
                OpeningDay = new DateTime(2011, 12, 01, 14, 00, 00), ClosingDay = new DateTime(2011, 12, 30, 19, 00, 00), ShowtimeEve = new DateTime(2011, 12, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2011, 12, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 15, IsCurrent = false},

                // productions for season 14

                new Production{Title = "Dead Man's Cell Phone", Playwright = "Sarah Ruhl", Description = "Dead Man's Cell Phone explores the paradox of modern " +
                "technology's ability to both unite and isolate people in the digital age. The play was awarded a Helen Hayes Award for Outstanding New Play.",
                OpeningDay = new DateTime(2010, 6, 01, 14, 00, 00), ClosingDay = new DateTime(2010, 6, 30, 14, 00, 00), ShowtimeEve = new DateTime(2010, 6, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2010, 6, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 14, IsCurrent = false},

                new Production{Title = "99 Ways to Fuck a Swan", Playwright = "Kim Rosenstock", Description = "A long, long time ago, Leda makes love to a swan. 3,000 years later, "+
                "Michelangelo paints a picture. 350 years later, Rudolph buys it. 130 years later, Dave and Fiona stand in a museum, gazing at what remains. " +
                "Set in a world of bizarre romantic obsessions and everyday ineptitude, 99 WAYS TO FUCK A SWAN explores the dark corners of desire and the eternal mysteries of love",
                OpeningDay = new DateTime(2010, 8, 01, 14, 00, 00), ClosingDay = new DateTime(2010, 8, 30, 14, 00, 00), ShowtimeEve = new DateTime(2010, 8, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2010, 6, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 14, IsCurrent = false},

                new Production{Title = "The Adding Machine", Playwright = "Elmer Rice", Description = "Elmer Rice’s The Adding Machine centers around the life, death, and afterlife, "+
                "of a man named Zero. He and his wife live in a society dominated by reverence for financial gain and opportunism, with an emphasis on morality and rigid determinations " +
                "of what is right and wrong. The day after Mrs. Zero successfully plants a seed of ambition in her husband, Mr. Zero is fired from his monotonous accounting job. " +
                "Having come in for the day hoping to ask for a raise for his consistent work, Zero’s spirits are crushed when his boss tells him he is being replaced by an adding machine.",
                OpeningDay = new DateTime(2010, 10, 01, 14, 00, 00), ClosingDay = new DateTime(2010, 10, 30, 14, 00, 00), ShowtimeEve = new DateTime(2010, 10, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2010, 10, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 100, Season = 14, IsCurrent = false},

                new Production{Title = "Anonymous Theatre: You Can't Take It with You", Playwright = "George S. Kaufman and Moss Hart", Description = "Family can do crazy things to people." +
                "And the Sycamore family is a little crazy to begin with. Wily Grandpa Vanderhof, heads the wacky Sycamore household and is also a leader of a happily eccentric gang " +
                "of snake collectors, cunning revolutionaries, ballet dancers and skyrocket makers. But when the youngest daughter brings her fiancé and his buttoned-up parents over " +
                "for dinner, that’s when the real fireworks start to fly.",
                OpeningDay = new DateTime(2011, 1, 01, 14, 00, 00), ClosingDay = new DateTime(2011, 1, 31, 14, 00, 00), ShowtimeEve = new DateTime(2011, 1, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2011, 1, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 14, IsCurrent = false},

                // productions for Season 13

                new Production{Title = "The Ruby Sunrise", Playwright = "Rinne Groff", Description = "Setting off from a farm in Indiana as a young girl Ruby struggles to turn " +
                "her dream of the first all-electrical television system into a reality. This play also jumps forward to a McCarthy-era New York TV studio where Ruby’s heirs fight over "+
                "how her story should be told.",
                OpeningDay = new DateTime(2009, 6, 01, 14, 00, 00), ClosingDay = new DateTime(2009, 6, 30, 14, 00, 00), ShowtimeEve = new DateTime(2009, 6, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2009, 6, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 13, IsCurrent = false},

                new Production{Title = "Boom", Playwright = "Peter Sinn Nachtrieb", Description = "Jules, a grad student in marine biology, and Jo, a journalism student, "+
                "meet one Saturday night in Jules’s small underground laboratory on a university campus, after Jo answers Jules’s online personal ad offering an encounter that promises "+
                "/“to change the course of the world./” During his research on a deserted tropical island, Jules discovers patterns among the behavior of fish that perdict the end of most earthly life. " +
                "He then turns his tiny lab/apartment into a place to wait out the disaster and begin remaking humanity.",
                OpeningDay = new DateTime(2009, 8, 01, 14, 00, 00), ClosingDay = new DateTime(2009, 8, 30, 14, 00, 00), ShowtimeEve = new DateTime(2009, 8, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2009, 8, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 13, IsCurrent = false},

                new Production{Title = "God's Ear", Playwright = "Jenny Schwartz", Description = "God's Ear follows a family in the aftermath of the tragic drowning of their son. " +
                "The characters try their hardest to reach out to one another, but socially prescribed behaviors and language, meant to help them, never quite do. Though not entirely " +
                "set in reality, God's Ear is honest and genuine in its exploration of grief. ",
                OpeningDay = new DateTime(2009, 10, 01, 14, 00, 00), ClosingDay = new DateTime(2009, 10, 30, 14, 00, 00), ShowtimeEve = new DateTime(2009, 10, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2009, 10, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 13, IsCurrent = false},

                new Production{Title = "Anonymous Theatre: Lend Me A Tenor", Playwright = "Ken Ludwig", Description = "World-renowned tenor Tito Merelli has signed on to play Otello " +
                "at a Cleveland opera company in the fall of 1934. He arrives late and, through a set of crazy circumstances, passes out after mixing wine with a huge dose of tranquilizers. "+
                "Believing that the divo is dead, the excitable opera manager taps his hapless assistant, an aspiring singer named Max, to suit up as the Moor and replace Merelli. "+
                "Meanwhile, the tenor’s jealous wife, his ambitious female co-star, Max’s young girlfriend and the flirtatious head of the opera guild are on the scene fighting "+
                "for the star’s attention.",
                OpeningDay = new DateTime(2010, 1, 01, 14, 00, 00), ClosingDay = new DateTime(2010, 1, 31, 14, 00, 00), ShowtimeEve = new DateTime(2010, 1, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2010, 1, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 13, IsCurrent = false},



                // productions for Season 12

                new Production{Title = "Pterodactyls", Playwright = "Nicky Silver", Description = "Pterodactyls is an absurdist black comedy about the demise of the Duncan family, "+
                "and, by extension, the species. Emma Duncan, a hypochondriac with memory problems, and her orphaned fiancé, Tommy, confront her mother, Grace, with the news of their "+
                "intended marriage. Disapproving at first, Grace acquiesces and puts Tommy to work as a maid",
                OpeningDay = new DateTime(2008, 6, 01, 14, 00, 00), ClosingDay = new DateTime(2008, 6, 30, 14, 00, 00), ShowtimeEve = new DateTime(2008, 6, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2008, 6, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 12, IsCurrent = false},

                new Production{Title = "Romance", Playwright = "David Mamet", Description = "Pulitzer Prize—winning playwright David Mamet's Romance is an uproarious, take-no-prisoners "+
                "courtroom comedy that gleefully lampoons everyone from lawyers and judges, to Arabs and Jews, to gays and chiropractors. It's hay fever season, and in a courtroom "+
                "a judge is popping antihistamines.",
                OpeningDay = new DateTime(2008, 8, 01, 14, 00, 00), ClosingDay = new DateTime(2008, 8, 30, 14, 00, 00), ShowtimeEve = new DateTime(2008, 8, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2008, 8, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 12, IsCurrent = false},

                new Production{Title = "Freakshow", Playwright = "Carson Kreitzer", Description = "At the turn of the last century, a traveling Freakshow grinds to a halt. "+
                "Things are changing. The anger of being stared at, trapped, caged, is at war with the comfort of knowing your place in the universe. But the growing defiance "+
                "of the freaks is no act. Will the Dog Faced Woman break her bonds from the show, collapsing the fragile bubble of sustaining interdependence? "+
                "Can the jaded Ringmaster, a profoundly broken man, find redemption through his love for the Woman With No Arms and No Legs? In this poetic, gritty world, "+
                "there may only be one way out.",
                OpeningDay = new DateTime(2008, 10, 01, 14, 00, 00), ClosingDay = new DateTime(2008, 10, 30, 14, 00, 00), ShowtimeEve = new DateTime(2008, 10, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2008, 10, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 12, IsCurrent = false},

                new Production{Title = "Anonymous Theatre: Macbeth", Playwright = "William Shakespeare", Description = "Macbeth is a Scottish general and the thane of Glamis "+
                "who is led to wicked thoughts by the prophecies of the three witches, especially after their prophecy that he will be made thane of Cawdor comes true. "+
                "Macbeth is a brave soldier and a powerful man, but he is not a virtuous one.",
                OpeningDay = new DateTime(2009, 1, 01, 14, 00, 00), ClosingDay = new DateTime(2009, 1, 31, 14, 00, 00), ShowtimeEve = new DateTime(2009, 1, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2009, 1, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 12, IsCurrent = false},


                // productions for Season 11

                new Production{Title = "A Doll's House  ", Playwright = "Henrik Ibsen", Description = "The play centres on an ordinary family—Torvald Helmer, a bank lawyer, "+
                "his wife Nora, and their three little children. Torvald supposes himself the ethical member of the family, while his wife assumes the role of the pretty " +
                "and irresponsible little woman in order to flatter him.",
                OpeningDay = new DateTime(2007, 6, 01, 14, 00, 00), ClosingDay = new DateTime(2007, 6, 30, 14, 00, 00), ShowtimeEve = new DateTime(2007, 6, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2007, 6, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 11, IsCurrent = false},

                new Production{Title = "Where's My Money? ", Playwright = "John Patrick Shanley", Description = "When Celeste, an out-of-work actor who's cheating on her boyfriend " +
                "with a married man, runs into Natalie, whom she hasn't seen in years, the two have some catching up to do. Natalie, an accountant married to a lawyer, "+
                "gives the impression of being very together and does not approve of Celeste's lifestyle.",
                OpeningDay = new DateTime(2007, 8, 01, 14, 00, 00), ClosingDay = new DateTime(2007, 8, 30, 14, 00, 00), ShowtimeEve = new DateTime(2007, 8, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2007, 8, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 11, IsCurrent = false},

                new Production{Title = "The Long Christmas Ride Home", Playwright = "Paula Vogel", Description = "The Long Christmas Ride Home dramatises a road trip by two parents " +
                "and their three young children to visit grandparents for the Christmas holiday, and the emotional turmoil that they undergo. A significant element of the production schema " +
                "is a Western, contemporary employment of bunraku, an ancient form of Japanese puppetry",
                OpeningDay = new DateTime(2007, 10, 01, 14, 00, 00), ClosingDay = new DateTime(2007, 10, 30, 14, 00, 00), ShowtimeEve = new DateTime(2007, 10, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2007, 10, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 11, IsCurrent = false},

                new Production{Title = "Anonymous Theatre: Rumors", Playwright = "Neil Simon", Description = "The play starts with Ken Gorman and his wife, Chris Gorman, " +  
                "at the 10th anniversary party of Charlie Brock, the Deputy Mayor of New York, and his wife, Myra. Unfortunately, things are not going quite to plan. " +
                "All the kitchen staff are gone, Myra is missing, and Charlie has shot himself in the head",
                OpeningDay = new DateTime(2008, 1, 01, 14, 00, 00), ClosingDay = new DateTime(2008, 1, 31, 14, 00, 00), ShowtimeEve = new DateTime(2008, 1, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2008, 1, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 11, IsCurrent = false},



                // productions for Season 10
                new Production{Title = "Valparaiso", Playwright = "Don DeLillo", Description = "Valparaiso is Don DeLillo 's second play, in which a man suddenly "+
                "becomes famous following a mistake in the itinerary of an ordinary business trip which takes him to Valparaíso, Chile, instead of Valparaiso, Indiana. " +
                "The 1999 play, which incorporates live performance with video projection, looks at how the media has affected modern mankind.",
                OpeningDay = new DateTime(2006, 6, 01, 14, 00, 00), ClosingDay = new DateTime(2006, 6, 30, 14, 00, 00), ShowtimeEve = new DateTime(2006, 6, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2006, 6, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 10, IsCurrent = false},

                new Production{Title = "Escape from Happiness", Playwright = "George F. Walker", Description = "Escape from Happiness takes place in the kitchen of an old, "+
                "slightly rundown house in a not-so-classy section of a large city. It's home to Nora, a good-natured, slow-moving, fairly batty middle-aged woman; "+
                "her daughter Gail, who is tough, sensible, and a little high-strung; Gail's husband Junior, an affable but rather dim fellow. Also living here is Tom, "+
                "who is dying of some unspecified disease; Tom is, according to Nora, a stranger who looks exactly like (and coincidentally has the same name as) her husband, "+
                "who deserted the family ten years ago after trying to burn down the house",
                OpeningDay = new DateTime(2006, 8, 01, 14, 00, 00), ClosingDay = new DateTime(2006, 8, 30, 14, 00, 00), ShowtimeEve = new DateTime(2006, 8, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2006, 8, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 10, IsCurrent = false},

                new Production{Title = "Tango", Playwright = "Stawomir Mrozek", Description = "Tango is set in a non-defined time in the home of Stomil and Eleonora, "+
                "parents of Artur. The place is disorganized not only in the way it is furnished, but also through the complete lack of house rules and common values. "+
                "Everyone can do whatever they want. It seems that perplexity may be the word that describes their lives best. Artur’s attempts are bound to fail, "+
                "but despite that he tries to make rules and grant some things meaning. Artur revolts against his father's slovenliness and his mother’s double "+
                "standards of morality.",
                OpeningDay = new DateTime(2006, 10, 01, 14, 00, 00), ClosingDay = new DateTime(2006, 10, 30, 14, 00, 00), ShowtimeEve = new DateTime(2006, 10, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2006, 10, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 10, IsCurrent = false},

                new Production{Title = "Anonymous Theatre: A Funny Thing Happened on the Way to the Forum", Playwright = "Larry Gelbart and Burt Shevelove", Description = "Inspired "+
                "by the farces of the ancient Roman playwright Plautus, the musical tells the bawdy story of a slave named Pseudolus and his attempts to win his freedom by helping "+
                "his young master woo the girl next door. The plot displays many classic elements of farce, including puns, the slamming of doors, cases of mistaken identity, and "+
                "satirical comments on social class.",
                OpeningDay = new DateTime(2007, 1, 01, 14, 00, 00), ClosingDay = new DateTime(2007, 1, 31, 14, 00, 00), ShowtimeEve = new DateTime(2006, 1, 01, 19, 00, 00),
                ShowtimeMat = new DateTime(2007, 1, 01, 14, 00, 00), TicketLink = "ticketsforyou.com", Runtime = 120, Season = 10, IsCurrent = false},
                #endregion
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
            // images for season 20
            Image image11 = Image.FromFile(Path.Combine(imagesRoot, @"assistance.jpg"));
            Image image12 = Image.FromFile(Path.Combine(imagesRoot, @"carnivora.jpg"));
            Image image13 = Image.FromFile(Path.Combine(imagesRoot, @"maze.jpg"));
            // images for season 19
            Image image14 = Image.FromFile(Path.Combine(imagesRoot, @"drunkenCity.jpg"));
            Image image15 = Image.FromFile(Path.Combine(imagesRoot, @"destroy.jpg"));
            Image image16 = Image.FromFile(Path.Combine(imagesRoot, @"love_and_information.jpg"));
            // images for season 18
            Image image17 = Image.FromFile(Path.Combine(imagesRoot, @"bob5.jpg"));
            Image image18 = Image.FromFile(Path.Combine(imagesRoot, @"sexual.jpg"));
            Image image19 = Image.FromFile(Path.Combine(imagesRoot, @"school_for_lies.jpg"));
            // images for season 17
            Image image20 = Image.FromFile(Path.Combine(imagesRoot, @"Hyde.jpg"));
            Image image21 = Image.FromFile(Path.Combine(imagesRoot, @"endsex.jpg"));
            Image image22 = Image.FromFile(Path.Combine(imagesRoot, @"pool.jpg"));
            Image image23 = Image.FromFile(Path.Combine(imagesRoot, @"crucible.jpg"));
            // images for season 16
            Image image24 = Image.FromFile(Path.Combine(imagesRoot, @"mother.jpg"));
            Image image25 = Image.FromFile(Path.Combine(imagesRoot, @"velvet.jpg"));
            Image image26 = Image.FromFile(Path.Combine(imagesRoot, @"aloha.jpg"));
            Image image27 = Image.FromFile(Path.Combine(imagesRoot, @"The_skin_of_our_teeth.jpg"));
            // images for season 15
            Image image28 = Image.FromFile(Path.Combine(imagesRoot, @"cloud9.jpg"));
            Image image29 = Image.FromFile(Path.Combine(imagesRoot, @"huntergatherers.jpg"));
            Image image30 = Image.FromFile(Path.Combine(imagesRoot, @"americanpilot.jpg"));
            Image image31 = Image.FromFile(Path.Combine(imagesRoot, @"doctor.jpg"));
            //images for season 14
            Image image32 = Image.FromFile(Path.Combine(imagesRoot, @"deadmanscellphone.jpg"));
            Image image33 = Image.FromFile(Path.Combine(imagesRoot, @"99ways.jpg"));
            Image image34 = Image.FromFile(Path.Combine(imagesRoot, @"addingmachine.jpg"));
            Image image35 = Image.FromFile(Path.Combine(imagesRoot, @"youcanttakeitwithyou.jpg"));
            //images for season 13
            Image image36 = Image.FromFile(Path.Combine(imagesRoot, @"ruby.jpg"));
            Image image37 = Image.FromFile(Path.Combine(imagesRoot, @"boomjpg.jpg"));
            Image image38 = Image.FromFile(Path.Combine(imagesRoot, @"godsear.jpg"));
            Image image39 = Image.FromFile(Path.Combine(imagesRoot, @"lendmeatenor.jpg"));
            //images for season 12
            Image image40 = Image.FromFile(Path.Combine(imagesRoot, @"pterodactyls.jpg"));
            Image image41 = Image.FromFile(Path.Combine(imagesRoot, @"Romance.jpg"));
            Image image42 = Image.FromFile(Path.Combine(imagesRoot, @"freakshow.jpg"));
            Image image43 = Image.FromFile(Path.Combine(imagesRoot, @"Macbeth.jpg"));
            //images for season 11
            Image image44 = Image.FromFile(Path.Combine(imagesRoot, @"dollshouse.jpg"));
            Image image45 = Image.FromFile(Path.Combine(imagesRoot, @"wheresmymoneyjpg.jpg"));
            Image image46 = Image.FromFile(Path.Combine(imagesRoot, @"longchristmasridehome.jpg"));
            Image image47 = Image.FromFile(Path.Combine(imagesRoot, @"Rumors.jpg"));
            ////images for season 10
            Image image48 = Image.FromFile(Path.Combine(imagesRoot, @"Valparaiso.jpg"));
            Image image49 = Image.FromFile(Path.Combine(imagesRoot, @"EscapefromHappiness.jpg"));
            Image image50 = Image.FromFile(Path.Combine(imagesRoot, @"Tango.jpg"));
            Image image51 = Image.FromFile(Path.Combine(imagesRoot, @"Forum.jpg"));

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
                // Production photos for season 20
                new Photo
                {
                    OriginalHeight = image11.Height,
                    OriginalWidth = image11.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image11, typeof(byte[])),
                    Title = "Assistance Image"
                },
                new Photo
                {
                    OriginalHeight = image12.Height,
                    OriginalWidth = image12.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image12, typeof(byte[])),
                    Title = "Carnivora Image"
                },
                new Photo
                {
                    OriginalHeight = image13.Height,
                    OriginalWidth = image13.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image13, typeof(byte[])),
                    Title = "A Maze Image"
                },
                // Production photos for season 19
                new Photo
                {
                    OriginalHeight = image14.Height,
                    OriginalWidth = image14.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image14, typeof(byte[])),
                    Title = "The Drunken City Image"
                },
                new Photo
                {
                    OriginalHeight = image15.Height,
                    OriginalWidth = image15.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image15, typeof(byte[])),
                    Title = "I Want to Destroy You Image"
                },
                new Photo
                {
                    OriginalHeight = image16.Height,
                    OriginalWidth = image16.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image16, typeof(byte[])),
                    Title = "Love and Information Image"
                },
                // Production photos for season 18
                new Photo
                {
                    OriginalHeight = image17.Height,
                    OriginalWidth = image17.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image17, typeof(byte[])),
                    Title = "Bob: A Life in Five Acts Image"
                },
                new Photo
                {
                    OriginalHeight = image18.Height,
                    OriginalWidth = image18.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image18, typeof(byte[])),
                    Title = "Sexual Neuroses of Our Parents Image"
                },
                new Photo
                {
                    OriginalHeight = image19.Height,
                    OriginalWidth = image19.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image19, typeof(byte[])),
                    Title = "The School for Lies Image"
                },
                // Production photos for season 17
                new Photo
                {
                    OriginalHeight = image20.Height,
                    OriginalWidth = image20.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image20, typeof(byte[])),
                    Title = "Dr. Jekyll and Mr. Hyde Image"
                },
                new Photo
                {
                    OriginalHeight = image21.Height,
                    OriginalWidth = image21.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image21, typeof(byte[])),
                    Title = "The End of Sex Image"
                },
                new Photo
                {
                    OriginalHeight = image22.Height,
                    OriginalWidth = image22.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image22, typeof(byte[])),
                    Title = "pool (no water) Image"
                },
                new Photo
                {
                    OriginalHeight = image23.Height,
                    OriginalWidth = image23.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image23, typeof(byte[])),
                    Title = "Anonymous Theatre:  The Crucible Image"
                },
                // Production photos for season 16
                new Photo
                {
                    OriginalHeight = image24.Height,
                    OriginalWidth = image24.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image24, typeof(byte[])),
                    Title = "Mother Courage & Her Children Image"
                },
                new Photo
                {
                    OriginalHeight = image25.Height,
                    OriginalWidth = image25.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image25, typeof(byte[])),
                    Title = "The Velvet Sky Image"
                },
                new Photo
                {
                    OriginalHeight = image26.Height,
                    OriginalWidth = image26.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image26, typeof(byte[])),
                    Title = "Aloha, Say the Pretty Girls Image"
                },
                new Photo
                {
                    OriginalHeight = image27.Height,
                    OriginalWidth = image27.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image27, typeof(byte[])),
                    Title = "Anonymous Theatre:  The Skin of Our Teeth Image"
                },
                 // Production photos for season 15
                new Photo
                {
                    OriginalHeight = image28.Height,
                    OriginalWidth = image28.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image28, typeof(byte[])),
                    Title = "Cloud 9"
                },
                new Photo
                {
                    OriginalHeight = image29.Height,
                    OriginalWidth = image29.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image29, typeof(byte[])),
                    Title = "Hunter Gatherers"
                },
                new Photo
                {
                    OriginalHeight = image30.Height,
                    OriginalWidth = image30.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image30, typeof(byte[])),
                    Title = "The American Pilot"
                },
                new Photo
                {
                    OriginalHeight = image31.Height,
                    OriginalWidth = image31.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image31, typeof(byte[])),
                    Title = "Anonymous Theatre: The Good Doctor"
                },
                      // Production photos for season 14
                new Photo
                {
                    OriginalHeight = image32.Height,
                    OriginalWidth = image32.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image32, typeof(byte[])),
                    Title = "Dead Man's Cell Phone"
                },
                new Photo
                {
                    OriginalHeight = image33.Height,
                    OriginalWidth = image33.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image33, typeof(byte[])),
                    Title = "99 Ways to Fuck a Swan"
                },
                new Photo
                {
                    OriginalHeight = image34.Height,
                    OriginalWidth = image34.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image34, typeof(byte[])),
                    Title = "The Adding Machine"
                },
                new Photo
                {
                    OriginalHeight = image35.Height,
                    OriginalWidth = image35.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image35, typeof(byte[])),
                    Title = "Anonymous Theatre: You Can't Take It with You"
                },

                // Production photos for season 13

                new Photo
                {
                    OriginalHeight = image36.Height,
                    OriginalWidth = image36.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image36, typeof(byte[])),
                    Title = "The Ruby Sunrise"
                },

                new Photo
                {
                    OriginalHeight = image37.Height,
                    OriginalWidth = image37.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image37, typeof(byte[])),
                    Title = "Boom"
                },

                new Photo
                {
                    OriginalHeight = image38.Height,
                    OriginalWidth = image38.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image38, typeof(byte[])),
                    Title = "God's Ear"
                },

                new Photo
                {
                    OriginalHeight = image39.Height,
                    OriginalWidth = image39.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image39, typeof(byte[])),
                    Title = "Anonymous Theatre: Lend Me a Tenor"
                },

                // Production photos for season 12

                new Photo
                {
                    OriginalHeight = image40.Height,
                    OriginalWidth = image40.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image40, typeof(byte[])),
                    Title = "Pterodactyls"
                },

                new Photo
                {
                    OriginalHeight = image41.Height,
                    OriginalWidth = image41.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image41, typeof(byte[])),
                    Title = "Romance"
                },

                new Photo
                {
                    OriginalHeight = image42.Height,
                    OriginalWidth = image42.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image42, typeof(byte[])),
                    Title = "Freakshow"
                },

                new Photo
                {
                    OriginalHeight = image43.Height,
                    OriginalWidth = image43.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image43, typeof(byte[])),
                    Title = "Anonymous Theatre: MacBeth"
                },

                // Production photos for season 11

                new Photo
                {
                    OriginalHeight = image44.Height,
                    OriginalWidth = image44.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image44, typeof(byte[])),
                    Title = "A Doll's House"
                },

                new Photo
                {
                    OriginalHeight = image45.Height,
                    OriginalWidth = image45.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image45, typeof(byte[])),
                    Title = "Where's My Money? "
                },

                new Photo
                {
                    OriginalHeight = image46.Height,
                    OriginalWidth = image46.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image46, typeof(byte[])),
                    Title = "The Long Christmas Ride Home"
                },

                new Photo
                {
                    OriginalHeight = image47.Height,
                    OriginalWidth = image47.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image47, typeof(byte[])),
                    Title = "Anonymous Theatre: Rumors"
                },
                // Production photos for season 10

                new Photo
                {
                    OriginalHeight = image48.Height,
                    OriginalWidth = image48.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image48, typeof(byte[])),
                    Title = "Valparaiso"
                },

                new Photo
                {
                    OriginalHeight = image49.Height,
                    OriginalWidth = image49.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image49, typeof(byte[])),
                    Title = "Escape from Happiness"
                },

                new Photo
                {
                    OriginalHeight = image50.Height,
                    OriginalWidth = image50.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image50, typeof(byte[])),
                    Title = "Tango"
                },

                new Photo
                {
                    OriginalHeight = image51.Height,
                    OriginalWidth = image51.Width,
                    PhotoFile = (byte[])converter.ConvertTo(image51, typeof(byte[])),
                    Title = "Anonymous Theatre: A Funny Thing Happened on the Way to the Forum"
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
                },
                // Production photos for season 20
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Assistance Image").FirstOrDefault().PhotoId,
                    Title = "Assistance",
                    Description = "Assistance cover",
                    Production = context.Productions.Where(name => name.Title == "Assistance").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Carnivora Image").FirstOrDefault().PhotoId,
                    Title = "Carnivora",
                    Description = "Carnivora cover",
                    Production = context.Productions.Where(name => name.Title == "Carnivora").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "A Maze Image").FirstOrDefault().PhotoId,
                    Title = "A Maze",
                    Description = "A Maze cover",
                    Production = context.Productions.Where(name => name.Title == "A Maze").FirstOrDefault()
                },
                // Production photos for season 19
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "The Drunken City Image").FirstOrDefault().PhotoId,
                    Title = "The Drunken City",
                    Description = "The Drunken City cover",
                    Production = context.Productions.Where(name => name.Title == "The Drunken City").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "I Want to Destroy You Image").FirstOrDefault().PhotoId,
                    Title = "I Want to Destroy You",
                    Description = "I Want to Destroy You cover",
                    Production = context.Productions.Where(name => name.Title == "I Want to Destroy You").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Love and Information Image").FirstOrDefault().PhotoId,
                    Title = "Love and Information",
                    Description = "Love and Information cover",
                    Production = context.Productions.Where(name => name.Title == "Love and Information").FirstOrDefault()
                },
                // Production photos for season 18
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Bob: A Life in Five Acts Image").FirstOrDefault().PhotoId,
                    Title = "Bob: A Life in Five Acts",
                    Description = "Bob: A Life in Five Acts cover",
                    Production = context.Productions.Where(name => name.Title == "Bob: A Life in Five Acts").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Sexual Neuroses of Our Parents Image").FirstOrDefault().PhotoId,
                    Title = "Sexual Neuroses of Our Parents",
                    Description = "Sexual Neuroses of Our Parents cover",
                    Production = context.Productions.Where(name => name.Title == "Sexual Neuroses of Our Parents").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "The School for Lies Image").FirstOrDefault().PhotoId,
                    Title = "The School for Lies",
                    Description = "The School for Lies cover",
                    Production = context.Productions.Where(name => name.Title == "The School for Lies").FirstOrDefault()
                },
                // Production photos for season 17
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Dr. Jekyll and Mr. Hyde Image").FirstOrDefault().PhotoId,
                    Title = "Dr. Jekyll and Mr. Hyde",
                    Description = "Dr. Jekyll and Mr. Hyde cover",
                    Production = context.Productions.Where(name => name.Title == "Dr. Jekyll and Mr. Hyde").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "The End of Sex Image").FirstOrDefault().PhotoId,
                    Title = "The End of Sex",
                    Description = "The End of Sex cover",
                    Production = context.Productions.Where(name => name.Title == "The End of Sex").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "pool (no water) Image").FirstOrDefault().PhotoId,
                    Title = "pool (no water)",
                    Description = "pool (no water) cover",
                    Production = context.Productions.Where(name => name.Title == "pool (no water)").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Anonymous Theatre:  The Crucible Image").FirstOrDefault().PhotoId,
                    Title = "Anonymous Theatre:  The Crucible",
                    Description = "Anonymous Theatre:  The Crucible cover",
                    Production = context.Productions.Where(name => name.Title == "Anonymous Theatre:  The Crucible").FirstOrDefault()
                },
                // Production photos for season 16
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Mother Courage & Her Children Image").FirstOrDefault().PhotoId,
                    Title = "Mother Courage & Her Children",
                    Description = "Mother Courage & Her Children cover",
                    Production = context.Productions.Where(name => name.Title == "Mother Courage & Her Children").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "The Velvet Sky Image").FirstOrDefault().PhotoId,
                    Title = "The Velvet Sky",
                    Description = "The Velvet Sky cover",
                    Production = context.Productions.Where(name => name.Title == "The Velvet Sky").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Aloha, Say the Pretty Girls Image").FirstOrDefault().PhotoId,
                    Title = "Aloha, Say the Pretty Girls",
                    Description = "Aloha, Say the Pretty Girls cover",
                    Production = context.Productions.Where(name => name.Title == "Aloha, Say the Pretty Girls").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Anonymous Theatre:  The Skin of Our Teeth Image").FirstOrDefault().PhotoId,
                    Title = "Anonymous Theatre:  The Skin of Our Teeth",
                    Description = "Anonymous Theatre:  The Skin of Our Teeth cover",
                    Production = context.Productions.Where(name => name.Title == "Anonymous Theatre:  The Skin of Our Teeth").FirstOrDefault()
                },
                 // Production photos for season 15

                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Cloud 9").FirstOrDefault().PhotoId,
                    Title = "Cloud 9",
                    Description = "Cloud 9 cover",
                    Production = context.Productions.Where(name => name.Title == "Cloud 9").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Hunter Gatherers").FirstOrDefault().PhotoId,
                    Title = "Hunter Gatherers",
                    Description = "Hunter Gatherers cover",
                    Production = context.Productions.Where(name => name.Title == "Hunter Gatherers").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "The American Pilot").FirstOrDefault().PhotoId,
                    Title = "The American Pilot",
                    Description = "The American Pilot cover",
                    Production = context.Productions.Where(name => name.Title == "The American Pilot").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Anonymous Theatre: The Good Doctor").FirstOrDefault().PhotoId,
                    Title = "Anonymous Theatre: The Good Doctor",
                    Description = "Anonymous Theatre: The Good Doctor",
                    Production = context.Productions.Where(name => name.Title == "Anonymous Theatre: The Good Doctor").FirstOrDefault()
                },

                 // Production photos for season 14
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Dead Man's Cell Phone").FirstOrDefault().PhotoId,
                    Title = "Dead Man's Cell Phone",
                    Description = "Dead Man's Cell Phone",
                    Production = context.Productions.Where(name => name.Title == "Dead Man's Cell Phone").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "99 Ways to Fuck a Swan").FirstOrDefault().PhotoId,
                    Title = "99 Ways to Fuck a Swan",
                    Description = "99 Ways to Fuck a Swan cover",
                    Production = context.Productions.Where(name => name.Title == "99 Ways to Fuck a Swan").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "The Adding Machine").FirstOrDefault().PhotoId,
                    Title = "The Adding Machine",
                    Description = "The Adding Machine cover",
                    Production = context.Productions.Where(name => name.Title == "The Adding Machine").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Anonymous Theatre: You Can't Take It with You").FirstOrDefault().PhotoId,
                    Title = "Anonymous Theatre: You Can't Take It with You",
                    Description = "Anonymous Theatre: You Can't Take It with You",
                    Production = context.Productions.Where(name => name.Title == "Anonymous Theatre: You Can't Take It with You").FirstOrDefault()
                },

                 // Production photos for season 13
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "The Ruby Sunrise").FirstOrDefault().PhotoId,
                    Title = "The Ruby Sunrise",
                    Description = "The Ruby Sunrise",
                    Production = context.Productions.Where(name => name.Title == "The Ruby Sunrise").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Boom").FirstOrDefault().PhotoId,
                    Title = "Boom",
                    Description = "Boom",
                    Production = context.Productions.Where(name => name.Title == "Boom").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "God's Ear").FirstOrDefault().PhotoId,
                    Title = "God's Ear",
                    Description = "God's Ear",
                    Production = context.Productions.Where(name => name.Title == "God's Ear").FirstOrDefault()
                },
                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Anonymous Theatre: Lend Me a Tenor").FirstOrDefault().PhotoId,
                    Title = "Anonymous Theatre: Lend Me a Tenor",
                    Description = "Anonymous Theatre: Lend Me a Tenor",
                    Production = context.Productions.Where(name => name.Title == "Anonymous Theatre: Lend Me a Tenor").FirstOrDefault()
                },

                 // Production photos for season 12

                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Pterodactyls").FirstOrDefault().PhotoId,
                    Title = "Pterodactyls",
                    Description = "Pterodactyls",
                    Production = context.Productions.Where(name => name.Title == "Pterodactyls").FirstOrDefault()
                },

                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Romance").FirstOrDefault().PhotoId,
                    Title = "Romance",
                    Description = "Romance",
                    Production = context.Productions.Where(name => name.Title == "Romance").FirstOrDefault()
                },

                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Freakshow").FirstOrDefault().PhotoId,
                    Title = "Freakshow",
                    Description = "Freakshow",
                    Production = context.Productions.Where(name => name.Title == "Freakshow").FirstOrDefault()
                },

                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Anonymous Theatre: MacBeth").FirstOrDefault().PhotoId,
                    Title = "Anonymous Theatre: MacBeth",
                    Description = "Anonymous Theatre: MacBeth",
                    Production = context.Productions.Where(name => name.Title == "Anonymous Theatre: MacBeth").FirstOrDefault()
                },

                 // Production photos for season 11

                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "A Doll's House").FirstOrDefault().PhotoId,
                    Title = "A Doll's House",
                    Description = "A Doll's House",
                    Production = context.Productions.Where(name => name.Title == "A Doll's House").FirstOrDefault()
                },

                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Where's My Money? ").FirstOrDefault().PhotoId,
                    Title = "Where's My Money? ",
                    Description = "Where's My Money? ",
                    Production = context.Productions.Where(name => name.Title == "Where's My Money? ").FirstOrDefault()
                },

                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "The Long Christmas Ride Home").FirstOrDefault().PhotoId,
                    Title = "The Long Christmas Ride Home",
                    Description = "The Long Christmas Ride Home",
                    Production = context.Productions.Where(name => name.Title == "The Long Christmas Ride Home").FirstOrDefault()
                },

                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Anonymous Theatre: Rumors").FirstOrDefault().PhotoId,
                    Title = "Anonymous Theatre: Rumors",
                    Description = "Anonymous Theatre: Rumors",
                    Production = context.Productions.Where(name => name.Title == "Anonymous Theatre: Rumors").FirstOrDefault()
                },

                // Production photos for season 10

                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Valparaiso").FirstOrDefault().PhotoId,
                    Title = "Valparaiso",
                    Description = "Valparaiso",
                    Production = context.Productions.Where(name => name.Title == "Valparaiso").FirstOrDefault()
                },

                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Escape from Happiness").FirstOrDefault().PhotoId,
                    Title = "Escape from Happiness",
                    Description = "Escape from Happiness",
                    Production = context.Productions.Where(name => name.Title == "Escape from Happiness").FirstOrDefault()
                },

                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Tango").FirstOrDefault().PhotoId,
                    Title = "Tango",
                    Description = "Tango",
                    Production = context.Productions.Where(name => name.Title == "Tango").FirstOrDefault()
                },

                new ProductionPhotos
                {
                    PhotoId = context.Photo.Where(photo => photo.Title == "Anonymous Theatre: A Funny Thing Happened on the Way to the Forum").FirstOrDefault().PhotoId,
                    Title = "Anonymous Theatre: A Funny Thing Happened on the Way to the Forum",
                    Description = "Anonymous Theatre: A Funny Thing Happened on the Way to the Forum",
                    Production = context.Productions.Where(name => name.Title == "Anonymous Theatre: A Funny Thing Happened on the Way to the Forum").FirstOrDefault()
                },
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

        //Seeding database with dummy Parts
        private void SeedParts()
        {

            var parts = new List<Part> 
            {
                new Part{
                    Production = context.Productions.Where(p => p.Title == "Hamilton").FirstOrDefault(), 
                    Character="Alexander Hamilton", 
                    Type=Enum.PositionEnum.Actor, 
                    Person= context.CastMembers.Where(c => c.Name == "London Bauman").FirstOrDefault(), 
                    Details="The ten-dollar Founding Father without a father, Alexander Hamilton starts out as a penniless immigrant but " +
                    "rises up in the ranks and becomes an aide to George Washington himself. After the American Revolution, he becomes one of " +
                    "the most prominent politicians of the young United States, creating USA's financial system, writing in defense of the " +
                    "Constitution and founding the Federalist Party."},

                new Part{
                    Production= context.Productions.Where(p => p.Title == "Hamilton").FirstOrDefault(),
                    Character="Devon Roberts",
                    Type=Enum.PositionEnum.Director,
                    Person= context.CastMembers.Where(c => c.Name == "Devon Roberts").FirstOrDefault(),
                    Details="The director manages the creative aspects of the production. They direct the making of a film by visualizing the " +
                    "script while guiding the actors and technical crew to capture the vision for the screen. They control the film's dramatic and " +
                    "artistic aspects"},

                new Part{
                    Production= context.Productions.Where(p => p.Title == "Phantom of the Opera").FirstOrDefault(),
                    Character="Christine Daaé", 
                    Type=Enum.PositionEnum.Actor,
                    Person= context.CastMembers.Where(c => c.Name == "Jacquelle Davis").FirstOrDefault(),
                    Details="A young Swedish soprano who becomes torn between her loyalty for her mentor Erik, and her love for her childhood friend " +
                    "Raoul de Chagny."},

                new Part{
                    Production= context.Productions.Where(p => p.Title == "Phantom of the Opera").FirstOrDefault(),
                    Character="Erik",
                    Type=Enum.PositionEnum.Actor,
                    Person= context.CastMembers.Where(c => c.Name == "Tom Mounsey").FirstOrDefault(),
                    Details="Known as “P. of the Opera,” “the ghost,” “the Voice” and “the Master of the Traps,” Erik is the antagonist " +
                    "of the novel and a tragic, violent, and ultimately mysterious figure. Although the narrator asserts that Erik is a " +
                    "human being, he displays characteristics that suggest he might be more supernatural than purely human: his " +
                    "appearance as a skeleton covered in rotten skin, his extraordinary singing abilities, and his capacity for " +
                    "ventriloquism, which allows him to project his voice anywhere he pleases, making it seem as though he is in various" +
                    " places at once."},

                new Part{
                    Production= context.Productions.Where(p => p.Title == "Phantom of the Opera").FirstOrDefault(),
                    Character="Devon Roberts",
                    Type=Enum.PositionEnum.Director,
                    Person= context.CastMembers.Where(c => c.Name == "Devon Roberts").FirstOrDefault(),
                    Details="The director manages the creative aspects of the production. They direct the making of a film by visualizing the " +
                    "script while guiding the actors and technical crew to capture the vision for the screen. They control the film's dramatic and " +
                    "artistic aspects"},

                new Part{
                    Production= context.Productions.Where(p => p.Title == "The Book of Mormon").FirstOrDefault(),
                    Character="Arnold Cunningham", 
                    Type=Enum.PositionEnum.Actor,
                    Person= context.CastMembers.Where(c => c.Name == "Heath Hyun Houghton").FirstOrDefault(),
                    Details="Elder Arnold Cunningham is an enthusiastic but childish young Mormon who's excited to go out and proselytize with his " +
                    "new best friend... even though he hasn't actually read the Book of Mormon. He also has a very active imagination - that is to say, " +
                    "he lies a lot. This causes problems in the remote Ugandan village they've been assigned to."},

                new Part{
                    Production= context.Productions.Where(p => p.Title == "The Book of Mormon").FirstOrDefault(),
                    Character="Devon Roberts",
                    Type=Enum.PositionEnum.Director,
                    Person= context.CastMembers.Where(c => c.Name == "Devon Roberts").FirstOrDefault(),
                    Details="The director manages the creative aspects of the production. They direct the making of a film by visualizing the " +
                    "script while guiding the actors and technical crew to capture the vision for the screen. They control the film's dramatic and " +
                    "artistic aspects"},

                new Part{
                    Production= context.Productions.Where(p => p.Title == "Wicked").FirstOrDefault(),
                    Character="Glinda the Good Witch", 
                    Type=Enum.PositionEnum.Actor,
                    Person= context.CastMembers.Where(c => c.Name == "Adriana Gantzer").FirstOrDefault(),
                    Details="Glinda is a very bubbly, perky, and popular girl. Unlike in The Wonderful Wizard of Oz she has blonde hair instead of red " +
                    "hair and a blue dress instead of a light pink dress."},

                new Part{
                    Production= context.Productions.Where(p => p.Title == "Wicked").FirstOrDefault(),
                    Character="Devon Roberts",
                    Type=Enum.PositionEnum.Director,
                    Person= context.CastMembers.Where(c => c.Name == "Devon Roberts").FirstOrDefault(),
                    Details="The director manages the creative aspects of the production. They direct the making of a film by visualizing the " +
                    "script while guiding the actors and technical crew to capture the vision for the screen. They control the film's dramatic and " +
                    "artistic aspects"},

                new Part{
                    Production= context.Productions.Where(p => p.Title == "How to Succeed in Business Without Really Trying").FirstOrDefault(),
                    Character="J. Pierrepont Finch", 
                    Type=Enum.PositionEnum.Actor,
                    Person= context.CastMembers.Where(c => c.Name == "Tom Mounsey").FirstOrDefault(),
                    Details="Our story's protagonist. An irrepressible, clear-eyed, almost puckish hero, he is a window washer who applies for a job at " +
                    "the World Wide Wicket Company and attempts to climb the 'ladder of success' using instruction from his little book called How to Succeed " +
                    "in Business Without Really Trying"},

                new Part{

                    Production= context.Productions.Where(p => p.Title == "How to Succeed in Business Without Really Trying").FirstOrDefault(),
                    Character="Devon Roberts",
                    Type=Enum.PositionEnum.Director,
                    Person= context.CastMembers.Where(c => c.Name == "Devon Roberts").FirstOrDefault(),
                    Details="The director manages the creative aspects of the production. They direct the making of a film by visualizing the " +
                    "script while guiding the actors and technical crew to capture the vision for the screen. They control the film's dramatic and " +
                    "artistic aspects"},
            };

            parts.ForEach(x => // iterate through the list Parts
            {
                var tempPart = context.Parts.Where(p => p.Production.Title == x.Production.Title && p.Character == x.Character && p.Type == x.Type).FirstOrDefault(); // Where the production title, Character, and part Type match it will return the query data or null if it doesn't exist
                if (tempPart != null) // where it does not return null
                {
                    x.PartID = tempPart.PartID; // update the partID with the ID assigned to tempPart
                }
                context.Parts.AddOrUpdate(p => p.PartID, x); // runs the addorupdate- if tempPart returns null a new record will be added if it returned not null it will update based off of the PartID assigned in the if statement
            }); 
            context.SaveChanges();
        }


    }
}