using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories;

public class CommentRepository : ICommentsRepository
{
    private readonly DatabaseContextFactory _contextFactory;

    public CommentRepository(DatabaseContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task CreateAsync(CommentEntity comment)
    {
        throw new NotImplementedException("FindPostsWithLikesQuery not implemented");
        // using DatabaseContext context = _contextFactory.CreateDBContext();
        // context.Comments.Add(comment);

        // _ = await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid commentId)
    {
        throw new NotImplementedException("FindPostsWithLikesQuery not implemented");
        // using DatabaseContext context = _contextFactory.CreateDBContext();
        // var comment = await GetByIdAsync(commentId);

        // if (comment == null) return;

        // context.Comments.Remove(comment);
        // _ = await context.SaveChangesAsync();
    }

    public async Task<CommentEntity> GetByIdAsync(Guid commentId)
    {
        throw new NotImplementedException("FindPostsWithLikesQuery not implemented");
        // using DatabaseContext context = _contextFactory.CreateDBContext();
        // return await context.Comments.FirstOrDefaultAsync(x => x.CommentId == commentId);
    }

    public async Task UpdateAsync(CommentEntity comment)
    {
        throw new NotImplementedException("FindPostsWithLikesQuery not implemented");
        // using DatabaseContext context = _contextFactory.CreateDBContext();
        // // context.Comments.Update(comment);

        // _ = await context.SaveChangesAsync();
    }
}
