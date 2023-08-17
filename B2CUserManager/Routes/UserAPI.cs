using B2CUserManager.Models;
using B2CUserManager.Services.Interfaces;

namespace B2CUserManager.Routes
{
    public static class UserAPI
    {
        public static void MapUserRoutes(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("api/users", (IUserManager userManager) => { return userManager.GetUsers(); });

            builder.MapGet("api/users/{id}", (string id, IUserManager userManager) => { return userManager.GetUserById(id); });

            builder.MapPost("api/user", (Profile user, IUserManager userManager) => { return userManager.CreateUser(user); });

        }
    }
}