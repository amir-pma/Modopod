<template>
    <div style="background-color: #F2F2F6;">
        <my-navbar v-if="Boolean(response)" :categories="computedCategories" :subCategories="computedSubcategories"></my-navbar>
        <b-container fluid class="page">
            <b-row>
                <b-col cols="3">
                    <filters v-if="filters.brands.length" :filters="filters" />
                </b-col>
                <b-col cols="9">
                    <div class="mt-4 flex">
                        <search-bar class="search" />
                        <product-sort class="ml-4" />
                    </div>
                    <product-list :products="sortedProducts" />
                </b-col>
            </b-row>
        </b-container>
    </div>

</template>

<script>
import axios from "axios";
import Filters from "../components/catalogue/Filters.vue";
import SearchBar from "../components/catalogue/SearchBar.vue";
import ProductSort from "../components/catalogue/ProductSort.vue";
import ProductList from "../components/catalogue/ProductList.vue";
import MyNavbar from "../components/MyNavbar.vue";

    export default {
  name: "catalogue",
  components: {
    Filters,
    SearchBar,
    ProductSort,
      ProductList,
    MyNavbar
  },
  data() {
      return {
        response: null,
      products: [],
      filters: {
        brands: [],
        capacity: [],
        colours: [],
        os: [],
        features: []
      }
    };
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
    sort() {
      return this.$route.query.sort || 0;
    },
    sortedProducts() {
      switch (this.sort) {
        case 1:
          return this.products.sort((a, b) => {
            return b.price > a.price;
          });
          break;
        case 2:
          return this.products.sort((a, b) => {
            return a.name > b.name;
          });
          break;
        case 3:
          return this.products.sort((a, b) => {
            return b.name > a.name;
          });
          break;
        default:
          return this.products.sort((a, b) => {
            return a.price > b.price;
          });
      }
    }
  },
  methods: {
    setData(products, filters, categories) {
      this.products = products;
          this.filters = filters;
          this.response = categories;
      }
      
  },
  beforeRouteEnter(to, from, next) {
    var vm = this;
    axios
      .all([
        axios.get("/api/products", { params: to.query }),
          axios.get("/api/filters"),
           axios.get("/api/categories")
      ])
      .then(
        axios.spread((products, filters, categories) => {
          next(vm => vm.setData(products.data, filters.data, categories.data));
        })
      );
  },
  beforeRouteUpdate(to, from, next) {
      axios
      .all([
        axios.get("/api/products", { params: to.query }),
           axios.get("/api/categories")
      ])
      .then(
          axios.spread((products, categories) => {
              this.products = products.data;
              this.response = categories.data;
            next();
        })
      );
  }
        
};
</script>

<style scoped>
.flex {
        display: flex;
        flex-direction: row;
    }
    .flex.search{
        display: flex;
        flex-direction: row;
        flex: 1;
    }
</style>
