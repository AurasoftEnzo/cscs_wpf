using CefSharp.DevTools.IO;
using DevExpress.Xpf.Core.Native;
using hr.fina.eracun.signature;
using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using WpfCSCS.ServiceReference1_fina_wsdl;

namespace WpfCSCS.Fiskalizacija2.FINA
{


    
    public class FINACommon
    {
        class B2BCustomBinding
        {
            public CustomBinding GetB2BCustomBinding()
            {
                CustomBinding binding = new CustomBinding();
                binding.Name = "B2BCustomBinding";

                AsymmetricSecurityBindingElement sbe = (AsymmetricSecurityBindingElement)SecurityBindingElement.CreateMutualCertificateDuplexBindingElement(MessageSecurityVersion.WSSecurity10WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10);
                sbe.AllowSerializedSigningTokenOnReply = true;
                sbe.SecurityHeaderLayout = SecurityHeaderLayout.Lax;
                sbe.IncludeTimestamp = true;
                sbe.DefaultAlgorithmSuite = new B2BSecurityAlgorithmSuite();
                sbe.EnableUnsecuredResponse = false;

                binding.Elements.Add(sbe);

                binding.Elements.Add(new TextMessageEncodingBindingElement(MessageVersion.Soap11, System.Text.Encoding.UTF8));

                HttpsTransportBindingElement htbe = new HttpsTransportBindingElement();
                htbe.RequireClientCertificate = true;

                binding.Elements.Add(htbe);

                return binding;
            }
            public CustomBinding GetB2GCustomBinding()
            {
                CustomBinding binding = new CustomBinding();
                binding.Name = "B2BCustomBinding";

                AsymmetricSecurityBindingElement sbe = (AsymmetricSecurityBindingElement)SecurityBindingElement.CreateMutualCertificateDuplexBindingElement(MessageSecurityVersion.WSSecurity10WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10);
                sbe.AllowSerializedSigningTokenOnReply = true;
                sbe.SecurityHeaderLayout = SecurityHeaderLayout.Lax;
                sbe.IncludeTimestamp = true;
                sbe.DefaultAlgorithmSuite = new B2BSecurityAlgorithmSuite();
                sbe.EnableUnsecuredResponse = false;

                binding.Elements.Add(sbe);

                binding.Elements.Add(new TextMessageEncodingBindingElement(MessageVersion.Soap11, System.Text.Encoding.UTF8));

                HttpsTransportBindingElement htbe = new HttpsTransportBindingElement();
                htbe.RequireClientCertificate = true;

                binding.Elements.Add(htbe);

                return binding;
            }
        }

        class B2BSecurityAlgorithmSuite : SecurityAlgorithmSuite
        {
            public override string DefaultAsymmetricKeyWrapAlgorithm
            {
                get { return SecurityAlgorithmSuite.Basic256Sha256Rsa15.DefaultAsymmetricKeyWrapAlgorithm; }
            }

            public override string DefaultAsymmetricSignatureAlgorithm
            {
                get { return SecurityAlgorithms.RsaSha256Signature; }
            }

            public override string DefaultCanonicalizationAlgorithm
            {
                get { return SecurityAlgorithmSuite.Basic256Sha256Rsa15.DefaultCanonicalizationAlgorithm; }
            }

            public override string DefaultDigestAlgorithm
            {
                get { return SecurityAlgorithmSuite.Basic256Sha256Rsa15.DefaultDigestAlgorithm; }
            }

            public override string DefaultEncryptionAlgorithm
            {
                get { return SecurityAlgorithmSuite.Basic256Sha256Rsa15.DefaultEncryptionAlgorithm; }
            }

            public override int DefaultEncryptionKeyDerivationLength
            {
                get { return SecurityAlgorithmSuite.Basic256Sha256Rsa15.DefaultEncryptionKeyDerivationLength; }
            }

            public override string DefaultSymmetricKeyWrapAlgorithm
            {
                get { return SecurityAlgorithmSuite.Basic256Sha256Rsa15.DefaultSymmetricKeyWrapAlgorithm; }
            }

            public override string DefaultSymmetricSignatureAlgorithm
            {
                get { return SecurityAlgorithmSuite.Basic256Sha256Rsa15.DefaultSymmetricSignatureAlgorithm; }
            }

            public override int DefaultSignatureKeyDerivationLength
            {
                get { return SecurityAlgorithmSuite.Basic256Sha256Rsa15.DefaultSignatureKeyDerivationLength; }
            }

            public override int DefaultSymmetricKeyLength
            {
                get { return SecurityAlgorithmSuite.Basic256Sha256Rsa15.DefaultSymmetricKeyLength; }
            }


            public override bool IsAsymmetricKeyLengthSupported(int i)
            {
                return SecurityAlgorithmSuite.Basic256Sha256Rsa15.IsAsymmetricKeyLengthSupported(i);
            }

            public override bool IsSymmetricKeyLengthSupported(int i)
            {
                return SecurityAlgorithmSuite.Basic256Sha256Rsa15.IsSymmetricKeyLengthSupported(i);
            }
        }

        public static class SecurityAlgorithms
        {
            //
            // Summary:
            //     Specifies a URI that points to the 128-bit AES cryptographic algorithm for encrypting
            //     XML. This field is constant.
            public const string Aes128Encryption = "http://www.w3.org/2001/04/xmlenc#aes128-cbc";
            //
            // Summary:
            //     Specifies a URI that points to the 128-bit AES cryptographic algorithm for encrypting
            //     and decrypting symmetric keys (key wrap). This field is constant.
            public const string Aes128KeyWrap = "http://www.w3.org/2001/04/xmlenc#kw-aes128";
            //
            // Summary:
            //     Specifies a URI that points to the 192-bit AES cryptographic algorithm for encrypting
            //     XML. This field is constant.
            public const string Aes192Encryption = "http://www.w3.org/2001/04/xmlenc#aes192-cbc";
            //
            // Summary:
            //     Specifies a URI that points to the 192-bit AES cryptographic algorithm for encrypting
            //     and decrypting symmetric keys (key wrap). This field is constant.
            public const string Aes192KeyWrap = "http://www.w3.org/2001/04/xmlenc#kw-aes192";
            //
            // Summary:
            //     Specifies a URI that points to the 256-bit AES cryptographic algorithm for encrypting
            //     XML. This field is constant.
            public const string Aes256Encryption = "http://www.w3.org/2001/04/xmlenc#aes256-cbc";
            //
            // Summary:
            //     Specifies a URI that points to the 256-bit AES cryptographic algorithm for encrypting
            //     and decrypting symmetric keys (key wrap). This field is constant.
            public const string Aes256KeyWrap = "http://www.w3.org/2001/04/xmlenc#kw-aes256";
            //
            // Summary:
            //     Specifies a URI that points to the DES cryptographic algorithm for encrypting
            //     XML. This field is constant.
            public const string DesEncryption = "http://www.w3.org/2001/04/xmlenc#des-cbc";
            //
            // Summary:
            //     Specifies a URI that points to the DSA cryptographic algorithm for digitally
            //     signing XML. This field is constant.
            public const string DsaSha1Signature = "http://www.w3.org/2000/09/xmldsig#dsa-sha1";
            //
            // Summary:
            //     Represents the Exclusive XML Without Comments Canonicalization algorithm. This
            //     field is constant.
            public const string ExclusiveC14n = "http://www.w3.org/2001/10/xml-exc-c14n#";
            //
            // Summary:
            //     Represents the Exclusive XML With Comments Canonicalization algorithm. This field
            //     is constant.
            public const string ExclusiveC14nWithComments = "http://www.w3.org/2001/10/xml-exc-c14n#WithComments";
            //
            // Summary:
            //     Specifies a URI that points to the HMAC cryptographic algorithm for digitally
            //     signing XML. This field is constant.
            public const string HmacSha1Signature = "http://www.w3.org/2000/09/xmldsig#hmac-sha1";
            //
            // Summary:
            //     Specifies a URI that points to the 256-bit HMAC cryptographic algorithm for digitally
            //     signing XML. This field is constant.
            public const string HmacSha256Signature = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256";
            //
            // Summary:
            //     Represents the P-SHA1 key generation algorithm. This field is constant.
            public const string Psha1KeyDerivation = "http://schemas.xmlsoap.org/ws/2005/02/sc/dk/p_sha1";
            //
            // Summary:
            //     Represents the December 2007 version of the P-SHA1 key generation algorithm.
            //     This field is constant.
            public const string Psha1KeyDerivationDec2005 = "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512/dk/p_sha1";
            //
            // Summary:
            //     Specifies a URI that points to the RIPEMD-160 cryptographic digest algorithm.
            //     This field is constant.
            public const string Ripemd160Digest = "http://www.w3.org/2001/04/xmlenc#ripemd160";
            //
            // Summary:
            //     Specifies a URI that points to the RSAES-OAEP-ENCRYPT cryptographic algorithm
            //     for encrypting and decrypting asymmetric keys (key wrap). This field is constant.
            public const string RsaOaepKeyWrap = "http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p";
            //
            // Summary:
            //     Specifies a URI that points to the RSA-SHA1 cryptographic algorithm for digitally
            //     signing XML. This field is constant.
            public const string RsaSha1Signature = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
            //
            // Summary:
            //     Specifies a URI that points to the RSA-SHA256 cryptographic algorithm for digitally
            //     signing XML. This field is constant.
            public const string RsaSha256Signature = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
            //
            // Summary:
            //     Specifies a URI that points to the RSAES-PKCS1-v1_5 cryptographic algorithm for
            //     encrypting and decrypting asymmetric keys (key wrap). This field is constant.
            public const string RsaV15KeyWrap = "http://www.w3.org/2001/04/xmlenc#rsa-1_5";
            //
            // Summary:
            //     Specifies a URI that points to the 160-bit SHA-1 digest algorithm. This field
            //     is constant.
            public const string Sha1Digest = "http://www.w3.org/2000/09/xmldsig#sha1";
            //
            // Summary:
            //     Specifies a URI that points to the 256-bit SHA-256 digest algorithm. This field
            //     is constant.
            public const string Sha256Digest = "http://www.w3.org/2001/04/xmlenc#sha256";
            //
            // Summary:
            //     Specifies a URI that points to the 512-bit SHA-512 digest algorithm. This field
            //     is constant.
            public const string Sha512Digest = "http://www.w3.org/2001/04/xmlenc#sha512";
            //
            // Summary:
            //     Specifies a URI that points to the Transport Layer Security (TLS) algorithm for
            //     encrypting and decrypting symmetric keys (key wrap). This field is constant.
            public const string TlsSspiKeyWrap = "http://schemas.xmlsoap.org/2005/02/trust/tlsnego#TLS_Wrap";
            //
            // Summary:
            //     Specifies a URI that points to the Triple DES cryptographic algorithm for encrypting
            //     XML. This field is constant.
            public const string TripleDesEncryption = "http://www.w3.org/2001/04/xmlenc#tripledes-cbc";
            //
            // Summary:
            //     Specifies a URI that points to the Triple DES cryptographic algorithm for encrypting
            //     and decrypting symmetric keys (key wrap). This field is constant.
            public const string TripleDesKeyWrap = "http://www.w3.org/2001/04/xmlenc#kw-tripledes";
            //
            // Summary:
            //     Specifies a URI that points to the GSS-API cryptographic algorithm for encrypting
            //     and decrypting Kerberos ticket session keys (key wrap). This field is constant.
            public const string WindowsSspiKeyWrap = "http://schemas.xmlsoap.org/2005/02/trust/spnego#GSS_Wrap";
        }


        public static eRacunB2BPortTypeClient GeteRacunB2BPortTypeClientClient(string endpointUrl, string dnsIdentity, string serviceCertificatePath, string clientCertificatePath, string clientCertificatePassword)
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