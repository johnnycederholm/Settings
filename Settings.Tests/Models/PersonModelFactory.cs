namespace Settings.Tests.Models
{
    public static class PersonModelFactory
    {
        public static Person CreatePopulatedPerson()
        {
            return new Person
            {
                Name = "Anders Bengtsson",
                Age = 45,
                Address = new Address
                {
                    Street = "Storgatan 10",
                    ZipCode = "831 35",
                    City = "Lilla edet",
                    Country = new Country
                    {
                        Name = "Sweden",
                        CountryCode = 46
                    }
                }
            };
        }
    }
}
