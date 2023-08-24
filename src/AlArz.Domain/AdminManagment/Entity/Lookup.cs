using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp;

namespace Domain.Entity
{
    public class Lookup : BaseEntity
    {
        public string Name { get; set; }

        public string NameAr { get; set; }

        [ForeignKey(nameof(ParentLookup))]
        public Guid? Parent { get; set; }
        public Lookup ParentLookup { get; set; }


        public int Code { get; set; }

        public bool Enabled { get; set; }


        public Guid LookupTypeId { get; set; }

        //Navigation 


        public virtual LookupType LookupType { get; set; }
        public Lookup()
        {
            //Default constructor is needed for ORMs.
        }

        internal Lookup(
            Guid id,
            [NotNull] int code,
            [NotNull] string name,
            [NotNull] string nameAr,
            Guid lookupTypeId,
            bool Enabled)
        {

            Id = id;
            Code = code;
            LookupTypeId = lookupTypeId;
            Enabled = this.Enabled;
            SetName(Check.NotNullOrWhiteSpace(name, nameof(name)));
            SetNameAr(Check.NotNullOrWhiteSpace(nameAr, nameof(nameAr)));
        }

        public Lookup SetName([NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            Name = name;
            return this;
        }
        public Lookup SetNameAr([NotNull] string nameAr)
        {
            Check.NotNullOrWhiteSpace(nameAr, nameof(nameAr));

            NameAr = nameAr;
            return this;
        }

    }
}
