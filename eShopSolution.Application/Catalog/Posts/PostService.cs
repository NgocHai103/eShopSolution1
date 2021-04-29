using eShopSolution.Application.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Constants;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModels.Catalog.Post;
using eShopSolution.ViewModels.Catalog.PostImages;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
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
           // var images = _context.PostImages.Where(i => i.IsDefault == true && i.PostId == PostId);
            //foreach (var image in images)
            //{
            //    _storageService.DeleteFileAsync(image.ImagePath);
            //}
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
                       // join pi in _context.PostImages on p.Id equals pi.PostId into ppi
                       // from pi in ppi.DefaultIfEmpty()
                        where pt.LanguageId == request.LanguageId
                        select new { p, pt /*,pi*/};

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
                    Images = /* x.pi.ImagePath!=null?new List<string> { x.pi.ImagePath }: */new List<string> { }
                })
                .Distinct()
                .ToListAsync();

            //var dataGroupById = data.GroupBy(x => x.Id);
            //List<PostVm> result = new List<PostVm>();
            //foreach(var group in dataGroupById)
            //{
            //    foreach (var item in group)
            //    {
            //        var temp = result.Find(x => x.Id == item.Id);
            //        if (temp == null)
            //            result.Add(item);
            //        else if(item.Images.Count > 0)
            //            temp.Images.Add(item.Images.First());

            //    }
            //}

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

        public async Task<PostVm> GetById(int PostId, string languageId)
        {
            var post = await _context.Posts.FindAsync(PostId);

            var postTranslation = await _context.PostTranslations.FirstOrDefaultAsync(x => x.PostId == PostId
            && x.LanguageId == languageId);
            var postImages = await _context.PostImages.ToArrayAsync<PostImage>();
            //var postImage = postImages.Where(x => x.PostId == PostId && x.IsDefault == true).Select(x=>x.ImagePath).ToList();

       
            return new PostVm()
            {
                Id = post.Id,
                DateCreated = post.DateCreated,
                Description = postTranslation != null ? postTranslation.Description : null,
                LanguageId = postTranslation.LanguageId,
                Content = postTranslation != null ? postTranslation.Content : null,
                Name = postTranslation != null ? postTranslation.Name : null,
               // Images = postImage.Count > 0 ? postImage : new List<string> { "no-image.jpg" }
            };
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

        public async Task<string> UploadImage(PostImageCreateRequest request)
        {
            var image = new PostImage();
            var ImagePath = await this.SaveFile(request.ImageFile);
            image.Caption = request.Caption;
            image.FileSize = request.ImageFile.Length;
            image.ImagePath = ImagePath;
            image.IsDefault = request.IsDefault;
            return image.ImagePath;
        }

        public async Task<ApiResult<PageResult<PostImageVm>>> GetAllImage(GetPostImageRequest request)
        {
            var query = from p in _context.PostImages
                        select new { p};

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.p.Caption.Contains(request.Keyword));
            }

            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new PostImageVm()
                {
                    Id = x.p.Id,
                    Caption = x.p.Caption,
                    IsDefault = x.p.IsDefault,
                    ImagePath = x.p.ImagePath
                })
                .Distinct()
                .ToListAsync();

            var pageResult = new ApiSuccessResult<PageResult<PostImageVm>>(
               new PageResult<PostImageVm>()
               {
                   TotalRecords = totalRow,
                   PageIndex = request.PageIndex,
                   PageSize = request.PageSize,
                   Items = data
               });
            return pageResult;
        }
    }
}
