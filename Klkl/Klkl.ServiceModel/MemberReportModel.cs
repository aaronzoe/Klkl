using Domain;
using ServiceStack;
using ServiceStack.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klkl.ServiceModel
{
  
    [Route("/memberreport/hp", "Post,GET")]
    public class MemberReportHp
    {
    
        public DateTime Dt1 { get; set; }
        public DateTime Dt2 { get; set; }

        public string Khjl { get; set; }
        public string Khmc { get; set; }
    }
    public class MemberReportHpIndexResponse
    {
        public IList<Customer> Customers { get; set; }

        public IList<UserAuth> Users { get; set; }
    }
    public class MemberReportHpResponse
    {
        public string Name { get; set; }
        /// <summary>
        /// 含片订货数量(箱)
        /// </summary>
        public decimal Dhsl { get; set; }
      
        /// <summary>
        /// 含片搭赠(箱)
        /// </summary>

        public decimal Dzsl { get; set; }
        /// <summary>
        ///含片打款金额(元)
        /// </summary>

        public decimal Dkje { get; set; }
        /// <summary>
        /// 含片搭赠金额(元)
        /// </summary>

        public decimal Dzje { get; set; }
        /// <summary>
        /// 含片发货总金额
        /// </summary>

        public decimal Fhje { get; set; }

        public decimal Fyje { get
            {
                if (_fyje>0)
                {
                    return _fyje;
                }
                return Math.Round(Dzje * (decimal)0.67,2);
            }
            set { _fyje = value; }
        }
        public decimal Fb
        {
            get
            {
                if (Fhje-Dzje==0)
                {
                    return 0;
                }
                return Math.Round(Dzje * (decimal)0.67/(Fhje- Dzje), 2);
            }
        }
        public decimal Yj
        {
            get
            {
                if (_yj>0)
                {
                    return _yj;
                } return Math.Max(0, Math.Round(Dkje - Fhje * (decimal)0.67, 2)); }
            set { _yj = value; }
        }
        public decimal Yjbl
        {
            get
            {
                if (Dkje==0)
                {
                    return 0;
                } return Math.Round(Yj / Dkje,2); } }
    

        public decimal _fyje { get; set; }
     
        public decimal _yj { get; set; }
      
    }

    [Route("/memberreport/gd", "Post,GET")]
    public class MemberReportGd
    {
        public IList<int> Types { get; set; }
        public DateTime Dt1 { get; set; }
        public DateTime Dt2 { get; set; }

        public string Khjl { get; set; }
        public string Khmc { get; set; }
    }


    public class MemberReportGdIndexResponse
    {
        public IList<Customer> Customers { get; set; }

        public IList<UserAuth> Users { get; set; }
    }
    public class MemberReportGdResponse
    {
        public string Name { get; set; }
        public string Category { get; set; }
        /// <summary>
        /// 含片订货数量(箱)
        /// </summary>
        public decimal Dhsl { get; set; }

        /// <summary>
        /// 含片搭赠(箱)
        /// </summary>

        public decimal Dzsl { get; set; }
        public decimal Price { get; set; }
        /// <summary>
        ///含片打款金额(元)
        /// </summary>

        public decimal Dkje { get; set; }
        /// <summary>
        /// 含片搭赠金额(元)
        /// </summary>

        public decimal Dzje { get; set; }
        /// <summary>
        /// 含片发货总金额
        /// </summary>

        public decimal Fhje { get; set; }
        public decimal Szhpfhje { get; set; }

     
        public decimal Yj
        {
            get
            {
                if (_yj > 0)
                {
                    return _yj;
                }
                return Math.Max(0, Math.Round(Dkje*(decimal)0.03, 2));
            }
            set { _yj = value; }
        }
      



        public decimal _yj { get; set; }

    }
}
