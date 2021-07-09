<template>
    <b-dropdown v-if="isAuthenticated" right>
        <template slot="button-content">
            <i class="fas fa-user"></i>
        </template>
        <b-dropdown-item v-if="isCustomer" to="/account">
            <i class="fas fa-user"></i>
            اکانت
        </b-dropdown-item>
        <b-dropdown-item v-if="isCustomer" to="/makeproduct">
            <i class="fas fa-pencil-alt"></i>
            ساخن طرح
        </b-dropdown-item>
        <b-dropdown-item @click="logout">
            <i class="fas fa-sign-out-alt"></i>
            خروج
        </b-dropdown-item>
    </b-dropdown>
  <div v-else @click="login">
    <i class="fas fa-user"></i>
    ورود یا ثبت نام
  </div>
</template>

<script>
export default {
  name: "auth-nav-item",
  computed: {
    isAuthenticated() {
      return this.$store.getters.isAuthenticated;
    },
    isCustomer() {
      return this.$store.getters.isInRole("Customer");
    }
  },
  methods: {
    login() {
      this.$store.commit("showAuthModal");
    },
    logout() {
      this.$store.dispatch("logout").then(() => {
        if (this.$route.matched.some(route => route.meta.requiresAuth)) {
          this.$router.push("/");
        }
      });
    }
  }
};
</script>
