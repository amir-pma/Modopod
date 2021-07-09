using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class Product
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public long ProductId { get; set; }
        public string Name { get; set; }
        public string DesignPath { get; set; }
        public string DesignAddress { get; set; }
        public int PictureWidth { get; set; }
        public int PictureHeight { get; set; }
        public string EditOptionsListString { get; set; }
        public string SpecificColorsListString { get; set; }
        public string TagsListString { get; set; }
        public string CategoriesListString { get; set; }
        public int SelectedVariant { get; set; }
        public string TemplateBackgroundColour { get; set; }
        public string TemplateImageUrl { get; set; }
        public string TemplateImageDataUrl { get; set; }
        public int TemplateWidth { get; set; }
        public int TemplateHeight { get; set; }
        public int TemplatePrintAreaWidth { get; set; }
        public int TemplatePrintAreaHeight { get; set; }
        public int TemplatePrintAreaTop { get; set; }
        public int TemplatePrintAreaLeft { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public int Value { get; set; }
        public int PicWidth { get; set; }
        public int PicHeight { get; set; }
        public int Ratio { get; set; }
        public bool IsRepeat { get; set; }
        public string RepeatType { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDirty { get; set; }
        public string PreviewImage { get; set; }
        public int Profit { get; set; }
        public int Price { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; }
    }
}
