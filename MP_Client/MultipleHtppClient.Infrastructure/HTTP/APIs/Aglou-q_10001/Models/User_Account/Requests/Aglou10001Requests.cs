﻿namespace MultipleHtppClient.Infrastructure.HTTP.APIs.Aglou_q_10001.Models.User_Account.Requests;

public record CanTryLoginRequestBody(string email);
public record LoginRequestBody(string email, string password, bool isotp = true);
public record GetDossierCountRequestBody(string userId, string idRole, bool applyFilter);
