using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BackendJWTToken.Context;
using BackendJWTToken.Models;
using Microsoft.IdentityModel.Tokens;

namespace BackendJWTToken.Services
{
    public class UserService
    {
        private readonly DataContext _dataContext;

        public UserService(DataContext dataContext){
            _dataContext = dataContext;
        }

        public bool CreateUser(UserDTO newUser){
            bool result = false;

            if(!DoesUserExist(newUser.Email)){
                UserModel userToAdd = new();
                userToAdd.Email = newUser.Email;

                PasswordDTO hashPassword = HashPassword(newUser.Password);
                userToAdd.Hash = hashPassword.Hash;
                userToAdd.Salt = hashPassword.Salt;

                _dataContext.Users.Add(userToAdd);
                result = _dataContext.SaveChanges() != 0;
            }

            return result;
        }

        private bool DoesUserExist(string email){
            return _dataContext.Users.SingleOrDefault(users => users.Email == email) != null;
        }

        private static PasswordDTO HashPassword(string password){
            byte[] saltBytes = RandomNumberGenerator.GetBytes(64);

            string salt = Convert.ToBase64String(saltBytes);

            string hash;

            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 310000, HashAlgorithmName.SHA256)){
                hash = Convert.ToBase64String(deriveBytes.GetBytes(32));
            }

            PasswordDTO hashedPassword = new();
            hashedPassword.Salt = salt;
            hashedPassword.Hash = hash;

            return hashedPassword;
        }

        public string Login(UserDTO user){
            string result = null;

            UserModel foundUser = GetUserByEmail(user.Email);

            if(foundUser == null){
                return result;
            }

            if(VerifyPassword(user.Password, foundUser.Salt, foundUser.Hash)){
                // JWT: JSON Web Token = a type of token used for authentication or transfering information. 
                // Bearer Token: A token that grants access to a resource, such as an API. JWT can be used as a bearer token, but there are other types of tokens that can be used as a bearer token
                
                // Setting the string that will be encrypted into our JWT
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));

                // Now to encrypt our secret key
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                // Set the options for our token to define properties such as where the token is issued from, where it is allowed to be used, and most importantly how long the token lasts before expiring.
                var tokenOptions = new JwtSecurityToken(
                    // issuer = where is this token allowed to be generated from
                    issuer: "backendjwtapibs-dwdcf6hta0f3gdcs.westus-01.azurewebsites.net",
                    // audience = where this token is allowed to authenticate
                    // issuer and audience should be the same since our api is handling both login and authentication
                    audience: "backendjwtapibs-dwdcf6hta0f3gdcs.westus-01.azurewebsites.net",
                    // claims = additional options for authentication
                    claims: new List<Claim>(),
                    // Sets the token expiration time and date. In other words this is what makes our tokens temporary, thus keeping our access to our resources safe and secure
                    expires: DateTime.Now.AddMinutes(30),
                    // This attaches our newly encryted super secret key that was turned into sign in credentials
                    signingCredentials: signinCredentials
                );

                // Generate our JWT and save the token as a string into a variable
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                result = tokenString;

                // Token Anatomy: 
                // asldkfalsfs.asdgkasdg.asdgasdg
                // Header = asldkfalsfs
                // Payload = asdgkasdg this will have information about the token, including the expieration
                // Signature = asdgasdg encrypt and combine header and payload using the secret key
            }

            return result;
        }

        private UserModel GetUserByEmail(string email){
            return _dataContext.Users.SingleOrDefault(user => user.Email == email);
        }

        private static bool VerifyPassword(string password, string salt, string hash){
            byte[] saltBytes = Convert.FromBase64String(salt);

            string newHash;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 310000, HashAlgorithmName.SHA256)){
                newHash = Convert.ToBase64String(deriveBytes.GetBytes(32));
            }

            return hash == newHash;
        }

        public bool UpdatePassword(UserDTO user){
            bool result = false;

            var foundUser = GetUserByEmail(user.Email);

            if(foundUser == null){
                return result;
            }

            PasswordDTO hashPassword = HashPassword(user.Password);

            foundUser.Hash = hashPassword.Hash;
            foundUser.Salt = hashPassword.Salt;

            _dataContext.Update<UserModel>(foundUser);
            result = _dataContext.SaveChanges() != 0
            return result;
;        }
    }
}