using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raci.Common.Enums;
using Raci.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Controllers
{
    [DisableRequestSizeLimit]
    public class UploadController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly RaciDbContext _context;
        
        public UploadController(IWebHostEnvironment environment, RaciDbContext context)
        {
            _environment = environment;
            _context = context;
        }

        [HttpPost("upload/shop-image/{shopId}")]
        public IActionResult UploadShopImage(IFormFile file, Guid shopId)
        {
            try
            {
                UploadFile(file, "", "");
                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("upload/item-image/{itemId}")]
        public async Task<IActionResult> UploadItemImageAsync(IFormFile file, Guid itemId)
        {
            try
            {
                var item = await _context.Items
                        .Where(p => p.Id == itemId
                            && p.AuditStatus != AuditStatusEnum.Deleted
                            && p.AuditStatus != AuditStatusEnum.Undefined)
                        .SingleOrDefaultAsync();

                if (item == null)
                {
                    return StatusCode(404);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string directoryPath = @$"\images\item\{item.Id}";

                UploadFile(file, directoryPath, fileName);

                item.ImageUrl = @$"{directoryPath}\{fileName}";

                await _context.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("upload/avatar-image/{accountId}")]
        public IActionResult UploadAvatarImage(IFormFile file, Guid accountId)
        {
            try
            {
                UploadFile(file, "", "");
                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        public async Task UploadFile(IFormFile file, string directoryPath, string fileName)
        {
            if (file != null && file.Length > 0)
            {
                var uploadPath = _environment.WebRootPath + directoryPath;
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var fullPath = Path.Combine(uploadPath, fileName);
                using (FileStream fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
        }
    }
}
