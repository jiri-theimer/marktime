Public Interface Ip31WorksheetBL
    Inherits IFMother
    Function SaveOrigRecord(cRecTU As BO.p31WorksheetEntryInput, lisFF As List(Of BO.FreeField)) As Boolean
    Function SaveOrigRecordBatch(dates As List(Of Date), cRec As BO.p31WorksheetEntryInput, lisFF As System.Collections.Generic.List(Of BO.FreeField)) As Boolean
    Function Update_p31Text(intPID As Integer, strNewText As String, Optional strGUID_TempData As String = "") As Boolean
    Function Load(intPID As Integer) As BO.p31Worksheet
    Function LoadTempRecord(intPID As Integer, strGUID_TempData As String) As BO.p31Worksheet
    Function LoadMyLastCreated(bolLoadTheSameTypeIfNoData As Boolean, intP41ID As Integer) As BO.p31Worksheet
    Function LoadMyLastCreated_TimeRecord() As BO.p31Worksheet
    Function LoadSumRow(myQuery As BO.myQueryP31, bolIncludeWaiting4Approval As Boolean, bolIncludeWaiting4Invoice As Boolean, Optional strGUID_TempData As String = "") As BO.p31WorksheetSum
    Function GetList_BigSummary(myQuery As BO.myQueryP31) As IEnumerable(Of BO.p31WorksheetBigSummary)
    Function Delete(intPID As Integer) As Boolean
    Function GetList(myQuery As BO.myQuery, Optional strGUID_TempData As String = "") As IEnumerable(Of BO.p31Worksheet)
    Function GetVirtualCount(myQuery As BO.myQuery, Optional strGUID_TempData As String = "") As Integer
    Function InhaleRecordDisposition(intPID As Integer) As BO.p31WorksheetDisposition

    Function Save_Approving(cApproveInput As BO.p31WorksheetApproveInput, bolTempData As Boolean) As Boolean
    Function Validate_Before_Save_Approving(cApproveInput As BO.p31WorksheetApproveInput, bolTempData As Boolean) As Boolean
    
    Function GetList_CalendarHours(intJ02ID As Integer, d1 As Date, d2 As Date) As IEnumerable(Of BO.p31WorksheetCalendarHours)
    Function GetList_Pivot(rows As List(Of BO.PivotRowColumnField), cols As List(Of BO.PivotRowColumnField), sums As List(Of BO.PivotSumField), mq As BO.myQueryP31) As IEnumerable(Of BO.PivotRecord)
    Function MoveToBin(pids As List(Of Integer)) As Boolean
    Function MoveFromBin(pids As List(Of Integer)) As Boolean
    Function Move2Project(intDestP41ID As Integer, pids As List(Of Integer)) As Boolean
    Function RecalcRates(pids As List(Of Integer)) As Boolean
    Function AppendToInvoice(intP91ID As Integer, pids As List(Of Integer)) As Boolean
    Function RemoveFromInvoice(intP91ID As Integer, pids As List(Of Integer)) As Boolean
    Function UpdateInvoice(intP91ID As Integer, lis As List(Of BO.p31WorksheetInvoiceChange)) As Boolean
    Function GetSumHoursPerMonth(intJ02ID As Integer, d1 As Date, d2 As Date) As IEnumerable(Of BO.HoursInMonth)
    Function GetSumHoursPerPersonAndDate(j02ids As List(Of Integer), d1 As Date, d2 As Date) As IEnumerable(Of BO.p31HoursPerPersonAndDay)
    Function GetDataSourceForTimeline(j02ids As List(Of Integer), d1 As Date, d2 As Date) As IEnumerable(Of BO.p31DataSourceForTimeline)
    Sub UpdateDeleteApprovingSet(strApprovingSet As String, p31ids As List(Of Integer), bolClear As Boolean, strTempGUID As String)
    Function GetList_ApprovingSet(strTempGUID As String, p41ids As List(Of Integer), p28ids As List(Of Integer)) As List(Of String)
    Function GetList_ApprovingFramework(x29id As BO.x29IdEnum, myQuery As BO.myQueryP31) As IEnumerable(Of BO.ApprovingFramework)
    Function LoadRate(bolCostRate As Boolean, dat As Date, intJ02ID As Integer, intP41ID As Integer, intP32ID As Integer, ByRef intRetJ27ID As Integer) As Double

    Function GetList_ExpenseSummary(myQuery As BO.myQueryP31) As IEnumerable(Of BO.WorksheetExpenseSummary)
End Interface
Class p31WorksheetBL
    Inherits BLMother
    Implements Ip31WorksheetBL
    Private WithEvents _cDL As DL.p31WorksheetDL
    Private _IsBatchValidationOnly As Boolean = False

    Private Sub _cDL_NeedHandleAppEvents(strX45IDs As String, intP41ID As Integer) Handles _cDL.NeedHandleAppEvents
        Dim a() As String = Split(strX45IDs, ",")
        For Each strX45ID As String In a
            Select Case strX45ID
                Case "14110"
                    Me.RaiseAppEvent(BO.x45IDEnum.p41_limithours_over, intP41ID)
                Case "14111"
                    Me.RaiseAppEvent(BO.x45IDEnum.p41_limitfee_over, intP41ID)
                Case "32810", "32811"
                    Dim cP41 As BO.p41Project = Me.Factory.p41ProjectBL.Load(intP41ID)
                    If strX45ID = "32810" Then
                        Me.RaiseAppEvent(BO.x45IDEnum.p28_limithours_over, cP41.p28ID_Client, cP41.Client)
                    Else
                        Me.RaiseAppEvent(BO.x45IDEnum.p28_limitfee_over, cP41.p28ID_Client, cP41.Client)
                    End If

            End Select
            
        Next
    End Sub

    Private Sub _cDL_OnError(strError As String) Handles _cDL.OnError
        _Error = strError
    End Sub
    Private Sub _cDL_OnSaveRecord(intLastSavedPID As Integer) Handles _cDL.OnSaveRecord
        _LastSavedPID = intLastSavedPID
    End Sub

    Public Sub New(ServiceUser As BO.j03UserSYS)
        _cDL = New DL.p31WorksheetDL(ServiceUser)
        _cUser = ServiceUser

    End Sub


    Public Function Delete(intPID As Integer) As Boolean Implements Ip31WorksheetBL.Delete
        Return _cDL.Delete(intPID)
    End Function

    Public Function GetList(myQuery As BO.myQuery, Optional strGUID_TempData As String = "") As IEnumerable(Of BO.p31Worksheet) Implements Ip31WorksheetBL.GetList
        Return _cDL.GetList(myQuery, strGUID_TempData)
    End Function

    Public Function Load(intPID As Integer) As BO.p31Worksheet Implements Ip31WorksheetBL.Load
        Return _cDL.Load(intPID)
    End Function
    Public Function LoadTempRecord(intPID As Integer, strGUID As String) As BO.p31Worksheet Implements Ip31WorksheetBL.LoadTempRecord
        Return _cDL.LoadTempRecord(intPID, strGUID)
    End Function
    Public Function LoadMyLastCreated(bolLoadTheSameTypeIfNoData As Boolean, intP41ID As Integer) As BO.p31Worksheet Implements Ip31WorksheetBL.LoadMyLastCreated
        Return _cDL.LoadMyLastCreated(bolLoadTheSameTypeIfNoData, intP41ID)
    End Function
    Public Function LoadMyLastCreated_TimeRecord() As BO.p31Worksheet Implements Ip31WorksheetBL.LoadMyLastCreated_TimeRecord
        Return _cDL.LoadMyLastCreated_TimeRecord()
    End Function
    Public Function LoadSumRow(myQuery As BO.myQueryP31, bolIncludeWaiting4Approval As Boolean, bolIncludeWaiting4Invoice As Boolean, Optional strGUID_TempData As String = "") As BO.p31WorksheetSum Implements Ip31WorksheetBL.LoadSumRow
        Return _cDL.LoadSumRow(myQuery, bolIncludeWaiting4Approval, bolIncludeWaiting4Invoice, strGUID_TempData)
    End Function
    Public Function GetList_BigSummary(myQuery As BO.myQueryP31) As IEnumerable(Of BO.p31WorksheetBigSummary) Implements Ip31WorksheetBL.GetList_BigSummary
        Return _cDL.GetList_BigSummary(myQuery)
    End Function
    Public Function GetVirtualCount(myQuery As BO.myQuery, Optional strGUID_TempData As String = "") As Integer Implements Ip31WorksheetBL.GetVirtualCount
        Return _cDL.GetVirtualCount(myQuery, strGUID_TempData)
    End Function

    Public Function SaveOrigRecordBatch(dates As List(Of Date), cRec As BO.p31WorksheetEntryInput, lisFF As System.Collections.Generic.List(Of BO.FreeField)) As Boolean Implements Ip31WorksheetBL.SaveOrigRecordBatch
        _IsBatchValidationOnly = True 'nejdříve pokusem o uložení hromadně otestovat všechny úkony na vstupu
        For Each d In dates
            cRec.p31Date = d
            If Not SaveOrigRecord(cRec, lisFF) Then
                _Error = "Záznam pro den [" & Format(d, "dd.MM.yyyy") & "]<hr>" & _Error
                Return False
            End If
        Next

        _IsBatchValidationOnly = False  'nyní už se záznamy fyzicky ukládají
        For Each d In dates
            cRec.p31Date = d
            If Not SaveOrigRecord(cRec, lisFF) Then
                Return False
            End If
        Next

        Return True
    End Function
    Public Function SaveOrigRecord(cRec As BO.p31WorksheetEntryInput, lisFF As System.Collections.Generic.List(Of BO.FreeField)) As Boolean Implements Ip31WorksheetBL.SaveOrigRecord
        With cRec
            If .p41ID = 0 And .p56ID <> 0 Then  'dohledat projekt podle úkolu
                Dim cP56 As BO.p56Task = Factory.p56TaskBL.Load(.p56ID)
                If Not cP56 Is Nothing Then
                    .p41ID = cP56.p41ID
                End If
            End If
            If .p41ID = 0 Then _Error = "V záznamu chybí projekt." : Return False
            If .j02ID = 0 Then _Error = "V záznamu chybí osoba." : Return False
            If .p34ID = 0 Then _Error = "V záznamu chybí sešit." : Return False
            If .p32ID = 0 Then
                Dim cP34 As BO.p34ActivityGroup = Factory.p34ActivityGroupBL.Load(.p34ID)
                If cP34.p34ActivityEntryFlag = BO.p34ActivityEntryFlagENUM.AktivitaJePovinna Then
                    _Error = String.Format("Sešit [{0}] vyžaduje na vstupu zadání aktivity.", cP34.p34Name) : Return False
                End If
                Dim mq As New BO.myQueryP32 'zkusit najít výchozí systémovou aktivitu, pokud se aktivita nemá zadávat
                mq.p34ID = .p34ID
                Dim lis As IEnumerable(Of BO.p32Activity) = Me.Factory.p32ActivityBL.GetList(mq).Where(Function(p) p.p32IsSystemDefault = True)
                If lis.Count > 0 Then
                    .p32ID = lis(0).PID
                End If
            End If
            If .PID = 0 And .Value_Orig_Entried = "" Then .Value_Orig_Entried = .Value_Orig
        End With
        Dim cValidate As BO.p31ValidateBeforeSave = _cDL.ValidateBeforeSaveOrigRecord(cRec, lisFF)
        With cValidate
            If .ErrorMessage <> "" Then
                _Error = .ErrorMessage : Return False
            End If
            Select Case .p33ID
                Case BO.p33IdENUM.Cas
                    If Not cRec.ValidateEntryTime(.Round2Minutes) Then
                        _Error = cRec.ErrorMessage
                        Return False
                    End If
                Case BO.p33IdENUM.Kusovnik
                    If Not cRec.ValidateEntryKusovnik() Then
                        _Error = cRec.ErrorMessage
                        Return False
                    End If
                Case BO.p33IdENUM.PenizeBezDPH
                    If cRec.j27ID_Billing_Orig = 0 Then
                        _Error = "U částky chybí měna." : Return False
                    End If
                    If cRec.Amount_WithoutVat_Orig = 0 Then
                        _Error = "Částka na vstupu nesmí být NULA."
                        Return False
                    End If
                    cRec.RecalcEntryAmount(cRec.Amount_WithoutVat_Orig, .VatRate)  'dopočítat částku vč. DPH
                    cRec.VatRate_Orig = .VatRate
                Case BO.p33IdENUM.PenizeVcDPHRozpisu
                    If cRec.j27ID_Billing_Orig = 0 Then
                        _Error = "U částky chybí měna." : Return False
                    End If
                    If cRec.Amount_WithoutVat_Orig = 0 And cRec.Amount_WithVat_Orig = 0 Then
                        _Error = "Částka na vstupu nesmí být NULA."
                        Return False
                    End If
                    cRec.SetAmounts()
                    If cRec.VatRate_Orig <> 0 And (cRec.Amount_WithVat_Orig = 0 Or cRec.Amount_Vat_Orig = 0) Then
                        cRec.RecalcEntryAmount(cRec.Amount_WithoutVat_Orig, cRec.VatRate_Orig)  'dopočítat částku vč. DPH
                    End If
            End Select

        End With

        If Not lisFF Is Nothing Then
            If Not BL.BAS.ValidateFF(lisFF) Then
                _Error = BL.BAS.ErrorMessage : Return False
            End If
        End If
        If _IsBatchValidationOnly Then Return True 'v režimu hromadné kontroly vstupních úkonů - zatím se neukládá
        If Not Me.RaiseAppEvent_TailoringTestBeforeSave(cRec, lisFF, "p31") Then Return False

        Return _cDL.SaveOrigRecord(cRec, cValidate.p33ID, lisFF)
    End Function
    
   
    Public Function InhaleRecordDisposition(intPID As Integer) As BO.p31WorksheetDisposition Implements Ip31WorksheetBL.InhaleRecordDisposition
        Return _cDL.InhaleRecordDisposition(intPID)
    End Function

    Public Function Update_p31Text(intPID As Integer, strNewText As String, Optional strGUID_TempData As String = "") As Boolean Implements Ip31WorksheetBL.Update_p31Text
        If Trim(strNewText) = "" Then
            Dim cRec As BO.p31Worksheet
            If strGUID_TempData = "" Then
                cRec = Load(intPID)
            Else
                cRec = LoadTempRecord(intPID, strGUID_TempData)
            End If
            Dim cP32 As BO.p32Activity = Me.Factory.p32ActivityBL.Load(cRec.p32ID)
            If cP32.p32IsTextRequired Then
                _Error = "Pro aktivitu [" & cP32.p32Name & "] je povinný popis úkonu."
                Return False
            End If
        End If
        Return _cDL.Update_p31Text(intPID, strNewText, strGUID_TempData)
    End Function

    Public Function Validate_Before_Save_Approving(cApproveInput As BO.p31WorksheetApproveInput, bolTempData As Boolean) As Boolean Implements Ip31WorksheetBL.Validate_Before_Save_Approving
        _Error = ""
        With cApproveInput
            If bolTempData And .GUID_TempData = "" Then
                _Error = "Pro temp data musí být předán GUID_TempData." : Return False
            End If
            If .GUID_TempData > "" And Not bolTempData Then
                _Error = "Je předáván GUID_TempData, ale bolTempData=false." : Return False
            End If
            If .p71id = BO.p71IdENUM.Schvaleno And .p72id = BO.p72IdENUM._NotSpecified Then
                _Error = "Schválený úkon musí mít přiřazen některý z fakturačních statusů." : Return False
            End If
            Select Case .p33ID
                Case BO.p33IdENUM.Cas, BO.p33IdENUM.Kusovnik
                    If .p72id = BO.p72IdENUM.Fakturovat And .Rate_Billing_Approved = 0 Then
                        _Error = "U statusu [Fakturovat] nesmí být nulová fakturační sazba." : Return False
                    End If
                    If .p72id = BO.p72IdENUM.Fakturovat And .Value_Approved_Billing = 0 And .p33ID = BO.p33IdENUM.Kusovnik Then
                        _Error = "U statusu [Fakturovat] nesmí být nulový počet kusů." : Return False
                    End If
                    If .p72id = BO.p72IdENUM.Fakturovat And .Value_Approved_Billing = 0 And .p33ID = BO.p33IdENUM.Cas Then
                        _Error = "U statusu [Fakturovat] nesmí být nulové hodiny." : Return False
                    End If
                Case BO.p33IdENUM.PenizeBezDPH, BO.p33IdENUM.PenizeVcDPHRozpisu
                    If .p72id = BO.p72IdENUM.Fakturovat And .Value_Approved_Billing = 0 Then
                        _Error = "U statusu [Fakturovat] nesmí být nulová částka úkonu." : Return False
                    End If
            End Select
        End With
        Return True
    End Function
    Public Function Save_Approving(cApproveInput As BO.p31WorksheetApproveInput, bolTempData As Boolean) As Boolean Implements Ip31WorksheetBL.Save_Approving
        If Not Validate_Before_Save_Approving(cApproveInput, bolTempData) Then Return False

        With cApproveInput
            Select Case .p33ID
                Case BO.p33IdENUM.Cas, BO.p33IdENUM.Kusovnik
                    Return _cDL.Save_Approving(.GUID_TempData, .p31ID, .p71id, .p72id, .Value_Approved_Billing, .Rate_Billing_Approved, .Value_Approved_Internal, .Rate_Internal_Approved, .p31Text, Nothing, .p31ApprovingSet)
                Case BO.p33IdENUM.PenizeBezDPH
                    Return _cDL.Save_Approving(.GUID_TempData, .p31ID, .p71id, .p72id, .Value_Approved_Billing, Nothing, .Value_Approved_Internal, Nothing, .p31Text, Nothing, .p31ApprovingSet)
                Case BO.p33IdENUM.PenizeVcDPHRozpisu
                    Return _cDL.Save_Approving(.GUID_TempData, .p31ID, .p71id, .p72id, .Value_Approved_Billing, Nothing, .Value_Approved_Internal, Nothing, .p31Text, .VatRate_Approved, .p31ApprovingSet)
                Case Else
                    _Error = "unknown p33id input"
                    Return False

            End Select
        End With

    End Function

    Function GetList_CalendarHours(intJ02ID As Integer, d1 As Date, d2 As Date) As IEnumerable(Of BO.p31WorksheetCalendarHours) Implements Ip31WorksheetBL.GetList_CalendarHours
        Return _cDL.GetList_CalendarHours(intJ02ID, d1, d2)
    End Function

    Public Function GetList_Pivot(rows As List(Of BO.PivotRowColumnField), cols As List(Of BO.PivotRowColumnField), sums As List(Of BO.PivotSumField), mq As BO.myQueryP31) As IEnumerable(Of BO.PivotRecord) Implements Ip31WorksheetBL.GetList_Pivot
        Return _cDL.GetList_Pivot(rows, cols, sums, mq)
    End Function
    Public Function MoveToBin(pids As List(Of Integer)) As Boolean Implements Ip31WorksheetBL.MoveToBin
        If pids.Count = 0 Then
            _Error = "Na vstupu není žádný úkon." : Return False
        End If
        Return _cDL.BinOperation(pids, True)
    End Function
    Public Function MoveFromBin(pids As List(Of Integer)) As Boolean Implements Ip31WorksheetBL.MoveFromBin
        If pids.Count = 0 Then
            _Error = "Na vstupu není žádný úkon." : Return False
        End If
        Return _cDL.BinOperation(pids, False)
    End Function
    Public Function Move2Project(intDestP41ID As Integer, pids As List(Of Integer)) As Boolean Implements Ip31WorksheetBL.Move2Project
        If intDestP41ID = 0 Then _Error = "Chybí cílový projekt." : Return False
        If pids.Count = 0 Then
            _Error = "Na vstupu není žádný úkon." : Return False
        End If
        Return _cDL.Move2Project(intDestP41ID, pids)
    End Function
    Public Function RecalcRates(pids As List(Of Integer)) As Boolean Implements Ip31WorksheetBL.RecalcRates
        If pids.Count = 0 Then
            _Error = "Na vstupu není žádný úkon." : Return False
        End If
        Return _cDL.RecalcRates(pids)
    End Function
    Public Function AppendToInvoice(intP91ID As Integer, pids As List(Of Integer)) As Boolean Implements Ip31WorksheetBL.AppendToInvoice
        If pids.Count = 0 Then
            _Error = "Na vstupu není žádný úkon." : Return False
        End If
       
        Return _cDL.AppendToInvoice(intP91ID, pids)
    End Function
    Public Function RemoveFromInvoice(intP91ID As Integer, pids As List(Of Integer)) As Boolean Implements Ip31WorksheetBL.RemoveFromInvoice
        If pids.Count = 0 Then
            _Error = "Na vstupu není žádný úkon." : Return False
        End If
        Dim mq As New BO.myQueryP31
        mq.p91ID = intP91ID
        If GetList(mq).Count <= pids.Count Then
            _Error = "Faktura musí obsahovat minimálně jednu položku. Nemůžete vyjmout všechny úkony z faktury." : Return False
        End If
        Return _cDL.RemoveFromInvoice(intP91ID, pids)
    End Function
    Public Function UpdateInvoice(intP91ID As Integer, lis As List(Of BO.p31WorksheetInvoiceChange)) As Boolean Implements Ip31WorksheetBL.UpdateInvoice
        If lis.Count = 0 Then
            _Error = "Na vstupu není žádný úkon." : Return False
        End If
        If lis.Where(Function(p) p.p70ID = BO.p70IdENUM.Nic).Count > 0 Then
            _Error = "Na vstupu je minimálně jeden úkon, který postrádá fakturační status." : Return False
        End If
      
        If lis.Where(Function(p) p.p70ID = BO.p70IdENUM.Vyfakturovano And (p.p33ID = BO.p33IdENUM.Cas Or p.p33ID = BO.p33IdENUM.Kusovnik) And (p.InvoiceRate = 0 Or p.InvoiceValue = 0)).Count > 0 Then
            _Error = "Na vstupu je minimálně jeden časový úkon pro fakturaci s nulovou sazbou nebo nulovým počtem hodin." : Return False
        End If
        If lis.Where(Function(p) p.p70ID = BO.p70IdENUM.Vyfakturovano And (p.p33ID = BO.p33IdENUM.PenizeBezDPH Or p.p33ID = BO.p33IdENUM.PenizeVcDPHRozpisu) And p.InvoiceValue = 0).Count > 0 Then
            _Error = "Na vstupu je minimálně jeden peněžní úkon pro fakturaci s nulovou částkou." : Return False
        End If
        
        Return _cDL.UpdateInvoice(intP91ID, lis)
    End Function
    Public Function GetSumHoursPerMonth(intJ02ID As Integer, d1 As Date, d2 As Date) As IEnumerable(Of BO.HoursInMonth) Implements Ip31WorksheetBL.GetSumHoursPerMonth
        Return _cDL.GetSumHoursPerMonth(intJ02ID, d1, d2)
    End Function
    Public Function GetSumHoursPerPersonAndDate(j02ids As List(Of Integer), d1 As Date, d2 As Date) As IEnumerable(Of BO.p31HoursPerPersonAndDay) Implements Ip31WorksheetBL.GetSumHoursPerPersonAndDate
        Return _cDL.GetSumHoursPerPersonAndDate(j02ids, d1, d2)
    End Function
    Public Function GetDataSourceForTimeline(j02ids As List(Of Integer), d1 As Date, d2 As Date) As IEnumerable(Of BO.p31DataSourceForTimeline) Implements Ip31WorksheetBL.GetDataSourceForTimeline
        Return _cDL.GetDataSourceForTimeline(j02ids, d1, d2)
    End Function
    Public Sub UpdateDeleteApprovingSet(strApprovingSet As String, p31ids As List(Of Integer), bolClear As Boolean, strTempGUID As String) Implements Ip31WorksheetBL.UpdateDeleteApprovingSet
        _cDL.UpdateDeleteApprovingSet(strApprovingSet, p31ids, bolClear, strTempGUID)
    End Sub

    Public Function GetList_ApprovingSet(strTempGUID As String, p41ids As List(Of Integer), p28ids As List(Of Integer)) As List(Of String) Implements Ip31WorksheetBL.GetList_ApprovingSet
        Return _cDL.GetList_ApprovingSet(strTempGUID, p41ids, p28ids)
    End Function
    Public Function GetList_ApprovingFramework(x29id As BO.x29IdEnum, myQuery As BO.myQueryP31) As IEnumerable(Of BO.ApprovingFramework) Implements Ip31WorksheetBL.GetList_ApprovingFramework
        Return _cDL.GetList_ApprovingFramework(x29id, myQuery)
    End Function

    Public Function LoadRate(bolCostRate As Boolean, dat As Date, intJ02ID As Integer, intP41ID As Integer, intP32ID As Integer, ByRef intRetJ27ID As Integer) As Double Implements Ip31WorksheetBL.LoadRate
        Return _cDL.LoadRate(bolCostRate, dat, intJ02ID, intP41ID, intP32ID, intRetJ27ID)
    End Function

    Public Function GetList_ExpenseSummary(myQuery As BO.myQueryP31) As IEnumerable(Of BO.WorksheetExpenseSummary) Implements Ip31WorksheetBL.GetList_ExpenseSummary
        Return _cDL.GetList_ExpenseSummary(myQuery)
    End Function
End Class
