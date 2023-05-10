namespace ECommerce.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("{id}", Name = "GetById")]
    public async Task<IActionResult> GetById(int id)
    {
        var categories = _unitOfWork.Products.GetByIdAsync(id: id);

        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductReadDto dto)
    {
        if (dto is null)
            return BadRequest("Product is null");

        var product = _mapper.Map<Product>(dto);
        await _unitOfWork.Products.AddAsync(product);
        _unitOfWork.Complete();

        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);

        if (product is null)
            return BadRequest("Product is null");

        _unitOfWork.Products.Delete(product);
        _unitOfWork.Complete();

        return Ok(product);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _unitOfWork.Products.GetAllAsync(new string[] { Includes.Category });
        _unitOfWork.Complete();

        return Ok(categories);
    }
}
