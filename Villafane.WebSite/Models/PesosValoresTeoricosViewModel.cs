using Villafane.Domain.Models.DB;

namespace Villafane.WebSite.Models
{
    public class PesosValoresTeoricosViewModel
    {
        public string Periodo { get; set; }
        public List<PesosValoresTeorico> Pesos { get; set; }
    }
}
