<template>
    <div>
        <my-navbar v-if="Boolean(response)" :categories="computedCategories" :subCategories="computedSubcategories"></my-navbar>
        <b-container class="pt-4 page">
            <h1 class="pb-4">سبد خرید</h1>
            <p v-if="!items.length" class="mt-4 text-center">
                سبد خرید شما خالی است
            </p>
            <div v-else class="table-responsive">
                <table class="table">
                    <thead>
                        <tr>
                            <th>کالا</th>
                            <th>قیمت تک</th>
                            <th>تعداد</th>
                            <th>قیمت کل</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        <cart-item v-for="(item, index) in items" :key="index" :item="item" />
                    </tbody>
                    <tfoot>
                        <tr>
                            <td>
                                <b-button variant="secondary" @click="continueShopping">
                                    <i class="fas fa-chevron-left"></i>
                                    بازگشت به خرید
                                </b-button>
                            </td>
                            <td colspan="2"></td>
                            <td>
                                <strong>قیمت کل: {{ total | currency }}</strong>
                            </td>
                            <td>
                                <b-button variant="success" @click="checkout">
                                    پرداخت
                                    <i class="fas fa-chevron-right"></i>
                                </b-button>
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </div>
        </b-container>
    </div>
</template>

<script>
    import CartItem from "../components/cart/CartItem.vue";
    import axios from "axios";
    import MyNavbar from "../components/MyNavbar.vue";

export default {
  name: "cart",
  components: {
      CartItem,
      MyNavbar
        },
        data() {
            return {
                response: null
            };
        },
  computed: {
    items() {
      return this.$store.state.cart;
    },
    total() {
      return this.$store.getters.shoppingCartTotal;
      },
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
    },
    continueShopping() {
      this.$router.go(-1);
    },
    checkout() {
      this.$router.push("/checkout");
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

