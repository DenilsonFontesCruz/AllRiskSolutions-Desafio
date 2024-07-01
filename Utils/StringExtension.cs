namespace AllRiskSolutions_Desafio.Utils;

public static class StringExtension
{
    public static string GetBearerToken(this string token)
    {
        return token.Replace("Bearer ", "");
    }
}