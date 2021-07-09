#pragma execution_character_set("utf-8")
<template>
    <div>
        <my-navbar v-if="Boolean(response)" :categories="computedCategories" :subCategories="computedSubcategories"></my-navbar>
        <b-container class="page pt-4">
            <h1>پرداخت</h1>
            <checkout-success v-if="success" :order="order" />
            <b-row v-else>
                <b-col cols="4" order="2">
                    <cart-summary />
                </b-col>
                <b-col cols="8">
                    <checkout-form @success="onSuccess" />
                </b-col>
            </b-row>
        </b-container>
    </div>
</template>

<script>
    import CartSummary from "../components/checkout/CartSummary.vue";
    import CheckoutForm from "../components/checkout/CheckoutForm.vue";
    import CheckoutSuccess from "../components/checkout/CheckoutSuccess.vue";
    import MyNavbar from "../components/MyNavbar.vue";
    import axios from "axios";

    export default {
        name: "checkout",
        components: {
            CartSummary,
            CheckoutForm,
            CheckoutSuccess,
            MyNavbar
        },
        data() {
            return {
                response: null,
                success: false,
                order: null
            };
        },
        methods: {
            setData(categories) {
          this.response = categories;
    },
            onSuccess(order) {
                this.success = true;
                this.order = order;
                window.scrollTo(0, 0);
            }
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
