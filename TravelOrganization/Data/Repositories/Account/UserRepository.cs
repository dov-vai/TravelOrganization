using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using TravelOrganization.Data.Models.Account;
using TravelOrganization.Data.Models.Enumerators;

namespace TravelOrganization.Data.Repositories.Account
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            using var connection = _context.CreateConnection() as SqliteConnection;
            if (connection == null)
                throw new InvalidOperationException("Invalid database connection.");

            await connection.OpenAsync();

            var sql = @"
                SELECT 
                    u.id AS Id, 
                    u.el_pastas AS Email, 
                    u.slaptazodis AS Password, 
                    u.prof_nuotrauka AS ProfilePictureLink, 
                    u.registracijos_data AS RegistrationDate, 
                    u.vardas AS Name, 
                    u.pavarde AS Surname, 
                    u.gimimo_data AS BirthDate, 
                    u.role AS RoleId,
                    u.patvirtinimo_kodas AS ConfirmationToken,
                    u.kodo_galiojimas AS TokenExpiration, 
                    u.pastas_patvirtintas AS EmailConfirmed,
                    r.id AS RoleId, 
                    r.name AS RoleName
                FROM vartotojai u
                JOIN roles r ON u.role = r.id
                WHERE u.el_pastas = @Email;
            ";
            var userList = await connection.QueryAsync<User, Role, User>(
                sql,
                (user, role) =>
                {
                    user.Role = role;
                    return user;
                },
                new { Email = email },
                splitOn: "RoleId"
            );

            return userList.FirstOrDefault();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            using var connection = _context.CreateConnection() as SqliteConnection;
            if (connection == null)
                throw new InvalidOperationException("Invalid database connection.");

            await connection.OpenAsync();

            var sql = @"
                SELECT 
                    u.id AS Id, 
                    u.el_pastas AS Email, 
                    u.slaptazodis AS Password, 
                    u.prof_nuotrauka AS ProfilePictureLink, 
                    u.registracijos_data AS RegistrationDate, 
                    u.vardas AS Name, 
                    u.pavarde AS Surname, 
                    u.gimimo_data AS BirthDate, 
                    u.role AS RoleId,
                    u.patvirtinimo_kodas AS ConfirmationToken,
                    u.kodo_galiojimas AS TokenExpiration, 
                    u.pastas_patvirtintas AS EmailConfirmed,
                    r.id AS RoleId, 
                    r.name AS RoleName
                FROM vartotojai u
                JOIN roles r ON u.role = r.id
                WHERE u.id = @Id;
            ";

            var userList = await connection.QueryAsync<User, Role, User>(
                sql,
                (user, role) =>
                {
                    user.Role = role;
                    return user;
                },
                new { Id = id },
                splitOn: "RoleId"
            );

            return userList.FirstOrDefault();
        }

        public async Task<int> AddUserAsync(User user)
        {
            using var connection = _context.CreateConnection() as SqliteConnection;
            if (connection == null)
                throw new InvalidOperationException("Invalid database connection.");

            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();

            try
            {
                var sqlMaxId = "SELECT MAX(id) FROM vartotojai;";
                var maxId = await connection.ExecuteScalarAsync<int?>(sqlMaxId, transaction: transaction);
                int nextId = (maxId ?? 0) + 1;

                user.Id = nextId;

                var sqlInsert = @"
                    INSERT INTO vartotojai (
                        id, 
                        el_pastas, 
                        slaptazodis, 
                        prof_nuotrauka, 
                        registracijos_data, 
                        vardas, 
                        pavarde, 
                        gimimo_data, 
                        role,
                        patvirtinimo_kodas,
                        kodo_galiojimas,
                        pastas_patvirtintas
                    ) VALUES (
                        @Id, 
                        @Email, 
                        @Password, 
                        @ProfilePictureLink, 
                        @RegistrationDate, 
                        @Name, 
                        @Surname, 
                        @BirthDate, 
                        @RoleId,
                        @ConfirmationToken,
                        @TokenExpiration,
                        @EmailConfirmed
                    );
                ";

                await connection.ExecuteAsync(sqlInsert, user, transaction: transaction);

                transaction.Commit();

                return user.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                throw new Exception("An error occurred while adding the user to the database.", ex);
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            using var connection = _context.CreateConnection() as SqliteConnection;
            if (connection == null)
                throw new InvalidOperationException("Invalid database connection.");

            await connection.OpenAsync();

            var sql = @"
                UPDATE vartotojai
                SET 
                    el_pastas = @Email,
                    slaptazodis = @Password,
                    prof_nuotrauka = @ProfilePictureLink,
                    vardas = @Name,
                    pavarde = @Surname,
                    gimimo_data = @BirthDate,
                    role = @RoleId,
                    patvirtinimo_kodas = @ConfirmationToken,
                    kodo_galiojimas = @TokenExpiration,
                    pastas_patvirtintas = @EmailConfirmed
                WHERE id = @Id;
            ";

            await connection.ExecuteAsync(sql, user);
        }

        public async Task DeleteUserAsync(int id)
        {
            using var connection = _context.CreateConnection() as SqliteConnection;
            if (connection == null)
                throw new InvalidOperationException("Invalid database connection.");

            await connection.OpenAsync();

            var sql = "DELETE FROM vartotojai WHERE id = @Id;";
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
