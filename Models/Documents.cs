using Soneta.Business;
using Soneta.Types;
using Soneta.Handel;
using Soneta.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using WebApplicationYes.Models.AdditionalClasses;


namespace WebApplicationYes.Models
{
    public class Documents : ApiRows
    {
        public Documents() { }

        protected override object GetFromEnovaAction(string[] getParam)
        {
            var documentNumber = getParam == null || getParam.Length == 0 ? "" : getParam[0];

            if (!string.IsNullOrEmpty(documentNumber))
            {
                using (Session sesja = loginEnova.Login.CreateSession(true, false))
                {
                    var hM = HandelModule.GetInstance(sesja);
                    var document = hM.DokHandlowe.NumerWgNumeruDokumentu[documentNumber];

                    if (document == null || document.Kategoria != KategoriaHandlowa.ZamówienieOdbiorcy)
                        return null;

                    return ParseToDocument(document);
                }
            }
            else
            {
                var dateString = getParam[2];
                if (string.IsNullOrEmpty(dateString))
                    return null;

                using (Session sesja = loginEnova.Login.CreateSession(true, false))
                {
                    var hM = HandelModule.GetInstance(sesja);
                    var st = hM.DokHandlowe.WgDaty[Date.Parse(dateString)];
                    st = st[new FieldCondition.Equal(nameof(DokumentHandlowy.Kategoria), KategoriaHandlowa.ZamówienieOdbiorcy)];
                    var documents = st.ToArray<DokumentHandlowy>();
                    var parsedDocs = new List<Document>();
                    foreach (var d in documents)
                        parsedDocs.Add(ParseToDocument(d));
                    return parsedDocs;
                }
            }
        }

        protected override object CreateInEnovaAction(object row)
        {
            throw new NotImplementedException();
        }

        protected override object DeleteFromEnovaAction(object row)
        {
            throw new NotImplementedException();
        }

        private Document ParseToDocument(DokumentHandlowy documentRow)
        {
            var document = new Document();
            document.Date = new DateTime(documentRow.Data.Year, documentRow.Data.Month, documentRow.Data.Day, documentRow.Czas.Hours, documentRow.Czas.Minutes, 0);
            document.EnovaID = documentRow.ID;
            document.ForeignNumber = documentRow.Obcy.Numer;
            document.Number = documentRow.Numer.NumerPelny;
            document.Status = documentRow.Features.Definitions.Contains(EnovaConfig.FeatureDokHanStatus) ? documentRow.Features.GetString(EnovaConfig.FeatureDokHanStatus) : "Brak cechy ze statusem";
            document.NetValue = documentRow.Suma.NettoCy.Value;
            document.GrossValue = documentRow.Suma.BruttoCy.Value;
            document.Currency = documentRow.Suma.BruttoCy.Symbol;
            document.Description = documentRow.Opis;

            document.Customer = new Customer
            {
                EnovaID = documentRow.Kontrahent.ID,
                Code = documentRow.Kontrahent.Kod,
                Name = documentRow.Kontrahent.Nazwa
            };

            document.DocItems = new List<DocumentItem>();
            foreach (var poz in documentRow.Pozycje)
            {
                var di = new DocumentItem
                {
                    EnovaID = poz.ID,
                    ProductEnovaID = poz.Towar.ID,
                    ProductCode = poz.Towar.Kod,
                    ProductName = poz.Towar.Nazwa,
                    Qty = poz.Ilosc.Value,
                    Unit = poz.Ilosc.Symbol,
                    Tax = (double)poz.DefinicjaStawki.Stawka.Procent,
                    PriceType = documentRow.LiczonaOd == SposobLiczeniaVAT.OdBrutto ? "gross" : "netto"
                };

                try
                {
                    var type = !string.IsNullOrEmpty(EnovaConfig.PodrzednyType) ? EnovaConfig.PodrzednyType : "Faktura";
                    di.RemainingQty = poz.Podrzędne[type]?.PozostałoIlość.Value;
                }
                catch { }
                try
                {
                    di.Description = poz.Features.GetString(EnovaConfig.FeaturePozDescription);
                }
                catch { }
                try
                {
                    di.Type = poz.Features.GetString(EnovaConfig.FeaturePozType);
                }
                catch { }
                document.DocItems.Add(di);
            }

            return document;
        }

    }

}