<template>
    <div>
        <my-navbar v-if="Boolean(response)" :categories="computedCategories" :subCategories="computedSubcategories"></my-navbar>
        <b-container class="page pt-4">
            <h1>اکانت</h1>
            <order-list :orders="orders" />
        </b-container>
    </div>
</template>

<script>
import axios from "axios";
    import OrderList from "../components/shared/OrderList.vue";
        import MyNavbar from "../components/MyNavbar.vue";


export default {
  name: "account",
  components: {
      OrderList,
      MyNavbar
  },
  data() {
    return {
        orders: null,
        response: null
    };
  },
  methods: {
    setData(orders, categories) {
          this.orders = orders;
          this.response = categories;
    }
  },
  beforeRouteEnter(to, from, next) {
      var vm = this;
    axios
      .all([
        axios.get("/api/orders"),
           axios.get("/api/categories")
      ])
      .then(
        axios.spread((orders, categories) => {
          next(vm => vm.setData(orders.data, categories.data));
        })
      );
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
        }
};
</script>

