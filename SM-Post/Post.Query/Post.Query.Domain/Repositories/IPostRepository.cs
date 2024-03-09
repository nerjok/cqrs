using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories;

public interface IPostRepository
{
    Task CreateAsync(PostEntity post);
    Task EditAsync(PostEntity post);
    Task DeleteAsync(Guid postId);
    Task<PostEntity> GetByIdAsyn(Guid postId);
    Task<List<PostEntity>> ListAllAsync();
    Task<List<PostEntity>> ListByAuthorAsync(string author);
    Task<List<PostEntity>> ListWithLikesAsync(int numberOfLikes);
    Task<List<PostEntity>> ListWithCommentsAsync();
}
