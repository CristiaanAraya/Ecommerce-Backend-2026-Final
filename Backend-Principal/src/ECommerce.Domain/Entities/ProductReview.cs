using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class ProductReview
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid UserId { get; private set; }
    public int Rating { get; private set; }
    public string Comment { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public Product? Product { get; private set; }
    public User? User { get; private set; }

    private ProductReview() { }

    public ProductReview(Guid productId, Guid userId, int rating, string comment)
    {
        if (rating < 1 || rating > 5)
            throw new BusinessException("La calificación debe estar entre 1 y 5.");
        if (string.IsNullOrWhiteSpace(comment))
            throw new BusinessException("El comentario es obligatorio.");

        Id = Guid.NewGuid();
        ProductId = productId;
        UserId = userId;
        Rating = rating;
        Comment = comment.Trim();
        CreatedAt = DateTime.UtcNow;
    }
}
