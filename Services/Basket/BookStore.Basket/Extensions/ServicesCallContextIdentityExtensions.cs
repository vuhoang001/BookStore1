using System.Security.Claims;
using Grpc.Core;

namespace BookStore.Basket.Extensions;

internal static class ServicesCallContextIdentityExtensions
{
    public static string? GetUserIdentity(this ServerCallContext context)
    {
        // return context.GetHttpContext().User.GetClaimValue(ClaimTypes.NameIdentifier);
        return "";
    }
}