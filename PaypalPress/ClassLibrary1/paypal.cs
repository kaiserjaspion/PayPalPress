using System;
using PayPal.Api;
using Models.PaypalPress;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace PaypalPress
{
    
    public class paypal
    {
        #region Paypal Inicio
        private string _endereçoRetorno { get; set; }
        private string _endereçoCancelamento { get; set; }
        public APIContext APIContext { get; private set; }

        paypal()
        {
            this._endereçoRetorno = "http://mysite.com/return";
            this._endereçoCancelamento = "http://mysite.com/cancel";
        }
        public APIContext Paypal()
        {
            var config = ConfigManager.Instance.GetProperties();
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new APIContext(accessToken);
            return apiContext;
        }
        #endregion

        #region Assinatura
        public void BillingAgreement(PaypalBillinAgreement assinante)
        {
            var assinantura = PayPal.Api.Agreement.Create(Paypal(), new Agreement {
                name = assinante.nome,
                description = assinante.description,
                start_date = DateTime.UtcNow.AddMinutes(5).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                plan = new Plan
                {
                    id = assinante.plano
                },
                payer = new Payer
                {
                    payment_method = "paypal",
                    payer_info = new PayerInfo
                    {
                        email = assinante.email
                    }
                }
            });
        }

        public void executeBillingAgreement (string token)
        {
            APIContext = Paypal();
            var request = (HttpWebRequest)WebRequest.Create("https://api.sandbox.paypal.com/v1/payments/billing-agreements/" + token+ "/agreement-execute");
            request.Method = "POST";

            request.ContentType = "application/json";
            request.Headers.Add(HttpRequestHeader.Authorization, APIContext.AccessToken);

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            var jsondeserialized = JsonConvert.DeserializeObject<dynamic>(responseString);
        }

        public void salvar(PaypalBillinAgreement assinatura)
        {
            ContextPaypal contextPaypal = new ContextPaypal();
            contextPaypal.Paypal.

        }
        #endregion

        #region Pagamento
        public void Payment(PayPalPayment dados)
        {
            var pagamento = PayPal.Api.Payment.Create(Paypal(), new Payment
            {
                intent = "sale",
                payer = new Payer
                {
                    payment_method = "paypal"
                },
                transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        description = dados.Descricao,
                        invoice_number = dados.Id.ToString(),
                        amount = new Amount
                        {
                            currency = dados.Moeda,
                            total = dados.Valor.ToString().Replace(",","."),
                            details = new Details
                            {
                                tax = "0",
                                shipping = "0",
                                subtotal =  dados.Valor.ToString().Replace(",",".")
                            }
                        },
                        item_list = new ItemList
                        {
                            items = new List<Item>
                            {
                                new Item
                                {
                                    name = dados.Nome,
                                    currency = dados.Moeda,
                                    price = dados.Valor.ToString().Replace(",","."),
                                    quantity = "1",
                                    sku = "sku" + dados.Id.ToString()
                                }
                            }
                        }
                    }
                },
                redirect_urls = new RedirectUrls
                {
                    return_url = this._endereçoRetorno, //completar conta 
                    cancel_url = this._endereçoCancelamento  //Dont' Know !!!
                }
            });
        }
        #endregion
    }
}
