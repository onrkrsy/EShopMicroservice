using System.ComponentModel.DataAnnotations;

namespace EShop.Shared.Options;

public class IdentityServerOption
{
    [Required] public string Address { get; set; } = default!;
    [Required] public string Audience { get; set; } = default!;
}