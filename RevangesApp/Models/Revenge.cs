using System.ComponentModel.DataAnnotations;

namespace RevengeApp.Models
{
    public class Revenge
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "שם הנקמה הוא שדה חובה")]
        [Display(Name = "שם הנקמה")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "תיאור הנקמה הוא שדה חובה")]
        [Display(Name = "תיאור מפורט")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "יש לציין על מי הנקמה")]
        [Display(Name = "הקורבן")]
        public string Target { get; set; } = string.Empty;

        [Required]
        [Display(Name = "תאריך הנקמה")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "סטטוס")]
        public RevengeStatus Status { get; set; } = RevengeStatus.Planned;

        [Required]
        [Range(1, 5, ErrorMessage = "רמת הדרמה חייבת להיות בין 1 ל-5")]
        [Display(Name = "רמת דרמתיות")]
        public int DramaLevel { get; set; } = 1;

        [Display(Name = "קטגוריה")]
        public RevengeCategory Category { get; set; } = RevengeCategory.Ex;

        [Display(Name = "הערות נוספות")]
        public string? Notes { get; set; }

        [Display(Name = "תאריך ביצוע")]
        public DateTime? CompletedDate { get; set; }
    }

    public enum RevengeStatus
    {
        [Display(Name = "מתוכננת")]
        Planned,
        [Display(Name = "בביצוע")]
        InProgress,
        [Display(Name = "הושלמה")]
        Completed,
        [Display(Name = "בוטלה")]
        Cancelled
    }

    public enum RevengeCategory
    {
        [Display(Name = "נקמת אקס")]
        Ex,
        [Display(Name = "נקמת שכן")]
        Neighbor,
        [Display(Name = "נקמת לקוחה מעצבנת")]
        AnnoyingCustomer,
        [Display(Name = "נקמת עבודה")]
        Work,
        [Display(Name = "נקמת משפחה")]
        Family,
        [Display(Name = "נקמת חברה")]
        Friend,
        [Display(Name = "אחר")]
        Other
    }
}
