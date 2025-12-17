using hr.fina.eracun.signature;
using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCSCS.ServiceReference1_fina_wsdl;

namespace WpfCSCS.Fiskalizacija2.FINA
{
    public class GetOutgoingInvoiceStatus
    {

        private const string B2B_GET_INVOICE_STATUS_MESSAGE_TYPE = "9011";

        public Variable Send(string endpointAddress,
                                    string dnsIdentity,
                                    string serviceCertificatePath,
                                    string clientCertificatePath,
                                    string clientCertificatePassword,

                                    string messageId,
                                    string supplierID,
                                    string supplierInvoiceID,
                                    string invoiceYear,

                                    string additionalSupplierId = null,
                                    string erpid = null,
                                    string messageAttributes = null,
                                    bool documentCurrencyCode = false
                                    )
        {
            GetB2BOutgoingInvoiceStatusMsg message = new GetB2BOutgoingInvoiceStatusMsg();

            //Header
            HeaderSupplierType header = new HeaderSupplierType();
            header.MessageID = messageId;
            header.SupplierID = supplierID;
            header.MessageType = B2B_GET_INVOICE_STATUS_MESSAGE_TYPE;
            header.AdditionalSupplierID = additionalSupplierId;
            header.ERPID = erpid;
            header.MessageAttributes = messageAttributes;

            message.HeaderSupplier = header;

            //Data
            GetB2BOutgoingInvoiceStatusMsgData data = new GetB2BOutgoingInvoiceStatusMsgData();

            GetB2BOutgoingInvoiceStatusMsgDataB2BOutgoingInvoiceStatus B2BOutgoingInvoiceStatusEnvelope = new GetB2BOutgoingInvoiceStatusMsgDataB2BOutgoingInvoiceStatus();

            B2BOutgoingInvoiceStatusEnvelope.SupplierInvoiceID = supplierInvoiceID;
            B2BOutgoingInvoiceStatusEnvelope.InvoiceYear = invoiceYear;
            B2BOutgoingInvoiceStatusEnvelope.DocumentCurrencyCode = documentCurrencyCode;

            data.B2BOutgoingInvoiceStatus = B2BOutgoingInvoiceStatusEnvelope;

            message.Data = data;


            //SEND
            eRacunB2BPortTypeClient client = FINACommon.GeteRacunB2BPortTypeClientClient(endpointAddress,
                                                                              dnsIdentity,
                                                                              serviceCertificatePath,
                                                                              clientCertificatePath,
                                                                              clientCertificatePassword);
            GetB2BOutgoingInvoiceStatusAckMsg rez = client.getB2BOutgoingInvoiceStatus(message);


            MessageAckType messageAck = rez.MessageAck;
            GetB2BOutgoingInvoiceStatusAckMsgB2BOutgoingInvoiceStatus B2BOutgoingInvoiceStatus = rez.B2BOutgoingInvoiceStatus;


            //RETURN
            Variable retVar = new Variable(Variable.VarType.ARRAY);

            retVar.SetHashVariable("AckStatus", new Variable(messageAck.AckStatus.ToString()));
            retVar.SetHashVariable("AckStatusCode", new Variable(messageAck.AckStatusCode));
            retVar.SetHashVariable("AckStatusText", new Variable(messageAck.AckStatusText));
            retVar.SetHashVariable("MessageID", new Variable(messageAck.MessageID));
            retVar.SetHashVariable("MessageAckID", new Variable(messageAck.MessageAckID));
            retVar.SetHashVariable("MessageType", new Variable(messageAck.MessageType));

            Variable documentStatusesArray = new Variable(Variable.VarType.ARRAY);
            if (B2BOutgoingInvoiceStatus != null)
            {
                retVar.SetHashVariable("SupplierID", new Variable(B2BOutgoingInvoiceStatus.SupplierID));
                retVar.SetHashVariable("AdditionalSupplierID", new Variable(B2BOutgoingInvoiceStatus.AdditionalSupplierID));
                retVar.SetHashVariable("InvoiceID", new Variable(B2BOutgoingInvoiceStatus.InvoiceID));
                retVar.SetHashVariable("SupplierInvoiceID", new Variable(B2BOutgoingInvoiceStatus.SupplierInvoiceID));
                retVar.SetHashVariable("InvoiceTimestamp", new Variable(B2BOutgoingInvoiceStatus.InvoiceTimestamp));

                
                if (B2BOutgoingInvoiceStatus.DocumentStatus != null && B2BOutgoingInvoiceStatus.DocumentStatus.Length > 0)
                {

                    foreach (var documentStatus in B2BOutgoingInvoiceStatus.DocumentStatus)
                    {
                        Variable documentStatusDict = new Variable(Variable.VarType.ARRAY);

                        documentStatusDict.SetHashVariable("StatusCode", new Variable(documentStatus.StatusCode.ToString()));
                        documentStatusDict.SetHashVariable("StatusText", new Variable(documentStatus.StatusText));

                        documentStatusDict.SetHashVariable("StatusTimestampSpecified", new Variable(documentStatus.StatusTimestampSpecified));
                        if (documentStatus.StatusTimestampSpecified)
                        {
                            documentStatusDict.SetHashVariable("StatusTimestamp", new Variable(documentStatus.StatusTimestamp));
                        }

                        documentStatusDict.SetHashVariable("Note", new Variable(documentStatus.Note));

                        documentStatusDict.SetHashVariable("PartialAmountSpecified", new Variable(documentStatus.PartialAmountSpecified));
                        if (documentStatus.PartialAmountSpecified)
                        {
                            documentStatusDict.SetHashVariable("PartialAmount", new Variable(documentStatus.PartialAmount));
                        }

                        documentStatusDict.SetHashVariable("DocumentCurrencyCode", new Variable(documentStatus.DocumentCurrencyCode));
                        documentStatusesArray.AddVariable(documentStatusDict);
                    }
                }
                
            }
            retVar.SetHashVariable("DocumentStatuses", documentStatusesArray);

            return retVar;
        }
    }
}
