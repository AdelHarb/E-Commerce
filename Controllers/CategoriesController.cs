namespace ECommerce.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        if (_unitOfWork.Categories.GetByIdAsync(id) is not null)
            return BadRequest();
        
        var categories = await _unitOfWork.Categories.GetByIdAsync(id);
        _unitOfWork.Complete();

        return Ok(categories);
    }

    [HttpGet("GetByName")]
    public async Task<IActionResult> GetByName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return BadRequest("Name already exists or there is no name");
        
        var categories = await _unitOfWork.Categories.FindAsync(
            x => x.Name == name,
            new string[] { Includes.Product }
        );

        return Ok(categories);
    }
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
        _unitOfWork.Complete();

        return Ok(categories);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryReadDto dto)
    {
        if (dto is null)
            return BadRequest("Category is null");

        var category = _mapper.Map<Category>(dto);
        await _unitOfWork.Categories.AddAsync(category);
        _unitOfWork.Complete();

        return Ok(category);
    }
    [HttpPut]
    public async Task<IActionResult> Update(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if(category is not null)
            return BadRequest("Category is null");
        
        _unitOfWork.Categories.Update(category);
        _unitOfWork.Complete();

        return Ok(category);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if(category is not null)
            return BadRequest("Category is null");
        
        _unitOfWork.Categories.Delete(category);
        _unitOfWork.Complete();

        return Ok(category);
    }
}
