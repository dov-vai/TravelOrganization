using Microsoft.VisualBasic;

namespace TravelOrganization.Data.Models.Card
{
    public class Card
    {
        private int      number     { get; set; }
        private int      ccv        { get; set; }
        private DateTime expiryDate { get; set; }
        private string   ownerName  { get; set; }
        public Card(int num, int ccv, DateTime expdate, string ownname)
        {
            number = num;
            this.ccv = ccv;
            expiryDate = expdate;
            ownerName = ownname;
        }
    }
}
