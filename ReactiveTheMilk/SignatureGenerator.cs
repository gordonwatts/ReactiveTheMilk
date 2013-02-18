using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if NETFX_CORE
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.Security.Cryptography;
#else
using System.Security.Cryptography;
#endif

namespace ReactiveTheMilk
{
  public class SignatureGenerator
  {
    /// <summary>
    /// RTM API secret
    /// </summary>
    private readonly string secret;

    /// <summary>
    /// ReactiveTheMilk.SignatureGenerator クラスの新しいインスタンスを初期化し、secretを設定します。
    /// </summary>
    /// <param name="secret">RTM API secret。</param>
    public SignatureGenerator(string secret)
    {
      this.secret = secret;
    }

    /// <summary>
    /// RTM APIパラメーターを元に、signatureを生成します。
    /// </summary>
    /// <param name="parameters">RTM APIパラメーター。</param>
    /// <returns>signature。</returns>
    public string Generate(IEnumerable<Parameter> parameters)
    {
      var orderedFlattenedParameters = parameters
        .OrderBy(x => x.Key)
        .Select(x => x.Key + x.Value)
        .Concat();

      string signatureSource = secret + orderedFlattenedParameters;
#if NETFX_CORE
      HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
      CryptographicHash objHash = objAlgProv.CreateHash();

      IBuffer buffMsg1 = CryptographicBuffer.ConvertStringToBinary(signatureSource, BinaryStringEncoding.Utf8);
      objHash.Append(buffMsg1);
      IBuffer buffHash1 = objHash.GetValueAndReset();

      String hash = CryptographicBuffer.EncodeToHexString(buffHash1);
      return hash;
#else
      using (var md5 = MD5.Create())
      {
        byte[] md5sumBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(signatureSource));
        return BitConverter.ToString(md5sumBytes).ToLower().Replace("-", "");
      }
#endif
    }
  }
}
