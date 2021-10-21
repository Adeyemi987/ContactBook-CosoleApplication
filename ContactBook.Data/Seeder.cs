using ContactBook.Model;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ContactBook.Data
{
    public class Seeder
    {
        public async static Task SeedData(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, ContactBookContext context)
        {
            try
            {
                var dirDb = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);



                await context.Database.EnsureCreatedAsync();
                if (!context.Users.Any())
                {
                    List<string> roles = new() { "Admin", "Regular" };
                    foreach (var role in roles)
                    {
                        await roleManager.CreateAsync(new IdentityRole { Name = role });
                    }


                    var appUsers = File.ReadAllText(dirDb + @"/JSONfiles/AppUsers.json");
                    var users = JsonConvert.DeserializeObject<List<AppUser>>(appUsers);
                    await context.AppUsers.AddRangeAsync(users);
                    var result = await context.SaveChangesAsync();


                    List<AppUser> AppUser = new List<AppUser>()
                    {
                    new AppUser
                    {
                        FirstName = "charles",
                        LastName = "Ade",
                        Email = "cc@gmail.com",
                        UserName = "cPro",
                        PhoneNumber = "09034962686"
                    },

                    new AppUser
                    {
                        FirstName = "John",
                        LastName = "kenny",
                        Email = "jk@yahoo.com",
                        UserName = "JKen",
                        PhoneNumber = "09052782088"
                    },

                    };


                    foreach (var user in users)
                    {
                        await userManager.CreateAsync(user, "chemistryB3@");
                        if (user == users[0])
                        {
                            await userManager.AddToRoleAsync(user, "Admin");
                        }
                        else
                        {
                            await userManager.AddToRoleAsync(user, "Regular");
                        }

                    }

                }
            }
            catch (Exception Ex)
            {

                Console.WriteLine(Ex.Message); ;
            }
        }
    }
}
