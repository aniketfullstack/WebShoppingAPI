using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShoppingAPI.Infrastructure.Data;
using WebShoppingAPI.Infrastructure.Interfaces;
using WebShoppingAPI.Infrastructure.Models;

namespace WebShoppingAPI.Controllers
{
    public class MeasureController : BaseAPIController
    {
        private readonly IMeasureRepository _measureRepository;
        private readonly DatabaseContext _databaseContext;
        public MeasureController(IMeasureRepository measureRepository,
            DatabaseContext databaseContext)
        {
            _measureRepository = measureRepository;
            _databaseContext = databaseContext;
        }

        [HttpPost]
        [Route("CreateMeasure")]
        public async Task<IActionResult> CreateMeasure(Measure measure)
        {
            //    _databaseContext.Products.Add(product);
            //    int i = await _databaseContext.SaveChangesAsync();
            var i = await _measureRepository.CreateMeasureAsync(measure);
            if (i > 0)
            {
                return Ok("Measure added successfully");
            }
            else
            {
                return BadRequest("Unable to create measure");
            }
        }

        [HttpPut]
        [Route("UpdateMeasure")]
        public async Task<IActionResult> UpdateMeasure(int id, Measure measure)
        {
            if (id != measure.Id)
            {
                return BadRequest();
            }
            try
            {
                int m = await _measureRepository.UpdateMeasureAsync(measure);
                if (m > 0)
                {
                    return Ok("Measure updated successfully");
                }
                else
                {
                    return BadRequest("Measure update failed");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeasureExists(id))
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
        [Route("DeleteMeasure")]
        public async Task<IActionResult> DeleteMeasure(int id)
        {
            int d = await _measureRepository.DeleteMeasureAsync(id);
            if (d > 0)
            {
                return Ok("Measure deleted successfully");
            }
            else
            {
                return BadRequest("Measure delete failed");
            }
            return NoContent();
        }


        [HttpGet]
        [Route("GetMeasures")]
        public async Task<ActionResult<IEnumerable<Measure>>> GetMeasures()
        {
            var brands = await _measureRepository.GetMeasuresAsync();
            return Ok(brands);
        }

        [HttpGet]
        [Route("GetMeasureById")]
        public async Task<ActionResult<Measure>> GetMeasureById(int id)
        {
            var brand = await _measureRepository.GetMeasureByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            return brand;
        }


        private bool MeasureExists(int id)
        {
            return _databaseContext.Measure.Any(e => e.Id == id);
        }

    }
}
