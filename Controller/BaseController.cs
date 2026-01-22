using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace QLCSV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        // Dùng cho các chức năng BẮT BUỘC phải đăng nhập (VD: Tạo, Sửa, Xóa)
        // Nếu không có Token -> Tự động báo lỗi 401 Unauthorized
        protected long GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("id");

            if (userIdClaim != null && long.TryParse(userIdClaim.Value, out long userId))
            {
                return userId;
            }

            throw new UnauthorizedAccessException("User ID not found in token.");
        }

        // Dùng cho các chức năng CÔNG KHAI (VD: Xem danh sách, Xem chi tiết)
        // Nếu không có Token -> Trả về null (không báo lỗi)
        protected long? GetCurrentUserIdOptional()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("id");

            if (userIdClaim != null && long.TryParse(userIdClaim.Value, out long userId))
            {
                return userId;
            }

            return null; // Khách vãng lai
        }
    }
}