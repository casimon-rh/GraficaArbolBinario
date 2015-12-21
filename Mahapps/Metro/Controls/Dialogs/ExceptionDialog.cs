using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MahApps.Metro.Controls.Dialogs
{
    public partial class ExceptionDialog : BaseMetroDialog
    {
        internal ExceptionDialog(MetroWindow parentWindow)
            : this(parentWindow, null)
        {
        }
        internal ExceptionDialog(MetroWindow parentWindow, MetroDialogSettings settings)
            : base(parentWindow, settings)
        {
            InitializeComponent();
        }

        internal Task<MessageDialogResult> WaitForButtonPressAsync()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.Focus();

                PART_AffirmativeButton.Focus();
            }));

            TaskCompletionSource<MessageDialogResult> tcs = new TaskCompletionSource<MessageDialogResult>();

            RoutedEventHandler affirmativeHandler = null;
            KeyEventHandler affirmativeKeyHandler = null;


            Action cleanUpHandlers = () =>
            {
                PART_AffirmativeButton.Click -= affirmativeHandler;
                PART_AffirmativeButton.KeyDown -= affirmativeKeyHandler;
            };

            affirmativeKeyHandler = (sender, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    cleanUpHandlers();

                    tcs.TrySetResult(MessageDialogResult.Affirmative);
                }
            };

            affirmativeHandler = (sender, e) =>
            {
                cleanUpHandlers();

                tcs.TrySetResult(MessageDialogResult.Affirmative);

                e.Handled = true;
            };

            PART_AffirmativeButton.KeyDown += affirmativeKeyHandler;
            PART_AffirmativeButton.Click += affirmativeHandler;

            return tcs.Task;
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(ExceptionDialog), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty ExceptionProperty = DependencyProperty.Register("Ex", typeof(Exception), typeof(ExceptionDialog), new PropertyMetadata(default(Exception)));
        public static readonly DependencyProperty ExMessageProperty = DependencyProperty.Register("ExString", typeof(string), typeof(ExceptionDialog), new PropertyMetadata(default(string)));

        private static void SetButtonState(ExceptionDialog md)
        {
            md.PART_AffirmativeButton.Visibility = Visibility.Visible;
            
            switch (md.DialogSettings.ColorScheme)
            {
                case MetroDialogColorScheme.Accented:
                    break;
            }
        }

        private void Dialog_Loaded(object sender, RoutedEventArgs e)
        {
            SetButtonState(this);
        }

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
        public string ExString
        {
            get { return (string)GetValue(ExMessageProperty); }
            set { SetValue(ExMessageProperty, value); }

        }
        public Exception Ex
        {
            get { return (Exception)GetValue(ExceptionProperty); }
            set { SetValue(ExceptionProperty, value); }
        }

    }

}
