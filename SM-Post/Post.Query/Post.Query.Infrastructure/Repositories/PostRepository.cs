using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly DatabaseContextFactory _contextFactory;

    public PostRepository(DatabaseContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task CreateAsync(PostEntity post)
    {
        using DatabaseContext context = _contextFactory.CreateDBContext();
        context.Post.Add(post);
        _ = await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid postId)
    {
        using DatabaseContext context = _contextFactory.CreateDBContext();
        var post = await GetByIdAsync(postId);
        if (post == null)
            return;
        context.Post.Remove(post);
        _ = await context.SaveChangesAsync();
    }

    public async Task EditAsync(PostEntity post)
    {
        using DatabaseContext context = _contextFactory.CreateDBContext();
        context.Post.Update(post);
        _ = await context.SaveChangesAsync();
    }

    public async Task<PostEntity> GetByIdAsync(Guid postId)
    {
        using DatabaseContext context = _contextFactory.CreateDBContext();
        // return await context.Post.Include(p => p.Comments).FirstOrDefaultAsync(x => x.PostId == postId);
        return await context.Post.FirstOrDefaultAsync(x => x.PostId == postId);

    }

    public async Task<List<PostEntity>> ListAllAsync()
    {
        using DatabaseContext context = _contextFactory.CreateDBContext();
        return await context.Post.AsNoTracking().ToListAsync();
    }

    public async Task<List<PostEntity>> ListByAuthorAsync(string author)
    {
        using DatabaseContext context = _contextFactory.CreateDBContext();
        return await context.Post.AsNoTracking().Where(x => x.Author == author).ToListAsync();
    }

    // public async Task<List<PostEntity>> ListWithCommentsAsync()
    // {
    //     using DatabaseContext context = _contextFactory.CreateDBContext();
    //     return await context.Post.AsNoTracking().Include(p => p.Comments).Where(x => x.Comments != null && x.Comments.Any()).ToListAsync();
    // }

    // public async Task<List<PostEntity>> ListWithLikesAsync(int numberOfLikes)
    // {
    //     using DatabaseContext context = _contextFactory.CreateDBContext();
    //     return await context.Post.AsNoTracking().Include(p => p.Comments).Where(x => x.Likes == numberOfLikes).ToListAsync();
    // }
}
