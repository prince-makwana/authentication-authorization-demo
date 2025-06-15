using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cookie_authentication_authorization_demo.Models.ViewModels;

public class UpdateOrderViewModel
{
    [Required]
    public List<UpdateOrderItemViewModel> OrderItems { get; set; }
}

public class UpdateOrderItemViewModel
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0")]
    public decimal UnitPrice { get; set; }
} 