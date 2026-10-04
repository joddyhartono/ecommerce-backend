namespace Ecommerce.Api.Queries
{
    public static class SellerQueries
    {
        public const string qCreate = @"
            INSERT INTO sellers (user_id, name, description)
            SELECT @UserId, @Name, @Description
            WHERE NOT EXISTS (
                SELECT 1 FROM sellers WHERE user_id = @UserId
            )";
    }
}