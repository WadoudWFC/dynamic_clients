namespace MultipleHtppClient.API;

public class Aglou10001Requests
{

}
public record CanTryLoginRequestBody(string email);
public record LoginRequestBody(string email, string password, bool isotp = true);
public record GetDossierCountRequestBody(string userId, string idRole, bool applyFilter);
