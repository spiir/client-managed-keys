using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace ClientManagedKeys.Server
{
    public class DemoKeyProvider : IKeyProvider
    {
        private readonly string _keyId;
        private readonly RSA _rsa;

        public DemoKeyProvider()
        {
            var pfxFile = @"MIII8QIBAzCCCLcGCSqGSIb3DQEHAaCCCKgEggikMIIIoDCCA1cGCSqGSIb3DQEHBqCCA0gwggNEAgEAMIIDPQYJKoZIhvcNAQcBMBwGCiqGSIb3DQEMAQYwDgQIS+rH+Sj4+wQCAggAgIIDEASIX8EVwn2XB74Kp0RscSW7wYFcaZtj4L/zLXIM8EngphqD4STD+ZqMNlu/sKP1HvX0dwsDlzdYbNpisBbDPLfkFbYyTeiBD4+SiThMTXdQSndkJxntXeU8i3Yh9lvk802jjafb3omzKGrepwslPnQENUCxdCB4IXlZ/xPqWPdfKHSxifhmXEczpJDZOwVVvyKJ4lz5jNEBJUujaVVzEMZ/8CCw5tQrNGRAmUP9mInmG5koiosQRQCM6W5nvf7iQd/7sW31dTHog3kogPtATSA8VKH9X1dIstVLWZfGua3luNYzpZm5iThDPwTEYEk44eacjniISTIuxgsGIpgEMpt+TXEezFPYi1BQX7LFSp3rZoKnQX1dYCIlr0cxbNhnBd3CT5vB6fYdttgzFLfojpFIlsC/V2RKNn8RcKnuxNutoqOpsjP18H0/2g2WWkDApBaYCGPS6UPcYk/0cgoJIGY1yjswO65JaXPIZJ1G2n7Nuvwv8RCFLLJTBWrvocOxUA6P1Hd9ZpY5ungWZg61/v9sUEqMYEholPe5mqX02esOUBKnOZfftnUo/CjUKRrWBEYj9e8gu6uZaehThcy3NbVOYk0C3kwmxRlAav0jMaRFj+G+BQvM+Mc4i2Pk9JbNv8blFttGUIi/TLkQcrepwq1pyggigF4A9y574/7DWazB0tLLUkix0cdMgS+IRKk/p0CZtyMO5ABhjxgs/qshkuPrFXanLexp/C5CC6WSLvaxSKARyOMaaVo0kwbi7EoZC/yomaXVdOG0RtM122be4937LJ2uReDBG1u3H3QlhgVhbgqDz7ZFcwVJSSU+r4o4+tex0wcz1rkUyz+kMTsXVVy6wRA/iEZoI4xfKBOjAMCPHajFPPMI5rCz7PAKVxeL3j0U9Wu/C5i9TbmpUWAbqJsmyi8lgW4fF3FzY99clmkRkK15iP0NcnH1HczkQgVYP4QYFx7PyXj1UHemtWHnrkne8xluRTLPWihhN4n84KL7+WGPaqaD/ofwKgIcO9wU+Vvml4joh9im2El5aAnO3TgwggVBBgkqhkiG9w0BBwGgggUyBIIFLjCCBSowggUmBgsqhkiG9w0BDAoBAqCCBO4wggTqMBwGCiqGSIb3DQEMAQMwDgQIeZhv19gxjG8CAggABIIEyAPzoT5n7QqQHArjcbHjfqxDktGlm+yzZaNJSOSvAzpDMFJ/ifeqYruF5OcAkMsrYB7g+x376bO95hKbzcQ1Quo4dCfx5tD6ty7rjMEfDclVZTyKhCh+VHllMO/BL9TBi6rv/xpKzfym/iYwzPHkjsof6NjEuuwo9kQYi/V4ZFt/AlwFJyqTStC2af9fIxN56DERfEtXwiJb71p2rXIj4zdkUFzYEdB22khjvGCc3ADbppWQfBRECiJMob/dmzS80nSsSlos3+mrGwryp7onLv+TsxVxrdReFt4XfPfs/XH8PfLkgoRB3IPev5io1yf7jtkjKdcUh0H6Lsgz03+28GZ4xb5cIJPnncQVCTLL2YyEn7aVjre1b2d2kY8rJ09uYR5s+4B2ZnDI1nQaV0OfONg/EPQZuC608x72UDjwQTgzO0plN7CaFqks3N8nl0/fRSw8uz3hNWQMv/9gBBIJAeFacj8wZsVG0t7YXPgtSekh/KzsRJms9/4t9Iz2OKrQ0GqspyWRvoRbMUpDb/QWEY2e7/wwaJDCLbddzD9gWLZofanTutCHd/yxLXxN4q6OmXezw7qSLMpkOUOdcTgltIW3K7NpMu0wwrBBGjCoOYTucBdpfckspBjO60NsgOyKWfnsMb74eDnRRt3QAmrPFLDfYpL4LC7gBWTlb+3P0b6LoKnpvZdj4rQ5ybxGvuKcg3WZhK+O0TsmqpOyZZLe1IFBs95AOT+vdcxHLppU385/6J/v16rKuRJn7uzDFvQe+ioeYt06291/zUQI7BBkuAUthhy8cKlaSPJkqF22yMi9QN+2k43POhqqWiI7eVaAyYZLm9eCRQKSrW65R1RHpix5MBRWZdEQqAYutnGYAjsn4oB/CwU8v4SX92Cz+Y7dy5JGuo40j7c6K5WcnpuFraSQjsJwEfwKJU1PYX5KtN8HHIUB28SOnT/KIiuHeCbwZOXvT6d5VTGT4/vkdw/OLXZwntM0hfiG5pZzcJyTcDzvTTljM7buR8tx8SfXYh6YbasSz9z9Ez7GpFAa5E/zd+Alqx5s9s0poRUijwo/0Ugmk3Q6xzKIF9FItZMG719uteMhbkdOR0fSakxcazlzMKxs7jkJFK+LN5jFFEnIbv/29BIJO4SLUPNXhIw/0ukBTMq1MGPTdFjpHJa574UeQ/V7K9z34ogm42C8G9ytOuHoSJgm+PIPqKGDANCaC5wMJJTonRO9vCjGiQNrNnBWlay44VGy8TiOueaOB6ILsjaxYY4Y6ZRhD/AMfdk+OzvzsOnKTFrOeNHcT5nZ3hLw+sd8aOgvnsIT1HoHsCE+Q9Lt00E7HX1Nujdtpr+5ioevzvR0CdWV288TSi4Y838U8sVyvNzf98YVG4Zh68ZMscvbI+pQmk5lDI6qzNTCAlu32DhRn7rZMCGPDH2SClSg9ydYtlzgV56xJ00UHDwO0/H5JXJGY6UJK+WBHvXn9NZu2ZaTjGiEii5cxvTw+MFyySJTgcVfqDfzC8BrnckT6JM3HBIXtVpdkzFF1IrXnHWkLlbUQA4tnveTVFjVsNRjY8UkiLMEF1ZNg27Av8V+YUe5THahjK4THr0RU8PlBROMrOKMUOqmqi3rUiffcqkoP8XGPw8ODqTVhzElMCMGCSqGSIb3DQEJFTEWBBQOQIXArpQHitiZLj/fbr/7cH/6ijAxMCEwCQYFKw4DAhoFAAQUatQFxI7GTDZ2EjsFFn58ziVHzykECCvwalUGf5KNAgIIAA==";
            var password = "test";

            var pfxBytes = Convert.FromBase64String(pfxFile);

            var cert = new X509Certificate2(pfxBytes, password);
            
            _keyId = cert.Thumbprint.ToLowerInvariant();
            _rsa = (RSA)cert.PrivateKey;
        }

        public Dictionary<string, RSA> GetKeys()
        {
            return new Dictionary<string, RSA>
            {
                [_keyId] = _rsa
            };
        }
        
    }
}