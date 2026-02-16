using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCSCS.ServiceReference2_B2BFinaInvoiceWebService;

namespace WpfCSCS.Fiskalizacija2.FINA.B2BIncoming
{
    public class GetReceiverList
    {
        private const string GET_B2B_RECEIVER_LIST_MESSAGE_TYPE = "50041";

        public Variable Send(string endpointAddress,
                                string dnsIdentity,
                                string serviceCertificatePath,
                                string clientCertificatePath,
                                string clientCertificatePassword,

                                string messageId,
                                string supplierId,

                                string buyerOib,

                                string additionalSupplierId = null,
                                string messageAttributes = null)
        {
            GetReceiverListMsg message = new GetReceiverListMsg();

            //Header
            HeaderSupplierType header = new HeaderSupplierType();
            header.MessageID = messageId;
            header.SupplierID = supplierId;
            header.AdditionalSupplierID = additionalSupplierId;
            header.MessageType = GET_B2B_RECEIVER_LIST_MESSAGE_TYPE;
            header.MessageAttributes = messageAttributes;

            message.HeaderSupplier = header;

            //Data
            GetReceiverListMsgData data = new GetReceiverListMsgData();

            GetReceiverListMsgDataReceiverList receiverList = new GetReceiverListMsgDataReceiverList();

            // !!! FILTER 
            var filter = new object[] { new TextSearchType() { SearchField = "OIB", SearchValue = buyerOib } };
            //var filter = new object[] { new TextSearchType() { SearchField = "OIB", SearchValue = "26389058739" } }; // Aura soft OIB
            //var filter = new object[] { new TextSearchType() { SearchField = "NAZIV", SearchValue = "pris" } };
            //var filter = new object[] { new TextSearchType() { SearchField = "ALL", SearchValue = "ALL" } };
            receiverList.Filter = new FilterType() { Items = filter };

            data.ReceiverList = receiverList;

            message.Data = data;


            //SEND
            FinaInvoiceB2BPortTypeClient client = _client.GetFinaInvoiceB2BPortTypeClient(endpointAddress,
                                                                                         dnsIdentity,
                                                                                         serviceCertificatePath,
                                                                                         clientCertificatePath,
                                                                                         clientCertificatePassword);
            GetReceiverListAckMsg res = client.getReceiverList(message);

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
            if (res.BusinessBranchList != null)
            {
                retVar.SetHashVariable("ReceiverListCount", new Variable(res.BusinessBranchList.Length));
            }
            //else {
            //    retVar.SetHashVariable("ErrorCode", new Variable(error.ErrorCode));
            //    retVar.SetHashVariable("ErrorText", new Variable(error.ErrorText));
            //    retVar.SetHashVariable("ErrorMessage", new Variable(error.ErrorMessage));
            //}

            return retVar;
        }
    }
}
