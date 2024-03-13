namespace WebApiWithPostgreSQL.DTOs
{
    public record UserSession(string? Id, string? FirstName, string? LastName, string? Email, string? Role);
}