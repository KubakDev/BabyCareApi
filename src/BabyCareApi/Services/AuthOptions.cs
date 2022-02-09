using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BabyCareApi.Services;

public class AuthOptions
{
  internal static readonly string Config = "Auth";

  private byte[] _SecretBytes = Array.Empty<byte>();

  public byte[] SecretBytes
  {
    get
    {
      if (_SecretBytes.Length == 0)
        _SecretBytes = Encoding.ASCII.GetBytes(Secret);

      return _SecretBytes;

    }
  }

  [Required]
  public string Secret { get; set; } = string.Empty;

  [Required]
  public string Issuer { get; set; } = "babycare.com";

  public TimeSpan TokenExpirationTime { get; set; } = TimeSpan.FromMinutes(10);

  public TimeSpan RefreshTokenExpirationTime { get; set; } = TimeSpan.FromDays(90);

  public TimeSpan VerificationCodeExpirationTime { get; set; } = TimeSpan.FromDays(1);

}