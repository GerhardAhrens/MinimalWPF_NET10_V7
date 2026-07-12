namespace MinimalWPF.Beispiele
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Net.Mail;
    using System.Text.RegularExpressions;
    using System.Windows;

    public partial class EmailValidator : IValidator<string>
    {
        public ValidationResult Validate(string value)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(value) == true)
            {
                result.AddError("Email Adresse darf nicht leer sein.");
                return result;
            }


            (bool,string) validResult = IsValidExternalEmail(value);
            if (validResult.Item1 == false)
            {

                result.AddError(validResult.Item2);
            }

            return result;
        }

        /// <summary>
        /// Validates if an email is syntactically correct, has an "external" domain, 
        /// and that the domain actually exists (DNS check).
        /// </summary>
        /// <param name="email">Email address to validate</param>
        /// <returns>True if valid and external, false otherwise</returns>
        private static (bool IsValid, string ErrorMessage) IsValidExternalEmail(string email)
        {
            // 1 . Reject null, empty or whitespace emails
            if (string.IsNullOrWhiteSpace(email))
            {
                return (false, "Email Adresse darf nicht leer sein.");
            }

            try
            {
                // 2. Basic syntax check using MailAddress
                var addr = new MailAddress(email);
                var domain = addr.Host;

                // 3. Reject domain literals (like [192.168.1.1] or [mydomain.com])
                if (domain.StartsWith('[') || domain.EndsWith(']'))
                {
                    return (false, "Die E-Mail-Adresse darf weder mit eckigen Klammern beginnen noch enden. ('[', ']').");
                }

                // 4. Convert internationalized domain names (IDN) to ASCII (punycode)
                domain = new IdnMapping().GetAscii(domain);

                // 5. Domain must contain at least one dot
                if (domain.Contains('.') is false)
                {
                    return (false, "Die E-Mail-Adresse enthält im Host-/Domain-Teil keinen '.' im Host-/Domain-Teil.");
                }

                // 6. Domain must match valid pattern: letters, digits, hyphen; no leading/trailing hyphen; valid TLD
                if (DomainValidationRegex().IsMatch(domain) is false)
                {
                    return (false, "Die E-Mail-Adresse enthält ungültige Zeichen bzw. das Format des Hosts/der Domain ist ungültig.");
                }

                // 7. DNS check: ensure domain actually exists
                if (DomainExists(domain) is false)
                {
                    return (false, "Die E-Mail-Adresse enthält einen Host/eine Domain, die im DNS nicht verifiziert werden kann..");
                }

                // All checks passed
                return (true, null);
            }
            catch (Exception ex)
            {
                // 8. MailAddress throw an exception:
                return (false, $"Die E-Mail-Adresse ist ungültig.: '{ex.Message}'.");
            }
        }

        [GeneratedRegex(@"^(?!-)([a-zA-Z0-9-]{1,63}\.)+[a-zA-Z]{2,63}$")]
        private static partial Regex DomainValidationRegex();


        /// <summary>
        /// Checks if a domain has a valid DNS entry (A, AAAA or MX records)
        /// </summary>
        /// <param name="domain">Domain to check</param>
        /// <returns>True if domain exists, false otherwise</returns>
        private static bool DomainExists(string domain)
        {
            try
            {
                // Try to get host entry (will resolve A/AAAA records)
                var host = Dns.GetHostEntry(domain);
                return host.AddressList.Length > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
