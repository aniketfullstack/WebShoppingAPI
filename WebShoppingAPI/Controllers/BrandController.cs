using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShoppingAPI.Infrastructure.Data;
using WebShoppingAPI.Infrastructure.Interfaces;
using WebShoppingAPI.Infrastructure.Models;

namespace WebShoppingAPI.Controllers
{
    public class BrandController : BaseAPIController
    {
        private readonly IBrandRepository _brandRepository;
        private readonly DatabaseContext _databaseContext;
        public BrandController(IBrandRepository brandyRepository,DatabaseContext databaseContext)
        {
            _brandRepository = brandyRepository;
            _databaseContext = databaseContext;
        }

        [Authorize(Policy = "AdminLevelAccess")]
        [HttpPost]
        [Route("CreateBrand")]
        public async Task<ActionResult<Response>> CreateBrand(Brand brand)
        {
            var i = await _brandRepository.CreateBrandAsync(brand);
            if (i > 0)
            {
                return Ok("Brand Added");
            }
            else
            {
                return BadRequest("Unable to create Brand");
            }
        }

        [Authorize(Policy = "AdminLevelAccess")]
        [HttpPut]
        [Route("UpdateBrand")]
        public async Task<ActionResult<Response>> UpdateBrand(int brandId, Brand brand)
        {
            if (brandId != brand.Id)
            {
                return BadRequest();
            }
            try
            {
                int m = await _brandRepository.UpdateBrandAsync(brand);
                if (m > 0)
                {
                    return Ok("Brand Updated");
                }
                else
                {
                    return BadRequest("Brand Update Failed");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrandExists(brandId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [Authorize(Policy = "AdminLevelAccess")]
        [HttpDelete]
        [Route("DeleteBrand")]
        public async Task<ActionResult<Response>> DeleteBrand(int brandId)
        {
            int d = await _brandRepository.DeleteBrandAsync(brandId);
            if (d > 0)
            {
                return Ok("Brand Deleted Successfully");
            }
            else
            {
                return BadRequest("Brand Delete Failed");
            }
        }

        [Authorize(Policy = "AdminLevelAccess")]
        [HttpGet]
        [Route("GetBrands")]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {
            var brands = await _brandRepository.GetBrandsAsync();
            return Ok(brands);
        }

        [Authorize(Policy = "AdminLevelAccess")]
        [HttpGet]
        [Route("GetBrandById")]
        public async Task<ActionResult<Brand>> GetBrandById(int brandId)
        {
            var brand = await _brandRepository.GetBrandByIdAsync(brandId);
            if (brand == null)
            {
                return NotFound();
            }

            return brand;
        }


        private bool BrandExists(int id)
        {
            return _databaseContext.Brand.Any(e => e.Id == id);
        }

    }
}
