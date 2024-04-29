using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using uyg03.Dtos;
using uyg03.Models;

namespace uyg03.Controllers
{
    [Route("api/Education")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        ResultDto result = new ResultDto();
        public EducationController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public List<EducationDto> GetList()
        {
            var products = _context.Products.ToList();
            var productDtos = _mapper.Map<List<EducationDto>>(products);
            return productDtos;
        }


        [HttpGet]
        [Route("{id}")]
        public EducationDto Get(int id)
        {
            var product = _context.Products.Where(s => s.Id == id).SingleOrDefault();
            var productDto = _mapper.Map<EducationDto>(product);
            return productDto;
        }

        [HttpPost]
        public ResultDto Post(EducationDto dto)
        {
            if (_context.Products.Count(c => c.Name == dto.Name) > 0)
            {
                result.Status = false;
                result.Message = "Girilen Eğitim Adı Kayıtlıdır!";
                return result;
            }
            var product = _mapper.Map<Education>(dto);
            product.Updated = DateTime.Now;
            product.Created = DateTime.Now;
            _context.Products.Add(product);
            _context.SaveChanges();
            result.Status = true;
            result.Message = "Eğitim Eklendi";
            return result;
        }


        [HttpPut]
        public ResultDto Put(EducationDto dto)
        {
            var product = _context.Products.Where(s => s.Id == dto.Id).SingleOrDefault();
            if (product == null)
            {
                result.Status = false;
                result.Message = "Eğitim Bulunamadı!";
                return result;
            }
            product.Name = dto.Name;
            product.IsActive = dto.IsActive;
            product.Price = dto.Price;
            product.Updated = DateTime.Now;
            product.CategoryId = dto.CategoryId;
            _context.Products.Update(product);
            _context.SaveChanges();
            result.Status = true;
            result.Message = "Eğitim Düzenlendi";
            return result;
        }


        [HttpDelete]
        [Route("{id}")]
        public ResultDto Delete(int id)
        {
            var product = _context.Products.Where(s => s.Id == id).SingleOrDefault();
            if (product == null)
            {
                result.Status = false;
                result.Message = "Eğitim Bulunamadı!";
                return result;
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            result.Status = true;
            result.Message = "Eğitim Silindi";
            return result;
        }
    }
}
