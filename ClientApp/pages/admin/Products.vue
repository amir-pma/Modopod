<template>
    <div>
        <h3 class="float-left">کالا ها</h3>
        <b-button variant="primary" to="/admin/products/create" class="float-right mb-4">
            <i class="fas fa-plus"></i>
            اضافه کردن کالا جدید
        </b-button>
        <table class="table table-striped table-hover">
            <thead>
                <tr>
                    <th>#</th>
                    <th>نام کالا</th>
                    <th>قیمت</th>
                </tr>
            </thead>
            <tbody>
                <template v-if="products && products.length > 0">
                    <tr v-for="product in products" :key="product.id">
                        <td>{{ product.id }}</td>
                        <td>{{ product.name }}</td>
                        <td>{{ product.price | currency }}</td>
                    </tr>
                </template>
                <tr v-else>
                    <td colspan="3">کالایی برای نمایش وجود ندارد</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
import axios from "axios";

export default {
        name: "products",

  data() {
      return {
      products: null
    };
  },
  methods: {
    setData(products) {
          this.products = products;
    }
        },
 
  beforeRouteEnter(to, from, next) {
      var vm = this;
    axios
      .all([
        axios.get("/api/products")
      ])
      .then(
        axios.spread((products, categories) => {
          next(vm => vm.setData(products.data));
        })
      );
  }
};
</script>
