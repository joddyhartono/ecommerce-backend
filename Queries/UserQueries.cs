public static class UserQueries
{
    public const string GetByEmail = "SELECT * FROM users WHERE email = @Email";
    public const string Update = "UPDATE users SET name = @Name, image = @Image WHERE email = @Email";
}