namespace SimpeQuickGrid.Data
{
    public class PersonService
    {
        public Task<List<Person>> GetPeopleAsync()
        {
            var e = 1450.5;
            List<Person> people = new() 
            {                
                new() { FirstName = "Bailey", LastName = "Jessica", BirthDate = new(1977,02,07), Gender = Gender.Female , Sold = 1541.50m, Active = true },
                new() { FirstName = "Bailey", LastName = "Jessica", BirthDate = new(1989,07,18), Gender = Gender.Female , Sold = 1641.51m, Active = true },
                new() { FirstName = "Bailey", LastName = "Stephanie", BirthDate = new(1968,01,29), Gender = Gender.Female , Sold = 1581.50m, Active = true },
                new() { FirstName = "Bailey", LastName = "Andrea", BirthDate = new(1991,12,27), Gender = Gender.Female , Sold = 1345.50m, Active = true },
                new() { FirstName = "Baker", LastName = "George", BirthDate = new(1999,09,29), Gender = Gender.Male , Sold = 1324.74m, Active = true },
                new() { FirstName = "Baker", LastName = "Gerald", BirthDate = new(1986,03,26), Gender = Gender.Male , Sold = 1324.74m, Active = true },
                new() { FirstName = "Bennet", LastName = "Jerry", BirthDate = new(1995,01,09), Gender = Gender.Male , Sold = 1324.74m, Active = true },
                new() { FirstName = "Brooks", LastName = "Christine", BirthDate = new(1967,11,11), Gender = Gender.Female , Sold = 1324.74m, Active = true },
                new() { FirstName = "Brown", LastName = "Christine", BirthDate = new(1984,08,31), Gender = Gender.Female , Sold = 1324.74m, Active = true },
                new() { FirstName = "Campbell", LastName = "Jeremy", BirthDate = new(1989,12,02), Gender = Gender.Male , Sold = 1324.74m, Active = true },
            };
            return Task.FromResult(people);
        }
    }
}
