' https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

Public Class FileIOPerformanceCounter
    Inherits DependencyObject

    Public Property PathAPI As Double
        Get
            Return GetValue(PathAPIProperty)
        End Get
        Set
            SetValue(PathAPIProperty, Value)
        End Set
    End Property
    Public Shared ReadOnly PathAPIProperty As DependencyProperty =
                           DependencyProperty.Register(NameOf(PathAPI),
                           GetType(Double), GetType(FileIOPerformanceCounter),
                           New PropertyMetadata(0.0))

    Public Property PathLoaded As Integer
        Get
            Return GetValue(PathLoadedProperty)
        End Get
        Set
            SetValue(PathLoadedProperty, Value)
        End Set
    End Property
    Public Shared ReadOnly PathLoadedProperty As DependencyProperty =
                           DependencyProperty.Register(NameOf(PathLoaded),
                           GetType(Integer), GetType(FileIOPerformanceCounter),
                           New PropertyMetadata(0))

    Public Property Extension As String
        Get
            Return GetValue(ExtensionProperty)
        End Get
        Set
            SetValue(ExtensionProperty, Value)
        End Set
    End Property
    Public Shared ReadOnly ExtensionProperty As DependencyProperty =
                           DependencyProperty.Register(NameOf(Extension),
                           GetType(String), GetType(FileIOPerformanceCounter),
                           New PropertyMetadata(".png"))

End Class
