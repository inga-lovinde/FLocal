Imports System.Xml.Serialization

''' <summary>
''' Represents a way of describing an element.
''' </summary>
Public Structure BBCodeElementTypeDefinition

    Private __TagName As String
    Private __RequireClosingTag As Boolean
    Private __Type As Type

    ''' <summary>
    ''' Gets the name of the element.
    ''' </summary>
    <XmlAttribute()> _
    Public Property TagName() As String
        Get
            Return __TagName
        End Get
        Set(ByVal value As String)
            __TagName = value.ToUpperInvariant()
        End Set
    End Property

    <XmlAttribute()> _
    Public Property Type() As Type
        Get
            Return __Type
        End Get
        Set(ByVal value As Type)
            __Type = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a value indicating if the element requires a closing tag.
    ''' </summary>
    <XmlAttribute()> _
    Public Property RequireClosingTag() As Boolean
        Get
            Return __RequireClosingTag
        End Get
        Set(ByVal value As Boolean)
            __RequireClosingTag = value
        End Set
    End Property

    ''' <summary>Indicates whether this instance and a specified object are equal.</summary>
    ''' <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.</returns>
    ''' <param name="obj">Another object to compare to.</param>
    ''' <filterpriority>2</filterpriority>
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If Not (TypeOf obj Is BBCodeElementTypeDefinition) Then
            Return False
        End If
        Return (Me.TagName = obj.TagName AndAlso Me.Type.Equals(DirectCast(obj, BBCodeElementTypeDefinition).Type) AndAlso Me.RequireClosingTag = obj.RequireClosingTag)
    End Function

    ''' <summary>Returns the hash code for this instance.</summary>
    ''' <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
    ''' <filterpriority>2</filterpriority>
    Public Overrides Function GetHashCode() As Integer
        Dim hash As Long = Me.TagName.GetHashCode() + Me.Type.GetHashCode() + Me.RequireClosingTag.GetHashCode()
        Return hash And Integer.MinValue
    End Function

    Public Shared Operator =(ByVal left As BBCodeElementTypeDefinition, ByVal right As BBCodeElementTypeDefinition) As Boolean
        Return left.Equals(right)
    End Operator

    Public Shared Operator <>(ByVal left As BBCodeElementTypeDefinition, ByVal right As BBCodeElementTypeDefinition) As Boolean
        Return Not left = right
    End Operator

End Structure
