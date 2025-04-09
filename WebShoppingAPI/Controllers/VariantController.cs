using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using WebShoppingAPI.Errors;
using WebShoppingAPI.Infrastructure.Data;
using WebShoppingAPI.Infrastructure.Data.Repositories;
using WebShoppingAPI.Infrastructure.Interfaces;
using WebShoppingAPI.Infrastructure.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebShoppingAPI.Controllers
{
    public class VariantController : BaseAPIController
    {
        private readonly IVariantRepository _variantRepository;
        private readonly DatabaseContext _databaseContext;
        public VariantController(IVariantRepository variantRepository,
            DatabaseContext databaseContext)
        {
            _variantRepository = variantRepository;
            _databaseContext = databaseContext;
        }

        [HttpPost]
        [Route("CreateVariant")]
        public async Task<IActionResult> CreateVariant(Variant variant)
        {
            
            var i = await _variantRepository.CreateVariantAsync(variant);
            if (i > 0)
            {
                return Ok("Product variant added successfully");
            }
            else
            {
                return BadRequest("Unable to create product variant");
            }
        }

        [HttpPut]
        [Route("UpdateVariant")]
        public async Task<IActionResult> UpdateVariant(int variantId, Variant variant)
        {
            if (variantId != variant.Id)
            {
                return BadRequest();
            }
            try
            {
                int m = await _variantRepository.UpdateVariantAsync(variant);
                if (m > 0)
                {
                    return Ok("Product variant updated successfully");
                }
                else
                {
                    return BadRequest("Product variant update failed");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(variantId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpDelete]
        [Route("DeleteVariants")]
        public async Task<IActionResult> DeleteVariant(int variantId)
        {
            int d = await _variantRepository.DeleteVariantAsync(variantId);
            if (d > 0)
            {
                return Ok("Product variant deleted successfully");
            }
            else
            {
                return BadRequest("Product variant delete failed");
            }
        }

        [HttpGet]
        [Route("GetVariantsById")]
        public async Task<ActionResult<Variant>> GetVariantById(int variantId)
        {
            var variant = await _variantRepository.GetVariantsByIdAsync(variantId);
            if (variant == null)
            {
                return NotFound();
            }

            return variant;
        }

        [HttpGet]
        [Route("GetVariants")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Variant>>> GetVariants()
        {
            var variants = await _variantRepository.GetVariantsAsync();
            return Ok(variants);
        }

        [HttpGet]
        [Route("GetSingleVariants")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Variant>>> GetSingleVariants()
        {
            var variants = await _variantRepository.GetVariantsAsync();
            var singleVariants = variants.DistinctBy(prods => prods.ProductId);
            return Ok(singleVariants);
        }


        [HttpGet]
        [Route("GetVariantsByCategoryId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Variant>>>GetVariantsByCategoryId(int categoryId)
        {
            var variants = await _variantRepository.GetVariantsByCategoryIdAsync(categoryId);
            return Ok(variants);
        }

        [HttpGet]
        [Route("GetSingleVariantsByCategoryId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Variant>>> GetSingleVariantsByCategoryId(int categoryId)
        {
            var variants = await _variantRepository.GetVariantsByCategoryIdAsync(categoryId);
            var singleVariants = variants.DistinctBy(prods => prods.ProductId);
            return Ok(singleVariants);
        }


        [HttpGet]
        [Route("GetVariantsByProductId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Variant>>> GetVariantsByProductId(int productId)
        {
            var variants = await _variantRepository.GetVariantsByProductIdAsync(productId);
            return Ok(variants);
        }

        [HttpGet]
        [Route("GetStatus")]
        public async Task<ActionResult<IEnumerable<Status>>> GetStatus()
        {
            var prodStatus = await _databaseContext.Status.ToListAsync();
            return prodStatus;
        }

        private bool ProductExists(int variantId)
        {
            return _databaseContext.Product.Any(e => e.Id == variantId);
        }

        [HttpPost]
        [Route("UploadFile")]
        [DisableRequestSizeLimit]
        public IActionResult UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName).Replace("\\", "/");
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("UploadFiles")]
        public async Task<IActionResult> UploadFiles()
        {
            try
            {
                var files = Request.Form.Files;
                var uploadedFileUrls = new List<string>();

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var uploadsFolder = Path.Combine("Resources", "Images");
                        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder);
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var filePath = Path.Combine(uploadsFolder, fileName);
                        var dbPath = Path.Combine(uploadsFolder, fileName).Replace("\\", "/");
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        uploadedFileUrls.Add(dbPath);
                    }
                }

                return Ok(uploadedFileUrls);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
