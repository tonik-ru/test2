﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

public partial class EDFServerEntities : DbContext
{
    public EDFServerEntities()
        : base("name=EDFServerEntities")
    {
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }

    public DbSet<SCEUsers> SCEUsers { get; set; }
    public DbSet<Timetables> Timetables { get; set; }
    public DbSet<EDFModules> EDFModules { get; set; }
    public DbSet<UserModules> UserModules { get; set; }
    public DbSet<AppUsage> AppUsage { get; set; }
    public DbSet<EDFUsage> EDFUsage { get; set; }
}
