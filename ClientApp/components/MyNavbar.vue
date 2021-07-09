<template>
    <div>
        <!--<b-navbar toggleable="md" type="light" variant="light">
            <b-navbar-toggle target="nav_collapse"></b-navbar-toggle>
            <b-navbar-brand to="/" style="margin-right:20px;">مدوپد</b-navbar-brand>
            <b-collapse is-nav id="nav_collapse">
                <b-navbar-nav>
                    <b-nav-item v-if="isAdmin" to="/admin" style="margin-right:20px;">ادمین</b-nav-item>
                    <b-nav-item @click="setType('')" style="margin-right:20px;">همه کالا ها</b-nav-item>

                    <b-nav-item v-for="(category, index) in categories" :key="index" style="margin-right:20px;">
                        <div @click="setType(category + '|')" @mouseenter="mouseEnterCat(index)" @mouseleave="navItemSelected = false" style="padding:10px; margin:-10px;">
                            {{category}}
                        </div>
                        <ul @mouseenter="nevItemMenuSelected = true" @mouseleave="nevItemMenuSelected = false" v-show="(selectedCat === 'cat' + index) && (navItemSelected || nevItemMenuSelected)" style="text-align:center; padding:0px; width:120px; border-radius:6%; margin-top:10px; left:inherit; position:fixed; background-color:gainsboro; z-index:99999999;">
                            <li @click="setType(category + '|' + subCategory)" class="menuListHover menuList" v-for="(subCategory, subIndex) in subCategories[index]" :key="subIndex">
                                {{subCategory}}
                            </li>
                        </ul>
                    </b-nav-item>
                </b-navbar-nav>
                <b-navbar-nav class="ml-auto mr-4">
                    <cart-summary v-if="isCustomer" />
                    <auth-nav-item />
                </b-navbar-nav>
            </b-collapse>
        </b-navbar>-->
        <section class="navbarDesktop">
            <div class="navbarFixed">
                <h2 @click="$router.push('/')" style="cursor:pointer;">مدوپد</h2>
                <div style="width:20%">
                    <search-bar class="search" />
                </div>
                <b-nav-item v-if="isAdmin" to="/admin" style="margin-right:20px;">ادمین</b-nav-item>
                <cart-summary v-if="isCustomer" />
                <auth-nav-item />
            </div>
            <div class="navbarFixed">
                <!--<div @click="setType('')" style="margin-right:20px;">همه کالا ها</div>-->
                <div v-for="(category, index) in categories" :key="index" style="position:relative">
                    <div @click="setType(category + '|')" @mouseenter="mouseEnterCat(index)" @mouseleave="navItemSelected = false" class="categoryStyle">
                        {{category}}
                    </div>
                    <div style="width:110%; height:3px; background-color:red;" v-if="($route.query.type != null) && ($route.query.type.substring(0, $route.query.type.length-1) == category)">

                    </div>
                    <!--<b-button @click="aa()">

                    </b-button>-->
                    <img :src="triangle" @mouseenter="nevItemMenuSelected = true" @mouseleave="nevItemMenuSelected = false" v-show="(selectedCat === 'cat' + index) && (navItemSelected || nevItemMenuSelected)" :class="{'subCategoryContainerArrow':true, 'leftInheritArrow':(index < parseInt(categories.length / 2)), 'rightInheritArrow':(index >= parseInt(categories.length / 2))}" />
                    <ul @mouseenter="nevItemMenuSelected = true" @mouseleave="nevItemMenuSelected = false" v-show="(selectedCat === 'cat' + index) && (navItemSelected || nevItemMenuSelected)" :class="{'subCategoryContainer':true, 'leftInherit':(index < parseInt(categories.length / 2)), 'rightInherit':(index >= parseInt(categories.length / 2))}">
                        <li @click="setType(category + '|' + subCategory)" class="menuListHover menuList" v-for="(subCategory, subIndex) in subCategories[index]" :key="subIndex" style="width:100%; padding:8px 16px; text-align:right; font-family:IranSansMed">
                            {{subCategory}}
                        </li>
                    </ul>
                </div>
            </div>
        </section>
        <auth-modal :show="showAuthModal" />
    </div>
</template>

<script>
    import AuthNavItem from "./app/AuthNavItem.vue";
    import AuthModal from "./app/AuthModal.vue";
    import CartSummary from "./cart/CartSummary.vue";
    import SearchBar from "../components/catalogue/SearchBar.vue";

export default {
        name: "my-navbar",
        data() {
            return {
                selectedCat: "",
                navItemSelected: false,
                nevItemMenuSelected: false,
                triangle: "/assets/images/subMenuArrow.svg"
            };
        },
        props: {
            categories: {
              type: Array,
              required: true
            },
            subCategories: {
              type: Array,
              required: true
            }
        },
        components: {
            AuthNavItem,
            AuthModal,
            CartSummary,
            SearchBar
        },
        methods: {
            aa() {
                console.log("boo", this.$route.query.type == null);
            },
            mouseEnterCat(index) {
                this.selectedCat = 'cat' + index;
                this.navItemSelected = true;
            },
            mouseLeave() {
                this.selected = false;
            },
            setType(value) {
                this.nevItemMenuSelected = false;
                this.navItemSelected = false;
                let query = Object.assign({}, this.$route.query);
                query.type = value;
                this.$router.push({ path: "/", query: query});
            }
        },
        computed: {
            showAuthModal() {
              return this.$store.state.showAuthModal;
            },
            isAdmin() {
              return this.$store.getters.isInRole("Admin");
            },
            isCustomer() {
              return (
                this.$store.getters.isInRole("Customer") ||
                !this.$store.getters.isAuthenticated
              );
            }
          }
        };
</script>

<style scoped>
    .menuList:hover {
        color:red;
        cursor: pointer;
    }

    .menuList {
         text-align:center;
         color:black; 
         list-style-type: none;
         width:100%;
    }

    .navbarDesktop {
        width: 100%;
        padding: 1% 2.5%;
        background-color: #FFFFFF;
    }

    .navbarFixed {
        display:flex;
        justify-content: space-between;
        flex-direction: row-reverse;
    }
    
    .categoryStyle {
         color:#40354e; 
         font-size:16px; 
         font-weight:400;
         cursor: pointer;
         margin-bottom:-20px;
         padding-bottom: 40px;
    }

    .subCategoryContainer {
        text-align:center;
        border-radius:6%;
        padding:0 16px;
        position:fixed;
        padding: 0 !important;
        box-shadow: 0 16px 64px -16px rgba(0, 0, 0, 0.6), 0 0px 8px rgba(0, 0, 0, 0.1);
        background-color:#FFFFFF; 
        z-index:10;
        white-space: nowrap;
    }

    .leftInherit {
        left:0;
        position: absolute;
    }

    .rightInherit {
        right:0;
        position: absolute;
    }

    .leftInheritArrow {
        left:16px;
        right:auto;
    }

    .rightInheritArrow {
        right:16px;
        left:auto;
    }

    .subCategoryContainerArrow {
         width:20px; 
         height:20px;
         position: absolute;
         top: auto;
         bottom: -12px;
         z-index: 20;
    }

    .search{
        display: flex;
        flex-direction: row;
        flex: 1;
    }
</style>