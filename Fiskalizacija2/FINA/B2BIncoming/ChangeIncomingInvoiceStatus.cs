using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCSCS.ServiceReference2_B2BFinaInvoiceWebService;

namespace WpfCSCS.Fiskalizacija2.FINA.B2BIncoming
{
    public class ChangeIncomingInvoiceStatus
    {
        private const string CHANGE_B2B_INCOMING_INVOICE_STATUS_MESSAGE_TYPE = "107";

        public Variable Send(string endpointAddress,
                                string dnsIdentity,
                                string serviceCertificatePath,
                                string clientCertificatePath,
                                string clientCertificatePassword,

                                string messageId,
                                string buyerId,
                                string invoiceId,
                                InvoiceStatusTypeStatusCode newStatusCode,
                                
                                bool codeReasonSpecified,
                                InvoiceStatusTypeCodeReason codeReason,

                                string note,

                                string additionalBuyerId = null,
                                string messageAttributes = null,

                                bool documentCurrencyCode = false)
        {
            ChangeB2BIncomingInvoiceStatusMsg message = new ChangeB2BIncomingInvoiceStatusMsg();

            //Header
            HeaderBuyerType header = new HeaderBuyerType();
            header.MessageID = messageId;
            header.BuyerID = buyerId;
            header.AdditionalBuyerID = additionalBuyerId;
            header.MessageType = CHANGE_B2B_INCOMING_INVOICE_STATUS_MESSAGE_TYPE;
            header.MessageAttributes = messageAttributes;

            message.HeaderBuyer = header;

            //Data
            ChangeB2BIncomingInvoiceStatusMsgData data = new ChangeB2BIncomingInvoiceStatusMsgData();

            ChangeB2BIncomingInvoiceStatusMsgDataB2BIncomingInvoiceStatus B2BIncomingInvoiceStatus = new ChangeB2BIncomingInvoiceStatusMsgDataB2BIncomingInvoiceStatus();

            B2BIncomingInvoiceStatus.InvoiceID = invoiceId;
            InvoiceStatusType newInvoiceStatus = new InvoiceStatusType(){
                StatusCode = newStatusCode,
                CodeReasonSpecified = codeReasonSpecified,
                CodeReason = codeReason,
                Note = note
            };
            B2BIncomingInvoiceStatus.InvoiceStatus = newInvoiceStatus;

            B2BIncomingInvoiceStatus.InvoiceID = invoiceId;

            data.B2BIncomingInvoiceStatus = B2BIncomingInvoiceStatus;

            message.Data = data;


            //SEND
            FinaInvoiceB2BPortTypeClient client = _client.GetFinaInvoiceB2BPortTypeClient(endpointAddress,
                                                                                         dnsIdentity,
                                                                                         serviceCertificatePath,
                                                                                         clientCertificatePath,
                                                                                         clientCertificatePassword);
            ChangeB2BIncomingInvoiceStatusAckMsg res = client.changeB2BIncomingInvoiceStatus(message);

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
            if(res.Error != null)
            {
                retVar.SetHashVariable("ErrorCode", new Variable(res.Error.ErrorCode));
                retVar.SetHashVariable("ErrorText", new Variable(res.Error.ErrorText));
                retVar.SetHashVariable("ErrorMessage", new Variable(res.Error.ErrorMessage));
            }

            return retVar;
        }
    }
}
