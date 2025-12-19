using hr.fina.eracun.signature;
using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCSCS.ServiceReference1_fina_wsdl;

namespace WpfCSCS.Fiskalizacija2.FINA.Outgoing
{
    public class SendOutgoingInvoiceReporting
    {

        private const string B2B_SEND_INVOICE_REPORTING_MESSAGE_TYPE = "9113";

        public Variable Send(string endpointAddress,
                             string dnsIdentity,
                             string serviceCertificatePath,
                             string clientCertificatePath,
                             string clientCertificatePassword,
                             string messageId,
                             string supplierID,
                             string buyerID,
                             string supplierInvoiceID,
                             ItemChoiceType1 itemChoiceType,
                             string unsignedInvoiceXMLPath,
                             string additionalSupplierId = null,
                             string additionalBuyerId = null,
                             string erpid = null,
                             string messageAttributes = null,
                             SendB2BOutgoingInvoiceReportingMsgDataB2BOutgoingInvoiceEnvelopeSpecificationIdentifier specificationIdentifier = SendB2BOutgoingInvoiceReportingMsgDataB2BOutgoingInvoiceEnvelopeSpecificationIdentifier.urnceneuen169312017complianturnmfingovhrcius202510conformanturnmfingovhrext202510)
        {
            SendB2BOutgoingInvoiceReportingMsg message = new SendB2BOutgoingInvoiceReportingMsg();

            //Header
            HeaderSupplierType header = new HeaderSupplierType();
            header.MessageID = messageId;
            header.SupplierID = supplierID;
            header.AdditionalSupplierID = additionalSupplierId;
            header.ERPID = erpid;
            header.MessageType = B2B_SEND_INVOICE_REPORTING_MESSAGE_TYPE;
            header.MessageAttributes = messageAttributes;

            message.HeaderSupplier = header;

            //Data
            SendB2BOutgoingInvoiceReportingMsgData data = new SendB2BOutgoingInvoiceReportingMsgData();

            SendB2BOutgoingInvoiceReportingMsgDataB2BOutgoingInvoiceEnvelope B2BOutgoingInvoiceEnvelope = new SendB2BOutgoingInvoiceReportingMsgDataB2BOutgoingInvoiceEnvelope();

            B2BOutgoingInvoiceEnvelope.XMLStandard = SendB2BOutgoingInvoiceReportingMsgDataB2BOutgoingInvoiceEnvelopeXMLStandard.UBL;
            B2BOutgoingInvoiceEnvelope.SpecificationIdentifier = specificationIdentifier;
            B2BOutgoingInvoiceEnvelope.SupplierInvoiceID = supplierInvoiceID;
            B2BOutgoingInvoiceEnvelope.BuyerID = buyerID;
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
            SendB2BOutgoingInvoiceReportingAckMsg rez = client.sendB2BOutgoingInvoiceReporting(message);


            MessageAckType messageAck = rez.MessageAck;
            SendB2BOutgoingInvoiceReportingAckMsgB2BOutgoingInvoiceEnvelope resB2BOutgoingInvoiceEnvelope = rez.B2BOutgoingInvoiceEnvelope;


            //RETURN
            Variable retVar = new Variable(Variable.VarType.ARRAY);

            retVar.SetHashVariable("AckStatus", new Variable(messageAck.AckStatus.ToString()));
            retVar.SetHashVariable("AckStatusCode", new Variable(messageAck.AckStatusCode));
            retVar.SetHashVariable("AckStatusText", new Variable(messageAck.AckStatusText));
            retVar.SetHashVariable("MessageID", new Variable(messageAck.MessageID));
            retVar.SetHashVariable("MessageAckID", new Variable(messageAck.MessageAckID));
            retVar.SetHashVariable("MessageType", new Variable(messageAck.MessageType));


            // // !!! DODAT rezultat(B2BOutgoingInvoiceEnvelope) u retVar(Variable) i vratit CSCS-u

            //if (resB2BOutgoingInvoiceEnvelope != null && resB2BOutgoingInvoiceEnvelope.B2BOutgoingInvoiceProcessing != null)
            //{
            //    var item = resB2BOutgoingInvoiceEnvelope.B2BOutgoingInvoiceProcessing.Item;
            //    if (item is SendB2BOutgoingInvoiceAckMsgB2BOutgoingInvoiceEnvelopeB2BOutgoingInvoiceProcessingCorrectB2BOutgoingInvoice)
            //    {
            //        retVar.SetHashVariable("Correct", new Variable(true));

            //        var correctItem = item as SendB2BOutgoingInvoiceAckMsgB2BOutgoingInvoiceEnvelopeB2BOutgoingInvoiceProcessingCorrectB2BOutgoingInvoice;

            //        retVar.SetHashVariable("SupplierInvoiceID", new Variable(correctItem.SupplierInvoiceID));
            //        retVar.SetHashVariable("InvoiceID", new Variable(correctItem.InvoiceID));
            //        retVar.SetHashVariable("InvoiceTimestamp", new Variable(correctItem.InvoiceTimestamp));
            //    }
            //    else if (item is SendB2BOutgoingInvoiceAckMsgB2BOutgoingInvoiceEnvelopeB2BOutgoingInvoiceProcessingIncorrectB2BOutgoingInvoice)
            //    {
            //        retVar.SetHashVariable("Correct", new Variable(false));

            //        var incorrectItem = item as SendB2BOutgoingInvoiceAckMsgB2BOutgoingInvoiceEnvelopeB2BOutgoingInvoiceProcessingIncorrectB2BOutgoingInvoice;

            //        retVar.SetHashVariable("SupplierInvoiceID", new Variable(incorrectItem.SupplierInvoiceID));
            //        retVar.SetHashVariable("ErrorCode", new Variable(incorrectItem.ErrorCode));
            //        retVar.SetHashVariable("ErrorMessage", new Variable(incorrectItem.ErrorMessage));
            //    }
            //}

            return retVar;
        }

        private byte[] GetSignedUBLXml(string inputFilePath, string clientCertificatePath, string clientCertificatePassword)
        {
            byte[] inputByteArray = File.ReadAllBytes(inputFilePath);

            UBLSigner ublSigner = new UBLSigner(clientCertificatePath, clientCertificatePassword, "Invoice");
            byte[] signedXML = ublSigner.signUBLDocument(inputByteArray);

            return signedXML;
        }
    }
}
