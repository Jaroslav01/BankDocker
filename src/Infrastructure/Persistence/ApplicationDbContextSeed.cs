using CleanArchitecture.Application.Accounts.Commands.CreateAccount;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Events;
using CleanArchitecture.Domain.ValueObjects;
using CleanArchitecture.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IApplicationDbContext context, CancellationToken cancellationToken)
    {
        var administratorRole = new IdentityRole("Administrator");

        if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await roleManager.CreateAsync(administratorRole);
        }

        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost", };

        if (userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await userManager.CreateAsync(administrator, "Administrator1!");
            await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
        }

        var managerRole = new IdentityRole("Manager");

        if (roleManager.Roles.All(r => r.Name != managerRole.Name))
        {
            await roleManager.CreateAsync(managerRole);
        }

        var manager = new ApplicationUser { UserName = "manager@localhost", Email = "manager@localhost", };

        if (userManager.Users.All(u => u.UserName != manager.UserName))
        {
            await userManager.CreateAsync(manager, "Manager1!");
            await userManager.AddToRolesAsync(manager, new[] { managerRole.Name });
        }

        var bankUser = new ApplicationUser { UserName = "bankUser@localhost", Email = "bankUser@localhost", };

        if (userManager.Users.All(u => u.UserName != bankUser.UserName))
        {
            await userManager.CreateAsync(bankUser, "bankUser1!");
            await userManager.AddToRolesAsync(bankUser, new[] { administratorRole.Name });
        }

        if (context.Accounts.All(account => account.ApplicationUserId != bankUser.Id && account.Name != "Bank account"))
        {
            static string RandomDigits(int length)
            {
                var random = new Random();
                string s = string.Empty;
                for (int i = 0; i < length; i++)
                    s = String.Concat(s, random.Next(10).ToString());
                return s;
            }
            var accountNumber = RandomDigits(19);
            var entity = new Account
            {
                ApplicationUserId = bankUser.Id,
                AccountNumber = accountNumber,
                Name = "Bank account",
                Amount = 0,
            };

            entity.DomainEvents.Add(new AccountCreatedEvent(entity));

            context.Accounts.Add(entity);

            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public static async Task SeedSampleDataAsync(ApplicationDbContext context)
    {
        // Seed, if necessary
        if (!context.TodoLists.Any())
        {
            context.TodoLists.Add(new TodoList
            {
                Title = "Shopping",
                Colour = Colour.Blue,
                Items =
                    {
                        new TodoItem { Title = "Apples", Done = true },
                        new TodoItem { Title = "Milk", Done = true },
                        new TodoItem { Title = "Bread", Done = true },
                        new TodoItem { Title = "Toilet paper" },
                        new TodoItem { Title = "Pasta" },
                        new TodoItem { Title = "Tissues" },
                        new TodoItem { Title = "Tuna" },
                        new TodoItem { Title = "Water" }
                    }
            });

            await context.SaveChangesAsync();
        }
    }
}
