namespace SimpeQuickGrid.Data
{
    public class PersonComparer : IEqualityComparer<Person>
    {
        public bool Equals(Person x, Person y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;
            return x.FirstName == y.FirstName
                && x.LastName == y.LastName
                && x.BirthDate == y.BirthDate
                && x.Gender == y.Gender
                && x.Sold == y.Sold
                && x.Active == y.Active;
        }

        public int GetHashCode(Person obj)
        {
            int hash = 17;
            hash = hash * 23 + (obj.FirstName?.GetHashCode() ?? 0);
            hash = hash * 23 + (obj.LastName?.GetHashCode() ?? 0);
            hash = hash * 23 + (obj.BirthDate?.GetHashCode() ?? 0);
            hash = hash * 23 + (obj.Gender?.GetHashCode() ?? 0);
            hash = hash * 23 + (obj.Sold?.GetHashCode() ?? 0);
            hash = hash * 23 + (obj.Active?.GetHashCode() ?? 0);
            return hash;
        }
    }
}
