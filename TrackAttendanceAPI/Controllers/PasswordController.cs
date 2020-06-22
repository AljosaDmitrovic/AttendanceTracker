using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackAttendanceAPI.Models;

namespace TrackAttendanceAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        public class Login
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
        public class ChangePassword
        {
            public int Id { get; set; }
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }
        string GenerateSalt(int length)
        {
            var bytes = new byte[length];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }

            return Convert.ToBase64String(bytes);
        }
        public HashAlgorithmName HashAlgorithmName { get; } = HashAlgorithmName.SHA256;
        string GenerateHash(string password, string salt, int iterations, int length)
        {
            byte[] _password = Encoding.UTF8.GetBytes(password);
            byte[] _salt = Encoding.UTF8.GetBytes(salt);

            using (var deriveBytes = new Rfc2898DeriveBytes(_password, _salt, iterations, HashAlgorithmName))
            {
                return Convert.ToBase64String(deriveBytes.GetBytes(length));
            }
        }


        private readonly TrackAttendanceContext _context;

        public PasswordController(TrackAttendanceContext context)
        {
            _context = context;
        }

        // POST: api/Password/Login
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<Users>> LoginCheck(Login login)
        {
            List<Users> users = _context.Users.ToList();
            foreach (var user in users)
            {
                if (user.LoginName == login.Username || user.Email == login.Username || user.IndexNumber == login.Username)
                {
                    if (user.PasswordSalt == "")
                    {
                        user.PasswordSalt = GenerateSalt(32);
                        user.PasswordHash = GenerateHash(login.Password, user.PasswordSalt, 1000, 32);
                        _context.Entry(user).State = EntityState.Modified;
                        try
                        {
                            await _context.SaveChangesAsync();
                            var instance = new SignEntriesController(_context);
                            instance.updateSignEntryEmptyWeeks(user);
                            return Ok(user);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!UsersExists(user.Id))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                    else
                    {
                        if (user.PasswordHash == GenerateHash(login.Password, user.PasswordSalt, 1000, 32))
                        {
                            var instance = new SignEntriesController(_context);
                            instance.updateSignEntryEmptyWeeks(user);
                            return Ok(user);
                        }
                    }
                }
            }
            return NotFound();
        }
        // POST: api/Password/Change
        [HttpPost()]
        [Route("Change")]
        public async Task<IActionResult> ChangePass(ChangePassword changePassword)
        {
            var user = await _context.Users.FindAsync(changePassword.Id);
            if (GenerateHash(changePassword.OldPassword, user.PasswordSalt, 1000, 32) == user.PasswordHash)
            {
                user.PasswordSalt = GenerateSalt(32);
                user.PasswordHash = GenerateHash(changePassword.NewPassword, user.PasswordSalt, 1000, 32);
                _context.Entry(user).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                    return Ok(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                return NotFound();
            }
        }
        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
