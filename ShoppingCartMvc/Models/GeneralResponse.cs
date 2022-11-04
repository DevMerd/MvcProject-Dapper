namespace ShoppingCartMvc.Models
{
    public class GeneralResponse<T>
    {
        public T Result { get; set; }
        public bool IsError { get; set; } = false;
        public string ErrorMessage { get; set; }
    }
}
