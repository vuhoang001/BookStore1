using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text.Json.Nodes;

namespace BuildingBlocks.Chassis.Security.Extensions;

internal static class ClaimsPrincipalExtensions
{
    public static string[] GetRoles(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal
            .FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToArray();
    }

    public static string? GetClaimValue(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        var claim = claimsPrincipal.FindFirst(claimType);
        return claim?.Value;
    }

    public static bool TryGetJsonClaim(
        this ClaimsPrincipal claimsPrincipal,
        string claimType,
        [NotNullWhen(true)] out JsonNode? claimJson
    )
    {
        var candidateClaim = claimsPrincipal.FindFirst(claimType);

        if (candidateClaim != null &&
            string.Equals(candidateClaim.ValueType, "JSON", StringComparison.OrdinalIgnoreCase))
        {
            claimJson = JsonNode.Parse(candidateClaim.Value);
            return claimJson != null;
        }

        claimJson = null;
        return false;
    }
}