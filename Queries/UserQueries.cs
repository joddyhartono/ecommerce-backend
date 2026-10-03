public static class UserQueries
{
    public const string qGetByEmail = "SELECT * FROM users WHERE email = @Email";
    public const string qUpdate = "UPDATE users SET name = @Name, image = @Image WHERE email = @Email";
    public const string qInsert = "INSERT INTO users (name, email, password) VALUES (@Name, @Email, @Password) RETURNING id, name, email, image";
}