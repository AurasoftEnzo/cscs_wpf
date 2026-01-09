using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WpfCSCS.ServiceReference2_B2BFinaInvoiceWebService;

namespace WpfCSCS.Fiskalizacija2.FINA.B2BIncoming
{
    public class GetIncomingInvoice
    {
        private const string GET_B2B_INCOMING_INVOICE_MESSAGE_TYPE = "9103";

        public Variable Send(string endpointAddress,
                                string dnsIdentity,
                                string serviceCertificatePath,
                                string clientCertificatePath,
                                string clientCertificatePassword,

                                string messageId,
                                string buyerId,
                                string invoiceId,

                                string additionalBuyerId = null,
                                string messageAttributes = null,

                                bool documentCurrencyCode = false)
        {
            GetB2BIncomingInvoiceMsg message = new GetB2BIncomingInvoiceMsg();

            //Header
            HeaderBuyerType header = new HeaderBuyerType();
            header.MessageID = messageId;
            header.BuyerID = buyerId;
            header.AdditionalBuyerID = additionalBuyerId;
            header.MessageType = GET_B2B_INCOMING_INVOICE_MESSAGE_TYPE;
            header.MessageAttributes = messageAttributes;

            message.HeaderBuyer = header;

            //Data
            GetB2BIncomingInvoiceMsgData data = new GetB2BIncomingInvoiceMsgData();

            GetB2BIncomingInvoiceMsgDataB2BIncomingInvoice B2BIncomingInvoice = new GetB2BIncomingInvoiceMsgDataB2BIncomingInvoice();

            B2BIncomingInvoice.DocumentCurrencyCode = documentCurrencyCode;
            B2BIncomingInvoice.DocumentCurrencyCodeSpecified = documentCurrencyCode;

            B2BIncomingInvoice.InvoiceID = invoiceId;

            data.B2BIncomingInvoice = B2BIncomingInvoice;

            message.Data = data;


            //SEND
            FinaInvoiceB2BPortTypeClient client = _client.GetFinaInvoiceB2BPortTypeClient(endpointAddress,
                                                                                         dnsIdentity,
                                                                                         serviceCertificatePath,
                                                                                         clientCertificatePath,
                                                                                         clientCertificatePassword);
            GetB2BIncomingInvoiceAckMsg res = client.getB2BIncomingInvoice(message);

            MessageAckType messageAck = res.MessageAck;

            //RETURN
            Variable retVar = new Variable(Variable.VarType.ARRAY);

            //MessageAck
            retVar.SetHashVariable("AckStatus", new Variable(messageAck.AckStatus.ToString()));
            retVar.SetHashVariable("AckStatusCode", new Variable(messageAck.AckStatusCode));
            retVar.SetHashVariable("AckStatusText", new Variable(messageAck.AckStatusText));
            retVar.SetHashVariable("MessageID", new Variable(messageAck.MessageID));
            retVar.SetHashVariable("MessageAckID", new Variable(messageAck.MessageAckID));
            retVar.SetHashVariable("MessageType", new Variable(messageAck.MessageType));

            //rest
            if (res.Item is GetB2BIncomingInvoiceAckMsgB2BIncomingInvoice)
            {
                GetB2BIncomingInvoiceAckMsgB2BIncomingInvoice incomingInvoice = (GetB2BIncomingInvoiceAckMsgB2BIncomingInvoice)res.Item;

                retVar.SetHashVariable("InvoiceID", new Variable(incomingInvoice.InvoiceID));
                retVar.SetHashVariable("InvoiceTimestamp", new Variable(incomingInvoice.InvoiceTimestamp));

                //DocumentType
                retVar.SetHashVariable("XMLStandard", new Variable(incomingInvoice.DocumentType.XMLStandard.ToString()));
                retVar.SetHashVariable("SpecificationIdentifier", new Variable(incomingInvoice.DocumentType.SpecificationIdentifier.ToString()));
                retVar.SetHashVariable("DocumentTypeCode", new Variable(incomingInvoice.DocumentType.DocumentTypeCode));
                retVar.SetHashVariable("DocumentTypeText", new Variable(incomingInvoice.DocumentType.DocumentTypeText));

                //InvoiceStatus
                retVar.SetHashVariable("StatusCode", new Variable(incomingInvoice.InvoiceStatus.StatusCode));
                retVar.SetHashVariable("Note", new Variable(incomingInvoice.InvoiceStatus.Note));
                // PartialAmount ??
                // DocumentCurrencyCode ??
                retVar.SetHashVariable("CodeReasonSpecified", new Variable(incomingInvoice.InvoiceStatus.CodeReasonSpecified));
                if (incomingInvoice.InvoiceStatus.CodeReasonSpecified)
                {
                    retVar.SetHashVariable("CodeReason", new Variable(incomingInvoice.InvoiceStatus.CodeReason.ToString()));
                }

                retVar.SetHashVariable("DataInterchangeMethod", new Variable(incomingInvoice.DataInterchangeMethod.ToString()));

                // IncomingInvoiceEnvelope 
                retVar.SetHashVariable("ItemElementName", new Variable(incomingInvoice.IncomingInvoiceEnvelope.ItemElementName));
                string ItemContent = System.Text.Encoding.Default.GetString(incomingInvoice.IncomingInvoiceEnvelope.Item);
                retVar.SetHashVariable("ItemContent", new Variable(ItemContent));

                //string PdfDocumentContent = System.Text.Encoding.Default.GetString(incomingInvoice.IncomingInvoiceEnvelope.PdfDocument);
                string Base64PdfDocumentString = Convert.ToBase64String(incomingInvoice.IncomingInvoiceEnvelope.PdfDocument);
                retVar.SetHashVariable("Base64PdfDocumentString", new Variable(Base64PdfDocumentString));
            }
            else if (res.Item is ErrorType)
            {
                ErrorType error = (ErrorType)res.Item;
                retVar.SetHashVariable("ErrorCode", new Variable(error.ErrorCode));
                retVar.SetHashVariable("ErrorText", new Variable(error.ErrorText));
                retVar.SetHashVariable("ErrorMessage", new Variable(error.ErrorMessage));
            }

            return retVar;
        }
    }
}
