using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCSCS.ServiceReference4_B2GFinaInvoiceWebService;

namespace WpfCSCS.Fiskalizacija2.FINA.B2GIncoming
{
    public class GetOutgoingInvoiceFiskStatus
    {
        private const string GET_B2G_OUTGOING_INVOICE_FISK_STATUS_MESSAGE_TYPE = "9111";

        public Variable Send(string endpointAddress,
                                string dnsIdentity,
                                string serviceCertificatePath,
                                string clientCertificatePath,
                                string clientCertificatePassword,

                                string messageId,
                                string supplierId,
                                string invoiceId,

                                string additionalSupplierId = null,
                                string messageAttributes = null)
        {
            GetB2GOutgoingInvoiceFiskStatusMsg message = new GetB2GOutgoingInvoiceFiskStatusMsg();

            //Header
            HeaderSupplierType header = new HeaderSupplierType();
            header.MessageID = messageId;
            header.SupplierID = supplierId;
            header.AdditionalSupplierID = additionalSupplierId;
            header.MessageType = GET_B2G_OUTGOING_INVOICE_FISK_STATUS_MESSAGE_TYPE;
            header.MessageAttributes = messageAttributes;

            message.HeaderSupplier = header;

            //Data
            GetB2GOutgoingInvoiceFiskStatusMsgData data = new GetB2GOutgoingInvoiceFiskStatusMsgData();

            GetB2GOutgoingInvoiceFiskStatusMsgDataB2GOutgoingInvoiceFiskStatus B2GOutgoingInvoiceFiskStatus = new GetB2GOutgoingInvoiceFiskStatusMsgDataB2GOutgoingInvoiceFiskStatus();

            B2GOutgoingInvoiceFiskStatus.InvoiceID = invoiceId;

            data.B2GOutgoingInvoiceFiskStatus = B2GOutgoingInvoiceFiskStatus;

            message.Data = data;


            //SEND
            FinaInvoiceB2GPortTypeClient client = _client.GetFinaInvoiceB2GPortTypeClient(endpointAddress,
                                                                                         dnsIdentity,
                                                                                         serviceCertificatePath,
                                                                                         clientCertificatePath,
                                                                                         clientCertificatePassword);
            GetB2GOutgoingInvoiceFiskStatusAckMsg res = client.getB2GOutgoingInvoiceFiskStatus(message);

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
                else if (res.Item is GetB2GOutgoingInvoiceFiskStatusAckMsgB2GOutgoingInvoiceFiskStatus)
                {
                    var b2gStatus = (GetB2GOutgoingInvoiceFiskStatusAckMsgB2GOutgoingInvoiceFiskStatus)res.Item;
                    retVar.SetHashVariable("InvoiceID", new Variable(b2gStatus.InvoiceID));
                    if(b2gStatus.FiskStatus != null)
                    {
                        retVar.SetHashVariable("FiskStatus", new Variable(b2gStatus.FiskStatus.Status.ToString()));
                        retVar.SetHashVariable("FiskStatusTimestamp", new Variable(b2gStatus.FiskStatus.StatusTimestamp.ToString()));
                    }
                    if(b2gStatus.ReportPaymentStatus != null)
                    {
                        retVar.SetHashVariable("ReportPaymentStatus", new Variable(b2gStatus.ReportPaymentStatus.Status.ToString()));
                        retVar.SetHashVariable("ReportPaymentStatusTimestamp", new Variable(b2gStatus.ReportPaymentStatus.StatusTimestamp.ToString()));
                    }
                }
            }

            return retVar;
        }
    }
}
