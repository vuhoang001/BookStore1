namespace BuildingBlocks.Chassis.Security.Keycloak;

public static class KeycloakEndpoint
{
    public static string Token(string realm)
    {
        return $"/realms/{realm}/protocol/openid-connect/token";
    }

    public static string Authorize(string realm)
    {
        return $"/realms/{realm}/protocol/openid-connect/auth";
    }

    public static string Introspect(string realm)
    {
        return $"/realms/{realm}/protocol/openid-connect/token/introspect";
    }
}