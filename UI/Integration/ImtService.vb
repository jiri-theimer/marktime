Imports System.ServiceModel
Imports System.Runtime.Serialization



' NOTE: You can use the "Rename" command on the context menu to change the interface name "Imtdefault" in both code and config file together.
<ServiceContract()>
Public Interface ImtService


    <OperationContract()>
        <FaultContract(GetType(FaultException))>
    Function Ping(strLogin As String, strPassword As String) As Boolean


  
    <OperationContract()>        
    Function SaveTask(intPID As Integer, fields As Dictionary(Of String, Object), receivers As List(Of BO.x69EntityRole_Assign), strLogin As String, strPassword As String) As BO.ServiceResult
    <OperationContract()>
    Function LoadTaskExtended(intPID As Integer, strLogin As String, strPassword As String) As BO.p56TaskWsExtended
    <OperationContract()>
    Function LoadTask(intPID As Integer, strLogin As String, strPassword As String) As BO.p56Task

    <OperationContract()>
    Function SaveWorksheet(intPID As Integer, fields As Dictionary(Of String, Object), strLogin As String, strPassword As String) As BO.ServiceResult

    <OperationContract()>
    Function ListProjects(intP28ID As Integer, strLogin As String, strPassword As String) As IEnumerable(Of BO.p41Project)
    <OperationContract()>
    Function LoadProject(intPID As Integer, strLogin As String, strPassword As String) As BO.p41Project
    <OperationContract()>
    Function SaveProject(intPID As Integer, fields As Dictionary(Of String, Object), receivers As List(Of BO.x69EntityRole_Assign), strLogin As String, strPassword As String) As BO.ServiceResult
    <OperationContract()>
    Function ListClients(strLogin As String, strPassword As String) As IEnumerable(Of BO.p28Contact)
    <OperationContract()>
    Function LoadClient(intPID As Integer, strLogin As String, strPassword As String) As BO.p28Contact
    <OperationContract()>
    Function SaveClient(intPID As Integer, fields As Dictionary(Of String, Object), addresses As List(Of BO.o37Contact_Address), p58ids As List(Of Integer), strLogin As String, strPassword As String) As BO.ServiceResult

    <OperationContract()>
    Function ListProducts(intP28ID As Integer, strLogin As String, strPassword As String) As IEnumerable(Of BO.p58Product)
    <OperationContract()>
    Function ListPriorities(strLogin As String, strPassword As String) As IEnumerable(Of BO.p59Priority)
    <OperationContract()>
    Function ListTaskTypes(strLogin As String, strPassword As String) As IEnumerable(Of BO.p57TaskType)
    <OperationContract()>
    Function ListActivities(strLogin As String, strPassword As String) As IEnumerable(Of BO.p32Activity)
    <OperationContract()>
    Function ListSheets(strLogin As String, strPassword As String) As IEnumerable(Of BO.p34ActivityGroup)
    <OperationContract()>
    Function ListPersons(strLogin As String, strPassword As String) As IEnumerable(Of BO.j02Person)
    <OperationContract()>
    Function ListPersonTeams(strLogin As String, strPassword As String) As IEnumerable(Of BO.j11Team)
    <OperationContract()>
    Function ListRoles(strLogin As String, strPassword As String) As IEnumerable(Of BO.x67EntityRole)
    <OperationContract()>
    Function ListWorkflowStatuses(intB01ID As Integer, strLogin As String, strPassword As String) As IEnumerable(Of BO.b02WorkflowStatus)
    <OperationContract()>
    Function ListWorkflowSteps(intB01ID As Integer, strLogin As String, strPassword As String) As IEnumerable(Of BO.b06WorkflowStep)
    <OperationContract()>
    Function ListPossibleWorkflowSteps(intRecordPID As Integer, strRecordPrefix As String, intJ02ID As Integer, strLogin As String, strPassword As String) As List(Of BO.WorkflowStepPossible4User)

End Interface


''<DataContract()>
''Public Class ServiceFaultException
''    <DataMember()>
''    Public Property ErrorMessage As String
''    Public Sub New(reason As FaultReason)

''    End Sub
''End Class



