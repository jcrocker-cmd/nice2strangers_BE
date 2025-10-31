using Crud.Contracts;
using Crud.Models.Entities;
using Crud.ViewModel;
using Crud.Data;
using System;
using Microsoft.EntityFrameworkCore;

namespace Crud.Service
{
    public class SocialLinkService : ISocialLinksService
    {
        private readonly ApplicationDBContext _context;

        public SocialLinkService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<SocialLinksViewModel?> GetSocialLinksAsync()
        {
            var entity = await _context.SocialLinks.FirstOrDefaultAsync();

            if (entity == null)
                return null; 

            return new SocialLinksViewModel
            {
                Facebook = entity.Facebook,
                Twitter = entity.Twitter,
                Instagram = entity.Instagram,
                Youtube = entity.Youtube,
                Tiktok = entity.Tiktok
            };
        }


        public async Task<SocialLinksViewModel> UpdateSocialLinksAsync(SocialLinksViewModel updatedLinks)
        {
            var entity = await _context.SocialLinks.FirstOrDefaultAsync();

            if (entity == null)
            {
                entity = new SocialLinks();
                _context.SocialLinks.Add(entity);
            }

            entity.Facebook = updatedLinks.Facebook;
            entity.Twitter = updatedLinks.Twitter;
            entity.Instagram = updatedLinks.Instagram;
            entity.Youtube = updatedLinks.Youtube;
            entity.Tiktok = updatedLinks.Tiktok;

            await _context.SaveChangesAsync();

            return updatedLinks;
        }
    }
}
