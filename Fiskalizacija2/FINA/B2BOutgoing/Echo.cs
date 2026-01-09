using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCSCS.ServiceReference1_SendB2BOutgoingInvoicePKIWebService;

namespace WpfCSCS.Fiskalizacija2.FINA.B2BOutgoing
{
    public class Echo
    {
        private const string B2B_ECHO_MESSAGE_TYPE = "9999";

        public Variable Send(string endpointAddress,
                                 string dnsIdentity,
                                 string serviceCertificatePath,
                                 string clientCertificatePath,
                                 string clientCertificatePassword,
                                 string erpid,

                                 string messageId,
                                 string echoText,
                                 string supplierId,

                                 string additionalSupplierId = null,
                                 string messageAttributes = null)
        {
            EchoMsg message = new EchoMsg();

            //Header
            HeaderSupplierType header = new HeaderSupplierType();
            header.MessageID = messageId;
            header.SupplierID = supplierId;
            header.MessageType = B2B_ECHO_MESSAGE_TYPE;
            header.AdditionalSupplierID = additionalSupplierId;
            header.ERPID = erpid;
            header.MessageAttributes = messageAttributes;

            message.HeaderSupplier = header;

            //Data
            EchoMsgData data = new EchoMsgData();

            EchoMsgDataEchoData reqEchoData = new EchoMsgDataEchoData();

            reqEchoData.Echo = echoText;

            data.EchoData = reqEchoData;

            message.Data = data;


            //SEND
            eRacunB2BPortTypeClient client = _client.GeteRacunB2BPortTypeClient(endpointAddress,
                                                                                         dnsIdentity,
                                                                                         serviceCertificatePath,
                                                                                         clientCertificatePath,
                                                                                         clientCertificatePassword);
            EchoAckMsg rez = client.echo(message);

            MessageAckType messageAck = rez.MessageAck;
            EchoAckMsgEchoData resEchoData = rez.EchoData;


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
