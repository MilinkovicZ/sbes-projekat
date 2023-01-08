using Common;
using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class LocalDBCertValidator : X509CertificateValidator
    {
        public override void Validate(X509Certificate2 certificate)
        {
            X509Certificate2 myCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine,
                 "LocalDB");

            if (!myCert.Issuer.Equals(certificate.Subject))
            {
                throw new Exception("Certificate is self-issued.");
            }
        }
    }
}
