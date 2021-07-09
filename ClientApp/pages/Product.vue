<template>
    <div>
        <my-navbar v-if="Boolean(response)" :categories="computedCategories" :subCategories="computedSubcategories"></my-navbar>
        <div class="page">
            <product-details v-if="product" :product="product" />
        </div>
    </div>
  
</template>

<script>
import axios from "axios";
import ProductDetails from "../components/product/Details.vue";
    import MyNavbar from "../components/MyNavbar.vue";

export default {
  name: "product",
  components: {
      ProductDetails,
      MyNavbar
  },
  data() {
    return {
        product: null,
        response: null
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
            }
        },
  methods: {
    setData(product, categories) {
          this.product = product;
          this.response = categories;
    }
  },
  beforeRouteEnter(to, from, next) {
      var vm = this;
    axios
      .all([
        axios.get(`/api/products/${to.params.slug}`),
           axios.get("/api/categories")
      ])
      .then(
        axios.spread((product, categories) => {
          next(vm => vm.setData(product.data, categories.data));
        })
      );
  }
};
</script>
