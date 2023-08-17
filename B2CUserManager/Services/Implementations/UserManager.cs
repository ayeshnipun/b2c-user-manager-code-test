using Azure.Identity;
using B2CUserManager.Exceptions;
using B2CUserManager.Models;
using B2CUserManager.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace B2CUserManager.Services.Implementations
{
    public class UserManager : IUserManager
    {
        private readonly GraphServiceClient _graphClient;
        private readonly AzureB2CSettings _b2cSettings;

        public UserManager(IOptions<AzureB2CSettings> b2cSettings)
        {
            _b2cSettings = b2cSettings.Value;

            var scopes = new[] { _b2cSettings.Scopes };
            var tenantId = _b2cSettings.Tenant;
            var clientId = _b2cSettings.ClientId;
            var clientSecret = _b2cSettings.ClientSecret;

            var options = new TokenCredentialOptions { AuthorityHost = AzureAuthorityHosts.AzurePublicCloud };
            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret, options);
            var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

            _graphClient = graphClient;
        }

        public async Task<Profile> GetUserById(string id)
        {
            try
            {
                var user = await _graphClient.Users[$"{id}"].GetAsync();

                if (user != null)
                {
                    return new Profile { DisplayName = user.DisplayName, Id = user.Id, Email = user.UserPrincipalName };
                }

                throw new NotFoundException("User not found");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Profile>> GetUsers()
        {
            try
            {
                var userList = new List<Profile>();

                var users = await _graphClient.Users.GetAsync();

                if (users.Value.Any())
                {
                    foreach (var user in users.Value)
                    {
                        userList.Add(new Profile { DisplayName = user.DisplayName, Email = user.UserPrincipalName, Id = user.Id });
                    }
                }
                return userList;
            }
            catch
            {
                throw new Exception();
            }
        }

        public async Task<Profile> CreateUser(Profile user)
        {
            try
            {
                var requestBody = new User
                {
                    AccountEnabled = true,
                    DisplayName = user.DisplayName,
                    MailNickname = user.MailNickName,
                    UserPrincipalName = user.Email,
                    PasswordProfile = new PasswordProfile
                    {
                        ForceChangePasswordNextSignIn = true,
                        Password = "xWwvJ]6NMw+bWH-d",
                    },
                };

                var result = await _graphClient.Users.PostAsync(requestBody);

                if (result != null)
                {
                    return new Profile { Id = result.Id, Email = result.UserPrincipalName, DisplayName = result.DisplayName };
                }

                throw new Exception("Cant create the user");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}