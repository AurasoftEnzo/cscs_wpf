using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCSCS.ServiceReference4_B2GFinaInvoiceWebService;

namespace WpfCSCS.Fiskalizacija2.FINA.B2GIncoming
{
    public class GetOutgoingInvoiceList
    {
        private const string GET_B2G_OUTGOING_INVOICE_LIST_MESSAGE_TYPE = "9105";

        public Variable Send(string endpointAddress,
                                string dnsIdentity,
                                string serviceCertificatePath,
                                string clientCertificatePath,
                                string clientCertificatePassword,

                                string messageId,
                                string supplierId,

                                DateTime dateFrom,
                                DateTime dateTo,

                                string additionalSupplierId = null,
                                string messageAttributes = null,

                                bool documentCurrencyCode = false)
        {
            GetB2GOutgoingInvoiceListMsg message = new GetB2GOutgoingInvoiceListMsg();
            
            //Header
            HeaderSupplierType header = new HeaderSupplierType();
            header.MessageID = messageId;
            header.SupplierID = supplierId;
            header.AdditionalSupplierID = additionalSupplierId;
            header.MessageType = GET_B2G_OUTGOING_INVOICE_LIST_MESSAGE_TYPE;
            header.MessageAttributes = messageAttributes;

            message.HeaderSupplier = header;

            //Data
            GetB2GOutgoingInvoiceListMsgData data = new GetB2GOutgoingInvoiceListMsgData();

            GetB2GOutgoingInvoiceListMsgDataB2GOutgoingInvoiceList B2GOutgoingInvoiceList = new GetB2GOutgoingInvoiceListMsgDataB2GOutgoingInvoiceList();

            B2GOutgoingInvoiceList.DocumentCurrencyCode = documentCurrencyCode;
            B2GOutgoingInvoiceList.DocumentCurrencyCodeSpecified = documentCurrencyCode;


            //......
            var fromDateTime = dateFrom.Date;
            var toDateTime = dateTo.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            // !!! FILTER 
            B2GOutgoingInvoiceList.Item = new FilterType()
            {
                //Items = new object[] { new status }
                Items = new object[] { 
                    //new InvoiceStatusType() { StatusCode = InvoiceStatusTypeStatusCode.RECEIVED },
                    new DateRangeType() { From = fromDateTime, To = toDateTime}
                }
            };


            data.B2GOutgoingInvoiceList = B2GOutgoingInvoiceList;

            message.Data = data;


            //SEND
            FinaInvoiceB2GPortTypeClient client = _client.GetFinaInvoiceB2GPortTypeClient(endpointAddress,
                                                                                         dnsIdentity,
                                                                                         serviceCertificatePath,
                                                                                         clientCertificatePath,
                                                                                         clientCertificatePassword);
            GetB2GOutgoingInvoiceListAckMsg res = client.getB2GOutgoingInvoiceList(message);

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
            if (res.Item is GetB2GOutgoingInvoiceListAckMsgB2GOutgoingInvoiceList)
            {
                GetB2GOutgoingInvoiceListAckMsgB2GOutgoingInvoiceList outgoingInvoiceList = (GetB2GOutgoingInvoiceListAckMsgB2GOutgoingInvoiceList)res.Item;
                Variable invoicesArrayVar = new Variable(Variable.VarType.ARRAY);

                if (outgoingInvoiceList.B2GOutgoingInvoice != null)
                {
                    outgoingInvoiceList.B2GOutgoingInvoice.ToList().ForEach(B2GOutgoingInvoice =>
                    {
                        Variable invoiceVar = new Variable(Variable.VarType.ARRAY);


                        invoiceVar.SetHashVariable("XMLStandard", new Variable(B2GOutgoingInvoice.DocumentType.XMLStandard.ToString()));
                        invoiceVar.SetHashVariable("SpecificationIdentifier", new Variable(B2GOutgoingInvoice.DocumentType.SpecificationIdentifier.ToString()));
                        invoiceVar.SetHashVariable("DocumentTypeCode", new Variable(B2GOutgoingInvoice.DocumentType.DocumentTypeCode));
                        invoiceVar.SetHashVariable("DocumentTypeText", new Variable(B2GOutgoingInvoice.DocumentType.DocumentTypeText));

                        invoiceVar.SetHashVariable("StatusCode", new Variable(B2GOutgoingInvoice.InvoiceStatus.StatusCode.ToString()));
                        invoiceVar.SetHashVariable("Note", new Variable(B2GOutgoingInvoice.InvoiceStatus.Note));
                        invoiceVar.SetHashVariable("DocumentCurrencyCode", new Variable(B2GOutgoingInvoice.DocumentCurrencyCode));
                        // PartialAmount ??

                        invoiceVar.SetHashVariable("CodeReasonSpecified", new Variable(B2GOutgoingInvoice.InvoiceStatus.CodeReasonSpecified));
                        if (B2GOutgoingInvoice.InvoiceStatus.CodeReasonSpecified)
                        {
                            invoiceVar.SetHashVariable("CodeReason", new Variable(B2GOutgoingInvoice.InvoiceStatus.CodeReason.ToString()));
                        }

                        invoiceVar.SetHashVariable("BuyerID", new Variable(B2GOutgoingInvoice.BuyerID));
                        invoiceVar.SetHashVariable("AdditionalBuyerID", new Variable(B2GOutgoingInvoice.AdditionalBuyerID));
                        invoiceVar.SetHashVariable("BuyerRegistrationName", new Variable(B2GOutgoingInvoice.BuyerRegistrationName));
                        invoiceVar.SetHashVariable("BuyerCompanyID", new Variable(B2GOutgoingInvoice.BuyerCompanyID));
                        invoiceVar.SetHashVariable("InvoiceID", new Variable(B2GOutgoingInvoice.InvoiceID));
                        invoiceVar.SetHashVariable("SupplierInvoiceID", new Variable(B2GOutgoingInvoice.SupplierInvoiceID));
                        invoiceVar.SetHashVariable("DataInterchangeMethod", new Variable(B2GOutgoingInvoice.DataInterchangeMethod.ToString()));
                        invoiceVar.SetHashVariable("InvoiceIssueDate", new Variable(B2GOutgoingInvoice.InvoiceIssueDate));
                        invoiceVar.SetHashVariable("InvoiceDate", new Variable(B2GOutgoingInvoice.InvoiceDate));
                        invoiceVar.SetHashVariable("InvoiceTimestamp", new Variable(B2GOutgoingInvoice.InvoiceTimestamp));
                        invoiceVar.SetHashVariable("InvoicePayableAmount", new Variable(B2GOutgoingInvoice.InvoicePayableAmount));
                        invoiceVar.SetHashVariable("DocumentCurrencyCode", new Variable(B2GOutgoingInvoice.DocumentCurrencyCode));
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
