Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'setting property backgroundworker
        BackgroundWorker1.WorkerSupportsCancellation = True 'agar backgroundworker bisa di cancel
        BackgroundWorker1.WorkerReportsProgress = True 'agar backgroundworker dapat memperbarui report progress
    End Sub

    Private Sub Start_Click(sender As Object, e As EventArgs) Handles Start.Click
        ' jika backgroundworker tidak sedang berjalan/busy
        If (BackgroundWorker1.IsBusy = False) Then
            ' menjalankan backgroundworker
            BackgroundWorker1.RunWorkerAsync()
        End If
    End Sub

    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        'jika backgroundworker sedang berjalan
        If (BackgroundWorker1.IsBusy) Then
            'membatalkan proses backgroundworker
            BackgroundWorker1.CancelAsync()
        End If
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        ' perulangan untuk mengisi value pada progressbar
        For i As Integer = 0 To 100

            Dim status As String = ""
            If (i >= 1 And i < 30) Then
                status = "Inisialisasi"
            ElseIf (i >= 30 And i < 70) Then
                status = "Pengecekan file configurasi"

            ElseIf (i >= 70 And i < 100) Then
                status = "Pengecekan Database"
            ElseIf (i = 100) Then
                status = "Selesai"
            End If

            ' memperbarui reportprogress
            BackgroundWorker1.ReportProgress(i, status)

            System.Threading.Thread.Sleep(100)

            ' jika backgroundWorker1 di batalkan
            If (BackgroundWorker1.CancellationPending) Then
                e.Cancel = True
                Exit Sub
            End If
        Next
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        ProgressBar1.Value = Convert.ToDouble(e.ProgressPercentage) 'memperbarui value pada progressbar
        Label1.Text = e.ProgressPercentage.ToString() & "%" ' memperbarui text pada label1
        Label2.Text = DirectCast(e.UserState, String) ' memberparui text pada label2
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        If (e.Cancelled = True) Then 'jika backgroundWorker1 dibatalkan
            MessageBox.Show("Proses dibatalkan")
        ElseIf (e.Error IsNot Nothing) Then ' jika terjadi error selama backgroundworker berjalan
            MessageBox.Show(e.Error.Message)
        Else
            MessageBox.Show("Proses selesai")
        End If
    End Sub
End Class
