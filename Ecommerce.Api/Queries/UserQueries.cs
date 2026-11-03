public static class UserQueries
{
    public const string qGetByEmail = "SELECT u.id, u.name, u.email, u.image, u.password, (CASE WHEN s.user_id IS NOT NULL THEN 1 ELSE 0 END) AS IsSeller FROM users AS u LEFT JOIN sellers AS s ON u.id = s.user_id WHERE u.email = @Email";
    public const string qUpdate = @"
        WITH updated AS (
            UPDATE users 
            SET name = @Name, image = @Image 
            WHERE email = @Email 
            RETURNING id, name, email, image   
        )

        SELECT u.id, u.name, u.email, u.image, (CASE WHEN s.user_id IS NOT NULL THEN 1 ELSE 0 END) AS IsSeller
        FROM updated AS u
        LEFT JOIN sellers AS s ON u.id = s.user_id
        ";
    public const string qInsert = "INSERT INTO users (name, email, password) VALUES (@Name, @Email, @Password) RETURNING id, name, email, image";
}