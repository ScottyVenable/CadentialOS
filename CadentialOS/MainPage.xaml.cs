namespace CadentialOS;
using Xamarin.Essentials

public partial class MainPage : ContentPage
{
    private readonly Image _cursorImage = new Image
    {
        Source = "cursor.png",
        WidthRequest = 32,
        HeightRequest = 32,
        IsVisible = false
    };

    public MainPage()
    {
        var label = new Label
        {
            Text = "Click or press a key"
        };

        Content = new AbsoluteLayout
        {
            Children = {
                label,
                _cursorImage
            }
        };

        // Handle mouse events
        MouseDown += (sender, e) =>
        {
            _cursorImage.IsVisible = true;
            UpdateCursorPosition(e.Position);
            label.Text = $"Mouse down at ({e.Position.X}, {e.Position.Y})";
        };

        MouseUp += (sender, e) =>
        {
            _cursorImage.IsVisible = false;
            label.Text = $"Mouse up at ({e.Position.X}, {e.Position.Y})";
        };

        MouseMove += (sender, e) =>
        {
            if (_cursorImage.IsVisible)
            {
                UpdateCursorPosition(e.Position);
            }
            label.Text = $"Mouse moved to ({e.Position.X}, {e.Position.Y})";
        };

        // Scan for Bluetooth devices
        BluetoothScanner.ScanAsync().ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                // Handle exception
                return;
            }

            var devices = task.Result.Where(d => d.Type == BluetoothDeviceType.Mouse);
            foreach (var device in devices)
            {
                // Connect to Bluetooth mouse device
                BluetoothConnectionHandler.ConnectAsync(device);
            }
        });
    }

    private void UpdateCursorPosition(Point position)
    {
        AbsoluteLayout.SetLayoutBounds(_cursorImage, new Rectangle(position.X, position.Y, _cursorImage.WidthRequest, _cursorImage.HeightRequest));
    }
}

