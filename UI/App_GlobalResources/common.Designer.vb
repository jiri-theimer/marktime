'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option or rebuild the Visual Studio project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "12.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Class common
        
        Private Shared resourceMan As Global.System.Resources.ResourceManager
        
        Private Shared resourceCulture As Global.System.Globalization.CultureInfo
        
        <Global.System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>  _
        Friend Sub New()
            MyBase.New
        End Sub
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Shared ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("Resources.common", Global.System.Reflection.[Assembly].Load("App_GlobalResources"))
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Shared Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Zavřít.
        '''</summary>
        Friend Shared ReadOnly Property close() As String
            Get
                Return ResourceManager.GetString("close", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Odstranit.
        '''</summary>
        Friend Shared ReadOnly Property delete() As String
            Get
                Return ResourceManager.GetString("delete", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Nápověda.
        '''</summary>
        Friend Shared ReadOnly Property help() As String
            Get
                Return ResourceManager.GetString("help", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Obnovit z archivu.
        '''</summary>
        Friend Shared ReadOnly Property move_from_archive() As String
            Get
                Return ResourceManager.GetString("move-from-archive", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Přesunout do archivu.
        '''</summary>
        Friend Shared ReadOnly Property move_to_archive() As String
            Get
                Return ResourceManager.GetString("move-to-archive", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Ostatní.
        '''</summary>
        Friend Shared ReadOnly Property ostatni() As String
            Get
                Return ResourceManager.GetString("ostatni", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Obnovit.
        '''</summary>
        Friend Shared ReadOnly Property refresh() As String
            Get
                Return ResourceManager.GetString("refresh", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Uložit změny.
        '''</summary>
        Friend Shared ReadOnly Property save_changes() As String
            Get
                Return ResourceManager.GetString("save-changes", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Uživatelská pole.
        '''</summary>
        Friend Shared ReadOnly Property uzivatelska_pole() As String
            Get
                Return ResourceManager.GetString("uzivatelska-pole", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Vlastnosti.
        '''</summary>
        Friend Shared ReadOnly Property vlastnosti() As String
            Get
                Return ResourceManager.GetString("vlastnosti", resourceCulture)
            End Get
        End Property
    End Class
End Namespace
