using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using WpfCSCS.ServiceReference1_fina_wsdl;

namespace WpfCSCS.Fiskalizacija2.FINA.Outgoing
{
    internal class _client
    {
        public static eRacunB2BPortTypeClient GeteRacunB2BPortTypeClient(string endpointUrl, string dnsIdentity, string serviceCertificatePath, string clientCertificatePath, string clientCertificatePassword)
        {
            //Web service endpoint with DNS identity (Subject from certificate)
            EndpointAddress endpointAddress = new EndpointAddress(new Uri(endpointUrl), EndpointIdentity.CreateDnsIdentity(dnsIdentity));

            //CustomBinding for eBox web service with security setup
            B2BCustomBinding B2BCustomBinding = new B2BCustomBinding();

            CustomBinding binding = B2BCustomBinding.GetB2BCustomBinding();
            
            //Web service client
            eRacunB2BPortTypeClient client = new eRacunB2BPortTypeClient(binding, endpointAddress);
            client.Endpoint.Contract.ProtectionLevel = ProtectionLevel.Sign;

            //client certificate (private key for signature)
            X509Certificate2 clientCertificate = new X509Certificate2(clientCertificatePath, clientCertificatePassword);
            client.ClientCredentials.ClientCertificate.Certificate = clientCertificate;

            //service certificate (public key for signature validation)
            X509Certificate2 serviceCertificate = new X509Certificate2(serviceCertificatePath);
            client.ClientCredentials.ServiceCertificate.DefaultCertificate = serviceCertificate;
            //*****************
            //https://stackoverflow.com/questions/48618685/soap-security-negotiation-failed-after-windows-security-update-kb4056892
            //*****************
            client.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;

            return client;
        }
    }
}
