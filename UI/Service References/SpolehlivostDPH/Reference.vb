﻿'------------------------------------------------------------------------------
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


Namespace SpolehlivostDPH
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ServiceModel.ServiceContractAttribute([Namespace]:="http://adis.mfcr.cz/rozhraniCRPDPH/", ConfigurationName:="SpolehlivostDPH.rozhraniCRPDPH")>  _
    Public Interface rozhraniCRPDPH
        
        'CODEGEN: Generating message contract since the wrapper name (StatusNespolehlivyPlatceRequest) of message getStatusNespolehlivyPlatceRequest does not match the default value (getStatusNespolehlivyPlatce)
        <System.ServiceModel.OperationContractAttribute(Action:="http://adis.mfcr.cz/rozhraniCRPDPH/getStatusNespolehlivyPlatce", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function getStatusNespolehlivyPlatce(ByVal request As SpolehlivostDPH.getStatusNespolehlivyPlatceRequest) As SpolehlivostDPH.getStatusNespolehlivyPlatceResponse
        
        'CODEGEN: Generating message contract since the wrapper name (SeznamNespolehlivyPlatceRequest) of message getSeznamNespolehlivyPlatceRequest does not match the default value (getSeznamNespolehlivyPlatce)
        <System.ServiceModel.OperationContractAttribute(Action:="http://adis.mfcr.cz/rozhraniCRPDPH/getSeznamNespolehlivyPlatce", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function getSeznamNespolehlivyPlatce(ByVal request As SpolehlivostDPH.getSeznamNespolehlivyPlatceRequest) As SpolehlivostDPH.getSeznamNespolehlivyPlatceResponse
    End Interface
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://adis.mfcr.cz/rozhraniCRPDPH/")>  _
    Partial Public Class StatusType
        Inherits Object
        Implements System.ComponentModel.INotifyPropertyChanged
        
        Private odpovedGenerovanaField As Date
        
        Private statusCodeField As Integer
        
        Private statusTextField As String
        
        Private bezVypisuUctuField As BezVypisuUctuType
        
        Private bezVypisuUctuFieldSpecified As Boolean
        
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>  _
        Public Property odpovedGenerovana() As Date
            Get
                Return Me.odpovedGenerovanaField
            End Get
            Set
                Me.odpovedGenerovanaField = value
                Me.RaisePropertyChanged("odpovedGenerovana")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property statusCode() As Integer
            Get
                Return Me.statusCodeField
            End Get
            Set
                Me.statusCodeField = value
                Me.RaisePropertyChanged("statusCode")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property statusText() As String
            Get
                Return Me.statusTextField
            End Get
            Set
                Me.statusTextField = value
                Me.RaisePropertyChanged("statusText")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property bezVypisuUctu() As BezVypisuUctuType
            Get
                Return Me.bezVypisuUctuField
            End Get
            Set
                Me.bezVypisuUctuField = value
                Me.RaisePropertyChanged("bezVypisuUctu")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property bezVypisuUctuSpecified() As Boolean
            Get
                Return Me.bezVypisuUctuFieldSpecified
            End Get
            Set
                Me.bezVypisuUctuFieldSpecified = value
                Me.RaisePropertyChanged("bezVypisuUctuSpecified")
            End Set
        End Property
        
        Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
        
        Protected Sub RaisePropertyChanged(ByVal propertyName As String)
            Dim propertyChanged As System.ComponentModel.PropertyChangedEventHandler = Me.PropertyChangedEvent
            If (Not (propertyChanged) Is Nothing) Then
                propertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0"),  _
     System.SerializableAttribute(),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://adis.mfcr.cz/rozhraniCRPDPH/")>  _
    Public Enum BezVypisuUctuType
        
        '''<remarks/>
        ANO
    End Enum
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://adis.mfcr.cz/rozhraniCRPDPH/")>  _
    Partial Public Class NestandardniUcetType
        Inherits Object
        Implements System.ComponentModel.INotifyPropertyChanged
        
        Private cisloField As String
        
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property cislo() As String
            Get
                Return Me.cisloField
            End Get
            Set
                Me.cisloField = value
                Me.RaisePropertyChanged("cislo")
            End Set
        End Property
        
        Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
        
        Protected Sub RaisePropertyChanged(ByVal propertyName As String)
            Dim propertyChanged As System.ComponentModel.PropertyChangedEventHandler = Me.PropertyChangedEvent
            If (Not (propertyChanged) Is Nothing) Then
                propertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://adis.mfcr.cz/rozhraniCRPDPH/")>  _
    Partial Public Class StandardniUcetType
        Inherits Object
        Implements System.ComponentModel.INotifyPropertyChanged
        
        Private predcisliField As String
        
        Private cisloField As String
        
        Private kodBankyField As String
        
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property predcisli() As String
            Get
                Return Me.predcisliField
            End Get
            Set
                Me.predcisliField = value
                Me.RaisePropertyChanged("predcisli")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property cislo() As String
            Get
                Return Me.cisloField
            End Get
            Set
                Me.cisloField = value
                Me.RaisePropertyChanged("cislo")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property kodBanky() As String
            Get
                Return Me.kodBankyField
            End Get
            Set
                Me.kodBankyField = value
                Me.RaisePropertyChanged("kodBanky")
            End Set
        End Property
        
        Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
        
        Protected Sub RaisePropertyChanged(ByVal propertyName As String)
            Dim propertyChanged As System.ComponentModel.PropertyChangedEventHandler = Me.PropertyChangedEvent
            If (Not (propertyChanged) Is Nothing) Then
                propertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://adis.mfcr.cz/rozhraniCRPDPH/")>  _
    Partial Public Class ZverejnenyUcetType
        Inherits Object
        Implements System.ComponentModel.INotifyPropertyChanged
        
        Private itemField As Object
        
        Private datumZverejneniField As Date
        
        Private datumZverejneniUkonceniField As Date
        
        Private datumZverejneniUkonceniFieldSpecified As Boolean
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("nestandardniUcet", GetType(NestandardniUcetType), Order:=0),  _
         System.Xml.Serialization.XmlElementAttribute("standardniUcet", GetType(StandardniUcetType), Order:=0)>  _
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set
                Me.itemField = value
                Me.RaisePropertyChanged("Item")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>  _
        Public Property datumZverejneni() As Date
            Get
                Return Me.datumZverejneniField
            End Get
            Set
                Me.datumZverejneniField = value
                Me.RaisePropertyChanged("datumZverejneni")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>  _
        Public Property datumZverejneniUkonceni() As Date
            Get
                Return Me.datumZverejneniUkonceniField
            End Get
            Set
                Me.datumZverejneniUkonceniField = value
                Me.RaisePropertyChanged("datumZverejneniUkonceni")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property datumZverejneniUkonceniSpecified() As Boolean
            Get
                Return Me.datumZverejneniUkonceniFieldSpecified
            End Get
            Set
                Me.datumZverejneniUkonceniFieldSpecified = value
                Me.RaisePropertyChanged("datumZverejneniUkonceniSpecified")
            End Set
        End Property
        
        Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
        
        Protected Sub RaisePropertyChanged(ByVal propertyName As String)
            Dim propertyChanged As System.ComponentModel.PropertyChangedEventHandler = Me.PropertyChangedEvent
            If (Not (propertyChanged) Is Nothing) Then
                propertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://adis.mfcr.cz/rozhraniCRPDPH/")>  _
    Partial Public Class InformaceOPlatciType
        Inherits Object
        Implements System.ComponentModel.INotifyPropertyChanged
        
        Private zverejneneUctyField() As ZverejnenyUcetType
        
        Private dicField As String
        
        Private nespolehlivyPlatceField As NespolehlivyPlatceType
        
        Private datumZverejneniNespolehlivostiField As Date
        
        Private datumZverejneniNespolehlivostiFieldSpecified As Boolean
        
        Private cisloFuField As String
        
        '''<remarks/>
        <System.Xml.Serialization.XmlArrayAttribute(Order:=0),  _
         System.Xml.Serialization.XmlArrayItemAttribute("ucet", IsNullable:=false)>  _
        Public Property zverejneneUcty() As ZverejnenyUcetType()
            Get
                Return Me.zverejneneUctyField
            End Get
            Set
                Me.zverejneneUctyField = value
                Me.RaisePropertyChanged("zverejneneUcty")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property dic() As String
            Get
                Return Me.dicField
            End Get
            Set
                Me.dicField = value
                Me.RaisePropertyChanged("dic")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property nespolehlivyPlatce() As NespolehlivyPlatceType
            Get
                Return Me.nespolehlivyPlatceField
            End Get
            Set
                Me.nespolehlivyPlatceField = value
                Me.RaisePropertyChanged("nespolehlivyPlatce")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>  _
        Public Property datumZverejneniNespolehlivosti() As Date
            Get
                Return Me.datumZverejneniNespolehlivostiField
            End Get
            Set
                Me.datumZverejneniNespolehlivostiField = value
                Me.RaisePropertyChanged("datumZverejneniNespolehlivosti")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property datumZverejneniNespolehlivostiSpecified() As Boolean
            Get
                Return Me.datumZverejneniNespolehlivostiFieldSpecified
            End Get
            Set
                Me.datumZverejneniNespolehlivostiFieldSpecified = value
                Me.RaisePropertyChanged("datumZverejneniNespolehlivostiSpecified")
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property cisloFu() As String
            Get
                Return Me.cisloFuField
            End Get
            Set
                Me.cisloFuField = value
                Me.RaisePropertyChanged("cisloFu")
            End Set
        End Property
        
        Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
        
        Protected Sub RaisePropertyChanged(ByVal propertyName As String)
            Dim propertyChanged As System.ComponentModel.PropertyChangedEventHandler = Me.PropertyChangedEvent
            If (Not (propertyChanged) Is Nothing) Then
                propertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1586.0"),  _
     System.SerializableAttribute(),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://adis.mfcr.cz/rozhraniCRPDPH/")>  _
    Public Enum NespolehlivyPlatceType
        
        '''<remarks/>
        NE
        
        '''<remarks/>
        ANO
        
        '''<remarks/>
        NENALEZEN
    End Enum
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced),  _
     System.ServiceModel.MessageContractAttribute(WrapperName:="StatusNespolehlivyPlatceRequest", WrapperNamespace:="http://adis.mfcr.cz/rozhraniCRPDPH/", IsWrapped:=true)>  _
    Partial Public Class getStatusNespolehlivyPlatceRequest
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="http://adis.mfcr.cz/rozhraniCRPDPH/", Order:=0),  _
         System.Xml.Serialization.XmlElementAttribute("dic")>  _
        Public dic() As String
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal dic() As String)
            MyBase.New
            Me.dic = dic
        End Sub
    End Class
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced),  _
     System.ServiceModel.MessageContractAttribute(WrapperName:="StatusNespolehlivyPlatceResponse", WrapperNamespace:="http://adis.mfcr.cz/rozhraniCRPDPH/", IsWrapped:=true)>  _
    Partial Public Class getStatusNespolehlivyPlatceResponse
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="http://adis.mfcr.cz/rozhraniCRPDPH/", Order:=0)>  _
        Public status As SpolehlivostDPH.StatusType
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="http://adis.mfcr.cz/rozhraniCRPDPH/", Order:=1),  _
         System.Xml.Serialization.XmlElementAttribute("statusPlatceDPH")>  _
        Public statusPlatceDPH() As SpolehlivostDPH.InformaceOPlatciType
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal status As SpolehlivostDPH.StatusType, ByVal statusPlatceDPH() As SpolehlivostDPH.InformaceOPlatciType)
            MyBase.New
            Me.status = status
            Me.statusPlatceDPH = statusPlatceDPH
        End Sub
    End Class
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced),  _
     System.ServiceModel.MessageContractAttribute(WrapperName:="SeznamNespolehlivyPlatceRequest", WrapperNamespace:="http://adis.mfcr.cz/rozhraniCRPDPH/", IsWrapped:=true)>  _
    Partial Public Class getSeznamNespolehlivyPlatceRequest
        
        Public Sub New()
            MyBase.New
        End Sub
    End Class
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced),  _
     System.ServiceModel.MessageContractAttribute(WrapperName:="SeznamNespolehlivyPlatceResponse", WrapperNamespace:="http://adis.mfcr.cz/rozhraniCRPDPH/", IsWrapped:=true)>  _
    Partial Public Class getSeznamNespolehlivyPlatceResponse
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="http://adis.mfcr.cz/rozhraniCRPDPH/", Order:=0)>  _
        Public status As SpolehlivostDPH.StatusType
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="http://adis.mfcr.cz/rozhraniCRPDPH/", Order:=1),  _
         System.Xml.Serialization.XmlElementAttribute("statusPlatceDPH")>  _
        Public statusPlatceDPH() As SpolehlivostDPH.InformaceOPlatciType
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal status As SpolehlivostDPH.StatusType, ByVal statusPlatceDPH() As SpolehlivostDPH.InformaceOPlatciType)
            MyBase.New
            Me.status = status
            Me.statusPlatceDPH = statusPlatceDPH
        End Sub
    End Class
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Public Interface rozhraniCRPDPHChannel
        Inherits SpolehlivostDPH.rozhraniCRPDPH, System.ServiceModel.IClientChannel
    End Interface
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Partial Public Class rozhraniCRPDPHClient
        Inherits System.ServiceModel.ClientBase(Of SpolehlivostDPH.rozhraniCRPDPH)
        Implements SpolehlivostDPH.rozhraniCRPDPH
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String)
            MyBase.New(endpointConfigurationName)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As String)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal binding As System.ServiceModel.Channels.Binding, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(binding, remoteAddress)
        End Sub
        
        <System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Function SpolehlivostDPH_rozhraniCRPDPH_getStatusNespolehlivyPlatce(ByVal request As SpolehlivostDPH.getStatusNespolehlivyPlatceRequest) As SpolehlivostDPH.getStatusNespolehlivyPlatceResponse Implements SpolehlivostDPH.rozhraniCRPDPH.getStatusNespolehlivyPlatce
            Return MyBase.Channel.getStatusNespolehlivyPlatce(request)
        End Function
        
        Public Function getStatusNespolehlivyPlatce(ByVal dic() As String, <System.Runtime.InteropServices.OutAttribute()> ByRef statusPlatceDPH() As SpolehlivostDPH.InformaceOPlatciType) As SpolehlivostDPH.StatusType
            Dim inValue As SpolehlivostDPH.getStatusNespolehlivyPlatceRequest = New SpolehlivostDPH.getStatusNespolehlivyPlatceRequest()
            inValue.dic = dic
            Dim retVal As SpolehlivostDPH.getStatusNespolehlivyPlatceResponse = CType(Me,SpolehlivostDPH.rozhraniCRPDPH).getStatusNespolehlivyPlatce(inValue)
            statusPlatceDPH = retVal.statusPlatceDPH
            Return retVal.status
        End Function
        
        <System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Function SpolehlivostDPH_rozhraniCRPDPH_getSeznamNespolehlivyPlatce(ByVal request As SpolehlivostDPH.getSeznamNespolehlivyPlatceRequest) As SpolehlivostDPH.getSeznamNespolehlivyPlatceResponse Implements SpolehlivostDPH.rozhraniCRPDPH.getSeznamNespolehlivyPlatce
            Return MyBase.Channel.getSeznamNespolehlivyPlatce(request)
        End Function
        
        Public Function getSeznamNespolehlivyPlatce(<System.Runtime.InteropServices.OutAttribute()> ByRef statusPlatceDPH() As SpolehlivostDPH.InformaceOPlatciType) As SpolehlivostDPH.StatusType
            Dim inValue As SpolehlivostDPH.getSeznamNespolehlivyPlatceRequest = New SpolehlivostDPH.getSeznamNespolehlivyPlatceRequest()
            Dim retVal As SpolehlivostDPH.getSeznamNespolehlivyPlatceResponse = CType(Me,SpolehlivostDPH.rozhraniCRPDPH).getSeznamNespolehlivyPlatce(inValue)
            statusPlatceDPH = retVal.statusPlatceDPH
            Return retVal.status
        End Function
    End Class
End Namespace
