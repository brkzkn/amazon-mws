//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReadersHub.MongoDB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Criterion
    {
        public int Id { get; set; }
        public int Store_Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    
        public virtual Store Store { get; set; }
    }
}
