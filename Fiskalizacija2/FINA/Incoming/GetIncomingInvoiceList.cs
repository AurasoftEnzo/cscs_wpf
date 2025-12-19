using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCSCS.ServiceReference2_B2BFinaInvoiceWebService;

namespace WpfCSCS.Fiskalizacija2.FINA.Incoming
{
    public class GetIncomingInvoiceList
    {
        private const string GET_B2B_INCOMING_INVOICE_LIST_MESSAGE_TYPE = "9101";

        public Variable Send(string endpointAddress,
                                string dnsIdentity,
                                string serviceCertificatePath,
                                string clientCertificatePath,
                                string clientCertificatePassword,

                                string messageId,
                                string buyerId,

                                string additionalBuyerId = null,
                                string messageAttributes = null,

                                bool documentCurrencyCode = false)
        {
            GetB2BIncomingInvoiceListMsg message = new GetB2BIncomingInvoiceListMsg();

            //Header
            HeaderBuyerType header = new HeaderBuyerType();
            header.MessageID = messageId;
            header.BuyerID = buyerId;
            header.AdditionalBuyerID = additionalBuyerId;
            header.MessageType = GET_B2B_INCOMING_INVOICE_LIST_MESSAGE_TYPE;
            header.MessageAttributes = messageAttributes;

            message.HeaderBuyer = header;

            //Data
            GetB2BIncomingInvoiceListMsgData data = new GetB2BIncomingInvoiceListMsgData();

            GetB2BIncomingInvoiceListMsgDataB2BIncomingInvoiceList B2BIncomingInvoiceList = new GetB2BIncomingInvoiceListMsgDataB2BIncomingInvoiceList();

            B2BIncomingInvoiceList.DocumentCurrencyCode = documentCurrencyCode;
            B2BIncomingInvoiceList.DocumentCurrencyCodeSpecified = documentCurrencyCode;


            
            // !!! FILTER 
            B2BIncomingInvoiceList.Item = new FilterType()
            {
                //Items = new object[] { new status }
                Items = new object[] { new InvoiceStatusType() { StatusCode = InvoiceStatusTypeStatusCode.RECEIVED } }
            };



            data.B2BIncomingInvoiceList = B2BIncomingInvoiceList;

            message.Data = data;


            //SEND
            FinaInvoiceB2BPortTypeClient client = _client.GetFinaInvoiceB2BPortTypeClient(endpointAddress,
                                                                                         dnsIdentity,
                                                                                         serviceCertificatePath,
                                                                                         clientCertificatePath,
                                                                                         clientCertificatePassword);
            GetB2BIncomingInvoiceListAckMsg res = client.getB2BIncomingInvoiceList(message);

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
            if (res.Item is GetB2BIncomingInvoiceListAckMsgB2BIncomingInvoiceList)
            {
                GetB2BIncomingInvoiceListAckMsgB2BIncomingInvoiceList incomingInvoiceList = (GetB2BIncomingInvoiceListAckMsgB2BIncomingInvoiceList)res.Item;
                Variable invoicesArrayVar = new Variable(Variable.VarType.ARRAY);

                if (incomingInvoiceList.B2BIncomingInvoice != null)
                {
                    incomingInvoiceList.B2BIncomingInvoice.ToList().ForEach(B2BIncomingInvoice =>
                    {
                        Variable invoiceVar = new Variable(Variable.VarType.ARRAY);


                        invoiceVar.SetHashVariable("XMLStandard", new Variable(B2BIncomingInvoice.DocumentType.XMLStandard.ToString()));
                        invoiceVar.SetHashVariable("SpecificationIdentifier", new Variable(B2BIncomingInvoice.DocumentType.SpecificationIdentifier.ToString()));
                        invoiceVar.SetHashVariable("DocumentTypeCode", new Variable(B2BIncomingInvoice.DocumentType.DocumentTypeCode));
                        invoiceVar.SetHashVariable("DocumentTypeText", new Variable(B2BIncomingInvoice.DocumentType.DocumentTypeText));

                        invoiceVar.SetHashVariable("StatusCode", new Variable(B2BIncomingInvoice.InvoiceStatus.StatusCode.ToString()));
                        invoiceVar.SetHashVariable("Note", new Variable(B2BIncomingInvoice.InvoiceStatus.Note));
                        // PartialAmount ??
                        // DocumentCurrencyCode ??

                        invoiceVar.SetHashVariable("CodeReasonSpecified", new Variable(B2BIncomingInvoice.InvoiceStatus.CodeReasonSpecified));
                        if (B2BIncomingInvoice.InvoiceStatus.CodeReasonSpecified)
                        {
                            invoiceVar.SetHashVariable("CodeReason", new Variable(B2BIncomingInvoice.InvoiceStatus.CodeReason.ToString()));
                        }

                        invoiceVar.SetHashVariable("SupplierID", new Variable(B2BIncomingInvoice.SupplierID));
                        invoiceVar.SetHashVariable("AdditionalSupplierID", new Variable(B2BIncomingInvoice.AdditionalSupplierID));
                        invoiceVar.SetHashVariable("SupplierRegistrationName", new Variable(B2BIncomingInvoice.SupplierRegistrationName));
                        invoiceVar.SetHashVariable("SupplierCompanyID", new Variable(B2BIncomingInvoice.SupplierCompanyID));
                        invoiceVar.SetHashVariable("InvoiceID", new Variable(B2BIncomingInvoice.InvoiceID));
                        invoiceVar.SetHashVariable("SupplierInvoiceID", new Variable(B2BIncomingInvoice.SupplierInvoiceID));
                        invoiceVar.SetHashVariable("DataInterchangeMethod", new Variable(B2BIncomingInvoice.DataInterchangeMethod.ToString()));
                        invoiceVar.SetHashVariable("InvoiceIssueDate", new Variable(B2BIncomingInvoice.InvoiceIssueDate));
                        invoiceVar.SetHashVariable("InvoiceDate", new Variable(B2BIncomingInvoice.InvoiceDate));
                        invoiceVar.SetHashVariable("InvoiceTimestamp", new Variable(B2BIncomingInvoice.InvoiceTimestamp));
                        invoiceVar.SetHashVariable("InvoicePayableAmount", new Variable(B2BIncomingInvoice.InvoicePayableAmount));
                        invoiceVar.SetHashVariable("DocumentCurrencyCode", new Variable(B2BIncomingInvoice.DocumentCurrencyCode));

                        invoicesArrayVar.AddVariable(invoiceVar);
                    });
                }

                retVar.SetHashVariable("InvoiceList", invoicesArrayVar);
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
