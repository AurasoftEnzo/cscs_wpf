using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCSCS.ServiceReference2_B2BFinaInvoiceWebService;

namespace WpfCSCS.Fiskalizacija2.FINA.B2BIncoming
{
    public class GetIncomingInvoiceFiskStatus
    {
        private const string GET_B2B_INCOMING_INVOICE_FISK_STATUS_MESSAGE_TYPE = "9109";

        public Variable Send(string endpointAddress,
                                string dnsIdentity,
                                string serviceCertificatePath,
                                string clientCertificatePath,
                                string clientCertificatePassword,

                                string messageId,
                                string buyerId,
                                string invoiceId,

                                string additionalBuyerId = null,
                                string messageAttributes = null)
        {
            GetB2BIncomingInvoiceFiskStatusMsg message = new GetB2BIncomingInvoiceFiskStatusMsg();

            //Header
            HeaderBuyerType header = new HeaderBuyerType();
            header.MessageID = messageId;
            header.BuyerID = buyerId;
            header.AdditionalBuyerID = additionalBuyerId;
            header.MessageType = GET_B2B_INCOMING_INVOICE_FISK_STATUS_MESSAGE_TYPE;
            header.MessageAttributes = messageAttributes;

            message.HeaderBuyer = header;
            //Data
            GetB2BIncomingInvoiceFiskStatusMsgData data = new GetB2BIncomingInvoiceFiskStatusMsgData();

            GetB2BIncomingInvoiceFiskStatusMsgDataB2BIncomingInvoiceFiskStatus B2BIncomingInvoiceFiskStatus = new GetB2BIncomingInvoiceFiskStatusMsgDataB2BIncomingInvoiceFiskStatus();
            B2BIncomingInvoiceFiskStatus.InvoiceID = invoiceId;

            data.B2BIncomingInvoiceFiskStatus = B2BIncomingInvoiceFiskStatus;

            message.Data = data;


            //SEND
            FinaInvoiceB2BPortTypeClient client = _client.GetFinaInvoiceB2BPortTypeClient(endpointAddress,
                                                                                         dnsIdentity,
                                                                                         serviceCertificatePath,
                                                                                         clientCertificatePath,
                                                                                         clientCertificatePassword);
            GetB2BIncomingInvoiceFiskStatusAckMsg res = client.getB2BIncomingInvoiceFiskStatus(message);

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
            if (res.Item != null)
            {
                if (res.Item is ErrorType)
                {
                    var error = (ErrorType)res.Item;
                    retVar.SetHashVariable("ErrorCode", new Variable(error.ErrorCode));
                    retVar.SetHashVariable("ErrorText", new Variable(error.ErrorText));
                    retVar.SetHashVariable("ErrorMessage", new Variable(error.ErrorMessage));
                }
                else if (res.Item is GetB2BIncomingInvoiceFiskStatusAckMsgB2BIncomingInvoiceFiskStatus)
                {
                    var b2bStatus = (GetB2BIncomingInvoiceFiskStatusAckMsgB2BIncomingInvoiceFiskStatus)res.Item;
                    retVar.SetHashVariable("InvoiceID", new Variable(b2bStatus.InvoiceID));
                    if (b2bStatus.FiskStatus != null)
                    {
                        retVar.SetHashVariable("FiskStatus", new Variable(b2bStatus.FiskStatus.Status.ToString()));
                        retVar.SetHashVariable("FiskStatusTimestamp", new Variable(b2bStatus.FiskStatus.StatusTimestamp.ToString()));
                    }
                    if (b2bStatus.ReportRejectionStatus != null)
                    {
                        retVar.SetHashVariable("ReportRejectionStatus", new Variable(b2bStatus.ReportRejectionStatus.Status.ToString()));
                        retVar.SetHashVariable("ReportRejectionStatusTimestamp", new Variable(b2bStatus.ReportRejectionStatus.StatusTimestamp.ToString()));
                    }
                }
            }

            return retVar;
        }
    }
}
