<template>
    <div>
        <my-navbar v-if="Boolean(response)" :categories="computedCategories" :subCategories="computedSubcategories"></my-navbar>
        <b-container class="page pt-4">
            <b-row>
                <b-col cols="3">
                    <b-list-group>
                        <b-list-group-item to="/admin/orders">
                            <i class="fas fa-shopping-cart mr-2"></i>
                            Orders
                        </b-list-group-item>
                        <b-list-group-item to="/admin/products">
                            <i class="fas fa-mobile mr-2"></i>
                            Products
                        </b-list-group-item>
                    </b-list-group>
                </b-col>
                <b-col cols="9">
                    <router-view />
                </b-col>
            </b-row>
        </b-container>
    </div>
</template>

<script>
    import axios from "axios";
    import MyNavbar from "../../components/MyNavbar.vue";

export default {
  name: "admin-index",
  components: {
      MyNavbar
  },
  data() {
    return {
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
    setData(categories) {
          this.response = categories;
    }
  },
  beforeRouteEnter(to, from, next) {
      var vm = this;
    axios
      .all([
           axios.get("/api/categories")
      ])
      .then(
        axios.spread(categories => {
          next(vm => vm.setData(categories.data));
        })
      );
  }
};
</script>
