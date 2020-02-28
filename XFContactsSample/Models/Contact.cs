using SQLite;

namespace XFContactsSample.Models
{
    public class Contact
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        public string Image { get; set; }
        public string NamePrefix { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string NameSufix { get; set; }
        public string PhoneticLastName { get; set; }
        public string PhoneticMiddleName { get; set; }
        public string PhoneticFirstName { get; set; }
        public string NickName { get; set; }
        public string FileAs { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string IM { get; set; }
        public string WebSite { get; set; }
        public string SIP { get; set; }
        public string Notes { get; set; }
        public string Relationship { get; set; }
        public string NameSort { 
            get 
            { 
                if (string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName))
                {
                    return string.Empty;
                }

                return !string.IsNullOrEmpty(FirstName) ? 
                    FirstName[0].ToString().ToUpper() : LastName[0].ToString().ToUpper(); 
            } 
        }
        public string DisplayName 
        { 
            get 
            { 
                return !string.IsNullOrEmpty(FirstName) || !string.IsNullOrEmpty(LastName) ?
                    $"{FirstName} {LastName}" : Phone; 
            } 
        }
    }
}