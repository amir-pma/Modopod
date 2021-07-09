using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Data.Entities
{
    public class Template
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public bool? UsePrintful { get; set; }
        public int? PrintfulProductId { get; set; }
        public string VariantsListString { get; set; }
        public string EditOptionsListString { get; set; }
        public string SpecificColorsListString { get; set; }
        public string SpecificMockupListString { get; set; }
        public string MockupParametersString { get; set; }
        public string TagsListString { get; set; }
        public string CategoriesListString { get; set; }
        public string TemplateBackgroundColour { get; set; }
        public string TemplateImageUrl { get; set; }
        public int? TemplateWidth { get; set; }
        public int? TemplateHeight { get; set; }
        public int? TemplatePrintAreaWidth { get; set; }
        public int? TemplatePrintAreaHeight { get; set; }
        public int? TemplatePrintAreaTop { get; set; }
        public int? TemplatePrintAreaLeft { get; set; }
        public string Thumbnail { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public bool? IsAvailable { get; set; }

        public List<DesignPosition> DesignPositions { get; set; }
    }
}