//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BanroWebApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class t_vouchers_contractor
    {
        public int C_id_voucher { get; set; }
        public Nullable<int> C_id_Employed { get; set; }
        public Nullable<int> C_id_centre { get; set; }
        public string C_datedeb { get; set; }
        public string C_datefin { get; set; }
        public string C_namedoctor { get; set; }
        public string C_approuve { get; set; }
        public string C_motif { get; set; }
        public Nullable<decimal> C_cout { get; set; }
        public string C_service { get; set; }
    
        public virtual employee_contractor employee_contractor { get; set; }
        public virtual t_centre_soins t_centre_soins { get; set; }
    }
}
