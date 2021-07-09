<template>
    <div>
        <b-container fluid class="page">
            <b-row>
                <b-col cols="2">
                    <b-button style="direction:rtl; display: flex; align-items:center; justify-content:space-between" class="mt-4 ml-5" variant="outline-secondary" @click.prevent="back">
                        خروج <i style="margin-right:8px" class="fas fa-arrow-left"></i>
                    </b-button>
                </b-col>
            </b-row>

            <b-row style="justify-content: center;">
                <created-template-list @changeView="view" @closeAll="closeAll"
                               :templates="templatesArrayGenerated" :actualTemplates="templatesArray" />
            </b-row>
            <b-row>
                <h5 id="widthDommy"></h5>
                <h5 id="heightDommy"></h5>
                <h5 id="urlDommy"></h5>
            </b-row>
        </b-container>
        <auth-modal :show="showAuthModal" />
    </div>
</template>

<script>
    import axios from "axios";
    import SearchBar from "../components/catalogue/SearchBar.vue";
    import TemplateList from "../components/makeProduct/CreatedTemplateList.vue";
    import MyNavbar from "../components/MyNavbar.vue";
    import Nprogress from 'nprogress';

    export default {
        name: "createdProduct",
        components: {
            SearchBar,
            TemplateList,
            MyNavbar
        },
        data() {
            return {
                response: null,
                templatesArray: [],
                computedTemplates: [],
                isAnyThingSelected: false,
                activeTemplateIndex: null
            };
        },
        methods: {
            back() {
                localStorage.removeItem('accessToken');
                localStorage.removeItem('wpToken');
                //this.$router.go(-1);
                location.replace('/login')
            },
            setData(templates, categories) {
                this.templatesArray = templates;
                this.response = categories;
            },
            closeAll() {
                var i;
                this.isAnyThingSelected = false;
                this.activeTemplateIndex = null;
                for (i = 0; i < this.computedTemplates.length; i++) {
                    if (this.computedTemplates[i].isComponent) {
                        this.computedTemplates[i].display = false;
                    }
                }
            },
            view(template) {
                if (this.isAnyThingSelected === false) {
                    var i;
                    for (i = 0; i < this.computedTemplates.length; i++) {
                        if (template.number === this.computedTemplates[i].number && this.computedTemplates[i].isComponent) {
                            this.computedTemplates[i].display = true;
                            this.isAnyThingSelected = true;
                            this.activeTemplateIndex = template.number;
                            return;
                        }
                    }
                }
                if (template.number === this.activeTemplateIndex) {
                    var i;
                    for (i = 0; i < this.computedTemplates.length; i++) {
                        if (template.number === this.computedTemplates[i].number && this.computedTemplates[i].isComponent) {
                            this.computedTemplates[i].display = false;
                            this.isAnyThingSelected = false;
                            this.activeTemplateIndex = null;
                            return;
                        }
                    }
                }
                var i;
                var count = 0;
                for (i = 0; i < this.computedTemplates.length; i++) {
                    if (this.computedTemplates[i].number === template.number && this.computedTemplates[i].isComponent) {
                        this.computedTemplates[i].display = true;
                        count++;
                    }
                    if (this.computedTemplates[i].number === this.activeTemplateIndex && this.computedTemplates[i].isComponent) {
                        this.computedTemplates[i].display = false;
                        count++;
                    }
                    if (count === 2) {
                        this.activeTemplateIndex = template.number;
                        return;
                    }
                }
                
            },
            
        },
        beforeRouteEnter(to, from, next) {
            var vm = this;
            axios
              .all([
                axios.get("/api/templates", { params: to.query }),
                   axios.get("/api/categories")
              ])
              .then(
                axios.spread((templates, categories) => {
                  next(vm => vm.setData(templates.data, categories.data));
                })
              );
        },
        beforeRouteUpdate(to, from, next) {
            axios.get("/api/templates", { params: to.query }).then(response => {
              this.templatesArray = response.data;
              next();
            });
        },
        computed: {
            computedCategories() {
                return this.response.categories;
            },
            computedSubcategories() {
                var subs = [];
                var i;
                for (i = 0; i < this.response.categories.length; i++) {
                    subs.push([]);
                    var j;
                    for (j = 0; j < this.response.subCategories.length; j++) {
                        if (this.response.subCategoriesCategory[j] == this.response.categories[i]) {
                            subs[i].push(this.response.subCategories[j]);
                        }
                    }
                }
                return subs;
            },
            templatesArrayGenerated() {
                var i = 0;
                this.computedTemplates = [];
                while (true) {
                    if (i >= this.templatesArray.length) {
                        break;
                    }
                    this.computedTemplates.push({ isComponent: false, template: this.templatesArray[i], cols: 4, display: true, number: i });
                    i++;
                    if (i >= this.templatesArray.length) {
                        this.computedTemplates.push({ isComponent: true, template: this.templatesArray[i - 1], cols: 12, display: false, number: i - 1 });
                        break;
                    }
                    this.computedTemplates.push({ isComponent: false, template: this.templatesArray[i], cols: 4, display: true, number: i });
                    i++;
                    if (i >= this.templatesArray.length) {
                        this.computedTemplates.push({ isComponent: true, template: this.templatesArray[i - 2], cols: 12, display: false, number: i - 2 });
                        this.computedTemplates.push({ isComponent: true, template: this.templatesArray[i - 1], cols: 12, display: false, number: i - 1 });
                        break;
                    }
                    this.computedTemplates.push({ isComponent: false, template: this.templatesArray[i], cols: 4, display: true, number: i });
                    i++;
                    this.computedTemplates.push({ isComponent: true, template: this.templatesArray[i - 3], cols: 12, display: false, number: i - 3 });
                    this.computedTemplates.push({ isComponent: true, template: this.templatesArray[i - 2], cols: 12, display: false, number: i - 2 });
                    this.computedTemplates.push({ isComponent: true, template: this.templatesArray[i - 1], cols: 12, display: false, number: i - 1 });
                }

                return this.computedTemplates;
            }
        }
    };
</script>

<style scoped>
    .flex {
        display: flex;
        flex-direction: row;
    }
    .flex.search {
        display: flex;
        flex-direction: row;
        flex: 1;
    }
</style>
