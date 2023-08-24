
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp;

namespace Domain.Entity
{
    [Index(nameof(LookupTypeNumber), IsUnique = true)]
    public class LookupType : BaseEntity
    {
        public string Name { get; set; }
        public string NameAr { get; set; }



        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LookupTypeNumber { get; set; }

        public bool Enabled { get; set; }

        //Navigation


        public LookupType()
        {

        }

        internal LookupType(
            Guid id,
            [NotNull] int lookupTypeNumber,
            [NotNull] string name)
        {

            Id = id;
            LookupTypeNumber = lookupTypeNumber;
            SetName(Check.NotNullOrWhiteSpace(name, nameof(name)));
        }

        public LookupType SetName([NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            Name = name;
            return this;
        }

    }
}
