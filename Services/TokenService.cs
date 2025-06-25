public class TokenStoreService
{
    private string? _token;

    public void SetToken(string token) => _token = token;
    public string? GetToken() => _token;
}