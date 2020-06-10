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
            SeedParts();
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
                ClosingDay = new DateTime(2020, 02, 29, 19, 30, 00), ShowtimeEve = new DateTime(2020, 01, 02, 19, 30, 00) , ShowtimeMat = new DateTime(2020, 01, 02, 22, 30, 00),
                TicketLink = "ticketsforyou.com", Season = 23, IsCurrent = true},

                new Production{Title = "Phantom of the Opera", Playwright = "Andrew Lloyd Webber & Charles Hart", Description = "Based on a French " +
                "novel of the same name by Gaston Leroux, its central plot revolves around a beautiful soprano, Christine Daae, who becomes the obesession " +
                "of a mysterious, disfigured musical genius living in the subterranean labyrinth beneath the Paris Opera House.", OpeningDay = new DateTime(2019, 10, 01, 17, 30, 00),
                ClosingDay = new DateTime(2019, 11, 30, 17, 30, 00), ShowtimeEve = new DateTime(2019, 10, 04, 17, 30, 00), ShowtimeMat = new DateTime(2019, 10, 04, 19, 30, 00),
                TicketLink = "ticketsforyou.com", Season = 23, IsCurrent = true},

                new Production{Title = "The Book of Mormon", Playwright = "Trey Parker, Robert, Lopez, & Matt Stone", Description = "The Book of Mormon " +
                "follows two Latter-Day Saints missionaries as they attempt to preach the faith of the Church of Jesus Christ of Latter-Day Saints to the " +
                "inhabitants of a remote Ugandan village.", OpeningDay = new DateTime(2021, 01, 01, 19, 30, 00), ClosingDay = new DateTime(2021, 02, 28, 19, 30, 00),
                ShowtimeEve = new DateTime(2021, 01, 02, 19, 30, 00), ShowtimeMat = new DateTime(2021, 01, 02, 22, 30, 00), TicketLink = "ticketsforyou.com", Season = 24,
                IsCurrent = false},

                new Production{Title = "Wicked", Playwright = "Stephen Schwartz", Description = "This musical is told from the perspective of the witches of " +
                "the Land of Oz; its plot begins before and continues after Dorothy Gale arrives in Oz from Kansas, and includes several references to the 1939 film.",
                OpeningDay = new DateTime(2020, 10, 01, 19, 30, 00), ClosingDay = new DateTime(2020, 11, 30, 19, 30, 00), ShowtimeEve = new DateTime(2020, 10, 01, 19, 30, 00),
                ShowtimeMat = new DateTime(2020, 10, 01, 23, 30, 00), TicketLink = "ticketsforyou.com", Season = 24, IsCurrent = false},

                new Production{Title = "How to Succeed in Business Without Really Trying", Playwright = "Frank Loesser", Description = "This story concerns young, " +
                "ambitious J. Pierrepont Finch, who, with the help of the book How to Succeed in Business Without Really Trying, rises from window washer to chairman of " +
                "the board of the World Wide Wicket Company.", OpeningDay = new DateTime(2020, 04, 01, 19, 30, 00), ClosingDay = new DateTime(2020, 05, 31, 19, 30, 00),
                ShowtimeEve = new DateTime(2020, 04, 01, 19, 30, 00), ShowtimeMat = new DateTime(2020, 04, 01, 23, 30, 00), TicketLink = "ticketsforyou.com", Season = 23, IsCurrent = true},

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