//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace apiBKRFID.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Produto")]
    public partial class Produto
    {
        public string ValueTag { get; set; }
        [Index(IsUnique = true)]
        public string CodBarras { get; set; }
        public Nullable<int> Quantidade { get; set; }
        [Key]
        public int ID_Produto { get; set; }
    }
}
