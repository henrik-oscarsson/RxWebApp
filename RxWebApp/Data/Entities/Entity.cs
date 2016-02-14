using System;

namespace RxWebApp.Data.Entities
{
    internal abstract class Entity
    {
        protected Entity()
        {
            Created = DateTime.Now;
        }

        public int Id { get; set; }

        public DateTime Created { get; set; }

        public string Modifier { get; set; }

        public bool Deleted { get; set; }

        public int? EventModifierId { get; set; }

        public string EventModifierType { get; set; }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }

        public bool HasTransaction()
        {
            throw new NotImplementedException();
        }
    }
}