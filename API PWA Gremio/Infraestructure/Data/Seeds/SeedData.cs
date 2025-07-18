using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PWA_GREMIO_API.Core.Entities;
using PWA_GREMIO_API.Core.Entities.Groups;
using PWA_GREMIO_API.Core.Entities.Users;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Text.Json;

namespace PWA_GREMIO_API.Infraestructure.Data.Seeds
{
    public class SeedData
    {
        public static void Seeds(ModelBuilder modelBuilder)
        {
            SeedGroups(modelBuilder);
            SeedUsers(modelBuilder);
            SeedUsersSignalR(modelBuilder);
            SeedAnnoucements(modelBuilder);
            SeedUsersData(modelBuilder);

        }

        public static void SeedUsersAndAnnoucementsOfGroup(ModelBuilder modelBuilder)
        {
         
            SeedUsersOfGroup(modelBuilder);
            SeedAnnoucementsOfGroup(modelBuilder);
        }



        public static void SeedUsersData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserData>().HasData
            (
                 new UserData
                 {
                     Id = 1,
                     Name = "Fernando",
                     LastName = "Colazo",
                     Category = "Jefe de sector",
                     DNI = 12345678,
                     BirthDate = new DateOnly(1995, 9, 10),
                     CreatedBy = 0,
                     CreatedDate = new DateOnly(2024, 12, 2),
                     UserAuthId = 4

                 },
                new UserData
                {
                    Id = 2,
                    Name = "Nicolas",
                    LastName = "Mogliani",
                    Category = "Jefe de sector Informatico",
                    DNI = 12345679,
                    BirthDate = new DateOnly(1995, 9, 10),
                    CreatedBy = 0,
                    CreatedDate = new DateOnly(2024, 12, 2),
                    UserAuthId = 3

                }

            );
        }
        public static void SeedUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAuth>().HasData
            (
                 new UserAuth
                 {
                     Id = 4,
                     AffilliateNumber = 1,

                     Email = "testFernando@gmail.com",
                     Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                     Roles = ["NormalUser"],
                     CreatedBy = 0,
                     CreatedDate = new DateOnly(2024, 12, 2),

                 },
                new UserAuth
                {
                    Id = 3,
                    AffilliateNumber = 2,
                    
                    Email = "testNicolas@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                    Roles = ["Admin", "NormalUser"],
                    CreatedBy = 0,
                    CreatedDate = new DateOnly(2024, 12, 2),
                }
            );


        }

        public static void SeedGroups(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Group>().HasData(

                new Group
                {
                    Id = 2,
                    Name = "Coordinadores Sector 1",
                    CreatedBy = 0,
                    CreatedDate = new DateOnly(2024, 12, 2),
                    Description = "Coordinadores del Sector 1"

                },
                new Group
                {
                    Id = 4,
                    Name = "Afiliados Sector 1 ",
                    CreatedBy = 0,
                    CreatedDate = new DateOnly(2024, 12, 2),
                    Description = "Afiliados del Sector 1"
                }

            );

        }


        public static void SeedUsersSignalR(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserSignalR>().HasData
           (


              new UserSignalR
              {

                  Id = 1,
                  UserAuthId = 4,
                  CreatedBy = 0,
                  CreatedDate = new DateOnly(2024, 12, 2),
                  
              },
              new UserSignalR
              {

                  Id = 2,
                  UserAuthId = 3,
                  CreatedBy = 0,
                  CreatedDate = new DateOnly(2024, 12, 2),
              }

            );
        }


        public static void SeedUsersOfGroup(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserOfGroup>().HasData
            (

                  new UserOfGroup
                  {
                      Id = 0,
                      RoleInGroup = "Admin",
                      UserSignalRId = 1,
                      GroupId = 2,


                  },
                  new UserOfGroup
                  {
                      Id = 1,
                      RoleInGroup = "Admin",
                      UserSignalRId = 2,
                      GroupId = 2,
                  },

                  new UserOfGroup
                  {
                      Id = 2,
                      RoleInGroup = "Admin",
                      UserSignalRId = 1,
                      GroupId = 4,


                  },
                  new UserOfGroup
                  {
                      Id = 3,
                      RoleInGroup = "Admin",
                      UserSignalRId = 2,
                      GroupId = 4,
                  }


            );


        }


        public static void SeedAnnoucements(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnnoucementEntity>().HasData
          (

               new AnnoucementEntity
               {
                   Id = 1,
                   Title = "Becas Universitarias",
                   CreatedBy = 0,
                   CreatedDate = new DateOnly(2024, 12, 2),
                   Image_url = "https://scontent.fcor2-1.fna.fbcdn.net/v/t39.30808-6/426164806_799465508891353_9157247922856917991_n.jpg?_nc_cat=103&ccb=1-7&_nc_sid=3635dc&_nc_eui2=AeFDMgvFqUJ1Mo98DfPBkea5GtwXb7lgRgoa3BdvuWBGCjm9m63LNWFBvR0_RwZ5HmryAOadtBnZvYwaBD5fuqhh&_nc_ohc=Ek6DeW4sQOAAX8ox_Gy&_nc_ht=scontent.fcor2-1.fna&oh=00_AfA9D27X_OXY8xiuynfvWIitFuZTy1b-Q_V4_VF9KjBpKw&oe=65CC9ADD",
                   Text = "📚Becas universitarias 2024\r\n\r\n✅️ Nuestro Secretario General continúa impulsando el acceso a la educación superior, acercando a l@ trabajador@s y sus hij@s la oportunidad de crecer y desarrollarse en un área profesional.\r\n\r\n✊🏼 Desde el año 2002, continuamos sumando logros y conquistas para la gran familia trabajadora\r\n\r\n✊🏻Julio Mauricio Saillén\r\n💪🏼100x100 promesas cumplidas",
                   DateOfExpiration = new DateOnly(2024, 4, 30),
                   TimeOfExpiration = new System.TimeOnly(23, 59, 59),
                   DestinationGroupsIds = [2, 4],
                   AuthorUserSignalRId = 1
               }


          );

        }

        public static void SeedAnnoucementsOfGroup(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnnoucementOfGroup>().HasData
            (
                    new AnnoucementOfGroup
                    { 
                        Id = 0,
                        AnnoucementId = 1,
                        GroupId = 2,
                        UserSignalRId = 1,


                    },

                     new AnnoucementOfGroup
                     {
                         Id = 1,
                         AnnoucementId = 1,
                         GroupId = 4,
                         UserSignalRId = 1,


                     }

            );

        }


    }
}
