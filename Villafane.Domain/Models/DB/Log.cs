using System;
using System.Collections.Generic;

namespace Villafane.Domain.Models.DB
{
    public partial class Log
    {
        public int Id { get; set; }
        public string? Usuario { get; set; }
        public string? Accion { get; set; }
        public string? Pantalla { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
