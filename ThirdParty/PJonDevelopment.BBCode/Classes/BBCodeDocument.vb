'**************************************************
' FILE      : BBCodeDocument.vb
' AUTHOR    : Paulo Santos
' CREATION  : 4/29/2009 10:21:41 PM
' COPYRIGHT : Copyright � 2009
'             PJ on Development
'             All Rights Reserved.
'
' Description:
'       TODO: Add file description
'
' Change log:
' 0.1   4/29/2009 10:21:41 PM
'       Paulo Santos
'       Created.
'***************************************************

''' <summary>
''' Represents a document writen in BBCode.
''' </summary>
Public NotInheritable Class BBCodeDocument(Of TContext As Class)

	Private __Text As String
	Private __Parser As BBCodeParser(Of TContext)
	Private __Nodes As BBCodeNodeCollection(Of TContext)

	''' <summary>Initializes an instance of the <see cref="BBCodeDocument" /> class.</summary>
	''' <param name="parser">The <see cref="BBCodeParser"/> that created this instance.</param>
	Friend Sub New(ByVal parser As BBCodeParser(Of TContext))
		__Parser = parser
	End Sub

	''' <summary>
	''' Gets the <see cref="BBCodeParser"/> that is responsible for this <see cref="BBCodeDocument"/>.
	''' </summary>
	Friend ReadOnly Property Parser() As BBCodeParser(Of TContext)
		Get
			Return __Parser
		End Get
	End Property

	''' <summary>
	''' Gets or sets the BBCode of the document.
	''' </summary>
	Public Property Text() As String
		Get
			Return __Text
		End Get
		Set(ByVal value As String)
			__Text = value
			Nodes.Clear()
			Nodes.AddRange(Me.Parser.Parse(value).Nodes)
		End Set
	End Property

	''' <summary>
	''' Gets the <see cref="BBCodeNodeCollection"/> generated ba the BBCode text.
	''' </summary>
	''' <value>A <see cref="BBCodeNodeCollection"/> that represents the parsed text of the document.</value>
	Public ReadOnly Property Nodes() As BBCodeNodeCollection(Of TContext)
		Get
			If (__Nodes Is Nothing) Then
				__Nodes = New BBCodeNodeCollection(Of TContext)()
			End If
			Return __Nodes
		End Get
	End Property

	''' <summary>
	''' Returns the formatted text.
	''' </summary>
	''' <returns>The formatted text.</returns>
	Public Function Format(ByVal context As TContext) As String
		Return Format(context, New BBCodeHtmlFormatter(Of TContext)())
	End Function

	''' <summary>
	''' Returns the formatted text, using the specified <see cref="ITextFormatter"/>.
	''' </summary>
	''' <param name="formatter">An object that implements the <see cref="ITextFormatter"/> interface.</param>
	''' <returns>The formatted text.</returns>
	Public Function Format(ByVal context As TContext, ByVal formatter As ITextFormatter(Of TContext)) As String
		Dim sb As New Text.StringBuilder()
		For Each n In Nodes
			sb.Append(n.Format(context, formatter))
		Next
		Return sb.ToString()
	End Function

	Friend Sub SetText(ByVal text As String)
		__Text = text
	End Sub

End Class
