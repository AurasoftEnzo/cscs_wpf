using DevExpress.XtraCharts.Native;
using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCSCS.ServiceReference2_B2BFinaInvoiceWebService;

namespace WpfCSCS.Fiskalizacija2.FINA.B2BIncoming
{
    public class ChangeOutgoingInvoiceStatus
    {
        private const string CHANGE_B2B_OUTGOING_INVOICE_STATUS_MESSAGE_TYPE = "109";

        public Variable Send(string endpointAddress,
                                string dnsIdentity,
                                string serviceCertificatePath,
                                string clientCertificatePath,
                                string clientCertificatePassword,

                                string messageId,
                                string supplierId,

                                //string invoiceId,
                                string supplierInvoiceId,
                                string invoiceYear,
                                
                                DateTime payedDate,
                                decimal payedAmount,
                                string paymentMeansCode,

                                string additionalSupplierId = null,
                                string messageAttributes = null)
        {
            ChangeB2BOutgoingInvoiceStatusMsg message = new ChangeB2BOutgoingInvoiceStatusMsg();

            //Header
            HeaderSupplierType header = new HeaderSupplierType();
            header.MessageID = messageId;
            header.SupplierID = supplierId;
            header.AdditionalSupplierID = additionalSupplierId;
            header.MessageType = CHANGE_B2B_OUTGOING_INVOICE_STATUS_MESSAGE_TYPE;
            header.MessageAttributes = messageAttributes;

            message.HeaderSupplier = header;

            //Data
            ChangeB2BOutgoingInvoiceStatusMsgData data = new ChangeB2BOutgoingInvoiceStatusMsgData();

            ChangeB2BOutgoingInvoiceStatusMsgDataB2BOutgoingInvoiceStatus B2BOutgoingInvoiceStatus = new ChangeB2BOutgoingInvoiceStatusMsgDataB2BOutgoingInvoiceStatus();

            B2BOutgoingInvoiceStatus.Item = new ChangeB2BOutgoingInvoiceStatusMsgDataB2BOutgoingInvoiceStatusSupplierInvoice() { SupplierInvoiceID = supplierInvoiceId, InvoiceYear = invoiceYear };
            
            B2BOutgoingInvoiceStatus.OutgoingInvoiceStatus = new OutgoingInvoiceStatusType() {
                StatusCode = OutgoingInvoiceStatusTypeStatusCode.PAYMENT_RECEIVED,
                PayedDate = payedDate,
                PayedAmount = payedAmount,
                PaymentMeansCode = paymentMeansCode
            };

            data.B2BOutgoingInvoiceStatus = B2BOutgoingInvoiceStatus;

            message.Data = data;


            //SEND
            FinaInvoiceB2BPortTypeClient client = _client.GetFinaInvoiceB2BPortTypeClient(endpointAddress,
                                                                                         dnsIdentity,
                                                                                         serviceCertificatePath,
                                                                                         clientCertificatePath,
                                                                                         clientCertificatePassword);
            ChangeB2BOutgoingInvoiceStatusAckMsg res = client.changeB2BOutgoingInvoiceStatus(message);

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
