﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ReadersHubEntities : DbContext
    {
        public ReadersHubEntities()
            : base("name=ReadersHubEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Criterion> Criterion { get; set; }
        public virtual DbSet<Feed_Temp> Feed_Temp { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Product_Price> Product_Price { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<User_Role> User_Role { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}
