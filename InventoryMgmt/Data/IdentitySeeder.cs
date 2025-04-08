using Microsoft.AspNetCore.Identity;

public static class IdentitySeeder
{
    
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = { "Admin", "Manager", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));
                if (result.Succeeded)
                {
                    Console.WriteLine($"Created role: {role}");
                }
                else
                {
                    Console.WriteLine($"Error creating role '{role}':");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($" - {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Role already exists: {role}");
            }
        }
    }
    
    public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Ensure admin role exists
        string adminRole = "Admin";
        if (!await roleManager.RoleExistsAsync(adminRole))
        {
            await roleManager.CreateAsync(new IdentityRole(adminRole));
        }
        
        // Ensure manager role exists
        string managerRole = "Manager";
        if (!await roleManager.RoleExistsAsync(managerRole))
        {
            await roleManager.CreateAsync(new IdentityRole(managerRole));
        }
        
        // Check if the manager user already exists
        string managerEmail = "manager@example.com";
        string managerPassword = "Manager@123"; // Strong password!

        var managerUser = await userManager.FindByEmailAsync(managerEmail);
        if (managerUser == null)
        {
            var user = new IdentityUser
            {
                UserName = managerEmail,
                Email = managerEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, managerPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, managerRole);
                Console.WriteLine("Manager user created.");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error creating manager user: {error.Description}");
                }
            }
        }
        else
        {
            Console.WriteLine("Manager user already exists.");
        }

        // Check if the admin user already exists
        string adminEmail = "admin@example.com";
        string adminPassword = "Admin@123"; // Strong password!

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
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
                await userManager.AddToRoleAsync(user, adminRole);
                Console.WriteLine("Admin user created.");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error creating admin user: {error.Description}");
                }
            }
        }
        else
        {
            Console.WriteLine("Admin user already exists.");
        }
    }
}