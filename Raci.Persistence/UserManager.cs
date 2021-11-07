using Microsoft.EntityFrameworkCore;
using Raci.Domain.RaciAccountAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Raci.Persistence
{
    public class UserManager
    {
        private readonly RaciDbContext _context;
        public UserManager(RaciDbContext context)
        {
            _context = context;
        }

        public async Task<RaciAccount> FindAsync(string userName, string password)
        {
            string salt = "mysalt2020abcdef";

            string passwordHash = GeneratePasswordHash(password, salt);

            var account = await _context.RaciAccounts
                .SingleOrDefaultAsync(p => p.UserName == userName);

            if (IsMatchPassword(account.PasswordHash, salt, password))
            {
                return account;
            }

            return null;
        }

        public string GeneratePasswordHash(string password, string salt)
        {
            byte[] saltBytes = Encoding.ASCII.GetBytes(salt);
            new RNGCryptoServiceProvider().GetBytes(saltBytes);

            var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(saltBytes, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        public bool IsMatchPassword(string storedPassword, string salt, string inputPassword)
        {
            bool isMatching = true;

            byte[] hashBytes = Convert.FromBase64String(storedPassword);
 
            byte[] saltBytes = Encoding.ASCII.GetBytes(salt);
            Array.Copy(hashBytes, 0, saltBytes, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(inputPassword, saltBytes, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    isMatching = false;

            return isMatching;
        }
    }
}
