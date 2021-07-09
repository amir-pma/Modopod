<template>
    <b-col>
        <b-row :class="{designContainerNotUploaded:!showList, designContainerUploaded:showList}">
            <label v-if="Boolean(designAddress)" for="file-input">
                <div id="uploadOnTopImage">
                    <div style="height:300px; border-radius:5px; display: flex; justify-content: center; align-items: center;">
                        <img style="max-width:100%; max-height:100%; position: relative; border-radius:5px; padding: 5%;" :src="designAddress" />
                    </div>
                    <div id="onImageGray">
                        <h3><b>تغییر طرح</b></h3>
                    </div>
                </div>
            </label>
            <label v-else for="file-input">
                <div id="uploadOnTop">
                    <div style="cursor:pointer; display:flex; justify-content:space-between; flex-direction:column; align-items:center; width:180px; height:190px;">
                        <img :src="uploadIconAddress" style="width:103px; height:135px;" />
                        <h3><b>انتخاب طرح</b></h3>
                    </div>
                </div>
            </label>
            <b-form-file no-drop
                         id="file-input"
                         @input="myInputHandler"
                         style="text-align:left; overflow:hidden; margin-top: 5%; display:none;"
                         :state="Boolean(designAddress)"
                         placeholder="انتخاب کنید"
                         @change="saveFile($event, false, 0)"></b-form-file>

            <div v-show="showList" class="designInfoClass">
                <label for="designTitle" class="designInfoTitleLabel">
                    نام طرح&nbsp;
                </label>
                <input placeholder="نام طرح خود را وارد کنید" class="designInfoTitle" v-model="designInfoName" type="text" id="designTitle">
                <label for="designTitle" class="designInfoTitleLabel">
                    تگ ها&nbsp;
                </label>
                <select multiple="multiple" id="myMulti">
                    <option v-for="tag in tags" :key="tag.id" :value="tag.name">{{tag.name}}</option>
                </select>
                <label for="designDescription" class="designInfoTitleLabel">
                    توضیحات طرح&nbsp;
                </label>
                <textarea rows="3" placeholder="توضیحاتی درباره طرح خود بنویسید" v-model="designInfoDescription" class="designInfoDescription" id="designDescription"></textarea>
            </div>
        </b-row>
        <b-row v-if="showList" class="mt-4" style="width: 100%!important; margin:0px 30px 0px 0px;">
            <b-col v-show="template.display" class="mb-4" :cols="template.cols" v-for="template in templates" :key="template.template.id">
                <div v-if="template.isComponent">
                    <b-row>
                        <b-col cols="12" v-if="templatesData[template.number].previewLoadingForClosing" style=" height:600px; position:absolute; display:flex; justify-content:center; align-items:center; background-color: gray; opacity:0.6; z-index:10000">
                            <b-spinner style="width:6rem; height:6rem ;position:absolute; z-index:20;" variant="primary" label="Spinning"></b-spinner>
                        </b-col>

                        <b-col cols="8" style="display:flex; justify-content:center;">
                            <transition name="fade" mode="out-in">
                                <div :id="'wholeTemplate' + template.number" style="width:600px; height:600px; display:flex; justify-content:center;">
                                    <b-container @mouseup="updatePreview(null, template.number, false)" class="coverClass" :style="templateBGStyle(template.number)">
                                        <div :id="'colorPic' + template.number" v-if="Boolean(templatesData[template.number].designAddress)" :style="colorMakerStyle(template.number)">
                                        </div>
                                        <div :class="{noBorder:templatesData[template.number].previewLoading}" :style="templatePrintAreaStyle(template.number)">
                                            <VueDragResize :isResizable="false" :w="templatesData[template.number].isRepeat ? 2222 : templatesData[template.number].picWidth" :h="templatesData[template.number].isRepeat ? 2222 : templatesData[template.number].picHeight" :x="templatesData[template.number].left" :y="templatesData[template.number].top" v-on:dragging="resize($event,template.number)" style="cursor:grab">
                                                <div :id="'printAreaImage' + template.number" v-if="Boolean(templatesData[template.number].designAddress) && (templatesData[template.number].repeatType === 'bad')" style="width:100%; height: 100%; z-index: -1; position: relative; display:flex;">
                                                    <div :style="{display:'flex', height:'100%', width:(templatesData[template.number].picWidth*2) + 'px'}" v-for="i in numberOfBackgrounds(template.number)">
                                                        <div :style="badBackgroundImageFirstColumn(template.number)"></div>
                                                        <div :style="badBackgroundImageSecondColumn(template.number)"></div>
                                                    </div>
                                                </div>
                                                <div :id="'printAreaImage' + template.number" v-else-if="Boolean(templatesData[template.number].designAddress) && (templatesData[template.number].isRepeat)" style="width:100%; height: 100%; z-index: -1; position: relative; display:flex;">
                                                    <div :style="{display:'flex', height:'100%', width:(templatesData[template.number].picWidth*2) + 'px'}" v-for="i in numberOfBackgrounds(template.number)">
                                                        <div :style="badBackgroundImageFirstColumn(template.number)"></div>
                                                        <div :style="badBackgroundImageFirstColumn(template.number)"></div>
                                                    </div>
                                                </div>
                                                <div :id="'printAreaImage' + template.number" v-else-if="Boolean(templatesData[template.number].designAddress)" :class="{coverClass:!templatesData[template.number].isRepeat}" :style="draggableImageBackground(template.number)"></div><!--@mouseup="updatePreview"-->
                                                <!--<div :id="'printAreaImage' + template.number" v-else-if="Boolean(templatesData[template.number].designAddress)" :class="{coverClass:!templatesData[template.number].isRepeat}" :style="draggableImageBackground(template.number)"></div>--><!--@mouseup="updatePreview"-->
                                                <!--<img v-if="Boolean(templatesData[template.number].designAddress)" :src="templatesData[template.number].designAddress" style="width:100%; height: 100%; z-index: -1; position: relative; display: block;" />-->
                                                <img :id="'printAreaImage' + template.number" v-else :src="chooseFileAddress" style="border-radius:50%; width:100%; height: 100%; z-index: -1; position: relative; display: block;" /><!--@mouseup="updatePreview"-->
                                            </VueDragResize>
                                        </div>
                                    </b-container>
                                </div>
                            </transition>
                        </b-col>
                        <b-col cols="4">
                            <b-container class="right-side">
                                <div class="box">
                                    <div v-if="templatesData[template.number].editOptions.includes('replaceImage')" style="width:100%">
                                        <label :for="'fileInputReplace' + template.number" style="width:100%">
                                            <div id="replaceImageOption">
                                                <h5 style="text-align:center">تغییر طرح</h5>
                                            </div>
                                        </label>
                                        <input type="file"
                                               :id="'fileInputReplace' + template.number"
                                               @input="myInputHandler"
                                               style="display:none; text-align:left; overflow:hidden; margin-top: 5%;"
                                               :state="Boolean(templatesData[template.number].designAddress)"
                                               @change="saveFile($event, true, template.number)"></input>
                                        <hr />
                                    </div>
                                    <div v-if="templatesData[template.number].editOptions.includes('slider')">
                                        <span style="margin-bottom:20px;">
                                            <h6 style="text-align:right">سایز طرح</h6>
                                        </span>
                                        <div style="margin-top:-15px; margin-right:10px; margin-bottom:40px; width: 100%;">
                                            <slider @change="changeSlider($event, template.number)"
                                                    class="MySlider"
                                                    min="3"
                                                    max="100"
                                                    :value="templatesData[template.number].val"></slider>
                                        </div>
                                        <hr style="margin-top:-6px; margin-bottom:10px;" />
                                    </div>
                                    <div v-if="templatesData[template.number].editOptions.includes('centerize')">
                                        <b-row style="margin-top:30px;">
                                            <b-col cols="6" style="text-align:center">
                                                <button @click="centerizeVertically(template.number)" class="centerizeButton">
                                                    وسط عمودی
                                                </button>
                                            </b-col>
                                            <b-col cols="6" style="text-align:center">
                                                <button @click="centerizeHorizontally(template.number)" class="centerizeButton">
                                                    وسط افقی
                                                </button>
                                            </b-col>
                                        </b-row>
                                        <hr />
                                    </div>
                                    <div v-if="templatesData[template.number].editOptions.includes('allColours')">
                                        <div style="display:flex; text-align:right; width:100%; justify-content:space-between; align-items:center;">
                                            <div style="display:flex; align-items:center; width:100%">
                                                <b-dropdown class="SketchDropdown" style="width:100%;">
                                                    <span slot="text" style="width:100%; box-shadow:none;" class="colorSketchDropdownText">
                                                        <div style="display:flex; justify-content:space-between; width:100%; align-items:center;">
                                                            <h6 style="text-align:right;">رنگ پس زمینه</h6>
                                                            <div class="sketchCircleClass" :style="sketchCircleStyle(template.number)"></div>
                                                        </div>
                                                    </span>
                                                    <b-dropdown-item>
                                                        <Sketch :value="colors" @input="changeSketchColor($event.hex, template.number)" class="SketchStyle"></Sketch>
                                                    </b-dropdown-item>
                                                </b-dropdown>

                                            </div>
                                        </div>
                                        <hr />
                                    </div>
                                    <div v-if="templatesData[template.number].editOptions.includes('specificColour')">
                                        <h6 style="margin-bottom:20px; text-align:right;">رنگ پس زمینه</h6>
                                        <div style="display:flex; width:100%; justify-content:space-around; align-items:center;">
                                            <div @click="changeSketchColor(color, template.number)" v-for="color in templatesData[template.number].specificColors" :style="{cursor:'pointer', 'background-color':color, width:'20px', height:'20px', 'border-radius':'50%', border: '1px solid gray'}"></div>
                                        </div>
                                        <hr />
                                    </div>
                                    <div v-if="templatesData[template.number].editOptions.includes('repeat')">
                                        <b-dropdown class="repeatDropdown">
                                            <span slot="text"><h6>نوع تکرار طرح</h6></span>
                                            <b-dropdown-item @click="setRepeat(false, template.number, 'good')">
                                                <div style="display:flex; align-items:center; justify-content:flex-end; margin-right:-8px; margin-left:-5px;">
                                                    <h6 class="mr-2">بدون تکرار</h6>
                                                    <div style="width:25px; height:25px;">
                                                        <svg viewBox="0 0 1000 1000"><title>option-none</title><path fill="none" d="M0 0h1000v1000H0z"></path><path d="M700 200H300a25 25 0 0 0-25 25v550a25 25 0 0 0 25 25h400a25 25 0 0 0 25-25V225a25 25 0 0 0-25-25zm0 575H300V225h400z" fill="#8c95a5"></path></svg>
                                                    </div>
                                                </div>
                                            </b-dropdown-item>
                                            <b-dropdown-item @click="setRepeat(true, template.number, 'good')">
                                                <div style="display:flex; align-items:center; justify-content:flex-end; margin-right:-8px; margin-left:-5px;">
                                                    <h6 class="mr-2">منظم</h6>
                                                    <div style="width:25px; height:25px;">
                                                        <svg viewBox="0 0 1000 1000"><title>option-regular</title><path fill="none" d="M0 0h1000v1000H0z"></path><path d="M425 175H275a25 25 0 0 0-25 25v225a25 25 0 0 0 25 25h150a25 25 0 0 0 25-25V200a25 25 0 0 0-25-25zm0 250H275V200h150zm300-250H575a25 25 0 0 0-25 25v225a25 25 0 0 0 25 25h150a25 25 0 0 0 25-25V200a25 25 0 0 0-25-25zm0 250H575V200h150zM425 550H275a25 25 0 0 0-25 25v225a25 25 0 0 0 25 25h150a25 25 0 0 0 25-25V575a25 25 0 0 0-25-25zm0 250H275V575h150zm300-250H575a25 25 0 0 0-25 25v225a25 25 0 0 0 25 25h150a25 25 0 0 0 25-25V575a25 25 0 0 0-25-25zm0 250H575V575h150z" fill="#8c95a5"></path></svg>
                                                    </div>

                                                </div>
                                            </b-dropdown-item>
                                            <b-dropdown-item @click="setRepeat(true, template.number, 'bad')">
                                                <div style="display:flex; align-items:center; justify-content:flex-end; margin-right:-8px; margin-left:-5px;">
                                                    <h6 class="mr-2">به هم ریخته</h6>
                                                    <div style="width:25px; height:25px;">
                                                        <svg viewBox="0 0 1000 1000"><title>option-offset</title><path fill="none" d="M0 0h1000v1000H0z"></path><path d="M575 175H425a25 25 0 0 0-25 25v225a25 25 0 0 0 25 25h150a25 25 0 0 0 25-25V200a25 25 0 0 0-25-25zm0 250H425V200h150zm-275-75H150a25 25 0 0 0-25 25v225a25 25 0 0 0 25 25h150a25 25 0 0 0 25-25V375a25 25 0 0 0-25-25zm0 250H150V375h150zm550-250H700a25 25 0 0 0-25 25v225a25 25 0 0 0 25 25h150a25 25 0 0 0 25-25V375a25 25 0 0 0-25-25zm0 250H700V375h150zm-275-50H425a25 25 0 0 0-25 25v225a25 25 0 0 0 25 25h150a25 25 0 0 0 25-25V575a25 25 0 0 0-25-25zm0 250H425V575h150z" fill="#8c95a5"></path></svg>
                                                    </div>

                                                </div>
                                            </b-dropdown-item>
                                        </b-dropdown>
                                        <hr />
                                    </div>
                                    <div style="display:flex; justify-content:space-between; align-items:center; width:100%; direction:rtl">
                                        <label :for="'printAreaImage' + template.number" style="text-align: right; width: 108px;font-size: 1rem;font-weight: 500;line-height: 1.2;float: right;color: black;text-shadow: #fff 0 1px 0;">
                                            درصد سود&nbsp;
                                        </label>
                                        <input placeholder="درصد سود" class="productPrefitInput" v-model="templatesData[template.number].profit" type="number" step="0.1" min="0" :id="'printAreaImage' + template.number" value="0">
                                    </div>
                                    <div style="display:flex; justify-content:center; align-items:center; width:100%;">
                                        <b-button :disabled="templatesData[template.number].previewLoading" @click="updatePreview(template, template.number, true)" class="applyButton">
                                            اعمال تغییرات<b-spinner v-if="templatesData[template.number].previewLoading" style="margin-left:5px; width:1rem; height:1rem;" variant="light" label="Spinning"></b-spinner>
                                        </b-button>
                                    </div>
                                </div>
                            </b-container>
                        </b-col>
                    </b-row>

                </div>
                <div v-else>
                    <b-media class="template" style="padding:20px; text-align:center; border-radius:10px">
                        <div style="height:300px; border-radius:5px; display: flex; justify-content: center; align-items: center;">
                            <b-spinner v-if="templatesData[template.number].previewLoading" style="position:absolute; z-index:20;" variant="primary" label="Spinning"></b-spinner>
                            <img :class="{lessOpacity:templatesData[template.number].previewLoading}" v-if="templatesData[template.number].previewImage != ''" :src="templatesData[template.number].previewImage" :alt="templates.name" @click="updatePreview(template, template.number, true)" style="max-width:100%; max-height:100%; position: relative; border-radius:5px; z-index:1;" />
                            <img :class="{lessOpacity:templatesData[template.number].previewLoading}" v-else :src="template.template.thumbnail" :alt="templates.name" @click="updatePreview(template, template.number, true)" style="max-width:100%; max-height:100%; position: relative; border-radius:5px; z-index:1;" />
                        </div>

                        <h4 class="mt-2" @click="updatePreview(template, template.number, true)" style="margin-bottom:20px; cursor:pointer;">{{ template.template.name }}</h4>
                        <div style="width:100%; display:flex; justify-content:center;">
                            <div style="width:190px; display: flex; justify-content:space-between; align-items:center;">
                                <b-button :id="'scrollToHere' + template.number" @click="updatePreview(template, template.number, true)" class="editButton">
                                    <div style="display:flex; align-items:center; height:100%; margin-top:-5px;">
                                        تغییر
                                        <i class="fas fa-edit ml-2"></i>
                                    </div>
                                </b-button>
                                <b-button v-if="templatesData[template.number].isEnabled" @click="enableDisable(template.number, 'disable')" class="enableButton">
                                    <div style="display:flex; align-items:center; height:100%; margin-top:-4px;">
                                        فعال
                                        <font-awesome-icon icon="check" class="ml-1" />
                                    </div>
                                </b-button>
                                <b-button v-else @click="enableDisable(template.number, 'enable')" class="disableButton">
                                    <div style="display:flex; align-items:center; height:100%; margin-top:-4px;">
                                        انتخاب
                                        <!--<font-awesome-icon style="font-size:14px;" icon="times" class="ml-1" />-->
                                    </div>
                                </b-button>
                            </div>
                        </div>
                    </b-media>
                </div>
            </b-col>
        </b-row>
        <b-row>
            <h5 id="widthDommy" style="color:transparent"></h5>
            <h5 id="heightDommy" style="color:transparent"></h5>
            <h5 id="urlDommy" style="color:transparent"></h5>
        </b-row>
        <b-row v-if="showList" style="display:flex; justify-content:center">
            <b-button @click="finalizeOrder()" class="finalizeOrder" :disabled="isFinalizing">
                <div v-if="isFinalizing" style="display:flex; align-items:center; height:100%; margin-left:-26px; margin-top:-27px;">
                    <b-spinner style="width:2rem; height:2rem ;position:absolute; z-index:20;" variant="primary" label="Spinning"></b-spinner>
                </div>
                <div v-else style="display:flex; align-items:center; height:100%; margin-top:-5px;">
                    سفارش نهایی
                </div>
            </b-button>
        </b-row>


        <b-row v-if="showList" class="mt-4" style="width: 100%!important; margin:0px 30px 0px 0px;">
            <b-col class="mb-4" :cols="template.cols" v-for="template in templates" :key="'FirstPreview' + template.template.id">
                <div v-if="template.isComponent">

                    <!--GOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO-->

                    <b-row style="position:fixed; top:-1000px; right:-1000px;">
                        <b-col cols="12" v-if="templatesData[template.number].previewLoadingForClosing" style=" height:600px; position:absolute; display:flex; justify-content:center; align-items:center; background-color: gray; opacity:0.6; z-index:10000">
                            <b-spinner style="width:6rem; height:6rem ;position:absolute; z-index:20;" variant="primary" label="Spinning"></b-spinner>
                        </b-col>

                        <b-col cols="8" style="display:flex; justify-content:center;">
                            <transition name="fade" mode="out-in">
                                <div :id="'wholeTemplateFirstPreview' + template.number" style="width:600px; height:600px; display:flex; justify-content:center;">
                                    <b-container @mouseup="updatePreview(null, template.number, false)" class="coverClass" :style="templateBGStyle(template.number)">
                                        <div :id="'colorPicFirstPreview' + template.number" v-if="Boolean(templatesData[template.number].designAddress)" :style="colorMakerStyle(template.number)">
                                        </div>
                                        <div :class="{noBorder:templatesData[template.number].previewLoading}" :style="templatePrintAreaStyle(template.number)">
                                            <VueDragResize :isResizable="false" :w="templatesData[template.number].isRepeat ? 2222 : templatesData[template.number].picWidth" :h="templatesData[template.number].isRepeat ? 2222 : templatesData[template.number].picHeight" :x="templatesData[template.number].left" :y="templatesData[template.number].top" v-on:dragging="resize($event,template.number)" style="cursor:grab">
                                                <div :id="'printAreaImageFirstPreview' + template.number" v-if="Boolean(templatesData[template.number].designAddress) && (templatesData[template.number].repeatType === 'bad')" style="width:100%; height: 100%; z-index: -1; position: relative; display:flex;">
                                                    <div :style="{display:'flex', height:'100%', width:(templatesData[template.number].picWidth*2) + 'px'}" v-for="i in numberOfBackgrounds(template.number)">
                                                        <div :style="badBackgroundImageFirstColumn(template.number)"></div>
                                                        <div :style="badBackgroundImageSecondColumn(template.number)"></div>
                                                    </div>
                                                </div>
                                                <div :id="'printAreaImageFirstPreview' + template.number" v-else-if="Boolean(templatesData[template.number].designAddress) && (templatesData[template.number].isRepeat)" style="width:100%; height: 100%; z-index: -1; position: relative; display:flex;">
                                                    <div :style="{display:'flex', height:'100%', width:(templatesData[template.number].picWidth*2) + 'px'}" v-for="i in numberOfBackgrounds(template.number)">
                                                        <div :style="badBackgroundImageFirstColumn(template.number)"></div>
                                                        <div :style="badBackgroundImageFirstColumn(template.number)"></div>
                                                    </div>
                                                </div>
                                                <div :id="'printAreaImageFirstPreview' + template.number" v-else-if="Boolean(templatesData[template.number].designAddress)" :class="{coverClass:!templatesData[template.number].isRepeat}" :style="draggableImageBackground(template.number)"></div><!--@mouseup="updatePreview"-->
                                                <!--<div :id="'printAreaImage' + template.number" v-else-if="Boolean(templatesData[template.number].designAddress)" :class="{coverClass:!templatesData[template.number].isRepeat}" :style="draggableImageBackground(template.number)"></div>--><!--@mouseup="updatePreview"-->
                                                <!--<img v-if="Boolean(templatesData[template.number].designAddress)" :src="templatesData[template.number].designAddress" style="width:100%; height: 100%; z-index: -1; position: relative; display: block;" />-->
                                                <img :id="'printAreaImageFirstPreview' + template.number" v-else :src="chooseFileAddress" style="border-radius:50%; width:100%; height: 100%; z-index: -1; position: relative; display: block;" /><!--@mouseup="updatePreview"-->
                                            </VueDragResize>
                                        </div>
                                    </b-container>
                                </div>
                            </transition>
                        </b-col>
                        <b-col cols="4">

                        </b-col>
                    </b-row>

                    <!--GOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO-->
                </div>
            </b-col>
        </b-row>

    </b-col>

</template>

<script>
    import Nprogress from 'nprogress';
    import domtoimage from 'dom-to-image';
    import axios from "axios";
    import VueDragResize from 'vue-drag-resize';
    import Gallery from "../product/Gallery.vue";
    import AuthModal from "../app/AuthModal.vue";
    import VueSlider from "vue-slider-component";
    import Jimp from 'jimp';
    import { setTimeout } from 'timers';
    import Slider from "vue-custom-range-slider";
    import "vue-custom-range-slider/dist/vue-custom-range-slider.css";
    import { Sketch } from 'vue-color';
    import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

    export default {
        name: "created-template-list",
        components: {
            VueDragResize,
            Gallery,
            VueSlider,
            AuthModal,
            Jimp,
            Slider,
            Sketch,
            FontAwesomeIcon,
        },
        data() {
            return {
                colors: '#194d33',
                showList: false,
                templatesData: [],
                isReplace: false,
                templateNumber: 0,
                designPath: null,
                tempDesignPath: null,
                designAddress: null,
                tempDesignAddress: null,
                picWidth: 120,
                picHeight: 120,
                pictureWidth: 100,
                pictureHeight: 100,
                isEmpty: false,
                isPreviewReady: false,
                chooseFileAddress: "/assets/images/chooseFileAddress.jpg",
                uploadIconAddress: "/assets/images/uploadIcon.svg",
                myDrop: null,
                tags: null,
                designInfoName: "",
                designInfoTags: "",
                designInfoDescription: "",
                isFinalizing: false
            };
        },
        props: {
            templates: {
                type: Array,
                required: true
            },
            actualTemplates: {
                type: Array,
                reqired: true
            }
        },
        methods: {
            applyChanges(template) {
                this.updatePreview(template, template.number, true);
            },
            getSelectedTags() {
                var vm = this;
                var selected = [];
                for (var i = 0; i < vm.tags.length; i++) {
                    if (vm.myDrop.isSelected(i)) {
                        var toBePushed = {};
                        toBePushed.id = vm.tags[i].id;
                        toBePushed.name = vm.tags[i].name;
                        toBePushed.slug = vm.tags[i].slug;
                        selected.push(toBePushed);
                    }
                }
                return selected;
            },
            finalizeOrder() {
                var vm = this;
                if (vm.designInfoName == "") {
                    vm.$toastr("error", "نام طرح نمی تواند خالی باشد");
                    return;
                }
                vm.isFinalizing = true;
                var i;
                var files = [];
                for (i = 0; i < vm.templatesData.length; i++) {
                    if (vm.templatesData[i].isEnabled) {
                        var fileForm = new FormData();
                        var file = vm.dataUrlToFile(vm.templatesData[i].previewImage, vm.actualTemplates[i].slug + ".png");
                        fileForm.set("file", file, file.name);
                        files.push([i, fileForm])

                    }
                }
                if (files.length == 0) {
                    vm.$toastr("error", "محصولی انتخاب نشده است");
                    vm.isFinalizing = false;
                    return;
                }
                var mediaApiUrl = "/api/images/uploadUserDesign";
                //var mediaApiUrl = "https://dev.modopod.ir/wp-json/wp/v2/media";
                var productsApiUrl = "https://modopod.ir/wp-json/wc/v3/products/";
                //var productsApiUrl = "https://dev.modopod.ir/wp-json/wc/v3/products";
                var count = 0;
                for (let index = 0; index < files.length; index++) {
                    axios.post(mediaApiUrl, files[index][1],
                        {
                            headers: {
                                'Authorization': 'Bearer ' + localStorage.getItem('accessToken'),
                                'Access-Control-Allow-Origin': '*',
                                'Content-Type': 'image/png',
                                'Content-Disposition': 'attachment;'
                            }
                        })
                        .then(response => {
                            var postData = {};
                            //var images = [];
                            //var image = {};
                            //image.src = "https://modopod.ir/wp-content/uploads/2020/06/Mo-13.jpg" //"https://plugin.modopod.ir" + response.data.designAddress;
                            //images.push(image);
                            //postData.images = images;
                            postData.description = vm.designInfoDescription
                            postData.regular_price = (vm.templatesData[files[index][0]].price * (100 + vm.templatesData[files[index][0]].profit) / 100).toString();
                            postData.slug = vm.templatesData[files[index][0]].name + "-" + vm.designInfoName;
                            postData.name = vm.templatesData[files[index][0]].name + " با طرح " + vm.designInfoName;
                            postData.type = "simple";
                            //postData.tags = vm.getSelectedTags();
                            postData.tags = vm.templatesData[files[index][0]].tags;
                            //postData.categories = [
                            //    {
                            //        "id": 200
                            //    }
                            //];
                            postData.categories = vm.templatesData[files[index][0]].categories;
                            postData.images = [{
                                "src": "https://modopod.ir/wp-content/uploads/2020/06/Mo-13.jpg", //"https://plugin.modopod.ir" + response.data.designAddress;
                                "position": "0"
                            }]
                            axios.post(productsApiUrl,
                                postData,
                                {
                                    headers: {
                                        'Authorization': 'Bearer ' + localStorage.getItem('wpToken'),
                                        'Access-Control-Allow-Origin': '*',
                                        'Content-Type': 'application/json'
                                    }
                                })
                                .then(response => {
                                    count += 1;
                                    if (count == files.length) {
                                        vm.$toastr("success", "محصولات با موفقیت اضافه شدند");
                                        vm.isFinalizing = false;
                                    }
                                }).catch(response => {
                                    count += 1;
                                    if (count == files.length) {
                                        vm.isFinalizing = false;
                                    }
                                });
                        }).catch(response => {
                            count += 1;
                            if (count == files.length) {
                                vm.isFinalizing = false;
                            }
                        });
                }

            },
            dataUrlToFile(dataurl, filename) {
                var arr = dataurl.split(','),
                    mime = arr[0].match(/:(.*?);/)[1],
                    bstr = atob(arr[1]),
                    n = bstr.length,
                    u8arr = new Uint8Array(n);
                while (n--) {
                    u8arr[n] = bstr.charCodeAt(n);
                }
                return new File([u8arr], filename, { type: mime });
            },
            updatePreview(template, num, isViewing) {
                var vm = this;
                var isThisDirty = vm.templatesData[num].isDirty;
                if (isViewing && isThisDirty) {
                    vm.view(template, false);
                    return;
                }
                vm.templatesData[num].previewLoading = true;
                if (isViewing) {
                    vm.templatesData[num].previewLoadingForClosing = true;
                    vm.view(template, true);
                }
                //var node = document.getElementById('printAreaImage' + num);
                else {
                    this.$nextTick(function () {
                        var node = document.getElementById('wholeTemplate' + num);
                        domtoimage.toPng(node)
                            .then(function (dataUrl) {
                                vm.templatesData[num].previewImage = dataUrl;
                                vm.templatesData[num].previewLoading = false;
                            })
                            .catch(function (error) {
                                console.error('oops, something went wrong with domtoimage!', error);
                            });
                    })
                }
                //console.log(node);
                //domtoimage.toPng(node)
                //    .then(function (dataUrl) {
                //        Jimp.read(dataUrl)
                //            .then(image => {
                //                console.log(dataUrl);
                //                if (vm.templatesData[num].isRepeat) {
                //                    image.crop((-1) * parseInt( vm.templatesData[num].left ), (-1) * parseInt( vm.templatesData[num].top ), vm.templatesData[num].templatePrintAreaWidth, vm.templatesData[num].templatePrintAreaHeight);
                //                    image.resize(parseInt(vm.templatesData[num].templatePrintAreaWidth/vm.templatesData[num].ratio), parseInt(vm.templatesData[num].templatePrintAreaHeight / vm.templatesData[num].ratio));
                //                    image.getBase64(Jimp.AUTO, (err, res) => {
                //                            var colorPic = document.getElementById('colorPic' + num);
                //                            domtoimage.toPng(colorPic).then(function (colorPicDataUrl) {
                //                                Jimp.read(colorPicDataUrl).then(colorPicImage => {
                //                                    colorPicImage.resize(parseInt(vm.templatesData[num].templateWidth / vm.templatesData[num].ratio), parseInt(vm.templatesData[num].templateHeight / vm.templatesData[num].ratio));
                //                                    colorPicImage.getBase64(Jimp.AUTO, (err2, colorPicRes) => {
                //                                        mergeImages([
                //                                            { src: colorPicRes, x: 0, y: 0 },
                //                                            { src: res, x: parseInt(vm.templatesData[num].templatePrintAreaLeft / vm.templatesData[num].ratio ), y: parseInt(vm.templatesData[num].templatePrintAreaTop / vm.templatesData[num].ratio )},
                //                                            { src: vm.templatesData[num].templateImageDataUrl, x: 0, y: 0 }
                //                                        ])
                //                                            .then(b64 => {
                //                                                //console.log(b64);
                //                                                vm.templatesData[num].previewImage = b64;
                //                                                vm.templatesData[num].previewLoading = false;
                //                                                vm.view(template);
                //                                            });
                //                                    });
                //                                });
                //                            });
                //                    })
                //                }
                //                else {
                //                    console.log(vm.templatesData[num].left, vm.templatesData[num].templatePrintAreaLeft);
                //                    //image.crop();
                //                    image.resize(parseInt(vm.templatesData[num].picWidth / vm.templatesData[num].ratio), parseInt(vm.templatesData[num].picHeight  / vm.templatesData[num].ratio));
                //                    image.getBase64(Jimp.AUTO, (err, res) => {
                //                            var colorPic = document.getElementById('colorPic' + num);
                //                            domtoimage.toPng(colorPic).then(function (colorPicDataUrl) {
                //                                Jimp.read(colorPicDataUrl).then(colorPicImage => {
                //                                    colorPicImage.resize(parseInt(vm.templatesData[num].templateWidth / vm.templatesData[num].ratio), parseInt(vm.templatesData[num].templateHeight / vm.templatesData[num].ratio));
                //                                    colorPicImage.getBase64(Jimp.AUTO, (err, colorPicRes) => {
                //                                        mergeImages([
                //                                            { src: colorPicRes, x: 0, y: 0 },
                //                                            { src: res, x: parseInt((vm.templatesData[num].templatePrintAreaLeft + vm.templatesData[num].left) / vm.templatesData[num].ratio), y: parseInt((vm.templatesData[num].templatePrintAreaTop + vm.templatesData[num].top) / vm.templatesData[num].ratio) },
                //                                            { src: vm.templatesData[num].templateImageDataUrl, x: 0, y: 0 }
                //                                        ])
                //                                            .then(b64 => {
                //                                                //console.log(b64);
                //                                                vm.templatesData[num].previewImage = b64;
                //                                                vm.templatesData[num].previewLoading = false;
                //                                                vm.view(template);
                //                                            });
                //                                    });
                //                                });
                //                            });


                //                    })
                //                }
                //            //image.resize(vm.templatesData[num].picWidth, vm.templatesData[num].picHeight);
                //            //image.getBase64(Jimp.AUTO, (err, res) => {
                //            //    vm.templatesData[num].previewImage = res;
                //            //    document.getElementById("urlDommy").innerHTML = res;
                //            //})
                //        })
                //        .catch(err => {
                //            throw err;
                //        });
                //        //vm.templatesData[num].previewImage = dataUrl;
                //})
                //.catch(function (error) {
                //    console.error('oops, something went wrong with domtoimage!', error);
                //});
            },
            enableDisable(num, enableOrDisable) {
                if (enableOrDisable == 'enable') {
                    this.templatesData[num].isEnabled = true;
                }
                else {
                    this.templatesData[num].isEnabled = false;
                }
            },
            numberOfBackgrounds(num) {
                return parseInt(2222 / (this.templatesData[num].picWidth * 2));
            },
            setRepeat(repeatMode, num, repeatType) {
                this.templatesData[num].isRepeat = repeatMode;
                if (repeatMode) {
                    this.templatesData[num].top = parseInt(this.templatesData[num].templatePrintAreaHeight / 2 - 2222 / 2);
                    this.templatesData[num].left = parseInt(this.templatesData[num].templatePrintAreaWidth / 2 - 2222 / 2);
                    this.templatesData[num].repeatType = repeatType;
                }
                else {
                    this.templatesData[num].repeatType = 'good';
                    this.templatesData[num].top = parseInt(this.templatesData[num].templatePrintAreaHeight / 2 - this.templatesData[num].picHeight / 2);
                    this.templatesData[num].left = parseInt(this.templatesData[num].templatePrintAreaWidth / 2 - this.templatesData[num].picWidth / 2);
                }
                this.updatePreview(null, num, false);
            },
            changeSketchColor(color, num) {
                this.templatesData[num].templateBackgroundColour = color;
                this.updatePreview(null, num, false);
            },
            centerizeVertically(num) {
                if (this.templatesData[num].isRepeat) {
                    this.templatesData[num].top = parseInt(this.templatesData[num].templatePrintAreaHeight / 2 - 2222 / 2);
                }
                else {
                    this.templatesData[num].top = parseInt(this.templatesData[num].templatePrintAreaHeight / 2 - this.templatesData[num].picHeight / 2);
                }
                this.updatePreview(null, num, false);
            },
            centerizeHorizontally(num) {
                if (this.templatesData[num].isRepeat) {
                    this.templatesData[num].left = parseInt(this.templatesData[num].templatePrintAreaWidth / 2 - 2222 / 2);
                }
                else {
                    this.templatesData[num].left = parseInt(this.templatesData[num].templatePrintAreaWidth / 2 - this.templatesData[num].picWidth / 2);
                }

                this.updatePreview(null, num, false);
            },
            changeSlider(e, number) {
                this.templatesData[number].val = e;
                var prevWidth = this.templatesData[number].picWidth;
                var newWidth = parseInt(this.templatesData[number].val / 100 * (this.templatesData[number].pictureWidth) * this.templatesData[number].ratio);
                if (prevWidth !== newWidth) {
                    var changeWidth = (newWidth - prevWidth) / 2;
                    this.templatesData[number].left = this.templatesData[number].left - changeWidth;
                }
                var prevHeight = this.templatesData[number].picHeight;
                var newHeight = parseInt(this.templatesData[number].val / 100 * (this.templatesData[number].pictureHeight) * this.templatesData[number].ratio);
                if (prevHeight !== newHeight) {
                    var changeHeight = (newHeight - prevHeight) / 2;
                    this.templatesData[number].top = this.templatesData[number].top - changeHeight;
                }
                this.templatesData[number].picWidth = newWidth;
                this.templatesData[number].picHeight = newHeight;
                if (this.templatesData[number].isRepeat) {
                    this.centerizeVertically(number);
                    this.centerizeHorizontally(number);
                }
                else {
                    this.updatePreview(null, number, false);
                }


            },
            view(template, updatePrev) {
                Nprogress.start();
                var p = template.number;
                var vm = this;
                if (!vm.templatesData[p].isDirty) {
                    if (vm.actualTemplates[p].usePrintful) {
                        var o;
                        var variants = vm.actualTemplates[p].variantsListString.split("|");
                        for (o = 0; o < variants.length; o++) {
                            vm.templatesData[p].variantTemplates.push(0);
                        }
                        var placement = "";
                        if (this.actualTemplates[p].slug == "mug") {
                            placement = "default";
                        }
                        else {
                            placement = "front";
                        }
                        axios.get("https://cors-anywhere.herokuapp.com/https://api.printful.com/mockup-generator/templates/" + vm.actualTemplates[p].printfulProductId,
                            { headers: { Authorization: "Basic eDN1M3ViNHAtcnc2dC13b3N4OjU0ejAtZXlqMDF1bnVobWpq" } }).then(response => {
                                var i, j;
                                for (i = 0; i < response.data.result.variant_mapping.length; i++) {
                                    for (j = 0; j < variants.length; j++) {
                                        if (response.data.result.variant_mapping[i].variant_id == variants[j]) {
                                            var k;
                                            for (k = 0; k < response.data.result.variant_mapping[i].templates.length; k++) {
                                                if (response.data.result.variant_mapping[i].templates[k].placement == placement) {
                                                    var t;
                                                    for (t = 0; t < response.data.result.templates.length; t++) {
                                                        if (response.data.result.templates[t].template_id == response.data.result.variant_mapping[i].templates[k].template_id) {
                                                            vm.templatesData[p].variantTemplates[j] = response.data.result.templates[t]
                                                        }
                                                    }
                                                }

                                            }

                                        }
                                    }
                                }
                                vm.templatesData[p].templateBackgroundColour = vm.templatesData[p].variantTemplates[vm.templatesData[p].selectedVariant].background_color;
                                vm.templatesData[p].templateImageUrl = vm.templatesData[p].variantTemplates[vm.templatesData[p].selectedVariant].image_url;
                                if (vm.templatesData[p].variantTemplates[vm.templatesData[p].selectedVariant].template_width > vm.templatesData[p].variantTemplates[vm.templatesData[p].selectedVariant].template_height) {
                                    vm.templatesData[p].ratio = 600 / vm.templatesData[p].variantTemplates[vm.templatesData[p].selectedVariant].template_width;
                                }
                                else {
                                    vm.templatesData[p].ratio = 600 / vm.templatesData[p].variantTemplates[vm.templatesData[p].selectedVariant].template_height;
                                }
                                vm.templatesData[p].templateWidth = parseInt(vm.templatesData[p].ratio * vm.templatesData[p].variantTemplates[vm.templatesData[p].selectedVariant].template_width);
                                vm.templatesData[p].templateHeight = parseInt(vm.templatesData[p].ratio * vm.templatesData[p].variantTemplates[vm.templatesData[p].selectedVariant].template_height);
                                vm.templatesData[p].templatePrintAreaWidth = parseInt(vm.templatesData[p].ratio * vm.templatesData[p].variantTemplates[vm.templatesData[p].selectedVariant].print_area_width);
                                vm.templatesData[p].templatePrintAreaHeight = parseInt(vm.templatesData[p].ratio * vm.templatesData[p].variantTemplates[vm.templatesData[p].selectedVariant].print_area_height);
                                vm.templatesData[p].templatePrintAreaTop = parseInt(vm.templatesData[p].ratio * vm.templatesData[p].variantTemplates[vm.templatesData[p].selectedVariant].print_area_top);
                                vm.templatesData[p].templatePrintAreaLeft = parseInt(vm.templatesData[p].ratio * vm.templatesData[p].variantTemplates[vm.templatesData[p].selectedVariant].print_area_left);
                                vm.templatesData[p].picWidth = parseInt(vm.templatesData[p].ratio * vm.templatesData[p].picWidth);
                                vm.templatesData[p].picHeight = parseInt(vm.templatesData[p].ratio * vm.templatesData[p].picHeight);
                                vm.templatesData[p].top = parseInt(vm.templatesData[p].templatePrintAreaHeight / 2 - vm.templatesData[p].picHeight / 2);
                                vm.templatesData[p].left = parseInt(vm.templatesData[p].templatePrintAreaWidth / 2 - vm.templatesData[p].picWidth / 2);
                                vm.templatesData[p].designAddress = vm.designAddress;
                                vm.templatesData[p].designPath = vm.designPath;

                                axios.get(vm.templatesData[p].templateImageUrl,
                                    { responseType: 'arraybuffer' })
                                    .then(response => {
                                        vm.templatesData[p].templateImageDataUrl = 'data:image/png;base64, ' + new Buffer(response.data, 'binary').toString('base64');
                                    }).catch(response => console.log(response));

                                vm.$emit("changeView", template);
                                vm.templatesData[p].isDirty = true;
                                vm.$nextTick(function () {
                                    document.getElementById("scrollToHere" + p).scrollIntoView({ behavior: "smooth", block: "start" });
                                });
                                if (updatePrev) {
                                    this.$nextTick(function () {
                                        var node = document.getElementById('wholeTemplate' + p);
                                        domtoimage.toPng(node)
                                            .then(function (dataUrl) {
                                                vm.templatesData[p].previewImage = dataUrl;
                                                vm.templatesData[p].previewLoading = false;
                                                vm.templatesData[p].previewLoadingForClosing = false;
                                            })
                                            .catch(function (error) {
                                                console.error('oops, something went wrong with domtoimage!', error);
                                            });
                                    });
                                }

                                //vm.updatePreview(null, p, false);
                                //document.getElementById("scrollToHere" + p).scrollIntoView({behavior: "smooth", block: "start"});
                                Nprogress.done();
                            }).catch(r => {
                                console.log("something went wrong with printful", r);
                                Nprogress.done();
                            });
                    }
                    else {
                        vm.templatesData[p].templateBackgroundColour = vm.actualTemplates[p].templateBackgroundColour;
                        vm.templatesData[p].templateImageUrl = vm.actualTemplates[p].templateImageUrl;
                        vm.templatesData[p].templateImageDataUrl = vm.actualTemplates[p].templateImageUrl;
                        if (vm.actualTemplates[p].templateWidth > vm.actualTemplates[p].templateHeight) {
                            vm.templatesData[p].ratio = 600 / vm.actualTemplates[p].templateWidth;
                        }
                        else {
                            vm.templatesData[p].ratio = 600 / vm.actualTemplates[p].templateHeight;
                        }
                        vm.templatesData[p].templateWidth = parseInt(vm.templatesData[p].ratio * vm.actualTemplates[p].templateWidth);
                        vm.templatesData[p].templateHeight = parseInt(vm.templatesData[p].ratio * vm.actualTemplates[p].templateHeight);
                        vm.templatesData[p].templatePrintAreaWidth = parseInt(vm.templatesData[p].ratio * vm.actualTemplates[p].templatePrintAreaWidth);
                        vm.templatesData[p].templatePrintAreaHeight = parseInt(vm.templatesData[p].ratio * vm.actualTemplates[p].templatePrintAreaHeight);
                        vm.templatesData[p].templatePrintAreaTop = parseInt(vm.templatesData[p].ratio * vm.actualTemplates[p].templatePrintAreaTop);
                        vm.templatesData[p].templatePrintAreaLeft = parseInt(vm.templatesData[p].ratio * vm.actualTemplates[p].templatePrintAreaLeft);
                        vm.templatesData[p].picWidth = parseInt(vm.templatesData[p].ratio * vm.templatesData[p].picWidth);
                        vm.templatesData[p].picHeight = parseInt(vm.templatesData[p].ratio * vm.templatesData[p].picHeight);
                        vm.templatesData[p].top = parseInt(vm.templatesData[p].templatePrintAreaHeight / 2 - vm.templatesData[p].picHeight / 2);
                        vm.templatesData[p].left = parseInt(vm.templatesData[p].templatePrintAreaWidth / 2 - vm.templatesData[p].picWidth / 2);
                        vm.templatesData[p].designAddress = vm.designAddress;
                        vm.templatesData[p].designPath = vm.designPath;
                        vm.showList = true;
                        vm.$emit("changeView", template);
                        vm.templatesData[p].isDirty = true;
                        //document.getElementById("scrollToHere" + p).scrollIntoView({behavior: "smooth", block: "start"});
                        vm.$nextTick(function () {
                            document.getElementById("scrollToHere" + p).scrollIntoView({ behavior: "smooth", block: "start" });
                        });
                        if (updatePrev) {

                            vm.$nextTick(function () {
                                var node = document.getElementById('wholeTemplate' + p);
                                domtoimage.toPng(node)
                                    .then(function (dataUrl) {
                                        vm.templatesData[p].previewImage = dataUrl;
                                        vm.templatesData[p].previewLoading = false;
                                        vm.templatesData[p].previewLoadingForClosing = false;
                                    })
                                    .catch(function (error) {
                                        console.error('oops, something went wrong with domtoimage!', error);
                                    });
                            });

                        }
                        //this.updatePreview(null, p, false);
                        Nprogress.done();
                    }

                }
                else {
                    this.$emit("changeView", template);
                    //document.getElementById("scrollToHere" + p).scrollIntoView({behavior: "smooth", block: "start"});
                    //this.updatePreview(null, p, false);
                    vm.$nextTick(function () {
                        document.getElementById("scrollToHere" + p).scrollIntoView({ behavior: "smooth", block: "start" });
                    });
                    Nprogress.done();
                }
                //vm.templatesData[p].val = vm.templatesData[p].val + 1;
            },
            resize(newRect, number) {
                this.templatesData[number].top = newRect.top;
                this.templatesData[number].left = newRect.left;
                //this.updatePreview(null, number, false);
            },
            saveFile(e, is_Replace, num) {
                if (e.target.files.length !== 0) {
                    if (!is_Replace) {
                        this.showList = false;
                    }
                    this.$emit("closeAll");
                    //-------------- upload to api folder -----------------
                    var files = new FormData();
                    files.append("files", e.target.files[0], e.target.files[0].name);
                    var vm = this;
                    axios.post('/api/images/uploadUserDesign', files,
                        {
                            headers: {
                                'content-type': 'application/json'
                            }
                        }).then(response => {
                            if (is_Replace) {
                                this.tempDesignPath = response.data.designPathFolder;
                                this.tempDesignAddress = response.data.designPath;
                            }
                            else {
                                vm.designPath = response.data.designPathFolder;
                                vm.designAddress = response.data.designPath;
                            }

                        }).catch(err => console.log(err));

                    this.isReplace = is_Replace;
                    this.isEmpty = false;
                    this.templateNumber = num;
                    //----------------- get width and height of image -------------------
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
                }
                else {
                    this.isEmpty = true;
                    document.getElementById("widthDommy").innerHTML = "test";
                    document.getElementById("heightDommy").innerHTML = "test";
                }
            },
            myInputHandler() {
                if (document.getElementById("widthDommy").innerHTML == "" || document.getElementById("heightDommy").innerHTML == "") {
                    setTimeout(this.myInputHandler, 100);
                }
                else {
                    //this.$nextTick(function () {
                    if (this.isEmpty) {
                        document.getElementById("widthDommy").innerHTML = "";
                        document.getElementById("heightDommy").innerHTML = "";
                        return;
                    }
                    else {
                        var myWidth = parseInt(document.getElementById("widthDommy").innerHTML);
                        var myHeight = parseInt(document.getElementById("heightDommy").innerHTML);
                        if (this.isReplace) {
                            this.templatesData[this.templateNumber].pictureWidth = myWidth;
                            this.templatesData[this.templateNumber].pictureHeight = myHeight;
                            this.templatesData[this.templateNumber].isRepeat = false;
                            this.templatesData[this.templateNumber].repeatType = "good";
                            this.templatesData[this.templateNumber].picWidth = parseInt(this.templatesData[this.templateNumber].ratio * myWidth * this.templatesData[this.templateNumber].val / 100);
                            this.templatesData[this.templateNumber].picHeight = parseInt(this.templatesData[this.templateNumber].ratio * myHeight * this.templatesData[this.templateNumber].val / 100);
                            this.templatesData[this.templateNumber].top = parseInt(this.templatesData[this.templateNumber].templatePrintAreaHeight / 2 - this.templatesData[this.templateNumber].picHeight / 2);
                            this.templatesData[this.templateNumber].left = parseInt(this.templatesData[this.templateNumber].templatePrintAreaWidth / 2 - this.templatesData[this.templateNumber].picWidth / 2);
                        }
                        else {
                            this.pictureWidth = myWidth;
                            this.pictureHeight = myHeight;
                            this.picWidth = myWidth;
                            this.picHeight = myHeight;
                        }
                        document.getElementById("widthDommy").innerHTML = "";
                        document.getElementById("heightDommy").innerHTML = "";
                        if (this.isReplace) {
                            this.templatesData[this.templateNumber].designPath = this.tempDesignPath;
                            this.templatesData[this.templateNumber].designAddress = this.tempDesignAddress;
                            var vm = this;
                            var num = this.templateNumber;
                            var node = document.getElementById('wholeTemplateFirstPreview' + num);
                            domtoimage.toPng(node)
                                .then(function (dataUrl) {
                                    vm.templatesData[num].previewImage = dataUrl;
                                    vm.templatesData[num].previewLoading = false;
                                    vm.previewFirst(num + 1);
                                })
                                .catch(function (error) {
                                    console.error('oops, something went wrong with domtoimage!', error);
                                });
                        }
                        else {
                            this.makeListReady();

                            var vm = this;
                            vm.showList = true;

                            vm.$nextTick(function () {
                                var i;
                                for (i = 0; i < vm.actualTemplates.length; i++) {
                                    vm.templatesData[i].previewLoading = true;
                                }
                                vm.previewFirst(0);

                            })
                        }

                    }
                }

            },
            previewFirst(num) {
                var vm = this;
                if (num == vm.actualTemplates.length)
                    return;

                var p = num;
                vm.templatesData[p].templateBackgroundColour = vm.actualTemplates[p].templateBackgroundColour;
                vm.templatesData[p].templateImageUrl = vm.actualTemplates[p].templateImageUrl;
                vm.templatesData[p].templateImageDataUrl = vm.actualTemplates[p].templateImageUrl;
                if (vm.actualTemplates[p].templateWidth > vm.actualTemplates[p].templateHeight) {
                    vm.templatesData[p].ratio = 600 / vm.actualTemplates[p].templateWidth;
                }
                else {
                    vm.templatesData[p].ratio = 600 / vm.actualTemplates[p].templateHeight;
                }
                vm.templatesData[p].templateWidth = parseInt(vm.templatesData[p].ratio * vm.actualTemplates[p].templateWidth);
                vm.templatesData[p].templateHeight = parseInt(vm.templatesData[p].ratio * vm.actualTemplates[p].templateHeight);
                vm.templatesData[p].templatePrintAreaWidth = parseInt(vm.templatesData[p].ratio * vm.actualTemplates[p].templatePrintAreaWidth);
                vm.templatesData[p].templatePrintAreaHeight = parseInt(vm.templatesData[p].ratio * vm.actualTemplates[p].templatePrintAreaHeight);
                vm.templatesData[p].templatePrintAreaTop = parseInt(vm.templatesData[p].ratio * vm.actualTemplates[p].templatePrintAreaTop);
                vm.templatesData[p].templatePrintAreaLeft = parseInt(vm.templatesData[p].ratio * vm.actualTemplates[p].templatePrintAreaLeft);
                //vm.templatesData[p].picWidth = parseInt(vm.templatesData[p].ratio * vm.templatesData[p].picWidth);
                //vm.templatesData[p].picHeight = parseInt(vm.templatesData[p].ratio * vm.templatesData[p].picHeight);
                vm.templatesData[p].top = parseInt(vm.templatesData[p].templatePrintAreaHeight / 2 - vm.templatesData[p].picHeight / 2);
                vm.templatesData[p].left = parseInt(vm.templatesData[p].templatePrintAreaWidth / 2 - vm.templatesData[p].picWidth / 2);
                vm.templatesData[p].designAddress = vm.designAddress;
                vm.templatesData[p].designPath = vm.designPath;

                var node = document.getElementById('wholeTemplateFirstPreview' + num);
                domtoimage.toPng(node)
                    .then(function (dataUrl) {
                        vm.templatesData[num].previewImage = dataUrl;
                        vm.templatesData[num].previewLoading = false;
                        vm.previewFirst(num + 1);
                    })
                    .catch(function (error) {
                        console.error('oops, something went wrong with domtoimage!', error);
                    });

            },
            makeListReady() {
                this.templatesData = [];
                var p;
                var vm = this;
                for (p = 0; p < this.actualTemplates.length; p++) {
                    var toBePushed = {};
                    toBePushed.name = vm.actualTemplates[p].name;
                    toBePushed.designPath = null;
                    toBePushed.designAddress = null;
                    toBePushed.pictureWidth = vm.pictureWidth;
                    toBePushed.pictureHeight = vm.pictureHeight;
                    toBePushed.editOptions = vm.actualTemplates[p].editOptionsListString.split("|");
                    toBePushed.specificColors = vm.actualTemplates[p].specificColorsListString.split("|");
                    toBePushed.tags = vm.actualTemplates[p].tagsListString.split("|").map(x => { id = x });
                    toBePushed.categories = vm.actualTemplates[p].categoriesListString.split("|").map(x => { id = x });
                    toBePushed.selectedVariant = 0;
                    toBePushed.variantTemplates = [];
                    toBePushed.templateBackgroundColour = "";
                    toBePushed.templateImageUrl = "";
                    toBePushed.templateImageDataUrl = "";
                    toBePushed.templateWidth = 0;
                    toBePushed.templateHeight = 0;
                    toBePushed.templatePrintAreaWidth = 0;
                    toBePushed.templatePrintAreaHeight = 0;
                    toBePushed.templatePrintAreaTop = 0;
                    toBePushed.templatePrintAreaLeft = 0;
                    toBePushed.top = 0;
                    toBePushed.left = 0;
                    toBePushed.val = 20;
                    toBePushed.picWidth = parseInt(vm.picWidth * toBePushed.val / 100);
                    toBePushed.picHeight = parseInt(vm.picHeight * toBePushed.val / 100);
                    toBePushed.ratio = 1;
                    toBePushed.isRepeat = false;
                    toBePushed.repeatType = "good";
                    toBePushed.isEnabled = false;
                    toBePushed.isDirty = false;
                    toBePushed.previewImage = "";
                    toBePushed.previewLoading = false;
                    toBePushed.previewLoadingForClosing = false;
                    toBePushed.profit = 0;
                    toBePushed.price = vm.actualTemplates[p].price;
                    vm.templatesData.push(toBePushed);
                }
                vm.showList = true;
            },
            templateBGStyle(number) {
                return {
                    overflow: 'hidden',
                    position: 'relative',
                    width: this.templatesData[number].templateWidth + 'px',
                    height: this.templatesData[number].templateHeight + 'px',
                    background: 'url(' + this.templatesData[number].templateImageUrl + ') center center no-repeat'
                };
            },
            templatePrintAreaStyle(number) {
                return {
                    overflow: 'hidden',
                    position: 'absolute',
                    width: this.templatesData[number].templatePrintAreaWidth + 'px',
                    height: this.templatesData[number].templatePrintAreaHeight + 'px',
                    top: this.templatesData[number].templatePrintAreaTop + 'px',
                    left: this.templatesData[number].templatePrintAreaLeft + 'px',
                    //right: (this.templateWidth - this.templatePrintAreaWidth - this.templatePrintAreaLeft) + 'px',
                    border: '1px dashed rgba(0, 0, 0, 0.5)'
                };
            },
            colorMakerStyle(num) {
                return {
                    'z-index': '-100',
                    'background-color': this.templatesData[num].templateBackgroundColour,
                    position: 'absolute',
                    'z-index': '-100',
                    width: '100%',
                    height: '100%'
                };
            },
            sketchCircleStyle(number) {
                return {
                    'border-radius': '50%',
                    'background-color': this.templatesData[number].templateBackgroundColour,
                    width: '18px',
                    height: '18px',
                    border: '1px solid gray',
                    'margin-left': '5px'
                };
            },
            draggableImageBackground(number) {
                //if (this.templatesData[number].isRepeat) {
                //    return {
                //        background: 'url(' + this.templatesData[number].designAddress + ') center center repeat',
                //        'background-size': this.templatesData[number].picWidth + 'px ' + this.templatesData[number].picHeight + 'px',
                //        width:'100%',
                //        height: '100%',
                //        position: 'relative',
                //        'z-index': '-1'

                //    }
                //}
                //else {
                return {
                    background: 'url(' + this.templatesData[number].designAddress + ') center center no-repeat',
                    width: '100%',
                    height: '100%',
                    position: 'relative',
                    'z-index': '-1',
                    'background-size': 'cover!important'
                }
                //}
            },
            badBackgroundImageFirstColumn(num) {
                return {
                    height: '100%',
                    width: this.templatesData[num].picWidth + 'px',
                    background: 'url(' + this.templatesData[num].designAddress + ') repeat-y',
                    'background-size': this.templatesData[num].picWidth + 'px ' + this.templatesData[num].picHeight + 'px'
                };
            },
            badBackgroundImageSecondColumn(num) {
                return {
                    height: '100%',
                    width: this.templatesData[num].picWidth + 'px',
                    'margin-top': this.templatesData[num].picHeight / 2 + 'px',
                    background: 'url(' + this.templatesData[num].designAddress + ') repeat-y',
                    'background-size': this.templatesData[num].picWidth + 'px ' + this.templatesData[num].picHeight + 'px'
                };
            }
        },
        //mounted() {
        //    var vm = this;
        //    axios.get('https://modopod.ir/wp-json/wp/v2/tags')
        //        .then(response => {
        //            vm.tags = response.data;
        //            this.$nextTick(function () {
        //                this.myDrop = new drop({
        //                    selector: '#myMulti',
        //                });
        //            });
        //        }).catch(response => {
        //            console.log(response);
        //            vm.tags = [];
        //            this.$nextTick(function () {
        //                this.myDrop = new drop({
        //                    selector: '#myMulti',
        //                });
        //            });
        //        });
        //}
    };
</script>

<style lang="scss" scoped>
    .template {
        border: 3px solid #eee;
        img, h2

    {
        cursor: pointer;
    }

    }

    .right-side {
        border: 1px solid #dfdfdf;
        border-radius: 5px;
        padding: 5%;
    }

    .box {
        margin: 7.5%;
    }

    .notDisplay {
        display: none;
    }

    .fade-enter-active,
    .fade-leave-active {
        transition: opacity 0.4s ease-in-out;
    }

    .fade-enter,
    .fade-leave-to {
        opacity: 0;
    }

    .coverClass {
        background-size: cover !important;
    }

    .lessOpacity {
        opacity: 0.7;
    }

    .zeroOpacity {
        opacity: 0;
    }

    .noBorder {
        border: none !important;
    }

    .hidden {
        /*visibility:hidden;
        width:0;
        height:0;*/
        display: none;
        /*opacity:0;*/
    }

    .my-slider-process {
        background-color: black;
    }

    #uploadOnTop {
        display: flex;
        justify-content: center;
        width: 426px;
        height: 327px;
        align-items: center;
        border-radius: 10px;
        border: 1px solid lightgray;
        cursor: pointer;
    }

        #uploadOnTop:hover {
            background-color: lightgray;
        }

    #uploadOnTopImage {
        display: flex;
        justify-content: center;
        width: 426px;
        height: 327px;
        align-items: center;
        border-radius: 10px;
        cursor: pointer;
    }

    #onImageGray {
        display: flex;
        justify-content: center;
        align-items: flex-end;
        background-color: transparent;
        width: inherit;
        height: inherit;
        top: inherit;
        left: inherit;
        z-index: 5;
        position: absolute;
        color: transparent;
        border-radius: 5px;
    }

        #onImageGray:hover {
            background-color: rgba(211,211,211,0.6);
            color: #525252;
        }

    #replaceImageOption {
        display: flex;
        justify-content: center;
        width: 100%;
        height: 40px;
        align-items: center;
        border-radius: 20px;
        border: 1px solid lightgray;
        cursor: pointer;
    }

        #replaceImageOption:hover {
            background-color: lightgray;
        }

    .editButton {
        width: 90px;
        background-image: linear-gradient(to bottom, #e8e8e8, whitesmoke);
        color: black;
        height: 30px;
        display: flex;
        align-items: center;
        justify-content: center;
        box-shadow: none;
    }

        .editButton:hover {
            background-image: linear-gradient(to bottom, whitesmoke, #e8e8e8);
            color: red;
            box-shadow: none;
        }

    .finalizeOrder {
        width: 120px;
        background-image: linear-gradient(to bottom, #e8e8e8, whitesmoke);
        color: black;
        height: 50px;
        display: flex;
        align-items: center;
        justify-content: center;
        box-shadow: none;
    }

        .finalizeOrder:hover {
            background-image: linear-gradient(to bottom, whitesmoke, #e8e8e8);
            color: red;
            box-shadow: none;
        }

    .enableButton {
        width: 90px;
        background-image: linear-gradient(to bottom, #add98c, #7fc44c);
        color: white;
        height: 30px;
        display: flex;
        align-items: center;
        justify-content: center;
        box-shadow: none;
    }

        .enableButton:hover {
            color: white;
            background-image: linear-gradient(to bottom, #7fc44c, #add98c);
            box-shadow: none;
        }

    .disableButton {
        width: 90px;
        background-image: linear-gradient(to bottom, #e8e8e8, whitesmoke);
        color: black;
        height: 30px;
        display: flex;
        align-items: center;
        justify-content: center;
        box-shadow: none;
    }

        .disableButton:hover {
            background-image: linear-gradient(to bottom, whitesmoke, #e8e8e8);
            color: red;
            box-shadow: none;
        }

    .colorSketchDropdownText:hover {
        color: red;
    }

    .centerizeButton {
        width: 120px;
        background-image: linear-gradient(to bottom, #e8e8e8, whitesmoke);
        color: black;
        height: 30px;
        display: flex;
        align-items: center;
        justify-content: center;
        box-shadow: none;
        border: 1px solid gray;
        box-shadow: none;
        border-radius: 5px;
        outline: none;
    }

        .centerizeButton:hover {
            background-image: linear-gradient(to bottom, whitesmoke, #e8e8e8);
            color: red;
            box-shadow: none;
        }

    .applyButton {
        width: 320px;
        background-image: linear-gradient(to bottom, #ef4550, #e51522);
        color: white;
        height: 30px;
        display: flex;
        align-items: center;
        justify-content: center;
        box-shadow: none;
        border: 1px solid gray;
        box-shadow: none;
        border-radius: 5px;
        outline: none;
        padding-top: 2px;
    }

        .applyButton:hover {
            background-image: linear-gradient(to bottom, #e51522, #ef4550);
            color: white;
            box-shadow: none;
        }

    .designInfoClass {
        width: 426px;
        height: 327px;
        text-align: right;
        direction: rtl;
    }

    .designContainerNotUploaded {
        margin-top: 30px;
        text-align: center;
        display: flex;
        justify-content: center;
    }

    .designContainerUploaded {
        margin-top: 30px;
        text-align: center;
        padding: 0 20px;
        display: flex;
        justify-content: space-around;
    }

    .designInfoTitle {
        border: 2px solid #d6dadf;
        font-size: 12px;
        padding: 10px;
        margin: 8px 0 10px 0;
        background: #fff;
        border-radius: 8px;
        width: 100%;
        box-sizing: border-box;
        height: 46px;
    }

    .productPrefitInput {
        border: 2px solid #d6dadf;
        font-size: 12px;
        padding: 10px;
        margin: 8px 0 10px 0;
        background: #fff;
        border-radius: 8px;
        width: 100px;
        box-sizing: border-box;
        height: 35px;
    }

    .designInfoDescription {
        -webkit-font-smoothing: antialiased;
        -webkit-box-direction: normal;
        font-family: inherit;
        line-height: 1.15;
        overflow: auto;
        border: 2px solid #d6dadf;
        padding: 10px;
        margin: 8px 0 0 0;
        background: #fff;
        border-radius: 8px;
        width: 100%;
        box-sizing: border-box;
        font-size: 13px;
        resize: vertical;
        height: 105px;
        resize: none;
    }

    .designInfoTitleLabel {
        display: inline;
        font-size: 16px;
        font-weight: bold;
        line-height: 26px;
        color: #4b4f59;
        text-shadow: #fff 0 1px 0;
    }
</style>

<style lang="scss">

    .SketchStyle .vc-sketch-field, .vc-sketch-alpha-wrap, .vc-sketch-presets, .vc-checkerboard {
        display: none;
    }

    .SketchStyle .vc-sketch-hue-wrap {
        margin-top: 5px;
    }

    .SketchStyle .vc-sketch-controls {
        margin-bottom: 12px;
        margin-top: 3px;
    }


    .SketchStyle .vc-sketch-active-color {
        border-radius: 50%;
    }

    .SketchStyle .vc-sketch-color-wrap {
        margin-bottom: 7px;
    }

    .SketchDropdown .btn.dropdown-toggle.btn-secondary {
        color: black;
        background-color: transparent;
        border-color: transparent;
        font-size: 20px;
        outline: none;
        box-shadow: none;
    }

    .SketchDropdown .dropdown.btn-group.b-dropdown.SketchDropdown {
        outline: none;
        box-shadow: none;
    }

    .SketchDropdown .dropdown-menu.show {
        border: none;
        background-color: transparent;
        margin: 0;
        padding: 0;
    }

        .SketchDropdown .dropdown-menu.show:hover {
            border: none;
            background-color: transparent;
            margin: 0;
            padding: 0;
        }

    .SketchDropdown .dropdown-item {
        color: transparent;
        border: none;
        background-color: transparent;
        margin: 0;
        padding: 0;
    }

        .SketchDropdown .dropdown-item:hover {
            color: transparent;
            border: none;
            background-color: transparent;
            margin: 0;
            padding: 0;
        }

    .SketchDropdown .btn.dropdown-toggle.btn-secondary {
        display: flex;
        align-items: center;
        direction: rtl;
    }

    .repeatDropdown {
        width: 100%;
    }

        .repeatDropdown .btn.dropdown-toggle.btn-secondary {
            display: flex;
            justify-content: space-between;
            align-items: center;
            direction: rtl;
            color: black;
            background-color: transparent;
            box-shadow: none;
            border: none;
        }

    .btn.dropdown-toggle.btn-secondary:hover {
        color: red;
    }

    .MySlider .slider__label {
        display: none;
    }

    .MySlider .slider__track.slider__track--rectangular {
        border-radius: 10px;
        height: 5px;
    }

    .MySlider .slider__input {
        margin: 0;
    }
</style>
