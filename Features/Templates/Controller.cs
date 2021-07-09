using ECommerce.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Data.Entities;
using ECommerce.Features.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ECommerce.Features.Images;

namespace ECommerce.Features.Templates
{
    [Route("api/[controller]")]
    public class TemplatesController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly EcommerceContext _db;

        public TemplatesController(EcommerceContext db, IHostingEnvironment hostingEnvironment)
        {
            _db = db;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Find(string q)
        {
            var Query = $"%{q?.ToLower()}%";
            var templates = await _db.Templates
              .Where(x =>
                string.IsNullOrEmpty(q) ||
                (
                  EF.Functions.Like(x.Name.ToLower(), Query) ||
                  EF.Functions.Like(x.ShortDescription.ToLower(), Query) ||
                  EF.Functions.Like(x.Description.ToLower(), Query)
                )
              )
              .Select(x => new TemplateListViewModel
              {
                  Id = x.Id,
                  Slug = x.Slug,
                  Name = x.Name,
                  ShortDescription = x.ShortDescription,
                  Thumbnail = x.Thumbnail,
                  UsePrintful = x.UsePrintful,
                  PrintfulProductId = x.PrintfulProductId,
                  VariantsListString = x.VariantsListString,
                  EditOptionsListString = x.EditOptionsListString,
                  SpecificColorsListString = x.SpecificColorsListString,
                  SpecificMockupListString = x.SpecificMockupListString,
                  TagsListString = x.TagsListString,
                  CategoriesListString = x.CategoriesListString,
                  TemplateBackgroundColour = x.TemplateBackgroundColour,
                  TemplateImageUrl = x.TemplateImageUrl,
                  TemplateWidth = x.TemplateWidth,
                  TemplateHeight = x.TemplateHeight,
                  TemplatePrintAreaWidth = x.TemplatePrintAreaWidth,
                  TemplatePrintAreaHeight = x.TemplatePrintAreaHeight,
                  TemplatePrintAreaTop = x.TemplatePrintAreaTop,
                  TemplatePrintAreaLeft = x.TemplatePrintAreaLeft,
                  DesignPositions = x.DesignPositions.Select(v => new DesignPositionViewModel
                  {
                      LeftDistance = v.LeftDistance,
                      TopDistance = v.TopDistance,
                      DesignId = v.Design.Id
                  }),
                  Description = x.Description,
                  IsAvailable = x.IsAvailable
              })
              .ToListAsync();

            return Ok(templates);
        }


        //[Authorize]
        [HttpPost("mockup")]
        public async Task<IActionResult> CreateMockup(string uid, long id, [FromForm] ImagesPayloadViewModel model)
        {
            var mockupTemplate = _db.Templates.FirstOrDefault(t => t.Id == id);
            if(mockupTemplate == null)
            {
                return NotFound();
            }

            string fileName = $"{Guid.NewGuid().ToString().Replace("-", "")}.png";
            var file = model.Files.Files[0];
            if (file != null && file.Length != 0)
            {
                string filePath = Path.Combine(hostingEnvironment.WebRootPath, uid, fileName);
                file.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            int exitCode;
            ProcessStartInfo processInfo;
            Process process;
            //var uid = Regex.Replace(Convert.ToBase64String(Guid.Parse(guid).ToByteArray()), "[/+=]", "");
            //Directory.CreateDirectory(Path.Combine("wwwroot", "usersUploadedDesigns", uid));

            //processInfo = new ProcessStartInfo(@"E:\Work\Projects\Modopod\ECommerce\render.bat", uid);
            //processInfo = new ProcessStartInfo(@"natronrenderer", $@"uv-wrapping.ntp -i r1 ./wwwroot/{uid}/0.jpg -i r2 ./wwwroot/{uid}/0.jpg -i r3 ./wwwroot/{uid}/0.jpg -i r4 ./wwwroot/{uid}/0.jpg -i r5 ./wwwroot/{uid}/0.jpg -i r6 ./wwwroot/{uid}/0.jpg -i r7 ./wwwroot/{uid}/0.jpg -i r8 ./wwwroot/{uid}/0.jpg -i r9 ./wwwroot/{uid}/0.jpg -i r10 ./wwwroot/{uid}/0.jpg -i r11 ./wwwroot/{uid}/0.jpg -i r12 ./wwwroot/{uid}/0.jpg -i r13 ./wwwroot/{uid}/0.jpg -i r14 ./wwwroot/{uid}/0.jpg -i r15 ./wwwroot/{uid}/0.jpg -i r16 ./wwwroot/{uid}/0.jpg -i r17 ./wwwroot/{uid}/0.jpg -i r18 ./wwwroot/{uid}/0.jpg -i r19 ./wwwroot/{uid}/0.jpg -i r20 ./wwwroot/{uid}/0.jpg -i r21 ./wwwroot/{uid}/0.jpg -i r22 ./wwwroot/{uid}/0.jpg -i r23 ./wwwroot/{uid}/0.jpg -i r24 ./wwwroot/{uid}/0.jpg -i r25 ./wwwroot/{uid}/0.jpg -i r26 ./wwwroot/{uid}/0.jpg -i r27 ./wwwroot/{uid}/0.jpg -i r28 ./wwwroot/{uid}/0.jpg -i r29 ./wwwroot/{uid}/0.jpg -i r30 ./wwwroot/{uid}/0.jpg -i r31 ./wwwroot/{uid}/0.jpg -i r32 ./wwwroot/{uid}/0.jpg -i r33 ./wwwroot/{uid}/0.jpg -i r34 ./wwwroot/{uid}/0.jpg -i r35 ./wwwroot/{uid}/0.jpg -i r36 ./wwwroot/{uid}/0.jpg -i r37 ./wwwroot/{uid}/0.jpg -i r38 ./wwwroot/{uid}/0.jpg -i r39 ./wwwroot/{uid}/0.jpg -i r40 ./wwwroot/{uid}/0.jpg -i r41 ./wwwroot/{uid}/0.jpg -i r42 ./wwwroot/{uid}/0.jpg -i r43 ./wwwroot/{uid}/0.jpg -i r44 ./wwwroot/{uid}/0.jpg -i r45 ./wwwroot/{uid}/0.jpg -i r46 ./wwwroot/{uid}/0.jpg -i r47 ./wwwroot/{uid}/0.jpg -i r48 ./wwwroot/{uid}/0.jpg -i r49 ./wwwroot/{uid}/0.jpg -i r50 ./wwwroot/{uid}/0.jpg -i r51 ./wwwroot/{uid}/0.jpg -i r52 ./wwwroot/{uid}/0.jpg -i r53 ./wwwroot/{uid}/0.jpg -i r54 ./wwwroot/{uid}/0.jpg -i r55 ./wwwroot/{uid}/0.jpg -i r56 ./wwwroot/{uid}/0.jpg -i r57 ./wwwroot/{uid}/0.jpg -i r58 ./wwwroot/{uid}/0.jpg -i r59 ./wwwroot/{uid}/0.jpg -i r60 ./wwwroot/{uid}/0.jpg -i r61 ./wwwroot/{uid}/0.jpg -i r62 ./wwwroot/{uid}/0.jpg -i r63 ./wwwroot/{uid}/0.jpg -i r64 ./wwwroot/{uid}/0.jpg -i r65 ./wwwroot/{uid}/0.jpg -i r66 ./wwwroot/{uid}/0.jpg -i r67 ./wwwroot/{uid}/0.jpg -i r68 ./wwwroot/{uid}/0.jpg -i r69 ./wwwroot/{uid}/0.jpg -i r70 ./wwwroot/{uid}/0.jpg -i r71 ./wwwroot/{uid}/0.jpg -i r72 ./wwwroot/{uid}/0.jpg -i r73 ./wwwroot/{uid}/0.jpg -i r74 ./wwwroot/{uid}/0.jpg -i r75 ./wwwroot/{uid}/0.jpg -i r76 ./wwwroot/{uid}/0.jpg -i r77 ./wwwroot/{uid}/0.jpg -i r78 ./wwwroot/{uid}/0.jpg -i r79 ./wwwroot/{uid}/0.jpg -i r80 ./wwwroot/{uid}/0.jpg -i r81 ./wwwroot/{uid}/0.jpg -i r82 ./wwwroot/{uid}/0.jpg -w w1 ./wwwroot/{uid}/bed-collection-c0.jpg -w w2 ./wwwroot/{uid}/bed-collection-c1.jpg -w w3 ./wwwroot/{uid}/bed-collection-c2.jpg -w w4 ./wwwroot/{uid}/bed-collection-c3.jpg -w w5 ./wwwroot/{uid}/bed-collection-c4.jpg -w w6 ./wwwroot/{uid}/lampshade-c0.jpg -w w7 ./wwwroot/{uid}/lampshade-c1.jpg -w w8 ./wwwroot/{uid}/lampshade-c2.jpg -w w9 ./wwwroot/{uid}/pillow-c0.jpg -w w10 ./wwwroot/{uid}/pillow-c1.jpg -w w11 ./wwwroot/{uid}/pillow-c2.jpg -w w12 ./wwwroot/{uid}/carpet-c0.jpg -w w13 ./wwwroot/{uid}/carpet-c2.jpg -w w14 ./wwwroot/{uid}/carpet-c1.jpg -w w15 ./wwwroot/{uid}/window-shade-c3.jpg -w w16 ./wwwroot/{uid}/window-shade-c2.jpg -w w17 ./wwwroot/{uid}/window-shade-c1.jpg -w w18 ./wwwroot/{uid}/window-shade-c0.jpg -w w19 ./wwwroot/{uid}/curtain-c3.jpg -w w20 ./wwwroot/{uid}/curtain-c2.jpg -w w21 ./wwwroot/{uid}/curtain-c1.jpg -w w22 ./wwwroot/{uid}/lustre-c1.jpg -w w23 ./wwwroot/{uid}/lustre-c2.jpg -w w24 ./wwwroot/{uid}/lustre-c0.jpg -w w25 ./wwwroot/{uid}/wallpaper-c2.jpg -w w26 ./wwwroot/{uid}/wallpaper-c1.jpg -w w27 ./wwwroot/{uid}/zebra-curtain-c2.jpg -w w28 ./wwwroot/{uid}/zebra-curtain-c1.jpg -w w29 ./wwwroot/{uid}/zebra-curtain-c3.jpg -w w30 ./wwwroot/{uid}/baby-bodysuit.jpg -w w31 ./wwwroot/{uid}/1072-F1.jpg -w w32 ./wwwroot/{uid}/2001-F1.jpg -w w33 ./wwwroot/{uid}/2102-F1.jpg -w w34 ./wwwroot/{uid}/TR401-F1.jpg -w w35 ./wwwroot/{uid}/mockup017.jpg -w w36 ./wwwroot/{uid}/mockup103.jpg -w w37 ./wwwroot/{uid}/mockup105.jpg -w w38 ./wwwroot/{uid}/mockup094.jpg -w w39 ./wwwroot/{uid}/mockup095.jpg -w w40 ./wwwroot/{uid}/mockup145.jpg -w w41 ./wwwroot/{uid}/mockup146.jpg -w w42 ./wwwroot/{uid}/mockup147.jpg -w w43 ./wwwroot/{uid}/mockup151.jpg -w w44 ./wwwroot/{uid}/mockup014.jpg -w w45 ./wwwroot/{uid}/mockup097.jpg -w w46 ./wwwroot/{uid}/mockup046.jpg -w w47 ./wwwroot/{uid}/mockup195.jpg -w w48 ./wwwroot/{uid}/mockup197.jpg -w w49 ./wwwroot/{uid}/mockup019.jpg -w w50 ./wwwroot/{uid}/mockup180.jpg -w w51 ./wwwroot/{uid}/mockup179.jpg -w w52 ./wwwroot/{uid}/mockup040.jpg -w w53 ./wwwroot/{uid}/mockup169.jpg -w w54 ./wwwroot/{uid}/mockup022.jpg -w w55 ./wwwroot/{uid}/mockup023.jpg -w w56 ./wwwroot/{uid}/mockup111.jpg -w w57 ./wwwroot/{uid}/mockup020.jpg -w w58 ./wwwroot/{uid}/mockup021.jpg -w w59 ./wwwroot/{uid}/mockup114.jpg -w w60 ./wwwroot/{uid}/mockup035.jpg -w w61 ./wwwroot/{uid}/mockup015.jpg -w w62 ./wwwroot/{uid}/mockup004.jpg -w w63 ./wwwroot/{uid}/mockup062.jpg -w w64 ./wwwroot/{uid}/mockup064.jpg -w w65 ./wwwroot/{uid}/mockup133.jpg -w w66 ./wwwroot/{uid}/mockup134.jpg -w w67 ./wwwroot/{uid}/mockup135.jpg -w w68 ./wwwroot/{uid}/mockup137.jpg -w w69 ./wwwroot/{uid}/mockup142.jpg -w w70 ./wwwroot/{uid}/mockup144.jpg -w w71 ./wwwroot/{uid}/mockup044.jpg -w w72 ./wwwroot/{uid}/mockup183.jpg -w w73 ./wwwroot/{uid}/mockup185.jpg -w w74 ./wwwroot/{uid}/mockup186.jpg -w w75 ./wwwroot/{uid}/mockup188.jpg -w w76 ./wwwroot/{uid}/mockup190.jpg -w w77 ./wwwroot/{uid}/mockup191.jpg -w w78 ./wwwroot/{uid}/mockup029.jpg -w w79 ./wwwroot/{uid}/mockup138.jpg -w w80 ./wwwroot/{uid}/mockup030.jpg -w w81 ./wwwroot/{uid}/mockup037.jpg -w w82 ./wwwroot/{uid}/mockup161.jpg -i r83 ./wwwroot/{uid}/0.jpg -w w83 ./wwwroot/{uid}/mockup050.jpg -i r84 ./wwwroot/{uid}/0.jpg -w w84 ./wwwroot/{uid}/mockup051.jpg -i r85 ./wwwroot/{uid}/0.jpg -w w85 ./wwwroot/{uid}/mockup089.jpg -i r86 ./wwwroot/{uid}/0.jpg -w w86 ./wwwroot/{uid}/mockup049.jpg -i r87 ./wwwroot/{uid}/0.jpg -w w87 ./wwwroot/{uid}/mockup052.jpg -i r88 ./wwwroot/{uid}/0.jpg -w w88 ./wwwroot/{uid}/mockup054.jpg -i r89 ./wwwroot/{uid}/0.jpg -w w89 ./wwwroot/{uid}/mockup053.jpg -i r90 ./wwwroot/{uid}/0.jpg -w w90 ./wwwroot/{uid}/mockup125.jpg -i r91 ./wwwroot/{uid}/0.jpg -w w91 ./wwwroot/{uid}/mockup121.jpg -i r92 ./wwwroot/{uid}/0.jpg -w w92 ./wwwroot/{uid}/mockup124.jpg -i r93 ./wwwroot/{uid}/0.jpg -w w93 ./wwwroot/{uid}/mockup106.jpg -i r94 ./wwwroot/{uid}/0.jpg -w w94 ./wwwroot/{uid}/mockup109.jpg -i r95 ./wwwroot/{uid}/0.jpg -w w95 ./wwwroot/{uid}/mockup087.jpg -i r96 ./wwwroot/{uid}/0.jpg -w w96 ./wwwroot/{uid}/mockup088.jpg ");
            processInfo = new ProcessStartInfo(@"C:\App\Natron-2.3.15\bin\natronrenderer", $@"{hostingEnvironment.ContentRootPath}\uv-wrapping.ntp {mockupTemplate.MockupParametersString.Replace("{uid}", uid).Replace("{name}", fileName)}");
            //processInfo = new ProcessStartInfo(@"C:\Windows\System32\cmd.exe", $@"natronrenderer uv-wrapping.ntp -i r1 .\wwwroot\{uid}\0.jpg -w w1 .\wwwroot\{uid}\mockup161.jpg");
            //processInfo = new ProcessStartInfo(@"C:\Windows\System32\cmd.exe", $@"natronrenderer uv-wrapping.ntp -i r1 .\wwwroot\{uid}\0.jpg -i r2 .\wwwroot\{uid}\0.jpg -i r3 .\wwwroot\{uid}\0.jpg -i r4 .\wwwroot\{uid}\0.jpg -i r5 .\wwwroot\{uid}\0.jpg -i r6 .\wwwroot\{uid}\0.jpg -i r7 .\wwwroot\{uid}\0.jpg -i r8 .\wwwroot\{uid}\0.jpg -i r9 .\wwwroot\{uid}\0.jpg -i r10 .\wwwroot\{uid}\0.jpg -i r11 .\wwwroot\{uid}\0.jpg -i r12 .\wwwroot\{uid}\0.jpg -i r13 .\wwwroot\{uid}\0.jpg -i r14 .\wwwroot\{uid}\0.jpg -i r15 .\wwwroot\{uid}\0.jpg -i r16 .\wwwroot\{uid}\0.jpg -i r17 .\wwwroot\{uid}\0.jpg -i r18 .\wwwroot\{uid}\0.jpg -i r19 .\wwwroot\{uid}\0.jpg -i r20 .\wwwroot\{uid}\0.jpg -i r21 .\wwwroot\{uid}\0.jpg -i r22 .\wwwroot\{uid}\0.jpg -i r23 .\wwwroot\{uid}\0.jpg -i r24 .\wwwroot\{uid}\0.jpg -i r25 .\wwwroot\{uid}\0.jpg -i r26 .\wwwroot\{uid}\0.jpg -i r27 .\wwwroot\{uid}\0.jpg -i r28 .\wwwroot\{uid}\0.jpg -i r29 .\wwwroot\{uid}\0.jpg -i r30 .\wwwroot\{uid}\0.jpg -i r31 .\wwwroot\{uid}\0.jpg -i r32 .\wwwroot\{uid}\0.jpg -i r33 .\wwwroot\{uid}\0.jpg -i r34 .\wwwroot\{uid}\0.jpg -i r35 .\wwwroot\{uid}\0.jpg -i r36 .\wwwroot\{uid}\0.jpg -i r37 .\wwwroot\{uid}\0.jpg -i r38 .\wwwroot\{uid}\0.jpg -i r39 .\wwwroot\{uid}\0.jpg -i r40 .\wwwroot\{uid}\0.jpg -i r41 .\wwwroot\{uid}\0.jpg -i r42 .\wwwroot\{uid}\0.jpg -i r43 .\wwwroot\{uid}\0.jpg -i r44 .\wwwroot\{uid}\0.jpg -i r45 .\wwwroot\{uid}\0.jpg -i r46 .\wwwroot\{uid}\0.jpg -i r47 .\wwwroot\{uid}\0.jpg -i r48 .\wwwroot\{uid}\0.jpg -i r49 .\wwwroot\{uid}\0.jpg -i r50 .\wwwroot\{uid}\0.jpg -i r51 .\wwwroot\{uid}\0.jpg -i r52 .\wwwroot\{uid}\0.jpg -i r53 .\wwwroot\{uid}\0.jpg -i r54 .\wwwroot\{uid}\0.jpg -i r55 .\wwwroot\{uid}\0.jpg -i r56 .\wwwroot\{uid}\0.jpg -i r57 .\wwwroot\{uid}\0.jpg -i r58 .\wwwroot\{uid}\0.jpg -i r59 .\wwwroot\{uid}\0.jpg -i r60 .\wwwroot\{uid}\0.jpg -i r61 .\wwwroot\{uid}\0.jpg -i r62 .\wwwroot\{uid}\0.jpg -i r63 .\wwwroot\{uid}\0.jpg -i r64 .\wwwroot\{uid}\0.jpg -i r65 .\wwwroot\{uid}\0.jpg -i r66 .\wwwroot\{uid}\0.jpg -i r67 .\wwwroot\{uid}\0.jpg -i r68 .\wwwroot\{uid}\0.jpg -i r69 .\wwwroot\{uid}\0.jpg -i r70 .\wwwroot\{uid}\0.jpg -i r71 .\wwwroot\{uid}\0.jpg -i r72 .\wwwroot\{uid}\0.jpg -i r73 .\wwwroot\{uid}\0.jpg -i r74 .\wwwroot\{uid}\0.jpg -i r75 .\wwwroot\{uid}\0.jpg -i r76 .\wwwroot\{uid}\0.jpg -i r77 .\wwwroot\{uid}\0.jpg -i r78 .\wwwroot\{uid}\0.jpg -i r79 .\wwwroot\{uid}\0.jpg -i r80 .\wwwroot\{uid}\0.jpg -i r81 .\wwwroot\{uid}\0.jpg -i r82 .\wwwroot\{uid}\0.jpg -w w1 .\wwwroot\{uid}\bed-collection-c0.jpg -w w2 .\wwwroot\{uid}\bed-collection-c1.jpg -w w3 .\wwwroot\{uid}\bed-collection-c2.jpg -w w4 .\wwwroot\{uid}\bed-collection-c3.jpg -w w5 .\wwwroot\{uid}\bed-collection-c4.jpg -w w6 .\wwwroot\{uid}\lampshade-c0.jpg -w w7 .\wwwroot\{uid}\lampshade-c1.jpg -w w8 .\wwwroot\{uid}\lampshade-c2.jpg -w w9 .\wwwroot\{uid}\pillow-c0.jpg -w w10 .\wwwroot\{uid}\pillow-c1.jpg -w w11 .\wwwroot\{uid}\pillow-c2.jpg -w w12 .\wwwroot\{uid}\carpet-c0.jpg -w w13 .\wwwroot\{uid}\carpet-c2.jpg -w w14 .\wwwroot\{uid}\carpet-c1.jpg -w w15 .\wwwroot\{uid}\window-shade-c3.jpg -w w16 .\wwwroot\{uid}\window-shade-c2.jpg -w w17 .\wwwroot\{uid}\window-shade-c1.jpg -w w18 .\wwwroot\{uid}\window-shade-c0.jpg -w w19 .\wwwroot\{uid}\curtain-c3.jpg -w w20 .\wwwroot\{uid}\curtain-c2.jpg -w w21 .\wwwroot\{uid}\curtain-c1.jpg -w w22 .\wwwroot\{uid}\lustre-c1.jpg -w w23 .\wwwroot\{uid}\lustre-c2.jpg -w w24 .\wwwroot\{uid}\lustre-c0.jpg -w w25 .\wwwroot\{uid}\wallpaper-c2.jpg -w w26 .\wwwroot\{uid}\wallpaper-c1.jpg -w w27 .\wwwroot\{uid}\zebra-curtain-c2.jpg -w w28 .\wwwroot\{uid}\zebra-curtain-c1.jpg -w w29 .\wwwroot\{uid}\zebra-curtain-c3.jpg -w w30 .\wwwroot\{uid}\baby-bodysuit.jpg -w w31 .\wwwroot\{uid}\1072-F1.jpg -w w32 .\wwwroot\{uid}\2001-F1.jpg -w w33 .\wwwroot\{uid}\2102-F1.jpg -w w34 .\wwwroot\{uid}\TR401-F1.jpg -w w35 .\wwwroot\{uid}\mockup017.jpg -w w36 .\wwwroot\{uid}\mockup103.jpg -w w37 .\wwwroot\{uid}\mockup105.jpg -w w38 .\wwwroot\{uid}\mockup094.jpg -w w39 .\wwwroot\{uid}\mockup095.jpg -w w40 .\wwwroot\{uid}\mockup145.jpg -w w41 .\wwwroot\{uid}\mockup146.jpg -w w42 .\wwwroot\{uid}\mockup147.jpg -w w43 .\wwwroot\{uid}\mockup151.jpg -w w44 .\wwwroot\{uid}\mockup014.jpg -w w45 .\wwwroot\{uid}\mockup097.jpg -w w46 .\wwwroot\{uid}\mockup046.jpg -w w47 .\wwwroot\{uid}\mockup195.jpg -w w48 .\wwwroot\{uid}\mockup197.jpg -w w49 .\wwwroot\{uid}\mockup019.jpg -w w50 .\wwwroot\{uid}\mockup180.jpg -w w51 .\wwwroot\{uid}\mockup179.jpg -w w52 .\wwwroot\{uid}\mockup040.jpg -w w53 .\wwwroot\{uid}\mockup169.jpg -w w54 .\wwwroot\{uid}\mockup022.jpg -w w55 .\wwwroot\{uid}\mockup023.jpg -w w56 .\wwwroot\{uid}\mockup111.jpg -w w57 .\wwwroot\{uid}\mockup020.jpg -w w58 .\wwwroot\{uid}\mockup021.jpg -w w59 .\wwwroot\{uid}\mockup114.jpg -w w60 .\wwwroot\{uid}\mockup035.jpg -w w61 .\wwwroot\{uid}\mockup015.jpg -w w62 .\wwwroot\{uid}\mockup004.jpg -w w63 .\wwwroot\{uid}\mockup062.jpg -w w64 .\wwwroot\{uid}\mockup064.jpg -w w65 .\wwwroot\{uid}\mockup133.jpg -w w66 .\wwwroot\{uid}\mockup134.jpg -w w67 .\wwwroot\{uid}\mockup135.jpg -w w68 .\wwwroot\{uid}\mockup137.jpg -w w69 .\wwwroot\{uid}\mockup142.jpg -w w70 .\wwwroot\{uid}\mockup144.jpg -w w71 .\wwwroot\{uid}\mockup044.jpg -w w72 .\wwwroot\{uid}\mockup183.jpg -w w73 .\wwwroot\{uid}\mockup185.jpg -w w74 .\wwwroot\{uid}\mockup186.jpg -w w75 .\wwwroot\{uid}\mockup188.jpg -w w76 .\wwwroot\{uid}\mockup190.jpg -w w77 .\wwwroot\{uid}\mockup191.jpg -w w78 .\wwwroot\{uid}\mockup029.jpg -w w79 .\wwwroot\{uid}\mockup138.jpg -w w80 .\wwwroot\{uid}\mockup030.jpg -w w81 .\wwwroot\{uid}\mockup037.jpg -w w82 .\wwwroot\{uid}\mockup161.jpg");
            //processInfo = new ProcessStartInfo($@"natronrenderer uv-wrapping.ntp -i r1 .\wwwroot\{uid}\0.jpg -i r2 .\wwwroot\{uid}\0.jpg -i r3 .\wwwroot\{uid}\0.jpg -i r4 .\wwwroot\{uid}\0.jpg -i r5 .\wwwroot\{uid}\0.jpg -i r6 .\wwwroot\{uid}\0.jpg -i r7 .\wwwroot\{uid}\0.jpg -i r8 .\wwwroot\{uid}\0.jpg -i r9 .\wwwroot\{uid}\0.jpg -i r10 .\wwwroot\{uid}\0.jpg -i r11 .\wwwroot\{uid}\0.jpg -i r12 .\wwwroot\{uid}\0.jpg -i r13 .\wwwroot\{uid}\0.jpg -i r14 .\wwwroot\{uid}\0.jpg -i r15 .\wwwroot\{uid}\0.jpg -i r16 .\wwwroot\{uid}\0.jpg -i r17 .\wwwroot\{uid}\0.jpg -i r18 .\wwwroot\{uid}\0.jpg -i r19 .\wwwroot\{uid}\0.jpg -i r20 .\wwwroot\{uid}\0.jpg -i r21 .\wwwroot\{uid}\0.jpg -i r22 .\wwwroot\{uid}\0.jpg -i r23 .\wwwroot\{uid}\0.jpg -i r24 .\wwwroot\{uid}\0.jpg -i r25 .\wwwroot\{uid}\0.jpg -i r26 .\wwwroot\{uid}\0.jpg -i r27 .\wwwroot\{uid}\0.jpg -i r28 .\wwwroot\{uid}\0.jpg -i r29 .\wwwroot\{uid}\0.jpg -i r30 .\wwwroot\{uid}\0.jpg -i r31 .\wwwroot\{uid}\0.jpg -i r32 .\wwwroot\{uid}\0.jpg -i r33 .\wwwroot\{uid}\0.jpg -i r34 .\wwwroot\{uid}\0.jpg -i r35 .\wwwroot\{uid}\0.jpg -i r36 .\wwwroot\{uid}\0.jpg -i r37 .\wwwroot\{uid}\0.jpg -i r38 .\wwwroot\{uid}\0.jpg -i r39 .\wwwroot\{uid}\0.jpg -i r40 .\wwwroot\{uid}\0.jpg -i r41 .\wwwroot\{uid}\0.jpg -i r42 .\wwwroot\{uid}\0.jpg -i r43 .\wwwroot\{uid}\0.jpg -i r44 .\wwwroot\{uid}\0.jpg -i r45 .\wwwroot\{uid}\0.jpg -i r46 .\wwwroot\{uid}\0.jpg -i r47 .\wwwroot\{uid}\0.jpg -i r48 .\wwwroot\{uid}\0.jpg -i r49 .\wwwroot\{uid}\0.jpg -i r50 .\wwwroot\{uid}\0.jpg -i r51 .\wwwroot\{uid}\0.jpg -i r52 .\wwwroot\{uid}\0.jpg -i r53 .\wwwroot\{uid}\0.jpg -i r54 .\wwwroot\{uid}\0.jpg -i r55 .\wwwroot\{uid}\0.jpg -i r56 .\wwwroot\{uid}\0.jpg -i r57 .\wwwroot\{uid}\0.jpg -i r58 .\wwwroot\{uid}\0.jpg -i r59 .\wwwroot\{uid}\0.jpg -i r60 .\wwwroot\{uid}\0.jpg -i r61 .\wwwroot\{uid}\0.jpg -i r62 .\wwwroot\{uid}\0.jpg -i r63 .\wwwroot\{uid}\0.jpg -i r64 .\wwwroot\{uid}\0.jpg -i r65 .\wwwroot\{uid}\0.jpg -i r66 .\wwwroot\{uid}\0.jpg -i r67 .\wwwroot\{uid}\0.jpg -i r68 .\wwwroot\{uid}\0.jpg -i r69 .\wwwroot\{uid}\0.jpg -i r70 .\wwwroot\{uid}\0.jpg -i r71 .\wwwroot\{uid}\0.jpg -i r72 .\wwwroot\{uid}\0.jpg -i r73 .\wwwroot\{uid}\0.jpg -i r74 .\wwwroot\{uid}\0.jpg -i r75 .\wwwroot\{uid}\0.jpg -i r76 .\wwwroot\{uid}\0.jpg -i r77 .\wwwroot\{uid}\0.jpg -i r78 .\wwwroot\{uid}\0.jpg -i r79 .\wwwroot\{uid}\0.jpg -i r80 .\wwwroot\{uid}\0.jpg -i r81 .\wwwroot\{uid}\0.jpg -i r82 .\wwwroot\{uid}\0.jpg -w w1 .\wwwroot\{uid}\bed-collection-c0.jpg -w w2 .\wwwroot\{uid}\bed-collection-c1.jpg -w w3 .\wwwroot\{uid}\bed-collection-c2.jpg -w w4 .\wwwroot\{uid}\bed-collection-c3.jpg -w w5 .\wwwroot\{uid}\bed-collection-c4.jpg -w w6 .\wwwroot\{uid}\lampshade-c0.jpg -w w7 .\wwwroot\{uid}\lampshade-c1.jpg -w w8 .\wwwroot\{uid}\lampshade-c2.jpg -w w9 .\wwwroot\{uid}\pillow-c0.jpg -w w10 .\wwwroot\{uid}\pillow-c1.jpg -w w11 .\wwwroot\{uid}\pillow-c2.jpg -w w12 .\wwwroot\{uid}\carpet-c0.jpg -w w13 .\wwwroot\{uid}\carpet-c2.jpg -w w14 .\wwwroot\{uid}\carpet-c1.jpg -w w15 .\wwwroot\{uid}\window-shade-c3.jpg -w w16 .\wwwroot\{uid}\window-shade-c2.jpg -w w17 .\wwwroot\{uid}\window-shade-c1.jpg -w w18 .\wwwroot\{uid}\window-shade-c0.jpg -w w19 .\wwwroot\{uid}\curtain-c3.jpg -w w20 .\wwwroot\{uid}\curtain-c2.jpg -w w21 .\wwwroot\{uid}\curtain-c1.jpg -w w22 .\wwwroot\{uid}\lustre-c1.jpg -w w23 .\wwwroot\{uid}\lustre-c2.jpg -w w24 .\wwwroot\{uid}\lustre-c0.jpg -w w25 .\wwwroot\{uid}\wallpaper-c2.jpg -w w26 .\wwwroot\{uid}\wallpaper-c1.jpg -w w27 .\wwwroot\{uid}\zebra-curtain-c2.jpg -w w28 .\wwwroot\{uid}\zebra-curtain-c1.jpg -w w29 .\wwwroot\{uid}\zebra-curtain-c3.jpg -w w30 .\wwwroot\{uid}\baby-bodysuit.jpg -w w31 .\wwwroot\{uid}\1072-F1.jpg -w w32 .\wwwroot\{uid}\2001-F1.jpg -w w33 .\wwwroot\{uid}\2102-F1.jpg -w w34 .\wwwroot\{uid}\TR401-F1.jpg -w w35 .\wwwroot\{uid}\mockup017.jpg -w w36 .\wwwroot\{uid}\mockup103.jpg -w w37 .\wwwroot\{uid}\mockup105.jpg -w w38 .\wwwroot\{uid}\mockup094.jpg -w w39 .\wwwroot\{uid}\mockup095.jpg -w w40 .\wwwroot\{uid}\mockup145.jpg -w w41 .\wwwroot\{uid}\mockup146.jpg -w w42 .\wwwroot\{uid}\mockup147.jpg -w w43 .\wwwroot\{uid}\mockup151.jpg -w w44 .\wwwroot\{uid}\mockup014.jpg -w w45 .\wwwroot\{uid}\mockup097.jpg -w w46 .\wwwroot\{uid}\mockup046.jpg -w w47 .\wwwroot\{uid}\mockup195.jpg -w w48 .\wwwroot\{uid}\mockup197.jpg -w w49 .\wwwroot\{uid}\mockup019.jpg -w w50 .\wwwroot\{uid}\mockup180.jpg -w w51 .\wwwroot\{uid}\mockup179.jpg -w w52 .\wwwroot\{uid}\mockup040.jpg -w w53 .\wwwroot\{uid}\mockup169.jpg -w w54 .\wwwroot\{uid}\mockup022.jpg -w w55 .\wwwroot\{uid}\mockup023.jpg -w w56 .\wwwroot\{uid}\mockup111.jpg -w w57 .\wwwroot\{uid}\mockup020.jpg -w w58 .\wwwroot\{uid}\mockup021.jpg -w w59 .\wwwroot\{uid}\mockup114.jpg -w w60 .\wwwroot\{uid}\mockup035.jpg -w w61 .\wwwroot\{uid}\mockup015.jpg -w w62 .\wwwroot\{uid}\mockup004.jpg -w w63 .\wwwroot\{uid}\mockup062.jpg -w w64 .\wwwroot\{uid}\mockup064.jpg -w w65 .\wwwroot\{uid}\mockup133.jpg -w w66 .\wwwroot\{uid}\mockup134.jpg -w w67 .\wwwroot\{uid}\mockup135.jpg -w w68 .\wwwroot\{uid}\mockup137.jpg -w w69 .\wwwroot\{uid}\mockup142.jpg -w w70 .\wwwroot\{uid}\mockup144.jpg -w w71 .\wwwroot\{uid}\mockup044.jpg -w w72 .\wwwroot\{uid}\mockup183.jpg -w w73 .\wwwroot\{uid}\mockup185.jpg -w w74 .\wwwroot\{uid}\mockup186.jpg -w w75 .\wwwroot\{uid}\mockup188.jpg -w w76 .\wwwroot\{uid}\mockup190.jpg -w w77 .\wwwroot\{uid}\mockup191.jpg -w w78 .\wwwroot\{uid}\mockup029.jpg -w w79 .\wwwroot\{uid}\mockup138.jpg -w w80 .\wwwroot\{uid}\mockup030.jpg -w w81 .\wwwroot\{uid}\mockup037.jpg -w w82 .\wwwroot\{uid}\mockup161.jpg");
            processInfo.CreateNoWindow = false;
            processInfo.UseShellExecute = false;
            // *** Redirect the output ***
            //processInfo.RedirectStandardError = true;
            //processInfo.RedirectStandardOutput = true;

            process = Process.Start(processInfo);
            process.WaitForExit();

            //Directory.Move(Path.Combine("wwwroot", "usersUploadedDesigns", uid), Path.Combine("wwwroot", "usersUploadedDesigns", guid));

            // *** Read the streams ***
            // Warning: This approach can lead to deadlocks, see Edit #2
            //string output = process.StandardOutput.ReadToEnd();
            //string error = process.StandardError.ReadToEnd();

            exitCode = process.ExitCode;

            //Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
            //Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
            //Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
            process.Close();

            return Ok();
        }
    }
}
