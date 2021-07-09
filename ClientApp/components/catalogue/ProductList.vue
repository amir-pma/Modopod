<template>
  <div>
    <b-row class="mt-4">
      <b-col class="mb-4" sm="6" v-for="product in products" :key="product.id">
          <b-media class="product" style="text-align:center; border-radius:10px">
              
              <div style="height:300px; border-radius:5px; display: flex; justify-content: center; align-items: center;">
                  <img slot="aside" :src="product.thumbnail" :alt="product.name" @click="view(product)" style="max-width:100%; max-height:100%; position: relative; border-radius:5px; padding: 2%;"  />
              </div>
              <h4 class="mt-2" @click="view(product)" style="margin-bottom:20px">{{ product.name }}</h4>

          </b-media>
      </b-col>
    </b-row>
  </div>
</template>

<script>
export default {
  name: "product-list",
  props: {
    products: {
      type: Array,
      required: true
    }
  },
  methods: {
    view(product) {
      this.$router.push(`/products/${product.slug}`);
    }
  }
};
</script>

<style lang="scss" scoped>
.product {
  border: 3px solid #eee;

  img,
  h2 {
    cursor: pointer;
  }
}
</style>
