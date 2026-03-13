using java.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WpfCSCS.ServiceReference5_EIzvjestavanjeService;
using SplitAndMerge;

namespace WpfCSCS.Fiskalizacija2.POREZNA
{
    /// <summary>
    /// Custom SignedXml class that can find elements by lowercase "id" attribute
    /// </summary>
    public class SignedXmlWithId : SignedXml
    {
        public SignedXmlWithId(XmlDocument xml) : base(xml) { }

        public override XmlElement GetIdElement(XmlDocument doc, string id)
        {
            // First try the base implementation
            XmlElement elem = base.GetIdElement(doc, id);
            if (elem != null) return elem;

            // Search through all elements for matching id attribute
            foreach (XmlElement element in doc.SelectNodes("//*"))
            {
                string[] idAttributeNames = { "id", "Id", "ID" };
                foreach (string attrName in idAttributeNames)
                {
                    if (element.HasAttribute(attrName) && element.GetAttribute(attrName) == id)
                    {
                        return element;
                    }
                }
            }

            return null;
        }
    }
        
    public class EvidentirajNaplatu
    {
        public Variable Send(string endpointAddress,

                                //string dnsIdentity,
                                //string serviceCertificatePath,

                                string clientCertificatePath,
                                string clientCertificatePassword,

                                string messageId,

                                string supplierInvoiceId,
                                DateTime invoiceIssueDate,


                                string supplierOib,
                                string buyerOib,

                                DateTime payedDate,

                                decimal payedAmount,
                                nacinPlacanja nacinPlacanja)
        {

            //string path = "D:\\WinX\\ERAC\\certifikati\\p12_aurasoft_demo.p12";
            //string clientCertificatePath = "T:\\winx\\erac\\certifikati\\p12_aurasoft_produk.p12";
            //string password = "Aurasoft1";

            var cert = new X509Certificate2(clientCertificatePath, clientCertificatePassword /*, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable*/ );

            //byte[] certBytes = cert.Export(X509ContentType.Cert, password);

            //var certificate = new X509Certificate2("D:\\WinX\\ERAC\\certifikati\\p12_aurasoft_demo.p12", "Aurasoft1");
            //var base64Certifikat = Convert.ToBase64String(certificate.Export(X509ContentType.Cert));

            //var requestUniqueId = Guid.NewGuid().ToString();

            var zahtjev = new EvidentirajNaplatuZahtjev()
            {
                id = messageId,
                Zaglavlje = new Zaglavlje()
                {
                    datumVrijemeSlanja = DateTime.Now
                },
                Naplata = new Naplata[] {
                    new Naplata()
                    {
                        brojDokumenta = supplierInvoiceId,
                        datumIzdavanja = invoiceIssueDate,
                        oibPorezniBrojIzdavatelja = supplierOib,
                        oibPorezniBrojPrimatelja = buyerOib,
                        datumNaplate = payedDate,
                        naplaceniIznos = payedAmount,
                        nacinPlacanja = nacinPlacanja
                    }
                }
                            //,
                            //Signature = new SignatureType()
                            //{
                            //    SignedInfo = new SignedInfoType()
                            //    {
                            //        CanonicalizationMethod = new CanonicalizationMethodType()
                            //        {
                            //            Algorithm = "http://www.w3.org/TR/2001/REC-xml-c14n"
                            //        },
                            //        SignatureMethod = new SignatureMethodType()
                            //        {
                            //            Algorithm = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"
                            //        },
                            //        Reference = new ReferenceType[] {
                            //            new ReferenceType() {
                            //                URI = Guid.NewGuid().ToString(),
                            //                Transforms = new TransformType[] {
                            //                    new TransformType() { Algorithm = "http://www.w3.org/2000/09/xmldsig#enveloped-signature" },
                            //                    new TransformType() { Algorithm = "http://www.w3.org/TR/2001/REC-xml-c14n" }
                            //                },
                            //                DigestMethod = new DigestMethodType() { Algorithm = "http://www.w3.org/2001/04/xmlenc#sha256" },
                            //                DigestValue = new byte[] { 255, 1, 2, 3, 4, 5 } // 4n6MLp...                            
                            //            }
                            //        }
                            //    },
                            //    SignatureValue = new SignatureValueType()
                            //    {
                            //        Value = new byte[] { 1, 2, 3 } // vK9QFpDUkKv...
                            //    },
                            //    KeyInfo = new KeyInfoType()
                            //    {
                            //        //ItemsElementName = new ItemsChoiceType2[] { ItemsChoiceType2.X509Data },
                            //        //Items = new object[] {
                            //        //    new X509DataType() {
                            //        //        ItemsElementName = new ItemsChoiceType[] { ItemsChoiceType.X509Certificate },
                            //        //        Items = new object[] { base64Certifikat }
                            //        //    }
                            //        //}
                            //        ItemsElementName = new ItemsChoiceType2[] { ItemsChoiceType2.X509Data },
                            //        Items = new object[] {
                            //            new X509DataType() {
                            //                ItemsElementName = new ItemsChoiceType[] { ItemsChoiceType.X509Certificate },
                            //                Items = new object[] { certBytes }
                            //            }
                            //        }
                            //        //Items = new object[] {
                            //        //    //new X509DataType() {
                            //        //    //    ItemsElementName = new ItemsChoiceType[] { ItemsChoiceType.X509Certificate },
                            //        //    //    //Items = new object[] { base64Certifikat }
                            //        //    //    Items = new object[] { base64Certifikat }
                            //        //    //}
                            //        //    base64Certifikat
                            //        //}
                            //    }
                            //}
            };


            // 1️ Serijalizacija u XmlDocument
            var serializer = new XmlSerializer(typeof(EvidentirajNaplatuZahtjev));
            var xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;

            // Definiraj namespace prefikse
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("eiz", "http://www.porezna-uprava.gov.hr/fin/2024/types/eIzvjestavanje");
            namespaces.Add("xd", "http://www.w3.org/2000/09/xmldsig#");

            using (var ms = new MemoryStream())
            {
                serializer.Serialize(ms, zahtjev, namespaces);
                ms.Position = 0;
                xmlDoc.Load(ms);
            }

            // 2️ Inicijalizacija SignedXml s custom klasom koja može pronaći element po lowercase "id"
            var signedXml = new SignedXmlWithId(xmlDoc);

            // 3 cert je već učitan

            signedXml.SigningKey = cert.GetRSAPrivateKey();

            // 4️ Kreiraj referencu na root element
            var reference = new Reference();
            reference.Uri = ""; // Sign entire document - izbjegava problem s pronalaženjem elementa po id
            reference.DigestMethod = SignedXml.XmlDsigSHA256Url;
            // Dodaj enveloped-signature transform (obavezno za potpis unutar dokumenta)
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigExcC14NTransform());
            signedXml.AddReference(reference);

            // 5️⃣ Dodaj KeyInfo
            var keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(cert));
            signedXml.KeyInfo = keyInfo;

            // 6️⃣ Compute signature
            signedXml.ComputeSignature();

            // 7️⃣ Ubaci Signature element u XML s "xd:" prefiksom
            XmlElement signatureXml = signedXml.GetXml();

            // Promijeni namespace prefix na "xd" za Signature element
            SetSignaturePrefix(signatureXml, "xd");

            xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(signatureXml, true));

            var kraj = xmlDoc.OuterXml;

            // 8️⃣ Deserialize natrag u EvidentirajNaplatuZahtjev
            EvidentirajNaplatuZahtjev signedZahtjev;
            using (var reader = new XmlNodeReader(xmlDoc))
            {
                signedZahtjev = (EvidentirajNaplatuZahtjev)serializer.Deserialize(reader);
            }



            //ŠALJI
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
            binding.MaxReceivedMessageSize = 20000000; // 20MB
            binding.ReaderQuotas.MaxStringContentLength = 20000000;


            //var testRemoteAddress = new EndpointAddress("https://cistest.apis-it.hr:8509/FiskalizacijaServiceEprod");
            //var productionRemoteAddress = new EndpointAddress("https://cis.porezna-uprava.hr:8509/FiskalizacijaService");

            var remoteAddress = new EndpointAddress(endpointAddress);

            EIzvjestavanjeServicePortTipClient client = new EIzvjestavanjeServicePortTipClient(binding, remoteAddress);
            client.ClientCredentials.ClientCertificate.Certificate = cert;


            // 7️ Optional: for test environment, ignore server certificate errors
            // SAMO ZA TEST !!!
            //ServicePointManager.ServerCertificateValidationCallback +=
            //    (sender, certificate, chain, sslPolicyErrors) => true;

            
            EvidentirajNaplatuOdgovor response = client.EvidentirajNaplatu(signedZahtjev);

            // Serijaliziraj response u XML string
            string responseXml = "";
            var responseSerializer = new XmlSerializer(typeof(EvidentirajNaplatuOdgovor));
            using (var sw = new StringWriter())
            {
                responseSerializer.Serialize(sw, response);
                responseXml = sw.ToString();
            }

            //RETURN
            Variable retVar = new Variable(Variable.VarType.ARRAY);

            // Response XML
            retVar.SetHashVariable("ResponseXml", new Variable(responseXml));


            // Zaglavlje odgovora
            if (response.Odgovor != null)
            {
                retVar.SetHashVariable("prihvacenZahtjev", new Variable(response.Odgovor.prihvacenZahtjev));
                retVar.SetHashVariable("greskaSifra", new Variable(response.Odgovor.greska.sifra.ToString()));
                retVar.SetHashVariable("greskaOpis", new Variable(response.Odgovor.greska.opis));
            }

            return retVar;


            //var zahtjev1 = new EvidentirajNaplatuZahtjev1()
            //{
            //    EvidentirajNaplatuZahtjev = zahtjev
            //};
            //XmlSerializer serializer = new XmlSerializer(typeof(EvidentirajNaplatuZahtjev1));
            //using (StringWriter writer = new StringWriter())
            //{
            //    serializer.Serialize(writer, zahtjev1);
            //    string xml = writer.ToString();
            //    //Console.WriteLine(xml);
            //}

            //var zahtjev = new EvidentirajNaplatuZahtjev()
            //{
            //    id = "asdasd",
            //    Zaglavlje = new Zaglavlje()
            //    {
            //        datumVrijeme = DateTime.Now,
            //        idPoruke = "asdasd",
            //        oibPorezniBrojIzdavatelja = "asdasd",
            //        oibPorezniBrojPrimatelja = "asdasd",
            //        poslovniProstorOznaka = "asdasd",
            //        uredjajOznaka = "asdasd",
            //        datumVrijemeSlanja = DateTime.Now
            //    },
            //    Naplata = new Naplata()
            //    {
            //        brojDokumenta = "asdasd",
            //        datumIzdavanja = DateTime.Now,
            //        datumNaplate = DateTime.Now,
            //        nacinPlacanja = nacinPlacanja.KARTICNO,
            //        naplaceniIznos = 123,
            //        oibPorezniBrojIzdavatelja = "asdasd",
            //        oibPorezniBrojPrimatelja = "asdasd"
            //    },
            //    Signature = new SignatureType()
            //    {
            //        Id = "asdasd",
            //        Object = new ObjectType[] {
            //            new ObjectType() {
            //                Id = "asdasd",
            //                MimeType = "asdasd",
            //                Encoding = "asdasd",
            //                Data = new byte[] { 1, 2, 3 } //,
            //                //Any = 
            //            }
            //        },
            //        KeyInfo = new KeyInfoType()
            //        {
            //            Id = "asdasd",
            //            X509Data = new X509DataType()
            //            {
            //            },
            //            Items,
            //            a,
            //        },
            //        SignatureValue = new SignatureValueType()
            //        {
            //            Id = "asdasd",
            //            Value = new byte[] { 1, 2, 3 }
            //        },
            //        SignedInfo = new SignedInfoType()
            //        {
            //            CanonicalizationMethod = new CanonicalizationMethodType()
            //            {
            //                Algorithm = "asdasd"
            //            },
            //            Id = "asdasd",
            //            Reference = new ReferenceType[] {
            //                new ReferenceType() {
            //                    Id = "asdasd",
            //                    Uri = "asdasd",
            //                    Type = "asdasd",
            //                    DigestMethod = new DigestMethodType() {
            //                        Algorithm = "asdasd"
            //                    },
            //                    DigestValue = new byte[] { 1, 2, 3 },
            //                    Transforms = new byte[] { 1,2, 3 },
            //                    URI = "asdasd"
            //                }
            //            },
            //            SignatureMethod = new SignatureMethodType()
            //            {
            //                Algorithm = "asdasd",
            //                HMACOutputLength = "asdasd",
            //                Any = new System.Xml.XmlNode[] { },
            //            }
            //        }
            //    }
            //};

            //XmlSerializer serializer = new XmlSerializer(typeof(EvidentirajNaplatuZahtjev));
            //using (StringWriter writer = new StringWriter())
            //{
            //    serializer.Serialize(writer, zahtjev);
            //    string xml = writer.ToString();
            //    //Console.WriteLine(xml);
            //}

            //var lala = new WpfCSCS.ServiceReference5_EIzvjestavanjeService.EvidentirajNaplatuZahtjev1(zahtjev);
            //lala.EvidentirajNaplatuZahtjev = zahtjev;

            ////ŠALJI
            //EIzvjestavanjeServicePortTipClient client = new EIzvjestavanjeServicePortTipClient("Test", "https://cistest.apis-it.hr:8509/FiskalizacijaServiceEprod");
            ////EIzvjestavanjeServicePortTipClient client = new EIzvjestavanjeServicePortTipClient("Produkcija", "https://cis.porezna-uprava.hr:8509/FiskalizacijaService");
            //EvidentirajNaplatuOdgovor response = client.EvidentirajNaplatu(zahtjev);
        }

        /// <summary>
        /// Rekurzivno postavlja namespace prefix za sve elemente s xmldsig namespaceom
        /// </summary>
        private static void SetSignaturePrefix(XmlElement element, string prefix)
        {
            const string xmldsigNamespace = "http://www.w3.org/2000/09/xmldsig#";

            if (element.NamespaceURI == xmldsigNamespace)
            {
                element.Prefix = prefix;
            }

            foreach (XmlNode child in element.ChildNodes)
            {
                if (child is XmlElement childElement)
                {
                    SetSignaturePrefix(childElement, prefix);
                }
            }
        }
    }

    //class XmlSigner
    //{
    //    static void Main()
    //    {
    //        // 1. Učitaj XML koji potpisuješ
    //        var xmlDoc = new XmlDocument();
    //        xmlDoc.PreserveWhitespace = true; // važno za fiskalizaciju
    //        xmlDoc.Load("poruka.xml");        // npr. eIzvještaj.xml

    //        // 2. Učitaj certifikat (iz Windows store-a ili PFX datoteke)

    //        // Primjer A: iz Windows "My" store-a, po subject name
    //        var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
    //        store.Open(OpenFlags.ReadOnly);
    //        var certs = store.Certificates.Find(
    //            X509FindType.FindBySubjectName,
    //            "NazivSubjektaIliOIB",           // dio Subjecta certifikata
    //             validOnly: false
    //         );
    //        if (certs.Count == 0)
    //            throw new Exception("Certifikat nije pronađen.");

    //        X509Certificate2 cert = certs[0];

    //        // Primjer B (alternativa): iz PFX datoteke
    //        // X509Certificate2 cert = new X509Certificate2("certifikat.pfx", "lozinka",
    //        //     X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);


    //        // 3. Priprema SignedXml objekta

    //        var signedXml = new SignedXml(xmlDoc);
    //        signedXml.SigningKey = cert.GetRSAPrivateKey();

    //        // 4. Referenca na dio XML-a koji se potpisuje
    //        //    Obično se potpisuje cijeli dokument: Reference.Uri = ""
    //        var reference = new Reference
    //        {
    //            Uri = ""   // cijeli dokument; po potrebi može biti npr. "#IdPoruke"
    //        };

    //        // Dodaj canonicalization transform (tipično za fiskalizaciju)
    //        reference.AddTransform(new XmlDsigExcC14NTransform());

    //        signedXml.AddReference(reference);

    //        // 5. KeyInfo – uključivanje certifikata u potpis
    //        var keyInfo = new KeyInfo();
    //        keyInfo.AddClause(new KeyInfoX509Data(cert));
    //        signedXml.KeyInfo = keyInfo;

    //        // 6. Izračun potpisa
    //        signedXml.ComputeSignature();

    //        // 7. Dodaj <Signature> u XML dokument
    //        XmlElement xmlDigitalSignature = signedXml.GetXml();

    //        // Pozicija umetanja – često unutar root elementa, npr. na kraj
    //        xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));

    //        // 8. Spremi potpisani XML
    //        xmlDoc.Save("poruka-potpisana.xml");
    //    }
    //}



    //    class SoapClientExample
    //    {
    //        static async Task Main()
    //        {
    //            // 1. Učitaj klijentski certifikat (FINA aplikacijski certifikat)
    //            //    Opcija A: iz PFX-a
    //            var cert = new X509Certificate2(
    //                @"C:\cert\fina-certifikat.pfx",
    //                "lozinka",
    //                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);

    //            //    Opcija B: iz Windows store-a (ako ga tamo držiš)
    //            // var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
    //            // store.Open(OpenFlags.ReadOnly);
    //            // var certs = store.Certificates.Find(
    //            //     X509FindType.FindBySubjectName, "NAZIV_SUBJEKTA_ILI_OIB", false);
    //            // if (certs.Count == 0) throw new Exception("Certifikat nije pronađen.");
    //            // var cert = certs[0];

    //            // 2. Pripremi HttpClientHandler s certifikatom
    //            var handler = new HttpClientHandler();
    //            handler.ClientCertificates.Add(cert);

    //            // (opcionalno) ignoriranje self-signed certova u DEV okruženju
    //            // handler.ServerCertificateCustomValidationCallback =
    //            //    (message, serverCert, chain, errors) => true;

    //            using var httpClient = new HttpClient(handler);

    //            // 3. SOAP endpoint – za primjer stavi svoj URL (test/prod CIS)
    //            var url = "https://cis.porezna-uprava.hr/FiskalizacijaService"; // primjer

    //            // 4. Potpisani XML koji šalješ u SOAP envelope
    //            string signedXml = await System.IO.File.ReadAllTextAsync(
    //                @"C:\xml\poruka-potpisana.xml", Encoding.UTF8);

    //            // 5. Složi SOAP envelope – prilagodi po WSDL-u Porezne
    //            string soapEnvelope =
    //    $@"<?xml version=""1.0"" encoding=""UTF-8""?>
    //<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
    //                  xmlns:fis=""http://www.apis-it.hr/fin/2012/types/f73"">
    //  <soapenv:Header/>
    //  <soapenv:Body>
    //    <!-- Ovdje ubaciš svoj potpisani XML kao payload,
    //         npr. element <fis:EvidentirajPlacanjeERacunaZahtjev> -->
    //    {signedXml}
    //  </soapenv:Body>
    //</soapenv:Envelope>";

    //            var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");

    //            // 6. (opcionalno) SOAPAction header – ako ga servis traži
    //            content.Headers.Add("SOAPAction",
    //                "http://www.apis-it.hr/fin/2012/services/f73/EvidentirajPlacanjeERacuna");

    //            // 7. Pošalji zahtjev
    //            HttpResponseMessage response = await httpClient.PostAsync(url, content);

    //            string responseBody = await response.Content.ReadAsStringAsync();

    //            Console.WriteLine("HTTP status: " + (int)response.StatusCode);
    //            Console.WriteLine("Odgovor:");
    //            Console.WriteLine(responseBody);
    //        }
    //    }


}
