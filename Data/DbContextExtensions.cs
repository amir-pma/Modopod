using System;
using System.Collections.Generic;
using System.Linq;
using ECommerce.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data
{
    public static class DbContextExtensions
    {
        public static RoleManager<AppRole> RoleManager { get; set; }
        public static UserManager<AppUser> UserManager { get; set; }

        public static void EnsureSeeded(this EcommerceContext context)
        {
            AddRoles(context);
            AddUsers(context);
            //AddProducts(context);
        }
        private static void AddRoles(EcommerceContext context)
        {
            if (RoleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult() == false)
            {
                RoleManager.CreateAsync(new AppRole("Admin")).GetAwaiter().GetResult();
            }

            if (RoleManager.RoleExistsAsync("Customer").GetAwaiter().GetResult() == false)
            {
                RoleManager.CreateAsync(new AppRole("Customer")).GetAwaiter().GetResult();
            }
        }

        private static void AddUsers(EcommerceContext context)
        {
            if (UserManager.FindByEmailAsync("amir.pma1378@gmail.com").GetAwaiter().GetResult() == null)
            {
                var user = new AppUser
                {
                    FirstName = "امیر",
                    LastName = "پورمحمدعلی",
                    UserName = "amir.pma1378@gmail.com",
                    Email = "amir.pma1378@gmail.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };

                UserManager.CreateAsync(user, "Amir123*").GetAwaiter().GetResult();
            }

            var admin = UserManager.FindByEmailAsync("amir.pma1378@gmail.com").GetAwaiter().GetResult();

            if (UserManager.IsInRoleAsync(admin, "Admin").GetAwaiter().GetResult() == false)
            {
                UserManager.AddToRoleAsync(admin, "Admin");
            }
        }

        private static void AddProducts(EcommerceContext context)
        {
            if (context.Templates.Any() == false)
            {
                var products = new List<Template>()
        {
         new Template
                  {
                    Name = "تی شرت",
                    Slug = "t-shirt",
                    IsAvailable = true,
                    UsePrintful = false,
                    PrintfulProductId = 48,
                    VariantsListString = "2811|2808|3064|3130",
                    EditOptionsListString = "allColours|specificColour|slider|centerize|replaceImage|repeat",
                    SpecificColorsListString = "white|red|green|black|blue|yellow|cyan|gray",
                    TagsListString = "1587|1137|1583|1585|1586|1086|1584|1142",
                    CategoriesListString = "226|218",
                    TemplateBackgroundColour = "white",
                    TemplateImageUrl = "/productTemplates/t-shirt.png",
                    TemplateWidth = 1000,
                    TemplateHeight = 1000,
                    TemplatePrintAreaWidth = 1000,
                    TemplatePrintAreaHeight = 1000,
                    TemplatePrintAreaTop = 0,
                    TemplatePrintAreaLeft = 0,
                    DesignPositions = null,
                    Thumbnail = "/productThumbnails/4868c075-9ea2-4137-b9a0-13c83fff6136_t-shirt.png",
                    ShortDescription = "",
                    Description = ""
                  },
         new Template
                  {
                    Name = "دامن بلند",
                    Slug = "dress",
                    IsAvailable = false,
                    UsePrintful = false,
                    PrintfulProductId = 0,
                    VariantsListString = "2811|2808|3064|3130",
                    EditOptionsListString = "allColours|specificColour|slider|centerize|replaceImage|repeat",
                    SpecificColorsListString = "white|red|green|black|blue|yellow|cyan|gray",
                    TagsListString = "",
                    CategoriesListString = "",
                    TemplateBackgroundColour = "white",
                    TemplateImageUrl = "/productTemplates/dress.png",
                    TemplateWidth = 600,
                    TemplateHeight = 544,
                    TemplatePrintAreaWidth = 600,
                    TemplatePrintAreaHeight = 544,
                    TemplatePrintAreaTop = 0,
                    TemplatePrintAreaLeft = 0,
                    DesignPositions = null,
                    Thumbnail = "/productThumbnails/4868h065-9pk2-e139-p9p0-1og83ffg5132_dress.png",
                    ShortDescription = "",
                    Description = ""
                  },
         new Template
                  {
                    Name = "دامن کوتاه",
                    Slug = "skirt",
                    IsAvailable = false,
                    UsePrintful = false,
                    PrintfulProductId = 0,
                    VariantsListString = "2811|2808|3064|3130",
                    EditOptionsListString = "allColours|specificColour|slider|centerize|replaceImage|repeat",
                    SpecificColorsListString = "white|red|green|black|blue|yellow|cyan|gray",
                    TagsListString = "",
                    CategoriesListString = "",
                    TemplateBackgroundColour = "white",
                    TemplateImageUrl = "/productTemplates/skirt.png",
                    TemplateWidth = 300,
                    TemplateHeight = 272,
                    TemplatePrintAreaWidth = 300,
                    TemplatePrintAreaHeight = 272,
                    TemplatePrintAreaTop = 0,
                    TemplatePrintAreaLeft = 0,
                    DesignPositions = null,
                    Thumbnail = "/productThumbnails/4868h065-9pk2-e139-p9p0-1oa83afg5132_skirt.png",
                    ShortDescription = "",
                    Description = ""
                  },
         new Template
                  {
                    Name = "ساک دستی",
                    Slug = "toteBag",
                    IsAvailable = true,
                    UsePrintful = false,
                    PrintfulProductId = 0,
                    VariantsListString = "2811|2808|3064|3130",
                    EditOptionsListString = "allColours|specificColour|slider|centerize|replaceImage|repeat",
                    SpecificColorsListString = "white|red|green|black|blue|yellow|cyan|gray",
                    TagsListString = "3595|3676|2695|3598|3597|3599|3678|2210|3687|3593|3594|3596|3601|3600",
                    CategoriesListString = "234|202",
                    TemplateBackgroundColour = "white",
                    TemplateImageUrl = "/productTemplates/toteBag.png",
                    TemplateWidth = 600,
                    TemplateHeight = 544,
                    TemplatePrintAreaWidth = 600,
                    TemplatePrintAreaHeight = 544,
                    TemplatePrintAreaTop = 0,
                    TemplatePrintAreaLeft = 0,
                    DesignPositions = null,
                    Thumbnail = "/productThumbnails/p868h065-9pk2-e139-o9p0-1og83ffg5132_toteBag.png",
                    ShortDescription = "",
                    Description = ""
                  },
         new Template
                  {
                    Name = "کوسن",
                    Slug = "pillow",
                    IsAvailable = true,
                    UsePrintful = false,
                    PrintfulProductId = 0,
                    VariantsListString = "2811|2808|3064|3130",
                    EditOptionsListString = "allColours|specificColour|slider|centerize|replaceImage|repeat",
                    SpecificColorsListString = "white|red|green|black|blue|yellow|cyan|gray",
                    TagsListString = "1095|1633|1098|1635|1634|247|1631|1630|1629|1632|1365|1636",
                    CategoriesListString = "165|145",
                    TemplateBackgroundColour = "white",
                    TemplateImageUrl = "/productTemplates/pillow.png",
                    TemplateWidth = 300,
                    TemplateHeight = 272,
                    TemplatePrintAreaWidth = 300,
                    TemplatePrintAreaHeight = 272,
                    TemplatePrintAreaTop = 0,
                    TemplatePrintAreaLeft = 0,
                    DesignPositions = null,
                    Thumbnail = "/productThumbnails/p898h065-9pk2-e139-o9p0-1og83dfg5132_pillow.png",
                    ShortDescription = "",
                    Description = ""
                  },
         new Template
                  {
                    Name = "پادری حمام",
                    Slug = "bathMat",
                    IsAvailable = true,
                    UsePrintful = false,
                    PrintfulProductId = 0,
                    VariantsListString = "2811|2808|3064|3130",
                    EditOptionsListString = "allColours|specificColour|slider|centerize|replaceImage|repeat",
                    SpecificColorsListString = "white|red|green|black|blue|yellow|cyan|gray",
                    TagsListString = "2646|2625|2635",
                    CategoriesListString = "190|145",
                    TemplateBackgroundColour = "white",
                    TemplateImageUrl = "/productTemplates/bathMat.png",
                    TemplateWidth = 600,
                    TemplateHeight = 544,
                    TemplatePrintAreaWidth = 600,
                    TemplatePrintAreaHeight = 544,
                    TemplatePrintAreaTop = 0,
                    TemplatePrintAreaLeft = 0,
                    DesignPositions = null,
                    Thumbnail = "/productThumbnails/4868c075-9ea2-4137-b5a0-13c73fff6136_bathMat.png",
                    ShortDescription = "",
                    Description = ""
                  },
                new Template
                  {
                    Name = "ماگ",
                    Slug = "mug_model1",
                    IsAvailable = true,
                    UsePrintful = false,
                    PrintfulProductId = 0,
                    VariantsListString = "1320|4830",
                    EditOptionsListString = "allColours|slider|centerize|replaceImage|repeat|miniPreview",
                    SpecificColorsListString = "white|red|green|black|blue|yellow|cyan|gray",
                    TagsListString = "1799|1793|1066|1787|1794|1792|1788|1791|1154|1797|1798|1789|1795|1790|1796",
                    CategoriesListString = "163|202",
                    TemplateBackgroundColour = "white",
                    TemplateImageUrl = "/productTemplates/mug_model1.png",
                    TemplateWidth = 300,
                    TemplateHeight = 272,
                    TemplatePrintAreaWidth = 300,
                    TemplatePrintAreaHeight = 272,
                    TemplatePrintAreaTop = 0,
                    TemplatePrintAreaLeft = 0,
                    DesignPositions = null,
                    Thumbnail = "/productThumbnails/4868c065-9ek2-4137-b9p0-1oc83fff6132_mug.png",
                    ShortDescription = "",
                    Description = ""
                  },
                new Template
                  {
                    Name = "ماگ 2",
                    Slug = "mug_model2",
                    IsAvailable = false,
                    UsePrintful = false,
                    PrintfulProductId = 0,
                    VariantsListString = "1320|4830",
                    EditOptionsListString = "allColours|slider|centerize|replaceImage|repeat|miniPreview",
                    SpecificColorsListString = "white|red|green|black|blue|yellow|cyan|gray",
                    TagsListString = "",
                    CategoriesListString = "",
                    TemplateBackgroundColour = "white",
                    TemplateImageUrl = "/productTemplates/mug_model2.png",
                    TemplateWidth = 300,
                    TemplateHeight = 272,
                    TemplatePrintAreaWidth = 300,
                    TemplatePrintAreaHeight = 272,
                    TemplatePrintAreaTop = 0,
                    TemplatePrintAreaLeft = 0,
                    DesignPositions = null,
                    Thumbnail = "/productThumbnails/4868h065-9pk2-5137-b9p0-1oc83fff6132_mug_model2.png",
                    ShortDescription = "Acme TNT 4 Android smartphone with true edge to edge display",
                    Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Perferendis tempora ad cum laudantium, omnis fugit amet iure animi corporis labore repellat magnam perspiciatis explicabo maiores fuga provident a obcaecati tenetur nostrum, quidem quod dignissimos, voluptatem quasi? Nisi quaerat, fugit voluptas ducimus facilis impedit quod dicta, laborum sint iure nihil veniam aspernatur delectus officia culpa, at cupiditate? Totam minima ut deleniti laboriosam dolores cumque in, nesciunt optio! Quod recusandae voluptate facere pariatur soluta vel corrupti tenetur aut maiores, cumque mollitia fugiat laudantium error odit voluptas nobis laboriosam quo, rem deleniti? Iste quidem amet perferendis sed iusto tempora modi illo tempore quibusdam laborum? Dicta aliquam libero, facere, maxime corporis qui officiis explicabo aspernatur non consequatur mollitia iure magnam odit enim. Eligendi suscipit, optio officiis repellat eos quis iure? Omnis, error aliquid quibusdam iste amet nihil nisi cumque magni sequi enim illo autem nesciunt optio accusantium animi commodi tenetur neque eum vitae est."
                  },
                new Template
                  {
                    Name = "قمقمه",
                    Slug = "bottle",
                    IsAvailable = false,
                    UsePrintful = false,
                    PrintfulProductId = 0,
                    VariantsListString = "1320|4830",
                    EditOptionsListString = "allColours|slider|centerize|replaceImage|repeat|miniPreview",
                    SpecificColorsListString = "white|red|green|black|blue|yellow|cyan|gray",
                    TagsListString = "",
                    CategoriesListString = "",
                    TemplateBackgroundColour = "white",
                    TemplateImageUrl = "/productTemplates/bottle.png",
                    TemplateWidth = 600,
                    TemplateHeight = 544,
                    TemplatePrintAreaWidth = 600,
                    TemplatePrintAreaHeight = 544,
                    TemplatePrintAreaTop = 0,
                    TemplatePrintAreaLeft = 0,
                    DesignPositions = null,
                    Thumbnail = "/productThumbnails/4868h065-9pk2-5137-p9p0-1oc83fff5132_bottle.png",
                    ShortDescription = "",
                    Description = ""
                  },
                new Template
                  {
                    Name = "ساعت",
                    Slug = "clock",
                    IsAvailable = false,
                    UsePrintful = false,
                    PrintfulProductId = 0,
                    VariantsListString = "1320|4830",
                    EditOptionsListString = "allColours|slider|centerize|replaceImage|repeat|miniPreview",
                    SpecificColorsListString = "white|red|green|black|blue|yellow|cyan|gray",
                    TagsListString = "",
                    CategoriesListString = "",
                    TemplateBackgroundColour = "white",
                    TemplateImageUrl = "/productTemplates/clock.png",
                    TemplateWidth = 600,
                    TemplateHeight = 544,
                    TemplatePrintAreaWidth = 600,
                    TemplatePrintAreaHeight = 544,
                    TemplatePrintAreaTop = 0,
                    TemplatePrintAreaLeft = 0,
                    DesignPositions = null,
                    Thumbnail = "/productThumbnails/48p8h065-9pk2-5177-p9p7-1oc83yff5132_clock.png",
                    ShortDescription = "",
                    Description = ""
                  },
                new Template
                  {
                    Name = "قاب گوشی",
                    Slug = "phoneCover",
                    IsAvailable = false,
                    UsePrintful = false,
                    PrintfulProductId = 0,
                    VariantsListString = "1320|4830",
                    EditOptionsListString = "allColours|slider|centerize|replaceImage|repeat|miniPreview",
                    SpecificColorsListString = "white|red|green|black|blue|yellow|cyan|gray",
                    TagsListString = "",
                    CategoriesListString = "",
                    TemplateBackgroundColour = "white",
                    TemplateImageUrl = "/productTemplates/phoneCover.png",
                    TemplateWidth = 300,
                    TemplateHeight = 272,
                    TemplatePrintAreaWidth = 300,
                    TemplatePrintAreaHeight = 272,
                    TemplatePrintAreaTop = 0,
                    TemplatePrintAreaLeft = 0,
                    DesignPositions = null,
                    Thumbnail = "/productThumbnails/4868c075-9ea2-41o7-b5a0-13cg3fff6136_b_phoneCover.png",
                    ShortDescription = "",
                    Description = ""
                  },
                new Template
                  {
                    Name = "پد موس",
                    Slug = "mousePad",
                    IsAvailable = false,
                    UsePrintful = false,
                    PrintfulProductId = 0,
                    VariantsListString = "1320|4830",
                    EditOptionsListString = "allColours|slider|centerize|replaceImage|repeat|miniPreview",
                    SpecificColorsListString = "white|red|green|black|blue|yellow|cyan|gray",
                    TagsListString = "",
                    CategoriesListString = "",
                    TemplateBackgroundColour = "white",
                    TemplateImageUrl = "/productTemplates/mousePad.png",
                    TemplateWidth = 300,
                    TemplateHeight = 272,
                    TemplatePrintAreaWidth = 300,
                    TemplatePrintAreaHeight = 272,
                    TemplatePrintAreaTop = 0,
                    TemplatePrintAreaLeft = 0,
                    DesignPositions = null,
                    Thumbnail = "/productThumbnails/4868h065-9pk2-5137-p9p0-1og83ffg5132_mousePad.png",
                    ShortDescription = "",
                    Description = ""
                  },
                new Template
                  {
                    Name = "پرده",
                    Slug = "curtain",
                    IsAvailable = false,
                    UsePrintful = false,
                    PrintfulProductId = 0,
                    VariantsListString = "",
                    EditOptionsListString = "allColours|slider|centerize|replaceImage|repeat",
                    SpecificColorsListString = "white|red|green|blue|yellow|cyan",
                    TagsListString = "",
                    CategoriesListString = "",
                    TemplateBackgroundColour = "white",
                    TemplateImageUrl = "/productTemplates/curtain.png",
                    TemplateWidth = 600,
                    TemplateHeight = 544,
                    TemplatePrintAreaWidth = 600,
                    TemplatePrintAreaHeight = 544,
                    TemplatePrintAreaTop = 0,
                    TemplatePrintAreaLeft = 0,
                    DesignPositions = null,
                    Thumbnail = "/productThumbnails/4868c665-8uk2-48937-b9o0-1oc8i3ff6172_curtain.png",
                    ShortDescription = "",
                    Description = ""
                  }
        };

                context.Templates.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}