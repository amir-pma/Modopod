<template>
    <b-container class="p-5">
        <b-row>
            <b-button variant="outline-secondary" @click.prevent="back">
                بازگشت <i class="fas fa-arrow-left"></i>
            </b-button>
        </b-row>
        <b-row class="pt-4">
            <b-col cols="4">
                <b-container class="right-side">
                    <div class="box">
                        <div style="width:100%">
                            <h5>انتخاب طرح</h5>
                        </div>
                        <div v-show="Boolean(designAddress)" style="height:200px; border-radius:5px; display: flex; justify-content: center; align-items: center;">
                            <img ref="design" style="max-width:100%; max-height:100%; position: relative; border-radius:5px; padding: 5%;" :src="designAddress" />
                        </div>
                        <b-form-file no-drop
                                     @input="myInputHandler"
                                     style="text-align:left; overflow:hidden; margin-top: 5%;"
                                     :state="Boolean(designAddress)"
                                     placeholder="انتخاب کنید"
                                     @change="saveFile"></b-form-file>

                    </div>
                    <hr v-if="Boolean(designAddress)" />
                    <span v-if="Boolean(designAddress)" style="margin-bottom:20px;" slot="header"> <h5>سایز طرح</h5> </span>
                    <div v-if="Boolean(designAddress)" style="margin-top:40px; margin-right:10px; margin-bottom:40px;">
                        <vue-slider :value="val"
                                    :min="1"
                                    :max="100"
                                    :lazy=true
                                    width="90%"
                                    @callback="changeSlider">
                        </vue-slider>
                    </div>
                    <div v-if="Boolean(designAddress)" class="box" style="display: flex; justify-content:center;">
                        <b-button variant="primary" @click="centerize" style="text-align:center;">
                            وسط قرار دادن طرح
                        </b-button>
                    </div>
                    <hr />
                    <div v-if="showPreview" style="height:200px; border-radius:5px; display: flex; justify-content: center; align-items: center;">
                        <!--mockupImages[0]-->
                        <img v-if="product.slug=='mug'" :disabled="loading" class="prevImg" @click="previewShow = true" :src="mockupImages[0]" style="max-width:100%; max-height:100%; position: relative; border-radius:5px; padding: 5%;" />
                        <img v-else :disabled="loading" class="prevImg" @click="previewShow = true" :src="myPreviewImage" style="max-width:100%; max-height:100%; position: relative; border-radius:5px; padding: 5%;" />
                    </div>
                    <div style="text-align: center">
                        <p v-if="loading" style="text-align: center">در حال آماده سازی</p>
                    </div>
                    <div v-if="Boolean(designAddress) && product.slug === 'mug'" class="box" style="display: flex; justify-content:center;">
                        <b-button :disabled="loading" variant="primary" @click="previewDesignOnProduct" style="text-align:center;">
                            مشاهده پیش نمایش
                        </b-button>
                    </div>
                    <hr v-if="Boolean(designAddress)"/>
                    <b-form-group label="">
                        <h5 style="margin-bottom:20px;">انتخاب رنگ</h5>
                        <b-form-select :options="colours" v-model="colour" />
                    </b-form-group>
                    <hr />
                    <p class="mt-4 mb-4">
                        <b style="margin-left:20px;">قیمت محصول:  </b> {{ variant.price | currency }}
                    </p>
                    <div v-if="designReady && isNotAdmin" class="box" style="display: flex; justify-content:center;">
                        <b-button :disabled="loading" variant="primary" @click="addProductToCart" style="text-align:center;">
                            افزودن به سبد خرید
                        </b-button>
                    </div>
                </b-container>
            </b-col>
            <b-col cols="8">
                <transition name="fade" mode="out-in">
                    <div style="position: relative; width: 560px; height: 560px;">
                        <div :style="templatePrintArea">
                            <VueDragResize :isResizable="false" :w="picWidth" :h="picHeight" :x="left" :y="top" v-on:dragging="resize">
                                <img @mouseup="updatePreview" id="toto" v-if="Boolean(designAddress)" :src="designAddress" style="width:100%; height: 100%; position: absolute; z-index: 2" />
                                <img @mouseup="updatePreview" id="toto" v-else :src="chooseFileAddress" style="border-radius:50%; width:100%; height: 100%; position: absolute; z-index: 2" />
                            </VueDragResize>
                        </div>
                        <div :style="templateBG"></div>
                    </div>
                    <!--<b-container ref = "bgImage" :class="'bgimg-front'" style="overflow:hidden; position: relative; border: 1px solid #dfdfdf; border-radius: 5px;">
                    <VueDragResize :isResizable="false" :w="picWidth" :h="picHeight" :x="left" :y="top" :parentLimitation="false" v-on:resizing="resize" v-on:dragging="resize">
                        <img v-if="Boolean(designAddress)" style="width:100%; height: 100%; z-index: -1; position: relative; display: block;" :src="designAddress" alt="" />
                        <img v-else style="border-radius:50%; width:100%; height: 100%; z-index: -1; position: relative; display: block;" :src="chooseFileAddress" alt="" />
                    </VueDragResize>
                </b-container>-->

                                                    <!--<div style="position: relative; width: 600px; height: 600px;">
                                    <div :style="templatePrintArea(template.number)">
                                        <VueDragResize :isResizable="false" :w="templatesData[template.number].picWidth" :h="templatesData[template.number].picHeight" :x="templatesData[template.number].left" :y="templatesData[template.number].top" v-on:dragging="resize($event,template.number)">
                                            <img v-if="Boolean(templatesData[template.number].designAddress)" :src="templatesData[template.number].designAddress" style="width:100%; height: 100%; position: absolute; z-index: 2" />
                                            <img v-else :src="chooseFileAddress" style="border-radius:50%; width:100%; height: 100%; position: absolute; z-index: 2" />
                                        </VueDragResize>
                                    </div>
                                    <div class="coverClass" :style="templateBG(template.number)"></div>
                                </div>-->
                </transition>

            </b-col>
            <h5 id="widthDommy"></h5>
            <h5 id="heightDommy"></h5>
            <h5 id="urlDommy"></h5>
        </b-row>
        <transition name="fade" mode="out-in">
            <gallery v-if="previewShow" :images="galleryImages" :initial="0" @close="previewShow = false" />
        </transition>
    </b-container>

</template>

<script>
    import Gallery from "./Gallery.vue";
    import VueDragResize from 'vue-drag-resize';
    import vueSlider from "vue-slider-component";
    import axios from 'axios';
    import Nprogress from 'nprogress';
    import Jimp from 'jimp';
    //import mergeImages from 'merge-images';

    export default {
        name: "product-details",
        data() {
            return {
                colour: null,
                colours: [],
                picWidth: 120,//*
                pictureWidth: 100,//*
                pictureHeight: 100,//*
                picHeight: 120,//*
                top: 0,//*
                left: 0,//*
                val: 20,//*
                designAddress: "",
                chooseFileAddress: "/assets/images/chooseFileAddress.jpg",
                variants: [],
                variantTemplates: [],
                variantPrintfiles: [],
                selectedVariant: 0,
                templateBackgroundColour: null,
                templateImageUrl: null,
                templateWidth: null,
                templateHeight: null,
                templatePrintAreaWidth: null,
                templatePrintAreaHeight: null,
                templatePrintAreaTop: null,
                templatePrintAreaLeft: null,
                showPreview: false,
                taskKey: "",
                mockups: null,
                designPath: null,//*
                designPathFolder: null,//*
                previewShow: false,
                loading: false,
                designReady: false,
                placement: null,
                myTestDesign: null,
                myPreviewImage: null
            };
        },
        computed: {
            galleryImages() {
                if (this.product.slug === 'mug') {
                    return this.mockupImages;
                }
                else {
                    return [this.myPreviewImage];
                }
            },
            variant() {
                return this.product.variants.find(v => v.colour == this.colour);
                 
            },
            templateBG() {
                return {
                    position: 'absolute',
                    width: this.templateWidth + 'px',
                    height: this.templateHeight + 'px',
                    background: 'url(' + this.templateImageUrl + ') center center no-repeat'
                };
            },
            templatePrintArea() {
                return {
                    overflow: 'hidden',
                    position: 'absolute',
                    width: this.templatePrintAreaWidth + 'px',
                    height: this.templatePrintAreaHeight + 'px',
                    top: this.templatePrintAreaTop + 'px',
                    right: (this.templateWidth - this.templatePrintAreaWidth - this.templatePrintAreaLeft) + 'px',
                    'background-color': this.templateBackgroundColour
                };
            },
            mockupImages() {
                var i;
                for (i = 0; i < this.mockups.length; i++) {
                    if (this.mockups[i].variant_ids[0] == this.variants[this.selectedVariant]) {
                        //console.log(this.mockups[i]);
                        var urls = [];
                        urls.push(this.mockups[i].mockup_url);
                        if (this.mockups[i].extra != null) {
                            var j;
                            for (j = 0; j < this.mockups[i].extra.length; j++) {
                                urls.push(this.mockups[i].extra[j].url)
                            }
                        }
                        return urls;
                    }
                }
            },
            isNotAdmin() {
                return !this.$store.getters.isInRole("Admin");
            }
        },
        props: {
            product: {
                type: Object,
                required: true
            }
        },
        components: {
            Gallery,
            VueDragResize,
            vueSlider
        },
        methods: {
            toDataURL() {
                
            },
            back() {
                this.$router.go(-1);
            },
            addProductToCart() {
                var i;
                var imagesToBeSaved = []
                axios.defaults.headers.common["Authorization"] = "";
                for (i = 0; i < this.mockupImages.length; i++) {
                    axios
                        .get('https://cors-anywhere.herokuapp.com/' + this.mockupImages[i], {
                            responseType: 'arraybuffer'
                            
                    })
                    .then(response => {
                        var image = new Buffer(response.data, 'binary').toString('base64');
                        imagesToBeSaved.push(image)
                    }).catch(response => console.log(response));
                    
                }
                if (this.$store.getters.isAuthenticated) {
                    axios.defaults.headers.common["Authorization"] = `Bearer ${
                        this.$store.state.auth.access_token
                    }`;
                }
                this.$store.dispatch("addProductToCart", { variant: this.variant, image: { imageURL: this.mockupImages[0], images: imagesToBeSaved }});
                this.$toastr("success", "محصول به سبد خرید اضافه شد");
            },
            updatePreview() {
                if (this.product.slug !== 'mug') {
                    var vm = this;
                    Jimp.read(this.designAddress)
                        .then(image => {
                            image.resize(this.picWidth, this.picHeight);
                            image.getBase64(Jimp.AUTO, (err, res) => {
                                document.getElementById("urlDommy").innerHTML = res;
                            })
                        })
                        .catch(err => {
                            throw err;
                        });
                    this.waiter();
                }
            },
            waiter() {
                if (document.getElementById("urlDommy").innerHTML === "") {
                    setTimeout(this.waiter, 100);

                }
                else {
                    //console.log("my url ---- ", document.getElementById("urlDommy").innerHTML);
                    var vm = this;
                    axios
                        .get('https://cors-anywhere.herokuapp.com/' + this.templateImageUrl, {
                            responseType: 'arraybuffer'
                            
                    })
                    .then(response => {
                        var templateImg = new Buffer(response.data, 'binary').toString('base64');
                        this.myTestDesign = document.getElementById("urlDommy").innerHTML;
                        document.getElementById("urlDommy").innerHTML = "";
                        //console.log(templateImg);
                        //console.log(vm.myTestDesign);
                        mergeImages([
                            
                            { src: vm.myTestDesign, x: vm.templatePrintAreaLeft + vm.left, y: vm.templatePrintAreaTop + vm.top },
                            { src: 'data:image/png;base64, ' + templateImg, x: 0, y: 0 }
                        ])
                            .then(b64 => {
                                vm.myPreviewImage = b64;
                                vm.showPreview = true;
                            });
                    }).catch(response => console.log(response));

                    
                }
                
            },
            resize(newRect) {
                this.top = newRect.top;
                this.left = newRect.left;
            },
            centerize() {
                this.top = parseInt(this.templatePrintAreaHeight / 2 - this.picHeight / 2);
                this.left = parseInt(this.templatePrintAreaWidth / 2 - this.picWidth / 2);
                this.updatePreview();
            },
            changeSlider(e) {
                this.val = e;
                var prevWidth = this.picWidth;
                var newWidth = parseInt(this.val / 100 * (this.pictureWidth));
                if (prevWidth !== newWidth) {
                    var changeWidth = (newWidth - prevWidth) / 2;
                    this.left = this.left - changeWidth;
                }
                var prevHeight = this.picHeight;
                var newHeight = parseInt(this.val / 100 * (this.pictureHeight));
                if (prevHeight !== newHeight) {
                    var changeHeight = (newHeight - prevHeight) / 2;
                    this.top = this.top - changeHeight;
                }
                this.picWidth = parseInt(this.val / 100 * (this.pictureWidth));
                this.picHeight = parseInt(this.val / 100 * (this.pictureHeight));

                this.updatePreview();
                
                
            },
            
            myInputHandler() {

                if (document.getElementById("widthDommy").innerHTML === "" || document.getElementById("heightDommy").innerHTML === "") {
                    setTimeout(this.myInputHandler, 100);
                        
                }
                else {
                    var myWidth = parseInt(document.getElementById("widthDommy").innerHTML);
                    var myHeight = parseInt(document.getElementById("heightDommy").innerHTML);
                    this.pictureWidth = myWidth;
                    this.pictureHeight = myHeight;
                    this.picWidth = parseInt(myWidth/5);
                    this.picHeight = parseInt(myHeight / 5);
                    this.val = 20;
                    this.top = parseInt(this.templatePrintAreaHeight / 2 - this.picHeight / 2);
                    this.left = parseInt(this.templatePrintAreaWidth / 2 - this.picWidth / 2);
                    document.getElementById("widthDommy").innerHTML = "";
                    document.getElementById("heightDommy").innerHTML = "";
                    this.updatePreview();
                }
                
            },
            saveFile(e) {
                

                var files = new FormData();
                files.append("files", e.target.files[0], e.target.files[0].name);
                files.append("slug", this.product.slug)
                axios.post('/api/images/uploadUserDesign', files,
                    {
                        headers: {
                            'content-type': 'application/json'
                        }
                    }).then(response => {
                        this.designPath = response.data.designPathFolder;
                        this.designAddress = response.data.designPath;
                    }).catch(err => console.log(err));


                //axios.post('/api/images/uploadUserDesign', files, {headers: {
                //    'content-type': 'multipart/form-data'
                //}
                //}).then(response => {
                //    this.designPath = response.data.designPath;
                //    this.designAddress = response.data.designPathFolder;
                //}).catch(err => console.log(err));

                var files = e.target.files || e.dataTransfer.files;
                var reader = new FileReader();
                reader.readAsDataURL(files[0]);
                reader.onload = function (e) {
                    var image = new Image();
                    image.src = e.target.result;
                    image.onload = function () {
                        var tempHeight = this.height;
                        var tempWidth = this.width;
                        document.getElementById("widthDommy").innerHTML = tempWidth;
                        document.getElementById("heightDommy").innerHTML = tempHeight;
                        return true;
                        
                    };
                }

                
            },
            previewDesignOnProduct() {
                Nprogress.start();
                this.loading = true;
                axios.post("https://cors-anywhere.herokuapp.com/https://api.printful.com/mockup-generator/create-task/" + this.product.printfulProductId,
                    {
                        "variant_ids": this.variants,
                        "format": "jpg",
                        "files": [{
                            "placement": this.placement,
                            "image_url": "https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png",//this.designPath
                            "position": {
                                "area_width": this.templatePrintAreaWidth,
                                "area_height": this.templatePrintAreaHeight,
                                "width": this.picWidth,
                                "height": this.picHeight,
                                "top": this.top,
                                "left": this.left
                            }
                        }]
                    },
                    { headers: { Authorization: "Basic eDN1M3ViNHAtcnc2dC13b3N4OjU0ejAtZXlqMDF1bnVobWpq" } }).then(response => {
                        
                        this.taskKey = response.data.result.task_key;
                        this.waitForMockup();
                        
                    }).catch(error => console.log(error));

            },
            waitForMockup() {
                axios.get("https://cors-anywhere.herokuapp.com/https://api.printful.com/mockup-generator/task?task_key=" + this.taskKey,
                        { headers: { Authorization: "Basic eDN1M3ViNHAtcnc2dC13b3N4OjU0ejAtZXlqMDF1bnVobWpq" } }).then(response => {
                            if (response.data.result.status == "pending") {
                                console.log("...");
                                setTimeout(this.waitForMockup, 4000);
                            }
                            else if (response.data.result.status == "completed") {
                                this.mockups = response.data.result.mockups;
                                console.log("completed");
                                Nprogress.done();
                                this.previewShow = true;
                                this.showPreview = true;
                                this.loading = false;
                                this.designReady = true;
                            }
                            else {
                                console.log("EROOOOOOOOOOOOOOR");
                                return;
                            }
                        });
            
            },
        },
        created() {
            Nprogress.start();
            this.colour = this.product.variants[0].colour;
            var i;
            for (i = 0; i < this.product.variants.length; i++) {
                this.colours.push(this.product.variants[i].colour);
            }
            this.variants = this.product.variantsListString.split(",");
            var o;
            for (o = 0; o < this.variants.length; o++) {
                this.variantTemplates.push(0);
            }
            if (this.product.slug == "mug") {
                this.placement = "default";
            }
            else {
                this.placement = "front";
            }
             
            axios.get("https://cors-anywhere.herokuapp.com/https://api.printful.com/mockup-generator/templates/" + this.product.printfulProductId,
                    { headers: { Authorization: "Basic eDN1M3ViNHAtcnc2dC13b3N4OjU0ejAtZXlqMDF1bnVobWpq" } }).then(response => {
                        //console.log(response.data);
                        var i, j;
                        for (i = 0; i < response.data.result.variant_mapping.length; i++) {
                            for (j = 0; j < this.variants.length; j++) {
                                if (response.data.result.variant_mapping[i].variant_id == this.variants[j]) {
                                    var k;
                                    for (k = 0; k < response.data.result.variant_mapping[i].templates.length; k++) {
                                        if (response.data.result.variant_mapping[i].templates[k].placement == this.placement) {
                                            var t;

                                            for (t = 0; t < response.data.result.templates.length; t++) {
                                                if (response.data.result.templates[t].template_id == response.data.result.variant_mapping[i].templates[k].template_id) {
                                                    this.variantTemplates[j] = response.data.result.templates[t]
                                                }
                                            }
                                        }
                                        
                                    }
                                    
                                }
                            }
                        }
                        this.templateBackgroundColour = this.variantTemplates[this.selectedVariant].background_color;
                        this.templateImageUrl = this.variantTemplates[this.selectedVariant].image_url;
                        this.templateWidth = this.variantTemplates[this.selectedVariant].template_width;
                        this.templateHeight = this.variantTemplates[this.selectedVariant].template_height;
                        this.templatePrintAreaWidth = this.variantTemplates[this.selectedVariant].print_area_width;
                        this.templatePrintAreaHeight = this.variantTemplates[this.selectedVariant].print_area_height;
                        this.templatePrintAreaTop = this.variantTemplates[this.selectedVariant].print_area_top;
                        this.templatePrintAreaLeft = this.variantTemplates[this.selectedVariant].print_area_left;
                        this.top = parseInt(this.templatePrintAreaHeight / 2 - this.picHeight / 2);
                        this.left = parseInt(this.templatePrintAreaWidth / 2 - this.picWidth / 2);
                        Nprogress.done();
                    });
            window.scrollTo(0, 0);
        }
    };

</script>

<style scoped>
    * {
        direction: rtl;
        text-align: right;
    }


    .bgimg-front {
        background-repeat: no-repeat;
        background-image: url('../../../wwwroot/TshirtFront.png');
        background-size: cover;
        width: 600px;
        height: 600px;
    }

    .bgimg-back {
        background-repeat: no-repeat;
        background-image: url('../../../wwwroot/TshirtBack.png');
        background-size: cover;
        width: 600px;
        height: 600px;
    }

    .right-side {
        border: 1px solid #dfdfdf;
        border-radius: 5px;
        padding: 5%;
    }

    .box {
        margin: 7.5%;
    }

    .fade-enter-active,
    .fade-leave-active {
        transition: opacity 0.4s ease-in-out;
    }

    .fade-enter,
    .fade-leave-to {
        opacity: 0;
    }

    .selectFrontBack {
        display: flex;
        flex-direction: column;
        justify-content: center;
        margin: 3.5%;
        border-radius: 3%;
        padding: 30px;
        box-shadow: 0 0 6px rgba(0,0,0, 0.3);
        padding: 5%;
        height: 100%;
    }

    .selectFrontBackSelected {
        display: flex;
        flex-direction: column;
        justify-content: center;
        margin: 3.5%;
        border-radius: 3%;
        padding: 30px;
        box-shadow: 0 0 15px rgba(0,0,0, 0.8);
        padding: 5%;
        height: 100%;
    }

    .fullWidth {
        width: 100%;
    }

    .fullHeight {
        height: 100%;
    }

    .prevImg {
        cursor: pointer;
    }
</style>


