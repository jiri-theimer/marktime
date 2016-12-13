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

Imports System.Runtime.Serialization

Namespace VatService
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),  _
     System.Runtime.Serialization.DataContractAttribute(Name:="matchCode", [Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types")>  _
    Public Enum matchCode As Integer
        
        <System.Runtime.Serialization.EnumMemberAttribute(Value:="1")>  _
        _1 = 0
        
        <System.Runtime.Serialization.EnumMemberAttribute(Value:="2")>  _
        _2 = 1
        
        <System.Runtime.Serialization.EnumMemberAttribute(Value:="3")>  _
        _3 = 2
    End Enum
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ServiceModel.ServiceContractAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat", ConfigurationName:="VatService.checkVatPortType")>  _
    Public Interface checkVatPortType
        
        'CODEGEN: Generating message contract since the wrapper namespace (urn:ec.europa.eu:taxud:vies:services:checkVat:types) of message checkVatRequest does not match the default value (urn:ec.europa.eu:taxud:vies:services:checkVat)
        <System.ServiceModel.OperationContractAttribute(Action:="", ReplyAction:="*")>  _
        Function checkVat(ByVal request As VatService.checkVatRequest) As VatService.checkVatResponse
        
        'CODEGEN: Generating message contract since the wrapper namespace (urn:ec.europa.eu:taxud:vies:services:checkVat:types) of message checkVatApproxRequest does not match the default value (urn:ec.europa.eu:taxud:vies:services:checkVat)
        <System.ServiceModel.OperationContractAttribute(Action:="", ReplyAction:="*")>  _
        Function checkVatApprox(ByVal request As VatService.checkVatApproxRequest) As VatService.checkVatApproxResponse
    End Interface
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced),  _
     System.ServiceModel.MessageContractAttribute(WrapperName:="checkVat", WrapperNamespace:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", IsWrapped:=true)>  _
    Partial Public Class checkVatRequest
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=0)>  _
        Public countryCode As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=1)>  _
        Public vatNumber As String
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal countryCode As String, ByVal vatNumber As String)
            MyBase.New
            Me.countryCode = countryCode
            Me.vatNumber = vatNumber
        End Sub
    End Class
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced),  _
     System.ServiceModel.MessageContractAttribute(WrapperName:="checkVatResponse", WrapperNamespace:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", IsWrapped:=true)>  _
    Partial Public Class checkVatResponse
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=0)>  _
        Public countryCode As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=1)>  _
        Public vatNumber As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=2)>  _
        Public requestDate As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=3)>  _
        Public valid As Boolean
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=4)>  _
        Public name As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=5)>  _
        Public address As String
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal countryCode As String, ByVal vatNumber As String, ByVal requestDate As String, ByVal valid As Boolean, ByVal name As String, ByVal address As String)
            MyBase.New
            Me.countryCode = countryCode
            Me.vatNumber = vatNumber
            Me.requestDate = requestDate
            Me.valid = valid
            Me.name = name
            Me.address = address
        End Sub
    End Class
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced),  _
     System.ServiceModel.MessageContractAttribute(WrapperName:="checkVatApprox", WrapperNamespace:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", IsWrapped:=true)>  _
    Partial Public Class checkVatApproxRequest
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=0)>  _
        Public countryCode As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=1)>  _
        Public vatNumber As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=2)>  _
        Public traderName As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=3)>  _
        Public traderCompanyType As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=4)>  _
        Public traderStreet As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=5)>  _
        Public traderPostcode As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=6)>  _
        Public traderCity As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=7)>  _
        Public requesterCountryCode As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=8)>  _
        Public requesterVatNumber As String
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal countryCode As String, ByVal vatNumber As String, ByVal traderName As String, ByVal traderCompanyType As String, ByVal traderStreet As String, ByVal traderPostcode As String, ByVal traderCity As String, ByVal requesterCountryCode As String, ByVal requesterVatNumber As String)
            MyBase.New
            Me.countryCode = countryCode
            Me.vatNumber = vatNumber
            Me.traderName = traderName
            Me.traderCompanyType = traderCompanyType
            Me.traderStreet = traderStreet
            Me.traderPostcode = traderPostcode
            Me.traderCity = traderCity
            Me.requesterCountryCode = requesterCountryCode
            Me.requesterVatNumber = requesterVatNumber
        End Sub
    End Class
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced),  _
     System.ServiceModel.MessageContractAttribute(WrapperName:="checkVatApproxResponse", WrapperNamespace:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", IsWrapped:=true)>  _
    Partial Public Class checkVatApproxResponse
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=0)>  _
        Public countryCode As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=1)>  _
        Public vatNumber As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=2)>  _
        Public requestDate As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=3)>  _
        Public valid As Boolean
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=4)>  _
        Public traderName As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=5)>  _
        Public traderCompanyType As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=6)>  _
        Public traderAddress As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=7)>  _
        Public traderStreet As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=8)>  _
        Public traderPostcode As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=9)>  _
        Public traderCity As String
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=10)>  _
        Public traderNameMatch As VatService.matchCode
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=11)>  _
        Public traderCompanyTypeMatch As VatService.matchCode
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=12)>  _
        Public traderStreetMatch As VatService.matchCode
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=13)>  _
        Public traderPostcodeMatch As VatService.matchCode
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=14)>  _
        Public traderCityMatch As VatService.matchCode
        
        <System.ServiceModel.MessageBodyMemberAttribute([Namespace]:="urn:ec.europa.eu:taxud:vies:services:checkVat:types", Order:=15)>  _
        Public requestIdentifier As String
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New( _
                    ByVal countryCode As String,  _
                    ByVal vatNumber As String,  _
                    ByVal requestDate As String,  _
                    ByVal valid As Boolean,  _
                    ByVal traderName As String,  _
                    ByVal traderCompanyType As String,  _
                    ByVal traderAddress As String,  _
                    ByVal traderStreet As String,  _
                    ByVal traderPostcode As String,  _
                    ByVal traderCity As String,  _
                    ByVal traderNameMatch As VatService.matchCode,  _
                    ByVal traderCompanyTypeMatch As VatService.matchCode,  _
                    ByVal traderStreetMatch As VatService.matchCode,  _
                    ByVal traderPostcodeMatch As VatService.matchCode,  _
                    ByVal traderCityMatch As VatService.matchCode,  _
                    ByVal requestIdentifier As String)
            MyBase.New
            Me.countryCode = countryCode
            Me.vatNumber = vatNumber
            Me.requestDate = requestDate
            Me.valid = valid
            Me.traderName = traderName
            Me.traderCompanyType = traderCompanyType
            Me.traderAddress = traderAddress
            Me.traderStreet = traderStreet
            Me.traderPostcode = traderPostcode
            Me.traderCity = traderCity
            Me.traderNameMatch = traderNameMatch
            Me.traderCompanyTypeMatch = traderCompanyTypeMatch
            Me.traderStreetMatch = traderStreetMatch
            Me.traderPostcodeMatch = traderPostcodeMatch
            Me.traderCityMatch = traderCityMatch
            Me.requestIdentifier = requestIdentifier
        End Sub
    End Class
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Public Interface checkVatPortTypeChannel
        Inherits VatService.checkVatPortType, System.ServiceModel.IClientChannel
    End Interface
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Partial Public Class checkVatPortTypeClient
        Inherits System.ServiceModel.ClientBase(Of VatService.checkVatPortType)
        Implements VatService.checkVatPortType
        
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
        Function VatService_checkVatPortType_checkVat(ByVal request As VatService.checkVatRequest) As VatService.checkVatResponse Implements VatService.checkVatPortType.checkVat
            Return MyBase.Channel.checkVat(request)
        End Function
        
        Public Function checkVat(ByRef countryCode As String, ByRef vatNumber As String, <System.Runtime.InteropServices.OutAttribute()> ByRef valid As Boolean, <System.Runtime.InteropServices.OutAttribute()> ByRef name As String, <System.Runtime.InteropServices.OutAttribute()> ByRef address As String) As String
            Dim inValue As VatService.checkVatRequest = New VatService.checkVatRequest()
            inValue.countryCode = countryCode
            inValue.vatNumber = vatNumber
            Dim retVal As VatService.checkVatResponse = CType(Me,VatService.checkVatPortType).checkVat(inValue)
            countryCode = retVal.countryCode
            vatNumber = retVal.vatNumber
            valid = retVal.valid
            name = retVal.name
            address = retVal.address
            Return retVal.requestDate
        End Function
        
        <System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Function VatService_checkVatPortType_checkVatApprox(ByVal request As VatService.checkVatApproxRequest) As VatService.checkVatApproxResponse Implements VatService.checkVatPortType.checkVatApprox
            Return MyBase.Channel.checkVatApprox(request)
        End Function
        
        Public Function checkVatApprox( _
                    ByRef countryCode As String,  _
                    ByRef vatNumber As String,  _
                    ByRef traderName As String,  _
                    ByRef traderCompanyType As String,  _
                    ByRef traderStreet As String,  _
                    ByRef traderPostcode As String,  _
                    ByRef traderCity As String,  _
                    ByVal requesterCountryCode As String,  _
                    ByVal requesterVatNumber As String,  _
                    <System.Runtime.InteropServices.OutAttribute()> ByRef valid As Boolean,  _
                    <System.Runtime.InteropServices.OutAttribute()> ByRef traderAddress As String,  _
                    <System.Runtime.InteropServices.OutAttribute()> ByRef traderNameMatch As VatService.matchCode,  _
                    <System.Runtime.InteropServices.OutAttribute()> ByRef traderCompanyTypeMatch As VatService.matchCode,  _
                    <System.Runtime.InteropServices.OutAttribute()> ByRef traderStreetMatch As VatService.matchCode,  _
                    <System.Runtime.InteropServices.OutAttribute()> ByRef traderPostcodeMatch As VatService.matchCode,  _
                    <System.Runtime.InteropServices.OutAttribute()> ByRef traderCityMatch As VatService.matchCode,  _
                    <System.Runtime.InteropServices.OutAttribute()> ByRef requestIdentifier As String) As String
            Dim inValue As VatService.checkVatApproxRequest = New VatService.checkVatApproxRequest()
            inValue.countryCode = countryCode
            inValue.vatNumber = vatNumber
            inValue.traderName = traderName
            inValue.traderCompanyType = traderCompanyType
            inValue.traderStreet = traderStreet
            inValue.traderPostcode = traderPostcode
            inValue.traderCity = traderCity
            inValue.requesterCountryCode = requesterCountryCode
            inValue.requesterVatNumber = requesterVatNumber
            Dim retVal As VatService.checkVatApproxResponse = CType(Me,VatService.checkVatPortType).checkVatApprox(inValue)
            countryCode = retVal.countryCode
            vatNumber = retVal.vatNumber
            valid = retVal.valid
            traderName = retVal.traderName
            traderCompanyType = retVal.traderCompanyType
            traderAddress = retVal.traderAddress
            traderStreet = retVal.traderStreet
            traderPostcode = retVal.traderPostcode
            traderCity = retVal.traderCity
            traderNameMatch = retVal.traderNameMatch
            traderCompanyTypeMatch = retVal.traderCompanyTypeMatch
            traderStreetMatch = retVal.traderStreetMatch
            traderPostcodeMatch = retVal.traderPostcodeMatch
            traderCityMatch = retVal.traderCityMatch
            requestIdentifier = retVal.requestIdentifier
            Return retVal.requestDate
        End Function
    End Class
End Namespace