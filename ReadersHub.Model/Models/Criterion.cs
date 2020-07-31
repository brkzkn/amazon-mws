using Repository.Pattern;

namespace ReadersHub.Model
{

    // Criterion
    
    public class Criterion : Entity
    {
        public int Id { get; set; } // Id (Primary key)
        public int StoreId { get; set; } // Store_Id
        public string Key { get; set; } // Key (length: 100)
        public string Value { get; set; } // Value (length: 5000)

        // Foreign keys
        public virtual Store Store { get; set; } // FK_Criterion_Store
    }

}

