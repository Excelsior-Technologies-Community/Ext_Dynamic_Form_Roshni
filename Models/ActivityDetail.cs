namespace Ext_Dynamic_Form.Models
{
    public class ActivityDetail
    {
        public Int64 ID { get; set; }
        public Int64 ActivityDeatailId { get; set; }
        public Int64 ActivityId { get; set; }
        public String Title { get; set; }
        public Int64 ActionTypeId { get; set; }
        public Int64 PageMasterId { get; set; }
        public List<FieldDetail> Details { get; set; } = new List<FieldDetail>();
    }

    public class FieldDetail
    {
        public string Title { get; set; }
        public long ActionTypeId { get; set; }
    }
}
