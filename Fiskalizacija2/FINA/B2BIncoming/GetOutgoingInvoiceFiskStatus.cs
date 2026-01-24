using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCSCS.ServiceReference2_B2BFinaInvoiceWebService;

namespace WpfCSCS.Fiskalizacija2.FINA.B2BIncoming
{
    public class GetOutgoingInvoiceFiskStatus
    {
        private const string GET_B2B_OUTGOING_INVOICE_FISK_STATUS_MESSAGE_TYPE = "9111";

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
            GetB2BOutgoingInvoiceFiskStatusMsg message = new GetB2BOutgoingInvoiceFiskStatusMsg();

            //Header
            HeaderSupplierType header = new HeaderSupplierType();
            header.MessageID = messageId;
            header.SupplierID = supplierId;
            header.AdditionalSupplierID = additionalSupplierId;
            header.MessageType = GET_B2B_OUTGOING_INVOICE_FISK_STATUS_MESSAGE_TYPE;
            header.MessageAttributes = messageAttributes;

            message.HeaderSupplier = header;

            //Data
            GetB2BOutgoingInvoiceFiskStatusMsgData data = new GetB2BOutgoingInvoiceFiskStatusMsgData();

            GetB2BOutgoingInvoiceFiskStatusMsgDataB2BOutgoingInvoiceFiskStatus B2BOutgoingInvoiceFiskStatus = new GetB2BOutgoingInvoiceFiskStatusMsgDataB2BOutgoingInvoiceFiskStatus();

            B2BOutgoingInvoiceFiskStatus.InvoiceID = invoiceId;

            data.B2BOutgoingInvoiceFiskStatus = B2BOutgoingInvoiceFiskStatus;

            message.Data = data;


            //SEND
            FinaInvoiceB2BPortTypeClient client = _client.GetFinaInvoiceB2BPortTypeClient(endpointAddress,
                                                                                         dnsIdentity,
                                                                                         serviceCertificatePath,
                                                                                         clientCertificatePath,
                                                                                         clientCertificatePassword);
            GetB2BOutgoingInvoiceFiskStatusAckMsg res = client.getB2BOutgoingInvoiceFiskStatus(message);

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
                if(res.Item is ErrorType)
                {
                    var error = (ErrorType)res.Item;
                    retVar.SetHashVariable("ErrorCode", new Variable(error.ErrorCode));
                    retVar.SetHashVariable("ErrorText", new Variable(error.ErrorText));
                    retVar.SetHashVariable("ErrorMessage", new Variable(error.ErrorMessage));
                }
                else if(res.Item is GetB2BOutgoingInvoiceFiskStatusAckMsgB2BOutgoingInvoiceFiskStatus)
                {
                    var b2bStatus = (GetB2BOutgoingInvoiceFiskStatusAckMsgB2BOutgoingInvoiceFiskStatus)res.Item;
                    retVar.SetHashVariable("InvoiceID", new Variable(b2bStatus.InvoiceID));
                    if(b2bStatus.FiskStatus != null)
                    {
                        retVar.SetHashVariable("FiskStatus", new Variable(b2bStatus.FiskStatus.Status.ToString()));
                        retVar.SetHashVariable("FiskStatusTimestamp", new Variable(b2bStatus.FiskStatus.StatusTimestamp.ToString()));
                    }
                    if(b2bStatus.ReportPaymentStatus != null)
                    {
                        retVar.SetHashVariable("ReportPaymentStatus", new Variable(b2bStatus.ReportPaymentStatus.Status.ToString()));
                        retVar.SetHashVariable("ReportPaymentStatusTimestamp", new Variable(b2bStatus.ReportPaymentStatus.StatusTimestamp.ToString()));
                    }
                }
            }

            return retVar;
        }
    }
}
