using paymatesapi.Models;
using paymatesapi.Contexts;

namespace paymatesapi.Services
{
    public class UserService : IUserService
    {
        private readonly UserContext _userContext;
        public UserService(UserContext userContext)
        {
            _userContext = userContext;
        }
        private static User dummyUser = new User
        {
            Uid = "ewrferf",
            Name = "randow",
            Surname = "korkie",
            Username = "korkews",
            Email = "werfrw@sewrf.com",
            Password = "erfwefwf"

        };
        public AuthenticationResponse getUser(string id)
        {
            return new AuthenticationResponse(dummyUser, "token");
        }
        public async Task<AuthenticationResponse> registerUser(User user)
        {
            var dbUser = _userContext.User.GetType();
            await _userContext.SaveChangesAsync();

            System.Console.WriteLine(dbUser.Name);

            return new AuthenticationResponse(dummyUser, "token");
        }
        public AuthenticationResponse loginUser(UserCreds user)
        {
            return new AuthenticationResponse(dummyUser, "token");
        }
        public bool deleteUser(string id)
        {
            return true;
        }
        public AuthenticationResponse updateUser(User user)
        {
            return new AuthenticationResponse(dummyUser, "token");
        }

    }
}
