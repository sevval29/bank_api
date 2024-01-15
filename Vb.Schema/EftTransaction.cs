using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Vb.Base.Schema;

namespace Vb.Schema;


public class EftTransactionRequest : BaseRequest
{
    [JsonIgnore]
    public int Id { get; set; }

    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }

    public string ReceiverAccount { get; set; }
    public string ReceiverIban { get; set; }
    public string ReceiverName { get; set; }
}

public class EftTransactionResponse : BaseResponse
{
    [JsonIgnore]
    public int Id { get; set; }

    public int AccountId { get; set; }
    public string ReferenceNumber { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }

    public string ReceiverAccount { get; set; }
    public string ReceiverIban { get; set; }
    public string ReceiverName { get; set; }
}
