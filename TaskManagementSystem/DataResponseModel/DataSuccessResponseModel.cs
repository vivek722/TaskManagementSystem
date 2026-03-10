namespace TaskManagementSystem.DataResponseModel;

public class DataSuccessResponseModel<T>
{
    public string Message { get; set; }
    public int StatusCode { get; set; }
    public T data { get; set; }
}
