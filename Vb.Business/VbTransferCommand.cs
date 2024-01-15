using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vb.Data.Entity;

namespace Vb.Business;

public record VbTransferCommand : IRequest<Customer>;

