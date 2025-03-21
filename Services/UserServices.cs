using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using blog.Context;
using blog.Models;
using blog.Models.DTOS;
using Microsoft.EntityFrameworkCore;

namespace blog.Services
{
    public class UserServices
    {
        private readonly DataContext _dataContext;

        public UserServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // Create our CreateUser Method
        // We have to also create a helper method to check whether or not the user already exists
        // We have to encrypt the password

        public async Task<bool> CreateUser(UserDTO newUser)
        {
            if(await DoesUserExist(newUser.Username)) return false;

            UserModel userToAdd = new();

            PasswordDTO encryptedPassword = HashPassword(newUser.Password);

            userToAdd.Hash = encryptedPassword.Hash;
            userToAdd.Salt = encryptedPassword.Salt;
            userToAdd.Username = newUser.Username;

            await _dataContext.User.AddAsync(userToAdd);
            return await _dataContext.SaveChangesAsync() != 0;


 
        }

        private static PasswordDTO HashPassword(string password)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(64);

            string salt = Convert.ToBase64String(saltBytes);

            string hash;

            using (var deryveBytes = new Rfc2898DeriveBytes(password, saltBytes, 310000, HashAlgorithmName.SHA256))
            {
                hash = Convert.ToBase64String(deryveBytes.GetBytes(32));
            }

            return new PasswordDTO
            {
                Salt = salt,
                Hash = hash

            };
        }

        private async Task<bool> DoesUserExist(string username)
        {
            return await _dataContext.User.SingleOrDefaultAsync(user => user.Username == username) != null;
        }
    }
}