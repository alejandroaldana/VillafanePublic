using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Villafane.Domain.ViewModels
{
    public  class ConfigurationViewModel
    {
        public int? Id { get; set; }
        public int? IdLogo { get; set; }
        public string? ColorPrincipal { get; set; }
        public string? ColorBarra { get; set; }
        public string? ColorCabeceraTabla { get; set; }
        public string? ColorCabeceraTablaVertical { get; set; }

        public IFormFile? Logo { get; set; }
    }
}
