using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Workers.Models
{
    public class WAP_INGRESOPEDIDOS
    {
        public string IdTransacción  { get; set; }
        [Key]
        public string Propietario { get; set; }
        public string RazonFalla { get; set; }
        public int Estado { get; set; }
    }
}
