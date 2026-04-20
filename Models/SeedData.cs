using Microsoft.EntityFrameworkCore;
using ComputerParts.Data;
using ComputerParts.Models;
using Microsoft.AspNetCore.Identity;

namespace ComputerParts.Models
{
    public static class SeedData
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ComputerPartsContext(
                serviceProvider.GetRequiredService<DbContextOptions<ComputerPartsContext>>()))
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                string adminEmail = "admin@test.bg";
                string adminPassword = "admin1234";

                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var user = new IdentityUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, adminPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                }

                var components = new List<HardwareComponent>
                {
                    new HardwareComponent
                    {
                        Name = "RTX 2060",
                        Manufacturer = "MSI",
                        Type = "Видеокарта",
                        Price = 267.00M,
                        ImageUrl = "/images/msi-rtx-2060.png"
                    },
                    new HardwareComponent
                    {
                        Name = "i5-14600k",
                        Manufacturer = "Intel",
                        Type = "Процесор",
                        Price = 274.00M,
                        ImageUrl = "/images/intel-i5-14600k.png"
                    },
                    new HardwareComponent
                    {
                        Name = "Ryzen 9 7950x",
                        Manufacturer = "AMD",
                        Type = "Процесор",
                        Price = 328.00M,
                        ImageUrl = "/images/amd-ryzen-9-7950x.png"
                    },
                    new HardwareComponent
                    {
                        Name = "NVIDIA RTX 4070",
                        Manufacturer = "ASUS",
                        Type = "Видеокарта",
                        Price = 700.00M,
                        ImageUrl = "/images/rtx4070.png"
                    },
                    new HardwareComponent
                    {
                        Name = "SAMSUNG 980 PRO 1TB NVME",
                        Manufacturer = "SAMSUNG",
                        Type = "SSD",
                        Price = 140.00M,
                        ImageUrl = "/images/Samsung-SSD-NVME.png"
                    },
                    new HardwareComponent
                    {
                        Name = "BARRACUDA HDD 1TB",
                        Manufacturer = "SEAGATE",
                        Type = "HDD",
                        Price = 80.00M,
                        ImageUrl = "/images/Barracuda-HDD.png"
                    },
                    new HardwareComponent
                    {
                        Name = "TRIDENTZ RGB DDR4 16GB",
                        Manufacturer = "G.SKILL",
                        Type = "RAM",
                        Price = 626.00M,
                        ImageUrl = "/images/TridentZ-ram-ddr4.png"
                    },
                    new HardwareComponent
                    {
                        Name = "FURY RGB DDR5 16GB",
                        Manufacturer = "KINGSTON",
                        Type = "RAM",
                        Price = 940.00M,
                        ImageUrl = "/images/Fury-ram-ddr5.png"
                    }
                };

                foreach (var item in components)
                {
                    if (!context.HardwareComponent.Any(x => x.Name == item.Name))
                    {
                        context.HardwareComponent.Add(item);
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }
}