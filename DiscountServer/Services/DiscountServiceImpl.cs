using DiscountShared;
using System.Security.Cryptography;
using DiscountStorage;
using DiscountStorage.Models;

using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace DiscountServer.Services;

public class DiscountServiceImpl : DiscountService.DiscountServiceBase
{
    private readonly DiscountDbContext _dbContext;

    public DiscountServiceImpl(DiscountDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<GenerateResponse> GenerateCodes(GenerateRequest request, ServerCallContext context)
    {
        if (request.Count > 2000 || request.Length < 7 || request.Length > 8)
        {
            return new GenerateResponse { Result = false };
        }

        var codes = new HashSet<string>();
        while (codes.Count < request.Count)
        {
            var code = GenerateRandomCode((int)request.Length);
            codes.Add(code);
        }

        var discountCodes = codes.Select(c => new DiscountCode { Code = c }).ToList();

        try
        {
            _dbContext.DiscountCodes.AddRange(discountCodes);
            await _dbContext.SaveChangesAsync();
            return new GenerateResponse { Result = true };
        }
        catch
        {
            return new GenerateResponse { Result = false };
        }
    }

    public override async Task<UseCodeResponse> UseCode(UseCodeRequest request, ServerCallContext context)
    {
        var code = await _dbContext.DiscountCodes
            .FirstOrDefaultAsync(c => c.Code == request.Code);

        if (code == null)
            return new UseCodeResponse { Result = 1 }; // Not found

        if (code.Used)
            return new UseCodeResponse { Result = 2 }; // Already used

        code.Used = true;
        await _dbContext.SaveChangesAsync();

        return new UseCodeResponse { Result = 0 }; // Success
    }

    private string GenerateRandomCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var bytes = RandomNumberGenerator.GetBytes(length);
        return new string(bytes.Select(b => chars[b % chars.Length]).ToArray());
    }
}
