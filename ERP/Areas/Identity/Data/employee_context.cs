﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;

using ERP.Models.HRMS.Address;
using HRMS.Family;
using ERP.HRMS.Employee_managment;
using ERP.Models.HRMS.Employee_managments;
using HRMS.Office;
using HRMS.Types;

namespace ERP.Areas.Identity.Data
{

    public class employee_context : IdentityDbContext<User>
    {
        public employee_context(DbContextOptions<employee_context> options) : base(options)
        {
        }
        //Address
        public DbSet<Region> Regions { get; set; }
        public DbSet<Subcity> Subcitys { get; set; }
        public DbSet<Woreda> Woredas { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Employee_Address> Employee_Addresss { get; set; }
        public DbSet<Employee_Contact> Employee_Contacts { get; set; }
        public DbSet<Employee_Office> Employee_Offices { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Disability_Type> Disability_Types { get; set; }
        public DbSet<Education_Level_Type> Education_Level_Types { get; set; }
        public DbSet<Education_Program_Type> Education_Program_Types { get; set; }
        public DbSet<Employement_Type> Employement_Types { get; set; }
        public DbSet<Family_RelationShip_Type> Family_RelationShip_Types { get; set; }
        public DbSet<Reward_Types> Reward_Types { get; set; }
        public DbSet<Marital_Status_Types> Marital_Status_Types { get; set; }
        public DbSet<Violation_Types> Violation_Typess { get; set; }
       
    }
}