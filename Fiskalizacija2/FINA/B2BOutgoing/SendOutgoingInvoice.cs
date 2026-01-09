using hr.fina.eracun.signature;
using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCSCS.ServiceReference1_SendB2BOutgoingInvoicePKIWebService;

namespace WpfCSCS.Fiskalizacija2.FINA.B2BOutgoing
{
    public class SendOutgoingInvoice
    {

        private const string B2B_SEND_INVOICE_MESSAGE_TYPE = "9001";

        public Variable Send(string endpointAddress,
                             string dnsIdentity,
                             string serviceCertificatePath,
                             string clientCertificatePath,
                             string clientCertificatePassword,
                             string erpid,

                             string messageId,
                             string supplierID,
                             string buyerID,
                             string supplierInvoiceID,
                             ItemChoiceType itemChoiceType,
                             string unsignedInvoiceXMLPath,

                             string additionalSupplierId = null,
                             string additionalBuyerId = null,
                             string messageAttributes = null,
                             SendB2BOutgoingInvoiceMsgDataB2BOutgoingInvoiceEnvelopeSpecificationIdentifier specificationIdentifier = SendB2BOutgoingInvoiceMsgDataB2BOutgoingInvoiceEnvelopeSpecificationIdentifier.urnceneuen169312017complianturnmfingovhrcius202510conformanturnmfingovhrext202510)
        {
            SendB2BOutgoingInvoiceMsg message = new SendB2BOutgoingInvoiceMsg();

            //Header
            HeaderSupplierType header = new HeaderSupplierType();
            header.MessageID = messageId;
            header.SupplierID = supplierID;
            header.MessageType = B2B_SEND_INVOICE_MESSAGE_TYPE;
            header.AdditionalSupplierID = additionalSupplierId;
            header.ERPID = erpid;
            header.MessageAttributes = messageAttributes;

            message.HeaderSupplier = header;

            //Data
            SendB2BOutgoingInvoiceMsgData data = new SendB2BOutgoingInvoiceMsgData();

            SendB2BOutgoingInvoiceMsgDataB2BOutgoingInvoiceEnvelope B2BOutgoingInvoiceEnvelope = new SendB2BOutgoingInvoiceMsgDataB2BOutgoingInvoiceEnvelope();

            B2BOutgoingInvoiceEnvelope.BuyerID = buyerID;
            B2BOutgoingInvoiceEnvelope.XMLStandard = SendB2BOutgoingInvoiceMsgDataB2BOutgoingInvoiceEnvelopeXMLStandard.UBL;
            //B2BOutgoingInvoiceEnvelope.SpecificationIdentifier = SendB2BOutgoingInvoiceMsgDataB2BOutgoingInvoiceEnvelopeSpecificationIdentifier.urnceneuen169312017_new; // !!!
            B2BOutgoingInvoiceEnvelope.SpecificationIdentifier = specificationIdentifier;
            B2BOutgoingInvoiceEnvelope.SupplierInvoiceID = supplierInvoiceID;
            B2BOutgoingInvoiceEnvelope.AdditionalBuyerID = additionalBuyerId;

            B2BOutgoingInvoiceEnvelope.ItemElementName = itemChoiceType;

            byte[] signedInvoiceXml = GetSignedUBLXml(unsignedInvoiceXMLPath, clientCertificatePath, clientCertificatePassword);
            B2BOutgoingInvoiceEnvelope.Item = signedInvoiceXml;

            data.B2BOutgoingInvoiceEnvelope = B2BOutgoingInvoiceEnvelope;

            message.Data = data;


            //SEND
            eRacunB2BPortTypeClient client = _client.GeteRacunB2BPortTypeClient(endpointAddress,
                                                                              dnsIdentity,
                                                                              serviceCertificatePath,
                                                                              clientCertificatePath,
                                                                              clientCertificatePassword);
            SendB2BOutgoingInvoiceAckMsg rez = client.sendB2BOutgoingInvoice(message);


            MessageAckType messageAck = rez.MessageAck;
            SendB2BOutgoingInvoiceAckMsgB2BOutgoingInvoiceEnvelope resB2BOutgoingInvoiceEnvelope = rez.B2BOutgoingInvoiceEnvelope;


            //RETURN
            Variable retVar = new Variable(Variable.VarType.ARRAY);

            retVar.SetHashVariable("AckStatus", new Variable(messageAck.AckStatus.ToString()));
            retVar.SetHashVariable("AckStatusCode", new Variable(messageAck.AckStatusCode));
            retVar.SetHashVariable("AckStatusText", new Variable(messageAck.AckStatusText));
            retVar.SetHashVariable("MessageID", new Variable(messageAck.MessageID));
            retVar.SetHashVariable("MessageAckID", new Variable(messageAck.MessageAckID));
            retVar.SetHashVariable("MessageType", new Variable(messageAck.MessageType));

            if (resB2BOutgoingInvoiceEnvelope != null && resB2BOutgoingInvoiceEnvelope.B2BOutgoingInvoiceProcessing != null)
            {
                var item = resB2BOutgoingInvoiceEnvelope.B2BOutgoingInvoiceProcessing.Item;
                if (item is SendB2BOutgoingInvoiceAckMsgB2BOutgoingInvoiceEnvelopeB2BOutgoingInvoiceProcessingCorrectB2BOutgoingInvoice)
                {
                    retVar.SetHashVariable("Correct", new Variable(true));

                    var correctItem = item as SendB2BOutgoingInvoiceAckMsgB2BOutgoingInvoiceEnvelopeB2BOutgoingInvoiceProcessingCorrectB2BOutgoingInvoice;

                    retVar.SetHashVariable("SupplierInvoiceID", new Variable(correctItem.SupplierInvoiceID));
                    retVar.SetHashVariable("InvoiceID", new Variable(correctItem.InvoiceID));
                    retVar.SetHashVariable("InvoiceTimestamp", new Variable(correctItem.InvoiceTimestamp));
                }
                else if (item is SendB2BOutgoingInvoiceAckMsgB2BOutgoingInvoiceEnvelopeB2BOutgoingInvoiceProcessingIncorrectB2BOutgoingInvoice)
                {
                    retVar.SetHashVariable("Correct", new Variable(false));

                    var incorrectItem = item as SendB2BOutgoingInvoiceAckMsgB2BOutgoingInvoiceEnvelopeB2BOutgoingInvoiceProcessingIncorrectB2BOutgoingInvoice;

                    retVar.SetHashVariable("SupplierInvoiceID", new Variable(incorrectItem.SupplierInvoiceID));
                    retVar.SetHashVariable("ErrorCode", new Variable(incorrectItem.ErrorCode));
                    retVar.SetHashVariable("ErrorMessage", new Variable(incorrectItem.ErrorMessage));
                }
            }

            return retVar;
        }

        private byte[] GetSignedUBLXml(string inputFilePath, string clientCertificatePath, string clientCertificatePassword)
        {
            byte[] inputByteArray = File.ReadAllBytes(inputFilePath);

            UBLSigner ublSigner = new UBLSigner(clientCertificatePath, clientCertificatePassword, "Invoice");
            byte[] signedXML = ublSigner.signUBLDocument(inputByteArray);

            return signedXML;


            ////XmlSerializer xmlSerializer = new XmlSerializer(typeof(InvoiceType));
            ////MemoryStream memoryStream = new MemoryStream();
            ////xmlSerializer.Serialize(memoryStream, InvoiceType, xmlSerializerNamespaces);
            //////UBLSigner ublSigner = new UBLSigner(


            //UBLSigner ublSigner = new UBLSigner(clientCertificatePath, clientCertificatePassword, "Invoice");

            //byte[] signedXML = ublSigner.signUBLDocument(inputByteArray);

            ////File.WriteAllBytes("D:/Temp/zzzzz.xml", signedXML);

            //return signedXML;

            //var xxyx = GetSignedInvoiceFormattedXmlString(InvoiceType, clientCertificatePath, clientCertificatePassword);
            ////string clientCertificatePath = "putanja/do/privatnikljuc.p12";
            ////string clientCertificatePassword = "***password od p12***";

            //InvoiceType invoice = new InvoiceType();

            //IDType brojRacuna = new IDType();
            //brojRacuna.Value = "broj-racuna-0001-NET";
            //invoice.ID = brojRacuna;
            ////TODO napuniti xml


            //XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            //xmlSerializerNamespaces.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            //xmlSerializerNamespaces.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            //xmlSerializerNamespaces.Add("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
            //xmlSerializerNamespaces.Add("hrextac", "urn:mfin.gov.hr:schema:xsd:HRExtensionAggregateComponents-1");

            //XmlSerializer xmlSerializer = new XmlSerializer(typeof(InvoiceType));
            //MemoryStream memoryStream = new MemoryStream();
            //xmlSerializer.Serialize(memoryStream, InvoiceType, xmlSerializerNamespaces);
            ////UBLSigner ublSigner = new UBLSigner(
            //UBLSigner ublSigner = new UBLSigner(clientCertificatePath, clientCertificatePassword, "Invoice");

            //byte[] signedXML = ublSigner.signUBLDocument(memoryStream.ToArray());

            ////File.WriteAllBytes("D:/Temp/zzzzz.xml", signedXML);

            //return signedXML;
        }
    }
}
