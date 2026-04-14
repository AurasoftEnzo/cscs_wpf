using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCSCS.ServiceReference2_B2BFinaInvoiceWebService;

namespace WpfCSCS.Fiskalizacija2.FINA.B2BIncoming
{
    public class GetOutgoingInvoice
    {
        private const string GET_B2B_OUTGOING_INVOICE_MESSAGE_TYPE = "9107";

        public Variable Send(string endpointAddress,
                                string dnsIdentity,
                                string serviceCertificatePath,
                                string clientCertificatePath,
                                string clientCertificatePassword,

                                string messageId,
                                string supplierId,
                                string invoiceId,

                                string additionalSupplierId = null,
                                string messageAttributes = null,

                                bool documentCurrencyCode = false)
        {
            GetB2BOutgoingInvoiceMsg message = new GetB2BOutgoingInvoiceMsg();

            //Header
            HeaderSupplierType header = new HeaderSupplierType();
            header.MessageID = messageId;
            header.SupplierID = supplierId;
            header.AdditionalSupplierID = additionalSupplierId;
            header.MessageType = GET_B2B_OUTGOING_INVOICE_MESSAGE_TYPE;
            header.MessageAttributes = messageAttributes;

            message.HeaderSupplier = header;

            //Data
            GetB2BOutgoingInvoiceMsgData data = new GetB2BOutgoingInvoiceMsgData();

            GetB2BOutgoingInvoiceMsgDataB2BOutgoingInvoice B2BOutgoingInvoice = new GetB2BOutgoingInvoiceMsgDataB2BOutgoingInvoice();

            B2BOutgoingInvoice.DocumentCurrencyCode = documentCurrencyCode;
            B2BOutgoingInvoice.DocumentCurrencyCodeSpecified = documentCurrencyCode;

            B2BOutgoingInvoice.InvoiceID = invoiceId;

            data.B2BOutgoingInvoice = B2BOutgoingInvoice;
            message.Data = data;


            //SEND
            FinaInvoiceB2BPortTypeClient client = _client.GetFinaInvoiceB2BPortTypeClient(endpointAddress,
                                                                                         dnsIdentity,
                                                                                         serviceCertificatePath,
                                                                                         clientCertificatePath,
                                                                                         clientCertificatePassword);
            GetB2BOutgoingInvoiceAckMsg res = client.getB2BOutgoingInvoice(message);

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
            if (res.Item is GetB2BOutgoingInvoiceAckMsgB2BOutgoingInvoice)
            {
                GetB2BOutgoingInvoiceAckMsgB2BOutgoingInvoice incomingInvoice = (GetB2BOutgoingInvoiceAckMsgB2BOutgoingInvoice)res.Item;

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

                // OutgoingInvoiceEnvelope 
                retVar.SetHashVariable("ItemElementName", new Variable(incomingInvoice.OutgoingInvoiceEnvelope.ItemElementName.ToString()));
                string ItemContent = System.Text.Encoding.UTF8.GetString(incomingInvoice.OutgoingInvoiceEnvelope.Item);
                // // //string ItemContent = Convert.ToBase64String(incomingInvoice.OutgoingInvoiceEnvelope.Item);
                retVar.SetHashVariable("ItemContent", new Variable(ItemContent));

                //string PdfDocumentContent = System.Text.Encoding.Default.GetString(incomingInvoice.OutgoingInvoiceEnvelope.PdfDocument);
                string Base64PdfDocumentString = Convert.ToBase64String(incomingInvoice.OutgoingInvoiceEnvelope.PdfDocument);
                if (!string.IsNullOrEmpty(Base64PdfDocumentString))
                {
                    retVar.SetHashVariable("Base64PdfDocumentString", new Variable(Base64PdfDocumentString));
                }
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
