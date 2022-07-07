using LivroManage.Domain.Models;
using System.Collections.Generic;

namespace LivroManage.Application.ViewModels
{
    public class CompanieVM : Companie
    {
        public CompanieVM(Companie companie)
        {
            CompanieId = companie.CompanieId;
            Name = companie.Name;
            Photo = companie.Photo;
            TelefonNo = companie.TelefonNo;
            Opening = companie.Opening;
            TemporaryClosed = companie.TemporaryClosed;
            IsActive = companie.IsActive;
            VisibleInApp = companie.VisibleInApp;
            TipCompanieRefId = companie.TipCompanieRefId;
        }
        public IEnumerable<TransportFee> TransportFees { get; set; }
    }
}
