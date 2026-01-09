using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCSCS.ServiceReference2_B2BFinaInvoiceWebService;


namespace WpfCSCS.Fiskalizacija2.FINA.B2BIncoming
{
    public class EchoBuyer
    {
        private const string B2B_ECHO_MESSAGE_TYPE = "9999";

        public Variable Send(string endpointAddress,
                                string dnsIdentity,
                                string serviceCertificatePath,
                                string clientCertificatePath,
                                string clientCertificatePassword,

                                string messageId,
                                string echoText,
                                string buyerId,

                                string additionalBuyerId = null,
                                string messageAttributes = null)
        {
            EchoBuyerMsg message = new EchoBuyerMsg();

            //Header
            HeaderBuyerType header = new HeaderBuyerType();
            header.MessageID = messageId;
            header.BuyerID = buyerId;
            header.AdditionalBuyerID = additionalBuyerId;
            header.MessageType = B2B_ECHO_MESSAGE_TYPE;
            header.MessageAttributes = messageAttributes;

            message.HeaderBuyer = header;

            //Data
            EchoBuyerMsgData data = new EchoBuyerMsgData();

            EchoBuyerMsgDataEchoData reqEchoData = new EchoBuyerMsgDataEchoData();

            reqEchoData.Echo = echoText;

            data.EchoData = reqEchoData;

            message.Data = data;


            //SEND
            FinaInvoiceB2BPortTypeClient client = _client.GetFinaInvoiceB2BPortTypeClient(endpointAddress,
                                                                                         dnsIdentity,
                                                                                         serviceCertificatePath,
                                                                                         clientCertificatePath,
                                                                                         clientCertificatePassword);
            EchoBuyerAckMsg rez = client.echo(message);

            MessageAckType messageAck = rez.MessageAck;
            EchoBuyerAckMsgEchoData resEchoData = rez.EchoData;


            //RETURN
            Variable retVar = new Variable(Variable.VarType.ARRAY);

            retVar.SetHashVariable("AckStatus", new Variable(messageAck.AckStatus.ToString()));
            retVar.SetHashVariable("AckStatusCode", new Variable(messageAck.AckStatusCode));
            retVar.SetHashVariable("AckStatusText", new Variable(messageAck.AckStatusText));
            retVar.SetHashVariable("MessageID", new Variable(messageAck.MessageID));
            retVar.SetHashVariable("MessageAckID", new Variable(messageAck.MessageAckID));
            retVar.SetHashVariable("MessageType", new Variable(messageAck.MessageType));
            retVar.SetHashVariable("Echo", new Variable(resEchoData.Echo));

            return retVar;
        }
    }
}
