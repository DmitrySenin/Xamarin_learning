using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Phoneword
{
    [Activity(Label = "Phoneword", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        EditText phoneNumberText;
        Button translateButton;
        Button callButton;

        string translatedNumber = string.Empty;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            this.DetectControls();

            this.translateButton.Click += TranslateButton_Click;
            this.callButton.Click += CallButton_Click;
        }

        protected virtual void DetectControls()
        {
            this.phoneNumberText = this.FindViewById<EditText>(Resource.Id.PhoneNumberText);
            this.translateButton = this.FindViewById<Button>(Resource.Id.TarnslateButton);
            this.callButton = this.FindViewById<Button>(Resource.Id.CallButton);
        }

        private void TranslateButton_Click(object sender, EventArgs e)
        {
            this.translatedNumber = Core.PhonewordTranslator.ToNumber(this.phoneNumberText.Text);

            if (string.IsNullOrWhiteSpace(translatedNumber))
            {
                this.callButton.Text = "Call";
                this.callButton.Enabled = false;
            }
            else
            {
                this.callButton.Text = "Call " + translatedNumber;
                this.callButton.Enabled = true;
            }
        }

        private void CallButton_Click(object sender, EventArgs e)
        {
            var callDialog = new AlertDialog.Builder(this);
            callDialog.SetMessage("Call" + this.translatedNumber);
            callDialog.SetNeutralButton("Call", delegate {
                var callIntent = new Intent(Intent.ActionCall);
                callIntent.SetData(Android.Net.Uri.Parse("tel:" + this.translatedNumber));
                StartActivity(callIntent);
            });
            callDialog.SetNegativeButton("Cancel", delegate { });

            callDialog.Show();
        }
    }
}

