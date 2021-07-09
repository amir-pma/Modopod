<template>
    <div id="Cart" @click="$router.push('/cart')" style="cursor:pointer;">
        <i class="fas fa-shopping-cart"></i>(سبد خرید ({{ count }}
        <!--<b-popover target="Cart" triggers="hover">
            <div>
                تعداد: {{ count }}
            </div>
            <div>
                قیمت کل: {{ total | currency }}
            </div>
        </b-popover>-->
    </div>
</template>

<script>
export default {
  name: "cart-summary",
  computed: {
    total() {
      return this.$store.getters.shoppingCartTotal;
    },
    count() {
      return this.$store.getters.shoppingCartItemCount;
    }
  }
};
</script>

