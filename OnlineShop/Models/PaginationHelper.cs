namespace OnlineShop.Models
{
    public static class PaginationHelper
    {
        public static List<T> Paginate<T>(IQueryable<T> query, int page, int pageSize, out int totalPages)
        {
            int totalItems = query.Count();
            totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
    }

}
