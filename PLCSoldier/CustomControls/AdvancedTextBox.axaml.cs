using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace PLCSoldier.CustomControls;

public partial class AdvancedTextBox : UserControl
{
    public static readonly StyledProperty<string?> DefaultTextProperty =
        AvaloniaProperty.Register<AdvancedTextBox, string?>(nameof(DefaultText));

    public string? DefaultText
    {
        get => GetValue(DefaultTextProperty);
        set => SetValue(DefaultTextProperty, value);
    }

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<AdvancedTextBox, string?>(nameof(Text), defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        textBox.TextChanged += OnTextChanged;
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if ((sender as TextBox)!.Text == string.Empty)
        {
            defaultTextTxtBlock.Opacity = 1;
            Bounds.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
        else
        {
            defaultTextTxtBlock.Opacity = 0;
            Bounds.Background = new SolidColorBrush(Color.FromRgb(255, 249, 138));
        }
    }

    public AdvancedTextBox()
    {
        InitializeComponent();
    }
}