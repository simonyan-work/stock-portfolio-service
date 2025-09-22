namespace StockPortfolioAPI.Models.Exceptions
{
    public class StockPortfolioException : Exception
    {
        public StockPortfolioException(string message) : base(message)
        {
        }

        public StockPortfolioException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class EntityNotFoundException : StockPortfolioException
    {
        public EntityNotFoundException(string entityName, int id) 
            : base($"{entityName} with ID {id} not found")
        {
        }

        public EntityNotFoundException(string entityName, string identifier) 
            : base($"{entityName} with identifier '{identifier}' not found")
        {
        }
    }

    public class InsufficientQuantityException : StockPortfolioException
    {
        public InsufficientQuantityException(decimal currentQuantity, decimal requestedQuantity) 
            : base($"Insufficient quantity. Current: {currentQuantity}, Requested: {requestedQuantity}")
        {
        }
    }

    public class DuplicateEntityException : StockPortfolioException
    {
        public DuplicateEntityException(string entityName, string identifier) 
            : base($"{entityName} with identifier '{identifier}' already exists")
        {
        }
    }
}
