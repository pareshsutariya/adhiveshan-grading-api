namespace AdhiveshanGrading.Models;

public class ServiceResponse
{
    public bool IsSuccessful { get; set; }

    public string? ErrorMessage { get; set; }

    public static ServiceResponse<T> Success<T>(T data) => new ServiceResponse<T> { Data = data, IsSuccessful = true };

    public static ServiceResponse Success() => new ServiceResponse { IsSuccessful = true };

    public static ServiceResponse<T> Fail<T>(string error) => new ServiceResponse<T> { IsSuccessful = false, ErrorMessage = error };

    public static ServiceResponse<T> Fail<T>(T value, string? error) => new ServiceResponse<T> { Data = value, IsSuccessful = false, ErrorMessage = error };

    public static ServiceResponse Fail(string error) => new ServiceResponse { IsSuccessful = false, ErrorMessage = error };
}

public class ServiceResponse<T>
{
    public bool IsSuccessful { get; set; }

    public string? ErrorMessage { get; set; }

    public T? Data { get; set; }
}