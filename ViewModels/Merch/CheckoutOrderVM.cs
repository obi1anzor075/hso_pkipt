using System.ComponentModel.DataAnnotations;

namespace HsoPkipt.ViewModels.Merch;

public class CheckoutOrderVM
{
    [Required(ErrorMessage = "Введите имя получателя")]
    public string RecipientName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите телефон")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите адрес доставки")]
    public string DeliveryAddress { get; set; } = string.Empty;
}
