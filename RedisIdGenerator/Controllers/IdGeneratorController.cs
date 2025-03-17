using Microsoft.AspNetCore.Mvc;
using RedisIdGenerator.Services;

namespace RedisIdGenerator.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdGeneratorController : ControllerBase
{
    private readonly IRedisIdGeneratorService _idGeneratorService;

    public IdGeneratorController(IRedisIdGeneratorService idGeneratorService)
    {
        _idGeneratorService = idGeneratorService;
    }

    /// <summary>
    /// 获取下一个ID
    /// </summary>
    /// <param name="key">ID键名</param>
    /// <returns>自增后的ID值</returns>
    [HttpGet("next/{key}")]
    public async Task<IActionResult> GetNextId(string key)
    {
        if (string.IsNullOrEmpty(key))
            return BadRequest("键名不能为空");

        var id = await _idGeneratorService.GetNextIdAsync(key);
        return Ok(new { key, id });
    }

    /// <summary>
    /// 获取当前ID值
    /// </summary>
    /// <param name="key">ID键名</param>
    /// <returns>当前ID值</returns>
    [HttpGet("current/{key}")]
    public async Task<IActionResult> GetCurrentId(string key)
    {
        if (string.IsNullOrEmpty(key))
            return BadRequest("键名不能为空");

        var id = await _idGeneratorService.GetCurrentIdAsync(key);
        return Ok(new { key, id });
    }

    /// <summary>
    /// 重置ID值
    /// </summary>
    /// <param name="key">ID键名</param>
    /// <param name="startValue">起始值，默认为0</param>
    /// <returns></returns>
    [HttpPost("reset/{key}")]
    public async Task<IActionResult> ResetId(string key, [FromQuery] long startValue = 0)
    {
        if (string.IsNullOrEmpty(key))
            return BadRequest("键名不能为空");

        await _idGeneratorService.ResetIdAsync(key, startValue);
        return Ok(new { key, startValue, message = "ID已重置" });
    }
}