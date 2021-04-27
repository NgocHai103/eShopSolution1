using eShopSolution.Application.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Constants;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModels.Catalog.Post;
using eShopSolution.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Posts
{
    public class PostService : IPostService
    {
        private readonly EShopDBContext _context;
        private readonly IStorageService _storageService;
        public PostService(EShopDBContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }
        public async Task<ApiResult<bool>> Create(PostCreateRequest request)
        {
            var languages = _context.Languages;
            var tranlations = new List<PostTranslation>();
            foreach (var language in languages)
            {
                if (language.Id == request.LanguageId)
                {
                    tranlations.Add(new PostTranslation()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Content = request.Content,
                        LanguageId = request.LanguageId
                    });

                }
                else
                {
                    tranlations.Add(new PostTranslation()
                    {
                        Name = SystemConstants.PostConstants.NA,
                        Description = SystemConstants.PostConstants.NA,
                        Content = SystemConstants.PostConstants.NA,
                        LanguageId = language.Id
                    });
                }
            }
            var post = new Post()
            {
                DateCreated = DateTime.Now,
                PostTranslations = tranlations,
                IsActive = true
            };
         
            _context.Posts.Add(post);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return new ApiErrorResult<bool>("Không thể thêm bài viết");
            return new ApiSuccessResult<bool>();
        }
        public async Task<int> Delete(int PostId)
        {
            var post = await _context.Posts.FindAsync(PostId);
            if (post == null) throw new EShopException($"Cannot find a Post: {PostId}");
            var images = _context.PostImages.Where(i => i.IsDefault == true && i.PostId == PostId);
            foreach (var image in images)
            {
                _storageService.DeleteFileAsync(image.ImagePath);
            }
            _context.Posts.Remove(post);
            return await _context.SaveChangesAsync();
        }

        public Task<List<PostVm>> GetAll(string languageId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<PageResult<PostVm>>> GetAllPaging(GetPostPagingRequest request)
        {
            var query = from p in _context.Posts
                        join pt in _context.PostTranslations on p.Id equals pt.PostId into ppt
                        from pt in ppt.DefaultIfEmpty()
                        join pi in _context.PostImages on p.Id equals pi.PostId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        where pt.LanguageId == request.LanguageId
                        select new { p, pt ,pi};

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }

            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new PostVm()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Content = x.pt.Content,
                    LanguageId = x.pt.LanguageId,
                    Images =  x.pi.ImagePath
                })
                .Distinct()
                .ToListAsync();

           

            var pageResult = new ApiSuccessResult<PageResult<PostVm>>(
                new PageResult<PostVm>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data
                });
            return pageResult;
        }

        public Task<PostVm> GetById(int PostId, string languageId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<bool>> Update(int PostId, PostUpdateRequest request)
        {
            var post = await _context.Posts.FindAsync(PostId);
            var postTralations = await _context.PostTranslations.FirstOrDefaultAsync(x => x.PostId == PostId && x.LanguageId == request.LanguageId);
            if (post == null || postTralations == null)
                throw new EShopException($"cannot find a post with id:{request.Id}");
            postTralations.Name = request.Name;
            postTralations.Description = request.Description;
            postTralations.Content = request.Content;
            //save image
            //if (request.ThumbnailImage != null)
            //{
            //    var thumbnailImage = await _context.postImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.postId == postId);
            //    if (thumbnailImage != null)
            //    {
            //        thumbnailImage.FileSize = request.ThumbnailImage.Length;
            //        thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
            //        _context.postImages.Update(thumbnailImage);
            //    }
            //    else
            //    {
            //        //save image
            //        if (request.ThumbnailImage != null)
            //        {
            //            post.postImages = new List<postImage>()
            //            {
            //                new postImage()
            //                {
            //                    Caption = "Thumbnail image",
            //                    DateCreated = DateTime.Now,
            //                    FileSize = request.ThumbnailImage.Length,
            //                    ImagePath = await this.SaveFile(request.ThumbnailImage),
            //                    IsDefault = true,
            //                    SortOrder = 1
            //                }
            //            };
            //        }
            //    }
            //}
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return new ApiErrorResult<bool>("Bài viết chưa có thay đổi");
            return new ApiSuccessResult<bool>();
        }
    }
}
