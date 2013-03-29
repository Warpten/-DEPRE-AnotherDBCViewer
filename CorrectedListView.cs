using System.Windows.Forms;

public partial class CorrectedListView : ListView
{
    public CorrectedListView()
    {
        InitializeComponent();
        this.DoubleBuffered = true;
        this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        this.SetStyle(ControlStyles.EnableNotifyMessage, true);
    }

    protected override void OnNotifyMessage(Message m)
    {
        if (m.Msg != 0x14) //WM_ERASEBKGND
            base.OnNotifyMessage(m);
    }
}
