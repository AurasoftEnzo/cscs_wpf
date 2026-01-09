using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCSCS.ServiceReference2_B2BFinaInvoiceWebService;

namespace WpfCSCS.Fiskalizacija2.FINA.B2BIncoming
{
    public class GetOutgoingInvoiceList
    {
        private const string GET_B2B_OUTGOING_INVOICE_LIST_MESSAGE_TYPE = "9105";

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
            GetB2BOutgoingInvoiceListMsg message = new GetB2BOutgoingInvoiceListMsg();

            //Header
            HeaderSupplierType header = new HeaderSupplierType();
            header.MessageID = messageId;
            header.SupplierID = supplierId;
            header.AdditionalSupplierID = additionalSupplierId;
            header.MessageType = GET_B2B_OUTGOING_INVOICE_LIST_MESSAGE_TYPE;
            header.MessageAttributes = messageAttributes;

            message.HeaderSupplier = header;

            //Data
            GetB2BOutgoingInvoiceListMsgData data = new GetB2BOutgoingInvoiceListMsgData();

            GetB2BOutgoingInvoiceListMsgDataB2BOutgoingInvoiceList B2BOutgoingInvoiceList = new GetB2BOutgoingInvoiceListMsgDataB2BOutgoingInvoiceList();

            B2BOutgoingInvoiceList.DocumentCurrencyCode = documentCurrencyCode;
            B2BOutgoingInvoiceList.DocumentCurrencyCodeSpecified = documentCurrencyCode;


            //......
            var fromDateTime = dateFrom.Date;
            var toDateTime = dateTo.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            // !!! FILTER 
            B2BOutgoingInvoiceList.Item = new FilterType()
            {
                //Items = new object[] { new status }
                Items = new object[] { 
                    //new InvoiceStatusType() { StatusCode = InvoiceStatusTypeStatusCode.RECEIVED },
                    new DateRangeType() { From = fromDateTime, To = toDateTime}
                }
            };


            data.B2BOutgoingInvoiceList = B2BOutgoingInvoiceList;

            message.Data = data;


            //SEND
            FinaInvoiceB2BPortTypeClient client = _client.GetFinaInvoiceB2BPortTypeClient(endpointAddress,
                                                                                         dnsIdentity,
                                                                                         serviceCertificatePath,
                                                                                         clientCertificatePath,
                                                                                         clientCertificatePassword);
            GetB2BOutgoingInvoiceListAckMsg res = client.getB2BOutgoingInvoiceList(message);

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
            if (res.Item is GetB2BOutgoingInvoiceListAckMsgB2BOutgoingInvoiceList)
            {
                GetB2BOutgoingInvoiceListAckMsgB2BOutgoingInvoiceList outgoingInvoiceList = (GetB2BOutgoingInvoiceListAckMsgB2BOutgoingInvoiceList)res.Item;
                Variable invoicesArrayVar = new Variable(Variable.VarType.ARRAY);

                if (outgoingInvoiceList.B2BOutgoingInvoice != null)
                {
                    outgoingInvoiceList.B2BOutgoingInvoice.ToList().ForEach(B2BOutgoingInvoice =>
                    {
                        Variable invoiceVar = new Variable(Variable.VarType.ARRAY);


                        invoiceVar.SetHashVariable("XMLStandard", new Variable(B2BOutgoingInvoice.DocumentType.XMLStandard.ToString()));
                        invoiceVar.SetHashVariable("SpecificationIdentifier", new Variable(B2BOutgoingInvoice.DocumentType.SpecificationIdentifier.ToString()));
                        invoiceVar.SetHashVariable("DocumentTypeCode", new Variable(B2BOutgoingInvoice.DocumentType.DocumentTypeCode));
                        invoiceVar.SetHashVariable("DocumentTypeText", new Variable(B2BOutgoingInvoice.DocumentType.DocumentTypeText));

                        invoiceVar.SetHashVariable("StatusCode", new Variable(B2BOutgoingInvoice.InvoiceStatus.StatusCode.ToString()));
                        invoiceVar.SetHashVariable("Note", new Variable(B2BOutgoingInvoice.InvoiceStatus.Note));
                        invoiceVar.SetHashVariable("DocumentCurrencyCode", new Variable(B2BOutgoingInvoice.DocumentCurrencyCode));
                        // PartialAmount ??

                        invoiceVar.SetHashVariable("CodeReasonSpecified", new Variable(B2BOutgoingInvoice.InvoiceStatus.CodeReasonSpecified));
                        if (B2BOutgoingInvoice.InvoiceStatus.CodeReasonSpecified)
                        {
                            invoiceVar.SetHashVariable("CodeReason", new Variable(B2BOutgoingInvoice.InvoiceStatus.CodeReason.ToString()));
                        }

                        invoiceVar.SetHashVariable("BuyerID", new Variable(B2BOutgoingInvoice.BuyerID));
                        invoiceVar.SetHashVariable("AdditionalBuyerID", new Variable(B2BOutgoingInvoice.AdditionalBuyerID));
                        invoiceVar.SetHashVariable("BuyerRegistrationName", new Variable(B2BOutgoingInvoice.BuyerRegistrationName));
                        invoiceVar.SetHashVariable("BuyerCompanyID", new Variable(B2BOutgoingInvoice.BuyerCompanyID));
                        invoiceVar.SetHashVariable("InvoiceID", new Variable(B2BOutgoingInvoice.InvoiceID));
                        invoiceVar.SetHashVariable("SupplierInvoiceID", new Variable(B2BOutgoingInvoice.SupplierInvoiceID));
                        invoiceVar.SetHashVariable("DataInterchangeMethod", new Variable(B2BOutgoingInvoice.DataInterchangeMethod.ToString()));
                        invoiceVar.SetHashVariable("InvoiceIssueDate", new Variable(B2BOutgoingInvoice.InvoiceIssueDate));
                        invoiceVar.SetHashVariable("InvoiceDate", new Variable(B2BOutgoingInvoice.InvoiceDate));
                        invoiceVar.SetHashVariable("InvoiceTimestamp", new Variable(B2BOutgoingInvoice.InvoiceTimestamp));
                        invoiceVar.SetHashVariable("InvoicePayableAmount", new Variable(B2BOutgoingInvoice.InvoicePayableAmount));
                        invoiceVar.SetHashVariable("DocumentCurrencyCode", new Variable(B2BOutgoingInvoice.DocumentCurrencyCode));
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
