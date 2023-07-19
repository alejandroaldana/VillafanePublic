using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villafane.Domain.Models.DB;

namespace Villafane.Domain.ViewModels
{
    public class VariableViewModel
    {
        public int Id { get; set; }
        public string Variable1 { get; set; } = null!;
        public int IdValor { get; set; }

        public ValorViewModel IdValorNavigation { get; set; } 
    }
}
