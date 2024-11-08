﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Online_Assessment
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class Online_AssessmentEntities : DbContext
    {
        public Online_AssessmentEntities()
            : base("name=Online_AssessmentEntities")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Admin_table> Admin_table { get; set; }
        public DbSet<Answer_table> Answer_table { get; set; }
        public DbSet<Difficulty_table> Difficulty_table { get; set; }
        public DbSet<Option_table> Option_table { get; set; }
        public DbSet<Question_mapping_table> Question_mapping_table { get; set; }
        public DbSet<Question_table> Question_table { get; set; }
        public DbSet<Subject_table> Subject_table { get; set; }
        public DbSet<Test_invitation_table> Test_invitation_table { get; set; }
        public DbSet<Test_table> Test_table { get; set; }
        public DbSet<User_table> User_table { get; set; }
    
        public virtual ObjectResult<Get_questions_Result> Get_questions(Nullable<int> subject_id, Nullable<int> difficulty_id)
        {
            var subject_idParameter = subject_id.HasValue ?
                new ObjectParameter("Subject_id", subject_id) :
                new ObjectParameter("Subject_id", typeof(int));
    
            var difficulty_idParameter = difficulty_id.HasValue ?
                new ObjectParameter("Difficulty_id", difficulty_id) :
                new ObjectParameter("Difficulty_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Get_questions_Result>("Get_questions", subject_idParameter, difficulty_idParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> Find_exist_testid(Nullable<int> test_id)
        {
            var test_idParameter = test_id.HasValue ?
                new ObjectParameter("test_id", test_id) :
                new ObjectParameter("test_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("Find_exist_testid", test_idParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> Get_mapped_questionIds(Nullable<int> test_id)
        {
            var test_idParameter = test_id.HasValue ?
                new ObjectParameter("test_id", test_id) :
                new ObjectParameter("test_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("Get_mapped_questionIds", test_idParameter);
        }
    
        public virtual int Delete_existing_questionIds(Nullable<int> test_id)
        {
            var test_idParameter = test_id.HasValue ?
                new ObjectParameter("test_id", test_id) :
                new ObjectParameter("test_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Delete_existing_questionIds", test_idParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> Find_email(string email)
        {
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("Find_email", emailParameter);
        }
    }
}
