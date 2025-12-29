namespace simple_crud.Client.Infrastructure;

public class LoaderService
{
    public event Action? OnChange;

    public bool IsLoading { get; private set; }

    public void Show()
    {
        IsLoading = true;
        OnChange?.Invoke();
    }

    public void Hide()
    {
        IsLoading = false;
        OnChange?.Invoke();
    }
}
