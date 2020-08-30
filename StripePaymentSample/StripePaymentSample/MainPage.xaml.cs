using Stripe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StripePaymentSample
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        public void PayViaStripe()
        {
            StripeConfiguration.ApiKey = "sk_test_LJ2dJvulsi1f4rsGrqLHCKGC*****";

            string cardno = cardNo.Text;
            string expMonth = expireMonth.Text;
            string expYear = expireYear.Text;
            string cardCvv = cvv.Text;

            // Step 1: create card option

            CreditCardOptions stripeOption = new CreditCardOptions();
            stripeOption.Number = cardno;
            stripeOption.ExpYear = Convert.ToInt64(expYear);
            stripeOption.ExpMonth = Convert.ToInt64(expMonth);
            stripeOption.Cvc = cardCvv;

            // step 2: Assign card to token object
            TokenCreateOptions stripeCard = new TokenCreateOptions();
            stripeCard.Card = stripeOption;

            TokenService service = new TokenService();
            Token newToken = service.Create(stripeCard);

            // step 3: assign the token to the source
            var option = new SourceCreateOptions
            {
                Type = SourceType.Card,
                Currency = "inr",
                Token = newToken.Id
            };

            var sourceService = new SourceService();
            Source source = sourceService.Create(option);

            // step 4: create customer
            CustomerCreateOptions customer = new CustomerCreateOptions
            {
                Name = "SP Tutorial",
                Email = "spaltutorials@gmail.com",
                Description = "Paying 10 Rs",
                Address = new AddressOptions { City = "Kolkata", Country = "India", Line1 = "Sample Address", Line2 = "Sample Address 2", PostalCode = "700030", State = "WB" }
            };

            var customerService = new CustomerService();
            var cust = customerService.Create(customer);

            // step 5: charge option
            var chargeoption = new ChargeCreateOptions
            {
                Amount = 45000,
                Currency = "INR",
                ReceiptEmail = "spaltutorials@gmail.com",
                Customer = cust.Id,
                Source = source.Id
            };

            // step 6: charge the customer
            var chargeService = new ChargeService();
            Charge charge = chargeService.Create(chargeoption);
            if(charge.Status == "succeeded")
            {
                // success
            }
            else
            {
                // failed
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            PayViaStripe();
        }
    }
}
