namespace Ext_Dynamic_Form.Models
{
    public class DynamicFormData
    {
        public Int64 ID { get; set; }
        public Int64 ActivityId { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
